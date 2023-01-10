require 'ostruct'
require 'test/unit/assertions'
include Test::Unit::Assertions

test_arr = [
  '..##.......',
  '#...#...#..',
  '.#....#..#.',
  '..#.#...#.#',
  '.#...##..#.',
  '..#.##.....',
  '.#.#.#....#',
  '.#........#',
  '#.##...#...',
  '#...##....#',
  '.#..#...#.#'
]

input_arr = File.open(File.join(File.dirname(__FILE__), 'input.txt')).map { |x| x.gsub("\n", '') }

input_slope = OpenStruct.new({
  x: 3,
  y: 1,
})

input_slopes = [
  OpenStruct.new({
    x: 1,
    y: 1,
  }),
  OpenStruct.new({
    x: 3,
    y: 1,
  }),
  OpenStruct.new({
    x: 5,
    y: 1,
  }),
  OpenStruct.new({
    x: 7,
    y: 1,
  }),
  OpenStruct.new({
    x: 1,
    y: 2,
  })
]

def count_trees(input, slope)
  pos = OpenStruct.new({
    x: 0,
    y: 0,
  })
  trees_count = 0;

  while true do
    pos.y += slope.y
    if pos.y >= input.length
      break
    end

    row = input[pos.y]
    row_length = row.length
    x = pos.x + slope.x;
    pos.x = x >= row_length ? x - row_length : x

    if row[pos.x] == '#'
      trees_count += 1
    end
  end

  trees_count
end

def check_slopes(input, slopes)
  slopes.map { |s| count_trees(input, s) }.reduce(:*)
end

assert_equal count_trees(test_arr, input_slope), 7
puts count_trees(input_arr, input_slope)

assert_equal check_slopes(test_arr, input_slopes), 336
puts check_slopes(input_arr, input_slopes)
