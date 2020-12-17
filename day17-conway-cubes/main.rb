require 'ostruct'
require 'test/unit/assertions'
include Test::Unit::Assertions

test_arr = [
  '.#.',
  '..#',
  '###',
]

input_arr = File.open(File.join(File.dirname(__FILE__), 'input.txt')).read().split("\n")

class GameOfLife
  @@DIRECTIONS_DIMENSIONS_3 = [
    OpenStruct.new({ x: -1, y: -1, z: -1, w: 0 }),
    OpenStruct.new({ x: -1, y: -1, z:  0, w: 0 }),
    OpenStruct.new({ x: -1, y: -1, z:  1, w: 0 }),
    OpenStruct.new({ x: -1, y:  0, z: -1, w: 0 }),
    OpenStruct.new({ x: -1, y:  0, z:  0, w: 0 }),
    OpenStruct.new({ x: -1, y:  0, z:  1, w: 0 }),
    OpenStruct.new({ x: -1, y:  1, z: -1, w: 0 }),
    OpenStruct.new({ x: -1, y:  1, z:  0, w: 0 }),
    OpenStruct.new({ x: -1, y:  1, z:  1, w: 0 }),

    OpenStruct.new({ x:  0, y: -1, z: -1, w: 0 }),
    OpenStruct.new({ x:  0, y: -1, z:  0, w: 0 }),
    OpenStruct.new({ x:  0, y: -1, z:  1, w: 0 }),
    OpenStruct.new({ x:  0, y:  0, z: -1, w: 0 }),
    # OpenStruct.new({ x:  0, y:  0 , z:  0, w: 0 }),
    OpenStruct.new({ x:  0, y:  0, z:  1, w: 0 }),
    OpenStruct.new({ x:  0, y:  1, z: -1, w: 0 }),
    OpenStruct.new({ x:  0, y:  1, z:  0, w: 0 }),
    OpenStruct.new({ x:  0, y:  1, z:  1, w: 0}),

    OpenStruct.new({ x:  1, y: -1, z: -1, w: 0 }),
    OpenStruct.new({ x:  1, y: -1, z:  0, w: 0 }),
    OpenStruct.new({ x:  1, y: -1, z:  1, w: 0 }),
    OpenStruct.new({ x:  1, y:  0, z: -1, w: 0 }),
    OpenStruct.new({ x:  1, y:  0, z:  0, w: 0 }),
    OpenStruct.new({ x:  1, y:  0, z:  1, w: 0 }),
    OpenStruct.new({ x:  1, y:  1, z: -1, w: 0 }),
    OpenStruct.new({ x:  1, y:  1, z:  0, w: 0 }),
    OpenStruct.new({ x:  1, y:  1, z:  1, w: 0 }),
  ]

  @@DIRECTIONS_DIMENSIONS_4 = [
    OpenStruct.new({ x: -1, y: -1, z: -1, w: -1 }),
    OpenStruct.new({ x: -1, y: -1, z: -1, w:  0 }),
    OpenStruct.new({ x: -1, y: -1, z: -1, w:  1 }),
    OpenStruct.new({ x: -1, y: -1, z:  0, w: -1 }),
    OpenStruct.new({ x: -1, y: -1, z:  0, w:  0 }),
    OpenStruct.new({ x: -1, y: -1, z:  0, w:  1 }),
    OpenStruct.new({ x: -1, y: -1, z:  1, w: -1 }),
    OpenStruct.new({ x: -1, y: -1, z:  1, w:  0 }),
    OpenStruct.new({ x: -1, y: -1, z:  1, w:  1 }),
    OpenStruct.new({ x: -1, y:  0, z: -1, w: -1 }),
    OpenStruct.new({ x: -1, y:  0, z: -1, w:  0 }),
    OpenStruct.new({ x: -1, y:  0, z: -1, w:  1 }),
    OpenStruct.new({ x: -1, y:  0, z:  0, w: -1 }),
    OpenStruct.new({ x: -1, y:  0, z:  0, w:  0 }),
    OpenStruct.new({ x: -1, y:  0, z:  0, w:  1 }),
    OpenStruct.new({ x: -1, y:  0, z:  1, w: -1 }),
    OpenStruct.new({ x: -1, y:  0, z:  1, w:  0 }),
    OpenStruct.new({ x: -1, y:  0, z:  1, w:  1 }),
    OpenStruct.new({ x: -1, y:  1, z: -1, w: -1 }),
    OpenStruct.new({ x: -1, y:  1, z: -1, w:  0 }),
    OpenStruct.new({ x: -1, y:  1, z: -1, w:  1 }),
    OpenStruct.new({ x: -1, y:  1, z:  0, w: -1 }),
    OpenStruct.new({ x: -1, y:  1, z:  0, w:  0 }),
    OpenStruct.new({ x: -1, y:  1, z:  0, w:  1 }),
    OpenStruct.new({ x: -1, y:  1, z:  1, w: -1 }),
    OpenStruct.new({ x: -1, y:  1, z:  1, w:  0 }),
    OpenStruct.new({ x: -1, y:  1, z:  1, w:  1 }),

    OpenStruct.new({ x:  0, y: -1, z: -1, w: -1 }),
    OpenStruct.new({ x:  0, y: -1, z: -1, w:  0 }),
    OpenStruct.new({ x:  0, y: -1, z: -1, w:  1 }),
    OpenStruct.new({ x:  0, y: -1, z:  0, w: -1 }),
    OpenStruct.new({ x:  0, y: -1, z:  0, w:  0 }),
    OpenStruct.new({ x:  0, y: -1, z:  0, w:  1 }),
    OpenStruct.new({ x:  0, y: -1, z:  1, w: -1 }),
    OpenStruct.new({ x:  0, y: -1, z:  1, w:  0 }),
    OpenStruct.new({ x:  0, y: -1, z:  1, w:  1 }),
    OpenStruct.new({ x:  0, y:  0, z: -1, w: -1 }),
    OpenStruct.new({ x:  0, y:  0, z: -1, w:  0 }),
    OpenStruct.new({ x:  0, y:  0, z: -1, w:  1 }),
    OpenStruct.new({ x:  0, y:  0, z:  0, w: -1 }),
    # OpenStruct.new({ x:  0, y:  0, z:  0, w:  0 }),
    OpenStruct.new({ x:  0, y:  0, z:  0, w:  1 }),
    OpenStruct.new({ x:  0, y:  0, z:  1, w: -1 }),
    OpenStruct.new({ x:  0, y:  0, z:  1, w:  0 }),
    OpenStruct.new({ x:  0, y:  0, z:  1, w:  1 }),
    OpenStruct.new({ x:  0, y:  1, z: -1, w: -1 }),
    OpenStruct.new({ x:  0, y:  1, z: -1, w:  0 }),
    OpenStruct.new({ x:  0, y:  1, z: -1, w:  1 }),
    OpenStruct.new({ x:  0, y:  1, z:  0, w: -1 }),
    OpenStruct.new({ x:  0, y:  1, z:  0, w:  0 }),
    OpenStruct.new({ x:  0, y:  1, z:  0, w:  1 }),
    OpenStruct.new({ x:  0, y:  1, z:  1, w: -1 }),
    OpenStruct.new({ x:  0, y:  1, z:  1, w:  0 }),
    OpenStruct.new({ x:  0, y:  1, z:  1, w:  1 }),

    OpenStruct.new({ x:  1, y: -1, z: -1, w: -1 }),
    OpenStruct.new({ x:  1, y: -1, z: -1, w:  0 }),
    OpenStruct.new({ x:  1, y: -1, z: -1, w:  1 }),
    OpenStruct.new({ x:  1, y: -1, z:  0, w: -1 }),
    OpenStruct.new({ x:  1, y: -1, z:  0, w:  0 }),
    OpenStruct.new({ x:  1, y: -1, z:  0, w:  1 }),
    OpenStruct.new({ x:  1, y: -1, z:  1, w: -1 }),
    OpenStruct.new({ x:  1, y: -1, z:  1, w:  0 }),
    OpenStruct.new({ x:  1, y: -1, z:  1, w:  1 }),
    OpenStruct.new({ x:  1, y:  0, z: -1, w: -1 }),
    OpenStruct.new({ x:  1, y:  0, z: -1, w:  0 }),
    OpenStruct.new({ x:  1, y:  0, z: -1, w:  1 }),
    OpenStruct.new({ x:  1, y:  0, z:  0, w: -1 }),
    OpenStruct.new({ x:  1, y:  0, z:  0, w:  0 }),
    OpenStruct.new({ x:  1, y:  0, z:  0, w:  1 }),
    OpenStruct.new({ x:  1, y:  0, z:  1, w: -1 }),
    OpenStruct.new({ x:  1, y:  0, z:  1, w:  0 }),
    OpenStruct.new({ x:  1, y:  0, z:  1, w:  1 }),
    OpenStruct.new({ x:  1, y:  1, z: -1, w: -1 }),
    OpenStruct.new({ x:  1, y:  1, z: -1, w:  0 }),
    OpenStruct.new({ x:  1, y:  1, z: -1, w:  1 }),
    OpenStruct.new({ x:  1, y:  1, z:  0, w: -1 }),
    OpenStruct.new({ x:  1, y:  1, z:  0, w:  0 }),
    OpenStruct.new({ x:  1, y:  1, z:  0, w:  1 }),
    OpenStruct.new({ x:  1, y:  1, z:  1, w: -1 }),
    OpenStruct.new({ x:  1, y:  1, z:  1, w:  0 }),
    OpenStruct.new({ x:  1, y:  1, z:  1, w:  1 }),
  ]

  def initialize(input, dimensions)
    @dimensions = dimensions
    @map = {}

    for y in 0..(input.length - 1)
      for x in 0..(input[y].length - 1)
        @map["#{x},#{y},0,0"] = true if input[y][x] == '#'
      end
    end

    set_map_limits()
  end

  def count_active_cubes
    @map.keys.length
  end

  def set_map_limits
    @min_x = Float::INFINITY
    @max_x = -Float::INFINITY
    @min_y = Float::INFINITY
    @max_y = -Float::INFINITY
    @min_z = Float::INFINITY
    @max_z = -Float::INFINITY
    @min_w = Float::INFINITY
    @max_w = -Float::INFINITY
    @map.keys().each do |key|
      x,y,z,w = key.split(',').map(&:to_i)
      @min_x = x - 1 if x <= @min_x
      @min_y = y - 1 if y <= @min_y
      @min_z = z - 1 if z <= @min_z
      @min_w = w - 1 if w <= @min_w
      @max_x = x + 1 if x >= @max_x
      @max_y = y + 1 if y >= @max_y
      @max_z = z + 1 if z >= @max_z
      @max_w = w + 1 if w >= @max_w
    end
  end

  def print
    for w in (@min_w + 1)..(@max_w - 1)
      puts "w=#{w}"
      for z in (@min_z + 1)..(@max_z - 1)
        puts "z=#{z}"
        for y in (@min_y + 1)..(@max_y - 1)
          line = []
          for x in (@min_x + 1)..(@max_x - 1)
            line.push(@map.dig("#{x},#{y},#{z},#{w}") ? '#': '.')
          end
          puts line.join('')
        end
        puts ''
      end
    end
  end

  def count_active_cubes_around(x, y, z, w)
    directions = @dimensions == 4 ? @@DIRECTIONS_DIMENSIONS_4 : @@DIRECTIONS_DIMENSIONS_3
    directions.reduce(0) { |acc, direction| acc += @map
      .dig("#{ x + direction.x },#{ y + direction.y },#{ z + direction.z },#{ w + direction.w }") ? 1 : 0 }
  end

  def swap_cube(new_map, x, y, z, w)
    active_cubes_around = count_active_cubes_around(x, y, z, w)

    key = "#{x},#{y},#{z},#{w}"
    if @map.dig(key)
      new_map[key] = true if (active_cubes_around == 2) || (active_cubes_around == 3)
    else
      new_map[key] = true if (active_cubes_around == 3)
    end
  end

  def round
    new_map = {}
    for w in @min_w..@max_w
      for z in @min_z..@max_z
        for y in @min_y..@max_y
          for x in @min_x..@max_x
            swap_cube(new_map, x, y, z, w)
          end
        end
      end
    end

    @map = new_map

    set_map_limits()
  end

  def run(rounds)
    for i in 0..(rounds - 1)
      round()
    end
    self
  end
end

def play(input, dimensions)
  GameOfLife.new(input, dimensions).run(6).count_active_cubes
end

assert_equal play(test_arr, 3), 112
puts play(input_arr, 3)

assert_equal play(test_arr, 4), 848
puts play(input_arr, 4)