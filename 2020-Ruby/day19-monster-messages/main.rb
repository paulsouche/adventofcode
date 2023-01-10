require 'test/unit/assertions'
include Test::Unit::Assertions

test_arr = [
  "0: 4 1 5\n1: 2 3 | 3 2\n2: 4 4 | 5 5\n3: 4 5 | 5 4\n4: \"a\"\n5: \"b\"",
  "ababbb\nbababa\nabbbab\naaabbb\naaaabbb"
]

test_arr_2 = [
  "42: 9 14 | 10 1\n9: 14 27 | 1 26\n10: 23 14 | 28 1\n1: \"a\"\n11: 42 31\n5: 1 14 | 15 1\n19: 14 1 | 14 14\n12: 24 14 | 19 1\n16: 15 1 | 14 14\n31: 14 17 | 1 13\n6: 14 14 | 1 14\n2: 1 24 | 14 4\n0: 8 11\n13: 14 3 | 1 12\n15: 1 | 14\n17: 14 2 | 1 7\n23: 25 1 | 22 14\n28: 16 1\n4: 1 1\n20: 14 14 | 1 15\n3: 5 14 | 16 1\n27: 1 6 | 14 18\n14: \"b\"\n21: 14 1 | 1 14\n25: 1 1 | 1 14\n22: 14 14\n8: 42\n26: 14 22 | 1 20\n18: 15 15\n7: 14 5 | 1 21\n24: 14 1",
  "abbbbbabbbaaaababbaabbbbabababbbabbbbbbabaaaa\nbbabbbbaabaabba\nbabbbbaabbbbbabbbbbbaabaaabaaa\naaabbbbbbaaaabaababaabababbabaaabbababababaaa\nbbbbbbbaaaabbbbaaabbabaaa\nbbbababbbbaaaaaaaabbababaaababaabab\nababaaaaaabaaab\nababaaaaabbbaba\nbaabbaaaabbaaaababbaababb\nabbbbabbbbaaaababbbbbbaaaababb\naaaaabbaabaaaaababaa\naaaabbaaaabbaaa\naaaabbaabbaaaaaaabbbabbbaaabbaabaaa\nbabaaabbbaaabaababbaabababaaab\naabbbbbaabbbaaaaaabbbbbababaaaaabbaaabba",
]

input_arr = File.open(File.join(File.dirname(__FILE__), 'input.txt')).read().split("\n\n")

def parse_rules(raw_rules)
  raw_rules.split("\n").reduce({}) do |acc, raw_rule|
    index, rule = raw_rule.split(': ')
    acc[index] = rule.include?('a') ? 'a' : rule.include?('b') ? 'b' : rule.split(' | ').map { |rule_indexes| rule_indexes.split(' ') }
    acc
  end
end

def each_match_string(rules, word, word_index, rule, ind = 0)
  if ind == rule.size
    yield word_index
  else
    each_match(rules, word, word_index, rule[ind]) do |next_word_index|
      each_match_string(rules, word, next_word_index, rule, ind + 1) do |next_next_word_index|
        yield next_next_word_index
      end
    end
  end
end

def each_match(rules, word, word_index, rule_ind)
  if rules[rule_ind] == 'a' || rules[rule_ind] == 'b'
    if word[word_index] == rules[rule_ind]
      yield word_index + 1
    end
  else
    rules[rule_ind].each do |rule|
      each_match_string(rules, word, word_index, rule) do |next_word_index|
        yield next_word_index
      end
    end
  end
end

def try_match(rules, word, matches)
  each_match(rules, word, 0, '0') do |next_word_index|
    matches.push(word) if next_word_index == word.length
  end
end

def count_matching_words(input)
  raw_rules, words = input
  rules = parse_rules(raw_rules)
  matches = []

  words.split("\n").each do |word|
    try_match(rules, word, matches)
  end

  matches.length
end

assert_equal count_matching_words(test_arr), 2
puts count_matching_words(input_arr)

assert_equal count_matching_words(test_arr_2), 3
test_arr_2[0] = test_arr_2.first.sub! '8: 42', '8: 42 | 42 8'
test_arr_2[0] = test_arr_2.first.sub! '11: 42 31', '11: 42 31 | 42 11 31'
assert_equal count_matching_words(test_arr_2), 12
input_arr[0] = input_arr.first.sub! '8: 42', '8: 42 | 42 8'
input_arr[0] = input_arr.first.sub! '11: 42 31', '11: 42 31 | 42 11 31'
puts count_matching_words(input_arr)