import re


def part1(instructions):
    x = 0
    y = 0
    for instruction in instructions:
        result = re.search(r"(forward|down|up)\s(\d+)", instruction)
        command = result.group(1)
        if(command=='forward'):
            x += int(result.group(2))
        elif(command=='down'):
            y += int(result.group(2))
        elif(command=='up'):
            y -= int(result.group(2))
        else:
            print('Unknown command')
    return x * y

def part2(instructions):
    x = 0
    y = 0
    aim = 0
    for instruction in instructions:
        result = re.search(r"(forward|down|up)\s(\d+)", instruction)
        command = result.group(1)
        if(command=='forward'):
            x += int(result.group(2))
            y += aim * int(result.group(2))
        elif(command=='down'):
            aim += int(result.group(2))
        elif(command=='up'):
            aim -= int(result.group(2))
        else:
            print('Unknown command')
    return x * y
