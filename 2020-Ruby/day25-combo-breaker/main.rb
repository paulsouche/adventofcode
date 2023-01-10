require 'test/unit/assertions'
include Test::Unit::Assertions

test_card_public_key = 5764801;
test_door_public_key = 17807724;

input_card_public_key = 335121;
input_door_public_key = 363891;

INPUT_MODULO = 20201227

def find_loop_size(public_key, subject_number)
  value = 1
  size = 0
  while true
    size += 1
    value = (value * subject_number) % INPUT_MODULO
    break if value == public_key
  end
  size
end

def calc_encryption_key(subject_number, loop_size)
  value = 1
  for i in 1..loop_size
    value = (value * subject_number) % INPUT_MODULO
  end
  value
end

def find_encryption_key(door_public_key, card_public_key)
  calc_encryption_key(door_public_key,  find_loop_size(card_public_key, 7))
end

assert_equal find_loop_size(test_card_public_key, 7), 8
assert_equal find_loop_size(test_door_public_key, 7), 11
assert_equal find_encryption_key(test_door_public_key,  test_card_public_key), 14897079
puts find_encryption_key(input_door_public_key,  input_card_public_key)