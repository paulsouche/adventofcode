require 'ostruct'
require 'test/unit/assertions'
include Test::Unit::Assertions

test_arr = [
  "Tile 2311:\n..##.#..#.\n##..#.....\n#...##..#.\n####.#...#\n##.##.###.\n##...#.###\n.#.#.#..##\n..#....#..\n###...#.#.\n..###..###",
  "Tile 1951:\n#.##...##.\n#.####...#\n.....#..##\n#...######\n.##.#....#\n.###.#####\n###.##.##.\n.###....#.\n..#.#..#.#\n#...##.#..",
  "Tile 1171:\n####...##.\n#..##.#..#\n##.#..#.#.\n.###.####.\n..###.####\n.##....##.\n.#...####.\n#.##.####.\n####..#...\n.....##...",
  "Tile 1427:\n###.##.#..\n.#..#.##..\n.#.##.#..#\n#.#.#.##.#\n....#...##\n...##..##.\n...#.#####\n.#.####.#.\n..#..###.#\n..##.#..#.",
  "Tile 1489:\n##.#.#....\n..##...#..\n.##..##...\n..#...#...\n#####...#.\n#..#.#.#.#\n...#.#.#..\n##.#...##.\n..##.##.##\n###.##.#..",
  "Tile 2473:\n#....####.\n#..#.##...\n#.##..#...\n######.#.#\n.#...#.#.#\n.#########\n.###.#..#.\n########.#\n##...##.#.\n..###.#.#.",
  "Tile 2971:\n..#.#....#\n#...###...\n#.#.###...\n##.##..#..\n.#####..##\n.#..####.#\n#..#.#..#.\n..####.###\n..#.#.###.\n...#.#.#.#",
  "Tile 2729:\n...#.#.#.#\n####.#....\n..#.#.....\n....#..#.#\n.##..##.#.\n.#.####...\n####.#.#..\n##.####...\n##..#.##..\n#.##...##.",
  "Tile 3079:\n#.#.#####.\n.#..######\n..#.......\n######....\n####.#..#.\n.#...#.##.\n#.#####.##\n..#.###...\n..#.......\n..#.###...",
]

input_arr = File.open(File.join(File.dirname(__FILE__), 'input.txt')).read().split("\n\n")

MONSTER = [
  '                  # '.split(''),
  '#    ##    ##    ###'.split(''),
  ' #  #  #  #  #  #   '.split(''),
]

class Matrix
  def initialize()
    @edges = {}
    @cells = []
  end

  def size
    @cells.length
  end

  def flip_vertically()
    for i in 0..(size - 1)
      @cells[i] = @cells[i].reverse
    end
    refresh_edges()
  end

  def rotate_90_clockwise()
    x = size / 2
    y = size - 1
    for i in 0..(x - 1)
      for j in i..(y - i - 1)
        mem = @cells[i][j]
        @cells[i][j] = @cells[y - j][i]
        @cells[y - j][i] = @cells[y - i][y - j]
        @cells[y - i][y - j] = @cells[j][y - i]
        @cells[j][y - i] = mem
      end
    end
    refresh_edges()
  end

  def refresh_edges()
    @edges['top'] = @cells.first.join('')
    @edges['right'] = @cells.map { |line| line.last }.join('')
    @edges['bottom'] = @cells.last.join('')
    @edges['left'] = @cells.map { |line| line.first }.join('')
  end

  def each_next_position()
    # A B C
    # D E F
    # G H I
    yield self
    rotate_90_clockwise()
    # G D A
    # H E B
    # I F C
    yield self
    rotate_90_clockwise()
    # I H G
    # F E D
    # C B A
    yield self
    rotate_90_clockwise()
    # C F I
    # B E H
    # A D G
    yield self
    rotate_90_clockwise()
    flip_vertically()
    # C B A
    # F E D
    # I H G
    yield self
    rotate_90_clockwise()
    # I F C
    # H E B
    # G D A
    yield self
    rotate_90_clockwise()
    # G H I
    # D E F
    # A B C
    yield self
    rotate_90_clockwise()
    # A D G
    # B E H
    # C F I
    yield self
  end

  def print
    @cells.each { |line| puts line.join('') }
    self
  end
end

class Tile < Matrix
  @@TILE_ID_REGEX = /Tile\s(\d+):/

  def initialize(raw_tile)
    super()
    data = raw_tile.split("\n")
    @id = @@TILE_ID_REGEX.match(data.shift()).captures.first.to_i
    @original = data.map { |line| line.split('') }
    @cells = @original.map { |line| line.dup }
    refresh_edges()
  end

  def id
    @id
  end

  def without_borders
    @cells[1..-2].map { |line| line[1..-2] }
  end

  def edges
    [
      @original.first,
      @original.map { |line| line.last },
      @original.last,
      @original.map { |line| line.first },

      @original.first.reverse,
      @original.map { |line| line.last }.reverse,
      @original.last.reverse,
      @original.map { |line| line.first }.reverse,
    ].map { |edge| edge.join('') }
  end

  def get_edge(border)
    @edges[border]
  end

  def orient_initial(tile_map)
    while true
      rotate_90_clockwise()
      break if tile_map.values
        .any? { |tile| tile.edges.include?(get_edge('right')) } &&
               tile_map.values
        .any? { |tile| tile.edges.include?(get_edge('bottom')) }
    end
  end

  def empty_borders(tiles)
    borders = 0
    tiles.each do |neighbour_tile|
      next if neighbour_tile.id == @id
      borders += 1 if neighbour_tile.edges.any? { |edge| edges.include?(edge) }
      break if borders >= 4
    end
    borders
  end

  def matches_tile(neighbour_tile, border)
    edge = get_edge(border.to_s)
    case border.to_s
    when 'top'
      neighbour_tile.get_edge('bottom') == edge
    when 'right'
      neighbour_tile.get_edge('left') == edge
    when 'bottom'
      neighbour_tile.get_edge('top') == edge
    when 'left'
      neighbour_tile.get_edge('right') == edge
    else
      raise
    end
  end

  def match(known_neighbours)
    match = false
    each_next_position() do |tile|
      abort = false
      known_neighbours.each do |border, neighbour|
        next if neighbour == nil
        abort = !matches_tile(neighbour, border)
        if abort == true
          break
        end
      end
      next if abort == true
      match = true
      break
    end
    match
  end
end

class Image < Matrix
  @@MONSTER_HEIGHT = MONSTER.length - 1
  @@MONSTER_WIDTH = MONSTER.first.length - 1

  def initialize(positions)
    super()
    positions_size = Math.sqrt(positions.size)
    tile_size = positions.first.last.size - 2
    @width = positions_size * tile_size

    for i in 0..(@width - 1)
      @cells.push([])
    end

    for y in 0..(positions_size - 1)
      for x in 0..(positions_size - 1)
        tile_image = positions["#{y}-#{x}"].without_borders

        tile_image.each.with_index do |line, index|
          line.each { |cell| @cells[y * tile_size + index].push(cell) }
        end
      end
    end
  end

  def find_monsters
    monster_coordinates = []
    for y in 0..@@MONSTER_HEIGHT
      for x in 0..@@MONSTER_WIDTH
        monster_coordinates.push(OpenStruct.new({
          x: x,
          y: y,
        })) if MONSTER[y][x] === '#'
      end
    end

    each_next_position() do |image|
      for y in 0..(@width - @@MONSTER_HEIGHT - 1)
        for x in 0..(@width - @@MONSTER_WIDTH - 1)
          if monster_coordinates.all? { |coordinates| @cells[y + coordinates.y][x + coordinates.x] == '#' }
            monster_coordinates.each { |coordinates| @cells[y + coordinates.y][x + coordinates.x] = '0' }
          end
        end
      end
    end
    self
  end

  def count_clear
    @cells.map { |line| line.join('') }.join('').count '#'
  end
end

def multiply_corners(input)
  tiles = input.map { |raw_tile| Tile.new(raw_tile) }
  tiles.select { |tile| tile.empty_borders(tiles) == 2 }.map(&:id).reduce(:*)
end

assert_equal multiply_corners(test_arr), 20899048083289
puts multiply_corners(input_arr)

def count_clear_cells(input)
  tiles = input.map { |raw_tile| Tile.new(raw_tile) }
  width = Math.sqrt(tiles.length)
  candidates = tiles.reduce({}) do |acc, tile|
    acc[tile.id] = tile
    acc
  end
  starting_corner = tiles.find { |tile| tile.empty_borders(tiles) == 2 }
  candidates.delete(starting_corner.id)
  starting_corner.orient_initial(candidates)
  positions = {}
  positions["#{0}-#{0}"] = starting_corner

  for y in 0..(width - 1)
    for x in 0..(width - 1)
      next if positions.dig("#{y}-#{x}")

      known_neighbours = {
        top: positions["#{y - 1}-#{x}"],
        right: positions["#{y}-#{x + 1}"],
        bottom: positions["#{y + 1}-#{x}"],
        left: positions["#{y}-#{x - 1}"],
      }

      candidates.each do |id, tile|
        if tile.match(known_neighbours)
          candidates.delete(id)
          positions["#{y}-#{x}"] = tile
          break
        end
      end
    end
  end

  Image.new(positions).find_monsters.count_clear
end

assert_equal count_clear_cells(test_arr), 273
puts count_clear_cells(input_arr)
