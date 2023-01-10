class SeaCucumberArea:
    def __init__(self, raw_sea_cucumbers):
        self.map = {}
        self.max_y = len(raw_sea_cucumbers) - 1
        self.max_x = len(raw_sea_cucumbers[0]) - 1
        for y, raw_sea_cucumbers_line in enumerate(raw_sea_cucumbers):
            for x, raw_sea_cucumber in enumerate(raw_sea_cucumbers_line):
                if raw_sea_cucumber == '>' or raw_sea_cucumber == 'v':
                    self.map[f'{y},{x}'] = raw_sea_cucumber

    def do_step(self):
        moved = 0
        to_delete = []

        moving_east_candidates = sorted(list(filter(lambda coords: self.map[coords] == '>' ,self.map.keys())))
        for candidate in moving_east_candidates:
            y, x = list(map(int, candidate.split(',')))
            new_x = x + 1 if x < self.max_x else 0
            key = f'{y},{new_x}'
            if not key in self.map:
                moved += 1
                self.map[key] = self.map[candidate]
                to_delete.append(candidate)

        for candidate in to_delete:
            del self.map[candidate]

        to_delete = []
        moving_south_candidates = sorted(list(filter(lambda coords: self.map[coords] == 'v' ,self.map.keys())))
        for candidate in moving_south_candidates:
            y, x = list(map(int, candidate.split(',')))
            new_y = y + 1 if y < self.max_y else 0
            key = f'{new_y},{x}'
            if not key in self.map:
                moved += 1
                self.map[key] = self.map[candidate]
                to_delete.append(candidate)

        for candidate in to_delete:
            del self.map[candidate]

        return moved

    def print(self):
        for y in range(self.max_y + 1):
            line = ''
            for x in range(self.max_x + 1):
                if f'{y},{x}' in self.map:
                    line += self.map[f'{y},{x}']
                else:
                    line += '.'
            print(line)


def part1(raw_sea_cucumbers):
    sea_cucumber_area = SeaCucumberArea(raw_sea_cucumbers)
    steps = 0
    while True:
        steps += 1
        moved = sea_cucumber_area.do_step()
        if moved == 0:
            break
    return steps
