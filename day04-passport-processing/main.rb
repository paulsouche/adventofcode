require 'test/unit/assertions'
include Test::Unit::Assertions

test_arr = [
  "ecl:gry pid:860033327 eyr:2020 hcl:#fffffd\nbyr:1937 iyr:2017 cid:147 hgt:183cm",
  "iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884\nhcl:#cfa07d byr:1929",
  "hcl:#ae17e1 iyr:2013\neyr:2024\necl:brn pid:760753108 byr:1931\nhgt:179cm",
  "hcl:#cfa07d eyr:2025 pid:166559648 iyr:2011\necl:brn hgt:59in",
]

test_arr_2 = [
  #Invalid
  "eyr:1972 cid:100\nhcl:#18171d ecl:amb hgt:170 pid:186cm iyr:2018 byr:1926",
  "iyr:2019\nhcl:#602927 eyr:1967 hgt:170cm\necl:grn pid:012533040 byr:1946",
  "hcl:dab227 iyr:2012\necl:brn hgt:182cm pid:021572410 eyr:2020 byr:1992 cid:277",
  "hgt:59cm ecl:zzz\neyr:2038 hcl:74454a iyr:2023\npid:3556412378 byr:2007",
  #Valid
  "pid:087499704 hgt:74in ecl:grn iyr:2012 eyr:2030 byr:1980\nhcl:#623a2f",
  "eyr:2029 ecl:blu cid:129 byr:1989\niyr:2014 pid:896056539 hcl:#a97842 hgt:165cm",
  "hcl:#888785\nhgt:164cm byr:2001 iyr:2015 cid:88\npid:545766238 ecl:hzl\neyr:2022",
  "iyr:2010 hgt:158cm hcl:#b6652a ecl:blu byr:1944 eyr:2021 pid:093154719",
]

input_arr = File.open(File.join(File.dirname(__FILE__), 'input.txt')).read().split("\n\n")

REQUIRED_KEYWORDS = %w[byr iyr eyr hgt hcl ecl pid].freeze

EYES_COLORS = %w[amb blu brn gry grn hzl oth].freeze

def line_is_valid(line)
  key, value = line.split(':')
  case key
  when 'byr'
    value.to_i >= 1920 && value.to_i <= 2002
  when 'iyr'
    value.to_i >= 2010 && value.to_i <= 2020
  when 'eyr'
    value.to_i >= 2020 && value.to_i <= 2030
  when 'hgt'
    height = value[0..-3].to_i
    unit = value[-2..-1]
    if unit == 'cm'
      height >= 150 && height <= 193
    else
      height >= 59 && height <= 76
    end
  when 'hcl'
    value.match(/^#[0-9a-f]{6}$/)
  when 'ecl'
    EYES_COLORS.include?(value)
  when 'pid'
    value.match(/^\d{9}$/)
  else
    true
  end
end

def contains_required_fields?(passport)
  REQUIRED_KEYWORDS.all? { |key| passport.include?(key) }
end

def are_valid?(passport)
  contains_required_fields?(passport) && passport
    .split(/[\s,\n]/)
    .all? { |line| line_is_valid(line) }
end

def count_valid_passports(input, filter)
  input.filter { |passport| method(filter).call(passport) }.length
end

assert_equal count_valid_passports(test_arr, :contains_required_fields?), 2
puts count_valid_passports(input_arr, :contains_required_fields?)

assert_equal count_valid_passports(test_arr_2, :are_valid?), 4
puts count_valid_passports(input_arr, :are_valid?)
