import re


class Volume:
    def get_intersection(volume1, volume2):
        if volume1.max_x < volume2.min_x or volume1.min_x > volume2.max_x:
            return None
        if volume1.max_y < volume2.min_y or volume1.min_y > volume2.max_y:
            return None
        if volume1.max_z < volume2.min_z or volume1.min_z > volume2.max_z:
            return None

        return Volume(
            max(volume1.min_x, volume2.min_x),
            min(volume1.max_x, volume2.max_x),
            max(volume1.min_y, volume2.min_y),
            min(volume1.max_y, volume2.max_y),
            max(volume1.min_z, volume2.min_z),
            min(volume1.max_z, volume2.max_z),
        )

    def __init__(self, min_x, max_x, min_y, max_y, min_z, max_z):
        self.min_x = min_x
        self.max_x = max_x
        self.min_y = min_y
        self.max_y = max_y
        self.min_z = min_z
        self.max_z = max_z

    def get_number_of_cells(self):
        return (self.max_x - self.min_x + 1) * (self.max_y - self.min_y + 1) * (self.max_z - self.min_z + 1)

class RebootStep(Volume):
    def __init__(self, toggle, min_x, max_x, min_y, max_y, min_z, max_z):
        super().__init__(min_x, max_x, min_y, max_y, min_z, max_z)
        self.toggle = toggle

class Reactor:
    def parse_raw_reboot_step(raw_reboot_step):
        result = re.search('^(on|off)\sx=(-?\d+)\.\.(-?\d+),y=(-?\d+)\.\.(-?\d+),z=(-?\d+)\.\.(-?\d+)$', raw_reboot_step)
        return RebootStep(
            result.group(1),
            int(result.group(2)),
            int(result.group(3)),
            int(result.group(4)),
            int(result.group(5)),
            int(result.group(6)),
            int(result.group(7)),
        )

    def __init__(self, raw_reboot_steps):
        self.reboot_steps = list(map(lambda raw_step: Reactor.parse_raw_reboot_step(raw_step), raw_reboot_steps))

    def calc_lighted_on_cubes(self, steps):
        volumes = []
        removeds = []
        for reboot_step in steps:
            if reboot_step.toggle == 'on':
                intersections = []
                # Mark what was previously lighted on to remove
                for volume in volumes:
                    intersection = Volume.get_intersection(reboot_step, volume)
                    if intersection:
                        intersections.append(intersection)

                # Relight what was previously lighted off
                for removed in removeds:
                    intersection = Volume.get_intersection(reboot_step, removed)
                    if intersection:
                        volumes.append(intersection)

                removeds += intersections
                volumes.append(reboot_step)
            else:
                intersections = []
                # Turn what was previously lighted on
                for volume in volumes:
                    intersection = Volume.get_intersection(reboot_step, volume)
                    if intersection:
                        intersections.append(intersection)

                # Do not light off twice
                for removed in removeds:
                    intersection = Volume.get_intersection(reboot_step, removed)
                    if intersection:
                        volumes.append(intersection)

                removeds += intersections

        result = 0
        for volume in volumes:
            result += volume.get_number_of_cells()

        for removed in removeds:
            result -= removed.get_number_of_cells()

        return result

def is_step_in_initialization_procedure(step):
    if step.min_x < -50 or step.max_x > 50:
        return False
    if step.min_y < -50 or step.max_y > 50:
        return False
    if step.min_z < -50 or step.max_z > 50:
        return False

    return True

def part1(raw_reboot_steps):
    reactor = Reactor(raw_reboot_steps)
    return reactor.calc_lighted_on_cubes(list(filter(lambda step: is_step_in_initialization_procedure(step), reactor.reboot_steps)))

def part2(raw_reboot_steps):
    reactor = Reactor(raw_reboot_steps)
    return reactor.calc_lighted_on_cubes(reactor.reboot_steps)
