require 'test/unit/assertions'
include Test::Unit::Assertions

test_arr = [
  'FBFBBFFRLR',
  'BFFFBBFRRR',
  'FFFBBBFRRR',
  'BBFFBBFRLL',
]

input_arr = File.open(File.join(File.dirname(__FILE__), 'input.txt')).read().split("\n")

def get_seat_id(pass)
  row_start, row_end = [0,127]
  column_start, column_end = [0,7]

  pass.each_char do |letter|
    row_length = (row_end - row_start + 1) / 2
    column_length = (column_end - column_start + 1) / 2
    case letter
      when 'F'
        row_end -= row_length
      when 'B'
        row_start += row_length
      when 'L'
        column_end -= column_length
      when 'R'
        column_start += column_length
      else
        raise
    end
  end

  if (row_start != row_end) || (column_start != column_end)
    raise
  end

  row_start * 8 + column_start
end

def max_seat_id(input)
  maximum = 0

  input.each do |pass|
    id = get_seat_id(pass)
    if id > maximum
      maximum = id
    end
  end

  maximum
end

def get_available_seat_id(input)
  pasenger_seat_ids = input.map { |pass| get_seat_id(pass) }

  min = 1024
  max = 0

  pasenger_seat_ids.each do |id|
    if id > max
      max = id
    end

    if id < min
      min = id
    end
  end

  plane_seat_ids = []
  for i in 0..127 do
    for j in 0..8 do
      id = i * 8 + j
      if id >= min && id <= max
        plane_seat_ids.push(id)
      end
    end
  end

  plane_seat_ids.select { |x| !pasenger_seat_ids.include?(x) }
end


assert_equal max_seat_id(test_arr), 820
puts max_seat_id(input_arr)
puts get_available_seat_id(input_arr)
