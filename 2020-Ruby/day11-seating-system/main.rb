require 'ostruct'
require 'test/unit/assertions'
include Test::Unit::Assertions

test_arr = [
  'L.LL.LL.LL',
  'LLLLLLL.LL',
  'L.L.L..L..',
  'LLLL.LL.LL',
  'L.LL.LL.LL',
  'L.LLLLL.LL',
  '..L.L.....',
  'LLLLLLLLLL',
  'L.LLLLLL.L',
  'L.LLLLL.LL',
]

input_arr = File.open(File.join(File.dirname(__FILE__), 'input.txt')).read().split("\n")

class GameOfLife
  def initialize(input)
    @seats = input
  end

  def count_occupied_adjacent_seats(y, x)
    occupied_seats = 0

    for i in ([0, y - 1].max)..([y + 1, @seats.length - 1].min)
      for j in ([0, x - 1].max)..([x + 1, @seats[y].length - 1].min)
        if i == y && j == x
          next
        end

        if @seats[i][j] == '#'
          occupied_seats += 1
        end
      end
    end

    occupied_seats
  end

  def count_occupied_visible_seats(y,x)
    occupied_seats = 0
    directions = [
      OpenStruct.new({ x:  0, y: -1 }),
      OpenStruct.new({ x:  1, y: -1 }),
      OpenStruct.new({ x:  1, y:  0 }),
      OpenStruct.new({ x:  1, y:  1 }),
      OpenStruct.new({ x:  0, y:  1 }),
      OpenStruct.new({ x: -1, y:  1 }),
      OpenStruct.new({ x: -1, y:  0 }),
      OpenStruct.new({ x: -1, y: -1 }),
    ]

    directions.each do |direction|
      j, i = y, x
      while true
        j += direction.y
        if (j < 0) || (j >= @seats.length)
          break
        end

        i += direction.x
        if (i < 0) || (i >= @seats[j].length)
          break
        end

        if @seats[j][i] == 'L'
          break
        end

        if @seats[j][i] == '#'
          occupied_seats += 1
          break
        end
      end
    end

    occupied_seats
  end

  def occupied_seats
    @seats.join('').count('#')
  end

  def swap_seat(fn, tolerance, seat, y, x)
    if seat == '.'
      return '.'
    end

    case seat
    when 'L'
      if method(fn).call(y,x) == 0
        return '#'
      end
    when '#'
      if method(fn).call(y,x) >= tolerance
        return 'L'
      end
    end
    seat
  end

  def round(fn, tolerance)
    swaps = 0
    new_seats = []
    for y in 0..@seats.length - 1
      new_row = []
      for x in 0..@seats[y].length - 1
        seat = @seats[y][x]
        new_seat = swap_seat(fn, tolerance, seat, y, x)

        if seat != new_seat
          swaps += 1
        end

        new_row.push(new_seat)
      end
      new_seats.push(new_row.join(''))
    end

    @seats = new_seats
    swaps
  end

  def run(fn, tolerance)
    loop do
      swaps = self.round(fn, tolerance)
      break if (swaps == 0)
    end

    self.occupied_seats
  end

  def print
    @seats.each { |seat| puts seat }
    puts ''
  end
end

def part1(input)
  GameOfLife.new(input).run(:count_occupied_adjacent_seats, 4)
end

def part2(input)
  GameOfLife.new(input).run(:count_occupied_visible_seats, 5)
end

assert_equal part1(test_arr), 37
puts part1(input_arr)

assert_equal part2(test_arr), 26
puts part2(input_arr)