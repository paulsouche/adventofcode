class TrenchMap:
    def get_cells_to_consider(x, y):
        return [
            f'{x - 1},{y - 1}', # Top Left
            f'{x},{y - 1}',     # Top
            f'{x + 1},{y - 1}', # Top Right
            f'{x - 1},{y}',     # Left
            f'{x},{y}',         # Center
            f'{x + 1},{y}',     # Right
            f'{x - 1},{y + 1}', # Bottom Left
            f'{x},{y + 1}',     # Bottom
            f'{x + 1},{y + 1}', # Bottom Right
        ]

    def __init__(self, raw_rule, raw_input):
        self.raw_rule = raw_rule
        self.input = {}
        self.min_y = 0
        self.max_y = len(raw_input)
        self.min_x = 0
        self.max_x = len(raw_input[0])
        for y, line in enumerate(raw_input):
            for x, column in enumerate(line):
                self.input[f'{x},{y}'] = column
        self.outside_value = '0'

    def print(self):
        for y in range(self.min_y, self.max_y):
            line = ''
            for x in range(self.min_x,self.max_x):
                key = f'{x},{y}'
                line += self.input[key]
            print(line)

    def enhance_cell(self, x, y):
        binary_digit = ''
        for coordinate in TrenchMap.get_cells_to_consider(x,y):
            if coordinate in self.input:
                binary_digit += '1' if self.input[coordinate] == '#' else '0'
            else:
                binary_digit += self.outside_value

        return self.raw_rule[int(binary_digit, 2)]

    def enhance_map(self):
        output = {}
        for y in range(self.min_y - 1, self.max_y + 1):
            for x in range(self.min_x - 1,self.max_x + 1):
                output[f'{x},{y}'] = self.enhance_cell(x,y)
        self.min_x -= 1
        self.max_x += 1
        self.min_y -= 1
        self.max_y += 1
        self.input = output
        if self.raw_rule[0] == '#' and self.outside_value == '0':
            self.outside_value = '1'
        elif self.raw_rule[511] == '.' and self.outside_value == '1':
            self.outside_value = '0'

        return self

    def light_pixels(self):
        return len(list(filter(lambda v: v == '#', self.input.values())))


def enhance(raw_rule, raw_input, times):
    trench_map = TrenchMap(raw_rule, raw_input)
    for time in range(times):
        trench_map.enhance_map()
    return trench_map.light_pixels()
