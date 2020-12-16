require 'ostruct'
require 'test/unit/assertions'
include Test::Unit::Assertions

test_arr = [
  "class: 1-3 or 5-7\nrow: 6-11 or 33-44\nseat: 13-40 or 45-50",
  "your ticket:\n7,1,14",
  "nearby tickets:\n7,3,47\n40,4,50\n55,2,20\n38,6,12",
]

test_arr_2 = [
  "class: 0-1 or 4-19\nrow: 0-5 or 8-19\nseat: 0-13 or 16-19",
  "your ticket:\n11,12,13",
  "nearby tickets:\n3,9,18\n15,1,5\n5,14,9",
]

input_arr = File.open(File.join(File.dirname(__FILE__), 'input.txt')).read().split("\n\n")

RULES_REGEX = /^([\w,\s]+):\s(\d+)-(\d+)\sor\s(\d+)-(\d+)$/

def parse_rules(raw_rules)
  raw_rules.split("\n").map do |rule|
    name, min_1, max_1, min_2, max_2 = RULES_REGEX.match(rule).captures

    OpenStruct.new({
      name: name,
      min_1: min_1.to_i,
      max_1: max_1.to_i,
      min_2: min_2.to_i,
      max_2: max_2.to_i,
    })
  end
end

def parse_tickets(raw_tickets)
  raw_tickets.split("\n") - ['nearby tickets:']
end

def parse_your_ticket(raw_your_ticket)
  raw_your_ticket.split("\n").last
end

def find_valid_rule(rules, valid_tickets)
  valid_rules = []
  for i in 0..(rules.length - 1)
    valid_rules.push(rules.select { |rule| valid_tickets.all? { |ticket| (ticket[i] >= rule.min_1 && ticket[i] <= rule.max_1) || (ticket[i] >= rule.min_2 && ticket[i] <= rule.max_2) } })
  end

  determined = []
  while (valid_rules.any? { |rules| rules.length > 1 })
    valid_rule = valid_rules.find { |rules| (rules.length == 1) && !determined.include?(rules.first.name) }.first
    determined.push(valid_rule.name)

    for i in 0..(valid_rules.length - 1)
      next if (valid_rules[i].length == 1)
      valid_rules[i] = valid_rules[i].select { |rule| rule.name != valid_rule.name }
    end
  end

  valid_rules.map { |rules| rules.first }
end

def count_invalid_tickets(input)
  raw_rules, raw_your_ticket, raw_tickets = input

  rules = parse_rules(raw_rules)
  tickets = parse_tickets(raw_tickets)

  tickets.reduce(0) do |acc, ticket|
    ticket.split(',').map { |v| v.to_i }
      .select { |value| rules.all? { |rule| (value < rule.min_1) || (value > rule.max_1 && value < rule.min_2) || (value > rule.max_2) } }
      .reduce(acc) { |acc, invalid_value| acc += invalid_value }
  end
end

def calculate_departure(input)
  raw_rules, raw_your_ticket, raw_tickets = input

  rules = parse_rules(raw_rules)
  your_ticket = parse_your_ticket(raw_your_ticket).split(',').map { |v| v.to_i }
  tickets = parse_tickets(raw_tickets).map{ |ticket| ticket.split(',').map { |v| v.to_i } }

  valid_tickets = tickets
    .select { |ticket| ticket
      .all? { |value| rules
        .any? { |rule| (value >= rule.min_1 && value <= rule.max_1) || (value >= rule.min_2 && value <= rule.max_2) } } }
    .push(your_ticket)

  valid_rule = find_valid_rule(rules, valid_tickets)

  your_ticket.each.with_index.reduce(1) do |acc, (value, i)|
    acc *= valid_rule[i].name.start_with?('departure') ? value : 1
  end
end

assert_equal count_invalid_tickets(test_arr), 71
puts count_invalid_tickets(input_arr)

assert_equal calculate_departure(test_arr_2), 1
puts calculate_departure(input_arr)
