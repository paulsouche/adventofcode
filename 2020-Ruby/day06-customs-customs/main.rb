require 'ostruct'
require 'test/unit/assertions'
include Test::Unit::Assertions

test_arr = [
  "abc",
  "a\nb\nc",
  "ab\nac",
  "a\na\na\na",
  "b",
]

input_arr = File.open(File.join(File.dirname(__FILE__), 'input.txt')).read().split("\n\n")

def group_sum_all_yes_answers(group)
  yes_answers = OpenStruct.new()

  group.split("\n").each do |form|
    form.each_char { |char| yes_answers[char] = true }
  end

  yes_answers.each_pair.count()
end

def group_sum_all_common_yes_answers(group)
  yes_answers = OpenStruct.new()
  sum = 0

  forms = group.split("\n")
  people_count = forms.length;

  forms.each do |form|
    form.each_char do |char|
      yes_answers[char] = yes_answers.dig(char) ? yes_answers[char] + 1 : 1

      if (yes_answers[char] == people_count)
        sum += 1
      end
    end
  end

  sum
end

def sum_counts(input, group_sum)
  input.map { |group| method(group_sum).call(group) }.reduce(:+)
end

assert_equal sum_counts(test_arr, :group_sum_all_yes_answers), 11
puts sum_counts(input_arr, :group_sum_all_yes_answers)

assert_equal sum_counts(test_arr, :group_sum_all_common_yes_answers), 6
puts sum_counts(input_arr, :group_sum_all_common_yes_answers)
