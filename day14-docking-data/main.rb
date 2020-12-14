require 'ostruct'
require 'test/unit/assertions'
include Test::Unit::Assertions

test_arr = [
  'mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X',
  'mem[8] = 11',
  'mem[7] = 101',
  'mem[8] = 0',
]

test_arr_2 = [
  'mask = 000000000000000000000000000000X1001X',
  'mem[42] = 100',
  'mask = 00000000000000000000000000000000X0XX',
  'mem[26] = 1',
]

input_arr = File.open(File.join(File.dirname(__FILE__), 'input.txt')).read().split("\n")

class Program
  @@MASK_REGEX = /^mask\s=\s([0,1,X]{36})$/
  @@MEM_REGEX = /^mem\[(\d+)\]\s=\s(\d+)$/
  @@MEM_TO_BIN_REGEX = /^([0]+)(\d+)$/

  def initialize(input)
    @input = input
    @memory = {}
    @mask = 'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX'
  end

  def memory_sum
    @memory.values.map {|value| memory_to_decimal(value).to_i }.reduce(:+)
  end

  def decimal_to_memory(decimal)
    decimal.to_i.to_s(2).rjust(36, '0')
  end

  def memory_to_decimal(memory)
    memory.gsub(@@MEM_TO_BIN_REGEX, '\2').reverse.chars.map
      .with_index{ |digit, index| digit.to_i * 2**index }
      .sum.to_i.to_s
  end

  def set_mask(instruction)
    @mask = @@MASK_REGEX.match(instruction).captures.first
  end

  def write_bitmask(instruction)
    mem_index, decimal = @@MEM_REGEX.match(instruction).captures

    value = decimal_to_memory(decimal)
    @memory[mem_index] = @mask.chars
      .map.with_index { |char, i| char != 'X' ? @mask[i] : value[i] }.join('')
  end

  def write_memory_adress_decoder(instruction)
    mem_index, decimal = @@MEM_REGEX.match(instruction).captures

    value = decimal_to_memory(decimal)
    address = decimal_to_memory(mem_index)
    floating_address = @mask.chars
      .map.with_index { |char, i| char == '0' ? address[i] : @mask[i] }.join('')

    list_addresses(floating_address)
      .each {|adress| @memory[adress] = value}
  end

  def list_addresses(floating_address)
    xpos = search_char(floating_address, 'X')
    [0, 1].repeated_permutation(xpos.size).map do |arr|
      arr.each_with_index do |a, i|
        floating_address[xpos[i]] = a.to_s
      end
      floating_address.to_i(2).to_s
    end
  end

  def search_char(str, char)
    res = []
    str.chars.each_with_index do |c, i|
      res.push(i) if c == char
    end
    res
  end

  def execute_instruction(fn, line)
    if (line.match(@@MASK_REGEX))
      set_mask(line)
    else
      method(fn).call(line)
    end
  end

  def run(fn)
    @input.each{ |line| execute_instruction(fn, line)}
    self
  end
end

def part1(input)
  Program.new(input).run(:write_bitmask).memory_sum
end

def part2(input)
  Program.new(input).run(:write_memory_adress_decoder).memory_sum
end

assert_equal part1(test_arr), 165
puts part1(input_arr)

assert_equal part2(test_arr_2), 208
puts part2(input_arr)
