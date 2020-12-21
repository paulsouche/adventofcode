require 'ostruct'
require 'test/unit/assertions'
include Test::Unit::Assertions

test_arr = [
  'mxmxvkd kfcds sqjhc nhms (contains dairy, fish)',
  'trh fvjkl sbzzf mxmxvkd (contains dairy)',
  'sqjhc fvjkl (contains soy)',
  'sqjhc mxmxvkd sbzzf (contains fish)',
]

input_arr = File.open(File.join(File.dirname(__FILE__), 'input.txt')).read().split("\n")

FOOD_REGEX = /([a-z\s]+)\s\(contains\s([a-z,\s]+)\)/

def parse_food(raw_food)
  raw_ingredients, raw_allergens = FOOD_REGEX.match(raw_food).captures

  OpenStruct.new({
    ingredients: raw_ingredients.split(' '),
    allergens: raw_allergens.split(', '),
  })
end

def parse_food_list(input)
  input.map { |raw_food| parse_food(raw_food) }
end

def get_ingredients(food_list)
  food_list.reduce({}) do |acc, food|
    food.ingredients.each do |ingredient|
      if acc.dig(ingredient)
        acc[ingredient] += 1
      else
        acc[ingredient] = 1
      end
    end
    acc
  end
end

def get_allergens(food_list)
  food_list.reduce({}) do |acc, food|
    food.allergens.each do |allergen|
      if acc.dig(allergen)
        acc[allergen] = acc[allergen].select { |prev_allergen| food.ingredients.include?(prev_allergen) }
      else
        acc[allergen] = food.ingredients.dup
      end
    end
    acc
  end
end

def count_safe_ingredients(input)
  food_list = parse_food_list(input)
  ingredients = get_ingredients(food_list)
  allergens = get_allergens(food_list).values.flat_map { |v| v }
  ingredients.keys.select { |ingredient| !allergens.include?(ingredient) }.reduce(0) { |acc, ingredient| acc += ingredients[ingredient] }
end

def get_canonical_dangerous_ingredients(input)
  food_list = parse_food_list(input)
  allergens = get_allergens(food_list)

  while allergens.values.any?{ |value| value.length > 1 }
    allergens.each do |allergen, ingredients|
      next if ingredients.length == 1
      known_allergens = allergens.select { |key, value| key != allergen && value.length == 1 }.values.flat_map { |v| v }
      allergens[allergen] = allergens[allergen].select { |ingredient| !known_allergens.include?(ingredient) }
    end
  end

  allergens.keys.sort.flat_map { |allergen| allergens[allergen] }.join(',')
end


assert_equal count_safe_ingredients(test_arr), 5
puts count_safe_ingredients(input_arr)

assert_equal get_canonical_dangerous_ingredients(test_arr), 'mxmxvkd,sqjhc,fvjkl'
puts get_canonical_dangerous_ingredients(input_arr)