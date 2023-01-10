require 'ostruct'
require 'test/unit/assertions'
include Test::Unit::Assertions

test_arr = ['1-3 a: abcde', '1-3 b: cdefg', '2-9 c: ccccccccc']

input_arr = File.open(File.join(File.dirname(__FILE__), 'input.txt')).map { |x| x }

def parse_input(input)
  policy, password = input.split(': ')
  range, char = policy.split(' ')
  range_start, range_end = range.split('-').map { |x| x.to_i }

  return OpenStruct.new({
    password: password,
    char: char,
    range_start: range_start,
    range_end: range_end
  })
end

def policy_1(line)
  occurences = line.password.count line.char
  occurences >= line.range_start && occurences <= line.range_end ? 1 : 0
end

def policy_2(line)
  (line.password[line.range_start - 1] == line.char) ^ (line.password[line.range_end - 1] == line.char) ? 1 : 0
end

def valid_password_count(input, policy)
  parsed_input = input.map { |x| parse_input(x) }

  parsed_input.reduce(0) do |sum, line|
    sum += method(policy).call(line)
  end
end

assert_equal valid_password_count(test_arr, :policy_1), 2
puts valid_password_count(input_arr, :policy_1)

assert_equal valid_password_count(test_arr, :policy_2), 1
puts valid_password_count(input_arr, :policy_2)
