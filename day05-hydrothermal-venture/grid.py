import re


class Point:
    def __init__(self, x, y):
        self.x = x
        self.y = y

class Vertice:
    def parse_raw_vertice(raw_vertice):
        result = re.search(r"^(\d+),(\d+)\s->\s(\d+),(\d+)$", raw_vertice)
        return Vertice(Point(int(result.group(1)),int(result.group(2))), Point(int(result.group(3)),int(result.group(4))))

    def intersections_sum(vertices):
        venture_map = {}
        for vertice in vertices:
            for coordinate in vertice.coordinates():
                venture_map[coordinate] = 1 if not coordinate in venture_map else venture_map[coordinate] + 1
        return len(list(filter(lambda x: x >= 2, venture_map.values())))

    def __init__(self, a, b):
        self.a = a
        self.b = b

    def is_horizontal(self):
        return self.a.y == self.b.y

    def is_vertical(self):
        return self.a.x == self.b.x

    def coordinates(self):
        coordinates = []
        increment_x = 1 if self.a.x < self.b.x else -1 if self.a.x > self.b.x else 0
        increment_y = 1 if self.a.y < self.b.y else -1 if self.a.y > self.b.y else 0
        x = self.a.x
        y = self.a.y
        while True:
            coordinates.append(f'{x},{y}')
            if x == self.b.x and y == self.b.y:
                break
            else:
                x += increment_x
                y += increment_y

        return coordinates

def part1(raw_vertices):
    return Vertice.intersections_sum(list(filter(lambda v: v.is_horizontal() or v.is_vertical(), list(map(Vertice.parse_raw_vertice, raw_vertices)))))

def part2(raw_vertices):
    return Vertice.intersections_sum(list(map(Vertice.parse_raw_vertice, raw_vertices)))
