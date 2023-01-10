require 'test/unit/assertions'
include Test::Unit::Assertions

test_arr = [16, 10, 15, 5, 1, 11, 7, 19, 6, 12, 4]

test_arr_2 = [28, 33, 18, 42, 31, 14, 46, 20, 48, 47, 24, 23, 49, 45, 19, 38, 39, 11, 1, 32, 25, 35, 8, 17, 7, 9, 4, 2, 34, 10, 3]

input_arr = File.open(File.join(File.dirname(__FILE__), 'input.txt')).read().split("\n").map { |x| x.to_i }

def joltage_differences(input)
  effective_joltage = 0
  differences = [0,0,1]
  input.sort.each do |adapter|
    differences[adapter - effective_joltage - 1] += 1
    effective_joltage = adapter
  end
  differences[0] * differences[2]
end

def count_arrangements(input)
  hash = { 0 => 1 }

  input.sort.each{ |x| hash[x] = [ hash[x-1], hash[x-2], hash[x-3] ].filter{ |y| y != nil }.reduce(:+) }

  hash.values.last
end

assert_equal joltage_differences(test_arr), 35
assert_equal joltage_differences(test_arr_2), 220
puts joltage_differences(input_arr)

assert_equal count_arrangements(test_arr), 8
assert_equal count_arrangements(test_arr_2), 19208
puts count_arrangements(input_arr)
