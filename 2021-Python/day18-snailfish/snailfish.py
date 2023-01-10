import math


def parse(line):
    parsed = []
    level = 0
    current_number = ''
    delimiters = { '[', ']', ',' }
    for char in line:
        if not current_number == '' and char in delimiters:
            parsed.append([int(current_number), level])
            current_number = ''
        if char == '[':
            level += 1
        elif char == ']':
            level -= 1
        elif char == ',':
            continue
        else:
            current_number += char

    return parsed

def add(number1, number2):
    result = []
    result += list(map(lambda digit: [digit[0], digit[1] + 1], number1))
    result += list(map(lambda digit: [digit[0], digit[1] + 1], number2))
    return result

def explode(number):
    exploding_index = -1
    for i in range(len(number)):
        value, level = number[i]
        if level < 5:
            continue
        next_value, next_level = number[i + 1]
        if level == next_level:
            exploding_index = i
            break

    if exploding_index == -1:
        return False

    if exploding_index > 0:
        number[exploding_index - 1][0] += value
    if exploding_index < len(number) - 2:
        number[exploding_index + 2][0] += next_value
    number[exploding_index] = [0,4]
    number.remove(number[exploding_index + 1])
    return True

def split(number):
    splitting_index = -1
    for i in range(len(number)):
        value, level = number[i]
        if value > 9:
            splitting_index = i
            break

    if splitting_index == -1:
        return False

    number[splitting_index] = [math.floor(value / 2), level + 1]
    number.insert(splitting_index + 1, [math.ceil(value / 2), level + 1])
    return True

def reduce(number):
    while True:
        if explode(number):
            continue
        if split(number):
            continue
        return number

def sum(numbers):
    number = parse(numbers.pop(0))
    for number2 in numbers:
        number = reduce(add(number, parse(number2)))
    return number

def magnitude(number):
    while len(number) > 1:
        magnitude_index = -1
        for i in range(len(number)):
            value, level = number[i]
            next_value, next_level = number[i + 1]
            if level == next_level:
                magnitude_index = i
                break

        value = 3 * value + 2 * next_value
        number[magnitude_index] = [value, level - 1]
        number.remove(number[magnitude_index + 1])

    return number[0][0]

def part1(numbers):
    return magnitude(sum(numbers))

def part2(numbers):
    max_magnitude = 0
    for i, number in enumerate(numbers):
        for j in range(i+1, len(numbers)):
            number1 = parse(number)
            number2 = parse(numbers[j])
            sum_magnitude = magnitude(reduce(add(number1, number2)))
            if sum_magnitude > max_magnitude:
                max_magnitude = sum_magnitude
            sum_magnitude = magnitude(reduce(add(number2, number1)))
            if sum_magnitude > max_magnitude:
                max_magnitude = sum_magnitude
    return max_magnitude
