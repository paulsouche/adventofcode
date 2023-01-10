class Octopus:
    def __init__(self, energy):
        self.energy = energy

    def do_step(self):
        self.energy += 1

    def reset(self):
        self.energy = 0

    def flashes(self):
        return self.energy > 9


class OctopusGrid:
    def get_adjacent_cells(key):
        x, y = list(map(int, key.split(',')))
        return [
            f'{x},{y - 1}',     # Top
            f'{x + 1},{y - 1}', # Top Right
            f'{x + 1},{y}',     # Right
            f'{x + 1},{y + 1}', # Bottom Right
            f'{x},{y + 1}',     # Bottom
            f'{x - 1},{y + 1}', # Bottom Left
            f'{x - 1},{y}',     # Left
            f'{x - 1},{y - 1}', # Top Left
        ]

    def __init__(self, raw_grid):
        self.flashes = 0
        self.step_flashes = 0
        self.grid = {}
        for y, line in enumerate(raw_grid):
            for x, energy in enumerate(line):
                self.grid[f'{x},{y}'] = Octopus(int(energy))

    def do_step(self):
        for octopus in self.grid.values():
            octopus.do_step()
        return self

    def flash(self):
        self.step_flashes = 0
        already_flashed = {}
        flashing_keys = list(filter(lambda key: self.grid[key].flashes(), self.grid.keys()))
        for flashing_key in flashing_keys:
             already_flashed[flashing_key] = True

        while len(flashing_keys) > 0:
            key = flashing_keys.pop()
            for adjacent_key in OctopusGrid.get_adjacent_cells(key):
                if not adjacent_key in self.grid:
                    continue

                if self.grid[adjacent_key].flashes():
                    continue

                if adjacent_key in already_flashed:
                    continue

                adjacent_octopus = self.grid[adjacent_key]

                adjacent_octopus.do_step()

                if adjacent_octopus.flashes():
                    flashing_keys.append(adjacent_key)
                    already_flashed[adjacent_key] = True

        self.step_flashes = len(already_flashed.keys())
        self.flashes += self.step_flashes
        return self

    def reset(self):
        for octopus in list(filter(lambda octopus: octopus.flashes(), self.grid.values())):
            octopus.reset()

def part1(raw_grid, steps):
    octopus_grid = OctopusGrid(raw_grid)
    for step in range(steps):
        octopus_grid.do_step().flash().reset()

    return octopus_grid.flashes

def part2(raw_grid):
    octopus_grid = OctopusGrid(raw_grid)
    step = 0
    while True:
        step += 1
        octopus_grid.do_step().flash().reset()
        if octopus_grid.step_flashes == 100:
            break

    return step
