class Path:
    def __init__(self, start_coordinates, end_coordinates, risk):
        self.start_coordinates = start_coordinates
        self.end_coordinates = end_coordinates
        self.risk = risk

class Cave:
    def get_path(start_coordinates, end_coordinates, raw_cave):
        x, y = list(map(int, end_coordinates.split(',')))
        return Path(start_coordinates, end_coordinates, int(raw_cave[y][x]))

    def find_lowest_risk_path(paths):
        min_risk_path = Path('0,0', '', float('inf'))
        for path in paths:
            if path.risk < min_risk_path.risk:
                min_risk_path = path

        return min_risk_path

    def __init__(self, raw_cave):
        self.graph = {}
        self.max_x = len(raw_cave[0]) - 1
        self.max_y = len(raw_cave) - 1
        self.start = '0,0'
        self.goal = f'{self.max_x},{self.max_y}'
        for y,line in enumerate(raw_cave):
            for x,risk in enumerate(line):
                self.graph[f'{x},{y}'] = list(map(lambda coord: Cave.get_path(f'{x},{y}', coord, raw_cave), self.get_adjacent_cells(x, y)))

    def get_adjacent_cells(self, x, y):
        adjacent_cells = []
        if y > 0:
            adjacent_cells.append(f'{x},{y - 1}') # Top
        if x < self.max_x:
            adjacent_cells.append(f'{x + 1},{y}') # Right
        if y < self.max_y:
            adjacent_cells.append(f'{x},{y + 1}') # Bottom
        if x > 0:
            adjacent_cells.append(f'{x - 1},{y}') # Left
        return adjacent_cells

    def find_lowest_risk(self):
        steps = {}
        steps[self.start] = Path(self.start, self.start, 0)
        paths = self.graph[self.start]

        while True:
            lowest_risk_path = Cave.find_lowest_risk_path(paths)
            steps[lowest_risk_path.end_coordinates] = Path(lowest_risk_path.start_coordinates, lowest_risk_path.end_coordinates, lowest_risk_path.risk)

            if lowest_risk_path.end_coordinates == self.goal:
                break

            paths = list(filter(lambda  path: path.end_coordinates not in steps, paths + list(map(lambda path: Path(path.start_coordinates, path.end_coordinates, path.risk + lowest_risk_path.risk), self.graph[lowest_risk_path.end_coordinates]))))

        return steps[self.goal].risk

def expand_raw_cave(raw_cave):
    max_x = len(raw_cave[0])
    max_y = len(raw_cave)
    expanded_raw_cave = []
    for j in range(5):
        for y in range(max_y):
            line = ''
            for i in range(5):
                for x in range(max_x):
                    risk = int(raw_cave[y][x]) + i + j
                    while risk > 9:
                        risk -= 9
                    line += str(risk)
            expanded_raw_cave.append(line)

    return expanded_raw_cave

def part1(raw_cave):
    cave = Cave(raw_cave)
    return cave.find_lowest_risk()

def part2(raw_cave):
    cave = Cave(expand_raw_cave(raw_cave))
    return cave.find_lowest_risk()
