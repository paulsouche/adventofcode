require 'test/unit/assertions'
include Test::Unit::Assertions

test_1 = '389125467'
input = '476138259'

class CrabCups
  def initialize(input)
    cups = input.split('').map(&:to_i)
    len = cups.length
    @cups = {}
    @current_cup = cups.first
    cups.each.with_index { |cup, ind| @cups[cup] = cups[ind + 1 < len ? ind + 1 : 0] }
  end

  def result
    current_cup = 1
    result = []
    loop do
      result.push(@cups[current_cup])
      current_cup = @cups[current_cup]
      break if @cups[current_cup] == 1
    end

    result.join('')
  end

  def move()
    picks = []

    current_pick = @current_cup
    while picks.length < 3
      picks.push(@cups[current_pick])
      current_pick = @cups[current_pick]
    end

    @cups[@current_cup] = @cups[current_pick]

    picks.each { |pick| @cups.delete(pick) }

    destination_cup = @current_cup - 1

    while !@cups.dig(destination_cup)
      destination_cup -= 1
      if destination_cup <= 0
        destination_cup = @cups.keys().max
      end
    end

    memo = @cups[destination_cup]

    picks.each do |pick|
      @cups[destination_cup] = pick
      destination_cup = pick
    end

    @cups[destination_cup] = memo

    @current_cup = @cups[@current_cup]

    self
  end

  def run(times)
    for i in 1..times
      move()
    end
    self
  end
end

class RealCrabCups < CrabCups
  def initialize(input)
    cups = input.split('').map(&:to_i)
    for i in (cups.max + 1)..1000000
      cups.push(i)
    end
    len = cups.length
    @cups = {}
    @current_cup = cups.first
    cups.each.with_index { |cup, ind| @cups[cup] = cups[ind + 1 < len ? ind + 1 : 0] }
  end

  def result
    @cups[1] * @cups[@cups[1]]
  end
end

def part1(input, times)
  CrabCups.new(input).run(times).result
end

def part2(input)
  RealCrabCups.new(input).run(10000000).result
end

assert_equal part1(test_1, 10), '92658374'
assert_equal part1(test_1, 100), '67384529'
puts part1(input, 100)

# Take more than 20s
# assert_equal part2(test_1), 149245887792
puts part2(input)