class CavesMap:
    def calc_three_largest_basins(basins_sizes):
        sorted_basins_sizes = sorted(basins_sizes)
        return sorted_basins_sizes.pop() * sorted_basins_sizes.pop() * sorted_basins_sizes.pop()

    def get_adjacent_cells(x,y):
        return [
            f'{x},{y - 1}', # Top
            f'{x + 1},{y}', # Right
            f'{x},{y + 1}', # Bottom
            f'{x - 1},{y}', # Left
        ]

    def __init__(self, tubes):
       self.max_x = len(tubes[0])
       self.max_y = len(tubes)

       self.map = {}
       for y,tube in enumerate(tubes):
           for x,height in enumerate(tube):
               self.map[f'{x},{y}'] = int(height)

    def find_low_points(self):
        low_points = []
        for y in range(self.max_y):
            for x in range(self.max_x):
                height = self.map[f'{x},{y}']
                if any(key in self.map and self.map[key] <= height for key in CavesMap.get_adjacent_cells(x, y)):
                    continue
                low_points.append(f'{x},{y}')

        return low_points

    def calc_risk_levels(self, points):
        return sum(list(map(lambda point: self.map[point] + 1, points)))

    def find_basins_sizes(self):
        low_points = self.find_low_points()
        basins_sizes = []
        for low_point in low_points:
            size = 0
            basin = []
            already_walked = {}
            basin.append(low_point)
            while len(basin) > 0:
                point = basin.pop()
                height = self.map[point]
                size += 1
                x, y = list(map(int, point.split(',')))
                for key in list(filter(lambda key: self.is_higher_point_of_basin(key, height, already_walked), CavesMap.get_adjacent_cells(x, y))):
                    already_walked[key] = True
                    basin.append(key)

            basins_sizes.append(size)

        return basins_sizes

    def is_higher_point_of_basin(self, key, height, already_walked):
        return key in self.map and not key in already_walked and not self.map[key] >= 9 and self.map[key] > height

def part1(tubes):
    caves_map = CavesMap(tubes)
    return caves_map.calc_risk_levels(caves_map.find_low_points())

def part2(tubes):
    caves_map = CavesMap(tubes)
    return CavesMap.calc_three_largest_basins(caves_map.find_basins_sizes())
