require 'ostruct'
require 'test/unit/assertions'
include Test::Unit::Assertions

test_arr = [
  'nop +0',
  'acc +1',
  'jmp +4',
  'acc +3',
  'jmp -3',
  'acc -99',
  'acc +1',
  'jmp -4',
  'acc +6',
]

input_arr = File.open(File.join(File.dirname(__FILE__), 'input.txt')).read().split("\n")

INSTRUCTIONS_REGEX = /^(\w{3})\s([+,-]\d+)$/

class Program
  def self.input_to_instructions(input)
    input.map do |line|
      instruction, argument = INSTRUCTIONS_REGEX.match(line).captures

      OpenStruct.new({
        instruction: instruction,
        argument: argument.to_i,
        times_executed: 0,
      })
    end
  end

  def initialize(input)
     @instructions = Program.input_to_instructions(input)
     @ind = 0
     @acc = 0
  end

  def switch_instruction(index)
    case @instructions[index].instruction
    when 'nop'
      @instructions[index].instruction = 'jmp'
    when 'jmp'
      @instructions[index].instruction = 'nop'
    else
      raise
    end
  end

  def execute_instruction()
    if @ind < 0 || @ind >= @instructions.length
      return OpenStruct.new({
        exited: true,
        looped: false,
        acc: @acc,
      })
    end

    execution = @instructions[@ind];

    if execution.times_executed > 0
      return OpenStruct.new({
        exited: false,
        looped: true,
        acc: @acc,
      })
    end

    execution.times_executed += 1

    case execution.instruction
    when 'acc'
      @acc += execution.argument
      @ind += 1
    when 'jmp'
      @ind += execution.argument
    else
      @ind += 1
    end

    OpenStruct.new({
      exited: false,
      looped: false,
      acc: @acc,
    })
  end

  def run()
    result = nil
    loop do
      result = self.execute_instruction()
      break if (result.exited || result.looped)
    end

    result
  end
end

def part1(input)
  Program.new(input).run().acc
end

def part2(input)
  for i in 0..input.length - 1
    if !input[i].include?('jmp') && !input[i].include?('nop')
      next
    end

    program = Program.new(input)
    program.switch_instruction(i)

    result = program.run()

    if (result.exited)
      return result.acc
    end
  end
end

assert_equal part1(test_arr), 5
puts part1(input_arr)

assert_equal part2(test_arr), 8
puts part2(input_arr)
