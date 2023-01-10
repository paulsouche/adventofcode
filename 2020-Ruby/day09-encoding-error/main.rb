require 'test/unit/assertions'
include Test::Unit::Assertions

test_arr = [
  35,
  20,
  15,
  25,
  47,
  40,
  62,
  55,
  65,
  95,
  102,
  117,
  150,
  182,
  127,
  219,
  299,
  277,
  309,
  576,
]

input_arr = File.open(File.join(File.dirname(__FILE__), 'input.txt')).read().split("\n").map { |x| x.to_i }

def validate_next_number(preamble, next_number)
  filtered_preamble = preamble.filter { |x| x < next_number }

  # Could do permutations here but it increases complexity
  for i in 0..filtered_preamble.length - 1
    for j in i..filtered_preamble.length - 1
      if filtered_preamble[i] + filtered_preamble[j] == next_number
        return true
      end
    end
  end

  false
end

def find_invalid_number(input, preamble_length)
  for i in 0..(input.length - preamble_length - 1)
    next_number = input[i + preamble_length]
    if !validate_next_number(input.slice(i, preamble_length), next_number)
      return next_number
    end
  end
end

def find_encryption_weakness(input, preamble_length)
  invalid_number = find_invalid_number(input, preamble_length)

  filtered_input = input.filter { |x| x < invalid_number }

  for i in 0..filtered_input.length - 1
    sum = 0
    list = []
    for j in i..filtered_input.length - 1
      sum += filtered_input[j]
      list.push(filtered_input[j])

      if sum == invalid_number
        return list.min + list.max
      end

      if sum > invalid_number
        break
      end
    end
  end
end

assert_equal find_invalid_number(test_arr, 5), 127
puts find_invalid_number(input_arr, 25)

assert_equal find_encryption_weakness(test_arr, 5), 62
puts find_encryption_weakness(input_arr, 25)
