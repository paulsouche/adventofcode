from itertools import permutations

EASY_DIGITS_LENGTH_MAP = {
    '1': 2,
    '4': 4,
    '7': 3,
    '8': 7,
}

POSSIBLE_ARRANGEMENTS = [''.join(p) for p in permutations('abcdefg')]

def find_digit(digits, digits_len):
    for digit in digits:
        if len(digit) == digits_len:
           return digit
    return None

def filter_easy(arrangement, digit, number):
    # rules to filter the list are
    # index 0 => top
    # index 1 => top left
    # index 2 => top right
    # index 3 => middle
    # index 4 => bottom left
    # index 5 => bottom right
    # index 6 => bottom
    if number == '1':
        return all(arrangement[i] in digit for i in [2,5])
    elif number == '4':
        return all(arrangement[i] in digit for i in [1,2,3,5])
    elif number == '7':
        return all(arrangement[i] in digit for i in [0,2,5])
    elif number == '8':
        return all(arrangement[i] in digit for i in [0,1,2,3,4,5,6])
    else:
        raise 'Cannot filter a non easy number'

def get_arrangement_digits_map(a):
    digits_map = {}
    # 0
    digits_map[''.join(sorted(f'{a[0]}{a[1]}{a[2]}{a[4]}{a[5]}{a[6]}'))] = 0
    # 1
    digits_map[''.join(sorted(f'{a[2]}{a[5]}'))] = 1
    # 2
    digits_map[''.join(sorted(f'{a[0]}{a[2]}{a[3]}{a[4]}{a[6]}'))] = 2
    # 3
    digits_map[''.join(sorted(f'{a[0]}{a[2]}{a[3]}{a[5]}{a[6]}'))] = 3
    # 4
    digits_map[''.join(sorted(f'{a[1]}{a[2]}{a[3]}{a[5]}'))] = 4
    # 5
    digits_map[''.join(sorted(f'{a[0]}{a[1]}{a[3]}{a[5]}{a[6]}'))] = 5
    # 6
    digits_map[''.join(sorted(f'{a[0]}{a[1]}{a[3]}{a[4]}{a[5]}{a[6]}'))] = 6
    # 7
    digits_map[''.join(sorted(f'{a[0]}{a[2]}{a[5]}'))] = 7
    # 8
    digits_map[''.join(sorted(f'{a[0]}{a[1]}{a[2]}{a[3]}{a[4]}{a[5]}{a[6]}'))] = 8
    # 9
    digits_map[''.join(sorted(f'{a[0]}{a[1]}{a[2]}{a[3]}{a[5]}{a[6]}'))] = 9
    return digits_map

def find_digits_map(digits):
    easy_possible_arrangements = list(POSSIBLE_ARRANGEMENTS)
    # We find the easy digits in the list and we filter it
    for number in EASY_DIGITS_LENGTH_MAP.keys():
        digit = find_digit(digits, EASY_DIGITS_LENGTH_MAP[number])
        if not digit == None:
            easy_possible_arrangements = list(filter(lambda a: filter_easy(a, digit, number), easy_possible_arrangements))

    possible_arrangements = list(easy_possible_arrangements)
    for arrangement in possible_arrangements:
        digits_map = get_arrangement_digits_map(arrangement)
        if all(''.join(sorted(digit)) in digits_map for digit in digits):
            return digits_map

def part1(input):
    digits_count = { '1': 0, '4': 0, '7': 0, '8': 0 }
    for line in input:
        for digit in line.split(' | ').pop().split(' '):
            for key in EASY_DIGITS_LENGTH_MAP.keys():
                if len(digit) == EASY_DIGITS_LENGTH_MAP[key]:
                    digits_count[key] += 1

    return digits_count['1'] + digits_count['4'] + digits_count['7'] + digits_count['8']

def part2(input):
    sum = 0
    for line in input:
        parsed_line = line.split(' | ')
        four_digit_output_value = parsed_line.pop().split(' ')
        unique_signal_patterns = parsed_line.pop().split(' ')
        digits_map = find_digits_map(unique_signal_patterns)
        multiplier = 1000
        output = 0
        for digit in four_digit_output_value:
            output += digits_map[''.join(sorted(digit))] * multiplier
            multiplier /= 10
        sum += output
    return int(sum)
