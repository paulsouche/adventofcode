require 'test/unit/assertions'
include Test::Unit::Assertions

test_arr = [
  'F10',
  'N3',
  'F7',
  'R90',
  'F11',
]

input_arr = File.open(File.join(File.dirname(__FILE__), 'input.txt')).read().split("\n")

class Ship
  @@INSTRUCTIONS_REGEX = /^([F,N,E,S,W,L,R])(\d+)$/
  @@DIRECTIONS = ['N', 'E', 'S', 'W']

  def initialize()
    @facing = 'E'
    @x = 0
    @y = 0
    @waypoint_x = 10
    @waypoint_y = 1
  end

  def manhattan
    @x.abs + @y.abs
  end

  def move(action, value)
    case action
    when 'N'
      @y += value
    when 'E'
      @x += value
    when 'S'
      @y -= value
    when 'W'
      @x -= value
    else
      raise
    end
  end

  def move_forward(value)
    move(@facing, value)
  end

  def turn(action, value)
    facing_index = @@DIRECTIONS.find_index(@facing)
    step = action == 'L' ? -(value / 90) : (value / 90)
    new_index = facing_index + step

    if new_index < 0
      new_index += @@DIRECTIONS.length
    elsif new_index >= @@DIRECTIONS.length
      new_index -= @@DIRECTIONS.length
    end

    @facing = @@DIRECTIONS[new_index]
  end

  def move_waypoint(action, value)
    case action
    when 'N'
      @waypoint_y += value
    when 'E'
      @waypoint_x += value
    when 'S'
      @waypoint_y -= value
    when 'W'
      @waypoint_x -= value
    else
      raise
    end
  end

  def move_to_waypoint(value)
    @x += value * @waypoint_x
    @y += value * @waypoint_y
  end

  def turn_waypoint(action, value)
    # Only clockwise rotation
    if action == 'L'
      case value
      when 90
        value = 270
      when 270
        value = 90
      end
    end

    # (x,y) => (y,-x) * rotation times
    for i in 1..(value / 90)
      x = @waypoint_x
      @waypoint_x = @waypoint_y
      @waypoint_y = -x
    end
  end

  def navigate(instruction)
    action, value = @@INSTRUCTIONS_REGEX.match(instruction).captures

    case action
    when 'F'
      move_forward(value.to_i)
    when 'N', 'E', 'S', 'W'
      move(action, value.to_i)
    when 'L', 'R'
      turn(action, value.to_i)
    else
      raise
    end
  end

  def waypoint_navigate(instruction)
    action, value = @@INSTRUCTIONS_REGEX.match(instruction).captures

    case action
    when 'F'
      move_to_waypoint(value.to_i)
    when 'N', 'E', 'S', 'W'
      move_waypoint(action, value.to_i)
    when 'L', 'R'
      turn_waypoint(action, value.to_i)
    else
      raise
    end
  end

  def run(input, fn)
    input.each { |instruction| method(fn).call(instruction) }
    self
  end
end

def part1(input)
  Ship.new().run(input, :navigate).manhattan
end

def part2(input)
  Ship.new().run(input, :waypoint_navigate).manhattan
end

assert_equal part1(test_arr), 25
puts part1(input_arr)

assert_equal part2(test_arr), 286
puts part2(input_arr)
