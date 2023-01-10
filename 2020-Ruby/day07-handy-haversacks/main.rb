require 'ostruct'
require 'test/unit/assertions'
include Test::Unit::Assertions

input_test = [
  'light red bags contain 1 bright white bag, 2 muted yellow bags.',
  'dark orange bags contain 3 bright white bags, 4 muted yellow bags.',
  'bright white bags contain 1 shiny gold bag.',
  'muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.',
  'shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.',
  'dark olive bags contain 3 faded blue bags, 4 dotted black bags.',
  'vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.',
  'faded blue bags contain no other bags.',
  'dotted black bags contain no other bags.',
];

input_test_2 = [
  'shiny gold bags contain 2 dark red bags.',
  'dark red bags contain 2 dark orange bags.',
  'dark orange bags contain 2 dark yellow bags.',
  'dark yellow bags contain 2 dark green bags.',
  'dark green bags contain 2 dark blue bags.',
  'dark blue bags contain 2 dark violet bags.',
  'dark violet bags contain no other bags.',
]

input_arr = File.open(File.join(File.dirname(__FILE__), 'input.txt')).read().split("\n")

input_bag = 'shiny gold'

RULES_REGEX = /(.*)\sbags\scontain\s(.*)\.$/

CONTAINS_REGEX = /^(\d+)\s(.*)\sbag(s?)$/

EMPTY_REGEX = /^no other bags$/

def parse_input(input)
  input.reduce(OpenStruct.new()) do |acc, rule|
    container, contains = RULES_REGEX.match(rule).captures

    if acc.dig(container)
      raise
    end

    acc[container] = contains.split(', ').select { |bags| !bags.match(EMPTY_REGEX) }.map do |bags|
      count, bag = CONTAINS_REGEX.match(bags).captures

      if count.to_i == 0
        raise
      end

      OpenStruct.new({
        bag: bag,
        count: count.to_i,
      })
    end

    acc
  end
end

def count_bag_colors(input, bag_kind)
  rules_map = parse_input(input)
  allowed_bags = OpenStruct.new()
  bags_kind = [bag_kind]

  while bags_kind.length > 0
    kind = bags_kind.pop()

    rules_map.each_pair do |key, values|
      if values.select { |value| value.bag == kind }.length == 0
        next
      end

      if allowed_bags.dig(key)
        next
      end

      allowed_bags[key] = true
      bags_kind.push(key.to_s)
    end
  end

  allowed_bags.each_pair.count
end

def count_allowed_bags_recc(map, kind, pound)
  pound * map[kind].reduce(1) do |sum, value|
    sum += count_allowed_bags_recc(map, value.bag, value.count)
  end
end

def count_allowed_bags(input, bag_kind)
  count_allowed_bags_recc(parse_input(input), bag_kind, 1) - 1
end

assert_equal count_bag_colors(input_test, input_bag), 4
puts count_bag_colors(input_arr, input_bag)

assert_equal count_allowed_bags(input_test, input_bag), 32
assert_equal count_allowed_bags(input_test_2, input_bag), 126
puts count_allowed_bags(input_arr, input_bag)
