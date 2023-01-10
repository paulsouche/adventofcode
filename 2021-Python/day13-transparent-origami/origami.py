import re


class Origami:
    def __init__(self, coordinates):
        self.map = {}
        self.min_x = float('inf')
        self.min_y = float('inf')
        self.max_x = float('-inf')
        self.max_y = float('-inf')
        for coordinate in coordinates:
            self.map[coordinate] = '#'
            x, y = list(map(int, coordinate.split(',')))
            self.min_x = x if x < self.min_x else self.min_x
            self.min_y = y if y < self.min_y else self.min_y
            self.max_x = x if x > self.max_x else self.max_x
            self.max_y = y if y > self.max_y else self.max_y

    def fold(self, instruction):
        result = re.search('fold along (x|y)=(\d+)', instruction)
        coordinate = result.group(1)
        value = int(result.group(2))
        if coordinate == 'x':
            self.max_x = value - 1

        if coordinate == 'y':
            self.max_y = value - 1

        keys_to_delete = []
        keys_to_create = []
        for key in self.map.keys():
            x, y = list(map(int, key.split(',')))
            if coordinate == 'x' and x > value:
                keys_to_delete.append(key)
                keys_to_create.append(f'{abs(x - 2 * value)},{y}')
            if coordinate == 'y' and y > value:
                keys_to_delete.append(key)
                keys_to_create.append(f'{x},{abs(y - 2 * value)}')
        for key in keys_to_delete:
            del self.map[key]
        for key in keys_to_create:
            self.map[key] = '#'

    def print(self):
        for y in range(self.max_y + 1):
            line = ''
            for x in range(self.max_x + 1):
                key = f'{x},{y}'
                line += self.map[key] if key in self.map else '.'
            print(line)

def part1(coordinates, instructions):
    origami = Origami(coordinates)
    origami.fold(instructions[0])
    return len(origami.map.keys())

def part2(coordinates, instructions):
    origami = Origami(coordinates)
    for instruction in instructions:
        origami.fold(instruction)
    origami.print()

