require 'test/unit/assertions'
include Test::Unit::Assertions

test_arr = [
  'sesenwnenenewseeswwswswwnenewsewsw',
  'neeenesenwnwwswnenewnwwsewnenwseswesw',
  'seswneswswsenwwnwse',
  'nwnwneseeswswnenewneswwnewseswneseene',
  'swweswneswnenwsewnwneneseenw',
  'eesenwseswswnenwswnwnwsewwnwsene',
  'sewnenenenesenwsewnenwwwse',
  'wenwwweseeeweswwwnwwe',
  'wsweesenenewnwwnwsenewsenwwsesesenwne',
  'neeswseenwwswnwswswnw',
  'nenwswwsewswnenenewsenwsenwnesesenew',
  'enewnwewneswsewnwswenweswnenwsenwsw',
  'sweneswneswneneenwnewenewwneswswnese',
  'swwesenesewenwneswnwwneseswwne',
  'enesenwswwswneneswsenwnewswseenwsese',
  'wnwnesenesenenwwnenwsewesewsesesew',
  'nenewswnwewswnenesenwnesewesw',
  'eneswnwswnwsenenwnwnwwseeswneewsenese',
  'neswnwewnwnwseenwseesewsenwsweewe',
  'wseweeenwnesenwwwswnew',
]

input_arr = File.open(File.join(File.dirname(__FILE__), 'input.txt')).read().split("\n")

DIRECTIONS = ['ne', 'e', 'se', 'sw', 'w', 'nw']

NORTH_EAST_REGEX = /ne/
SOUTH_EAST_REGEX = /se/
SOUTH_WEST_REGEX = /sw/
NORTH_WEST_REGEX = /nw/
KEEP_EAST_WEST_REGEX = /(ne|se|sw|nw)/

def path_to_tile(path)
  tile = {
    ne: path.scan(NORTH_EAST_REGEX).length,
    se: path.scan(SOUTH_EAST_REGEX).length,
    sw: path.scan(SOUTH_WEST_REGEX).length,
    nw: path.scan(NORTH_WEST_REGEX).length,
  }

  path = path.gsub(KEEP_EAST_WEST_REGEX, '')

  tile[:e] = path.count 'e'
  tile[:w] = path.count 'w'

  tile
end

def tile_to_path(tile)
  path = []
  DIRECTIONS.each do |direction|
    while tile[direction.to_sym] > 0
      path.push(direction)
      tile[direction.to_sym] -= 1
    end
  end
  path.join('')
end

def reduce_path(path)
  tile = path_to_tile(path)

  # Remove backwards operations
  tile = {
    ne: [0, tile[:ne] - tile[:sw] ].max,
    e:  [0, tile[:e] - tile[:w] ].max,
    se: [0, tile[:se] - tile[:nw] ].max,
    sw: [0, tile[:sw] - tile[:ne] ].max,
    w:  [0, tile[:w] - tile[:e] ].max,
    nw: [0, tile[:nw] - tile[:se] ].max,
  }

  # Reduce ne se & nw sw steps
  min_west = [tile[:nw],tile[:sw]].min
  tile[:w] += min_west
  tile[:nw] -= min_west
  tile[:sw] -= min_west

  min_east = [tile[:ne],tile[:se]].min
  tile[:e] += min_east
  tile[:ne] -= min_east
  tile[:se] -= min_east

  # Reduce se ne w operations & sw nw e operations
  while (tile[:w] > 0 && (tile[:ne] > 0 || tile[:se] > 0)) || (tile[:e] > 0 && (tile[:nw] > 0 || tile[:sw] > 0))
    while tile[:ne] > 0 && tile[:w] > 0
      tile[:ne] -= 1
      tile[:w] -= 1
      tile[:nw] += 1
    end

    while tile[:se] > 0 && tile[:w] > 0
      tile[:se] -= 1
      tile[:w] -= 1
      tile[:sw] += 1
    end

    while tile[:nw] > 0 && tile[:e] > 0
      tile[:nw] -= 1
      tile[:e] -= 1
      tile[:ne] += 1
    end

    while tile[:sw] > 0 && tile[:e] > 0
      tile[:sw] -= 1
      tile[:e] -= 1
      tile[:se] += 1
    end
  end

  # Remove backwards operations
  tile = {
    ne: [0, tile[:ne] - tile[:sw] ].max,
    e:  [0, tile[:e] - tile[:w] ].max,
    se: [0, tile[:se] - tile[:nw] ].max,
    sw: [0, tile[:sw] - tile[:ne] ].max,
    w:  [0, tile[:w] - tile[:e] ].max,
    nw: [0, tile[:nw] - tile[:se] ].max,
  }

  tile_to_path(tile)
end

def get_lobby_layout(input)
  input.reduce({}) do |acc,line|
    key = reduce_path(line)

    if acc.dig(key)
      acc.delete(key)
    else
      acc[key] = true
    end

    acc
  end
end

def count_black_neighbours(layout, neighbours)
  neighbours.reduce(0) { |acc, adjacent_path| acc += layout.dig(adjacent_path) ? 1 : 0 }
end

def get_neighbours(path)
  DIRECTIONS.map { |direction| reduce_path("#{path}#{direction}") }
end

def swap_layout(layout)
  new_layout = {}
  neighbours_map = {}

  layout.keys.each do |path|
    neighbours = get_neighbours(path)
    adjacent_black_tiles = count_black_neighbours(layout, neighbours)

    if (adjacent_black_tiles == 1 || adjacent_black_tiles == 2)
      # stays black
      new_layout[path] = true
    end

    neighbours.each do |adjacent_path|
      next if neighbours_map.dig(adjacent_path)

      adjacent_black_tiles = count_black_neighbours(layout, get_neighbours(adjacent_path))

      if adjacent_black_tiles == 2
        # becomes black
        new_layout[adjacent_path] = true
      end

      neighbours_map[adjacent_path] = adjacent_black_tiles
    end
  end

  new_layout
end

def layout_day(layout, days)
  for day in 1..days
    layout = swap_layout(layout)
  end
  layout
end

def count_black_tiles(layout)
  layout.keys.length
end

def part1(input)
  count_black_tiles(get_lobby_layout(input))
end

def part2(input, days)
  count_black_tiles(layout_day(get_lobby_layout(input), days))
end

def test_part2(input)
  layout = layout_day(get_lobby_layout(input), 1)
  assert_equal count_black_tiles(layout), 15
  layout = layout_day(layout, 1)
  assert_equal count_black_tiles(layout), 12
  layout = layout_day(layout, 1)
  assert_equal count_black_tiles(layout), 25
  layout = layout_day(layout, 1)
  assert_equal count_black_tiles(layout), 14
  layout = layout_day(layout, 1)
  assert_equal count_black_tiles(layout), 23
  layout = layout_day(layout, 1)
  assert_equal count_black_tiles(layout), 28
  layout = layout_day(layout, 1)
  assert_equal count_black_tiles(layout), 41
  layout = layout_day(layout, 1)
  assert_equal count_black_tiles(layout), 37
  layout = layout_day(layout, 1)
  assert_equal count_black_tiles(layout), 49
  layout = layout_day(layout, 1)
  assert_equal count_black_tiles(layout), 37
  layout = layout_day(layout, 10)
  assert_equal count_black_tiles(layout), 132
  layout = layout_day(layout, 10)
  assert_equal count_black_tiles(layout), 259
  layout = layout_day(layout, 10)
  assert_equal count_black_tiles(layout), 406
  layout = layout_day(layout, 10)
  assert_equal count_black_tiles(layout), 566
  layout = layout_day(layout, 10)
  assert_equal count_black_tiles(layout), 788
  layout = layout_day(layout, 10)
  assert_equal count_black_tiles(layout), 1106
  layout = layout_day(layout, 10)
  assert_equal count_black_tiles(layout), 1373
  layout = layout_day(layout, 10)
  assert_equal count_black_tiles(layout), 1844
  layout = layout_day(layout, 10)
  assert_equal count_black_tiles(layout), 2208
end

assert_equal part1(test_arr), 10
puts part1(input_arr)

# Take more than 60s
# test_part2(test_arr)
puts part2(input_arr, 100)
