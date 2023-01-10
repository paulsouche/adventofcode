import re


class ProbeLauncher:
    def __init__(self, raw_target):
        result = re.search('^target\sarea:\sx=(\d+)..(\d+),\sy=(-\d+)..(-\d+)$', raw_target)
        self.target_min_x = int(result.group(1))
        self.target_max_x = int(result.group(2))
        self.target_min_y = int(result.group(3))
        self.target_max_y = int(result.group(4))
        self.range_x = range(self.target_min_x, self.target_max_x + 1)
        self.range_y = range(self.target_min_y, self.target_max_y + 1)

    def launch_probe(self, speed_x, speed_y):
        x = 0
        y = 0
        max_y = 0
        while(True):
            x += speed_x
            y += speed_y
            if y > max_y:
                max_y = y
            if x > self.target_max_x or y < self.target_min_y:
                break
            if x in self.range_x and y in self.range_y:
                return max_y
            speed_x = 0 if speed_x == 0 else speed_x - 1 if speed_x > 0 else speed_x + 1
            speed_y -= 1

    def calc_min_x_speed(self):
        speed = 1
        target = self.target_min_x
        while target > 0:
            target -= speed
            speed += 1
        return speed - 1

    def find_max_reachable_y(self):
        min_speed_x = self.calc_min_x_speed()
        max_speed_y = abs(self.target_min_y)
        max_y = 0
        for speed_y in range(1, max_speed_y):
            hit = self.launch_probe(min_speed_x, speed_y)
            if hit:
                if hit > max_y:
                    max_y = hit

        return max_y

    def find_all_velocities(self):
        min_speed_x = self.calc_min_x_speed()
        max_speed_x = self.target_max_x + 1
        min_speed_y = self.target_min_y
        max_speed_y = abs(self.target_min_y)
        hits = []
        for speed_y in range(min_speed_y, max_speed_y):
            for speed_x in range(min_speed_x, max_speed_x):
                hit = self.launch_probe(speed_x, speed_y)
                if not hit == None:
                    hits.append(hit)

        return len(hits)


def part1(raw_target):
    probe_launcher = ProbeLauncher(raw_target)
    return probe_launcher.find_max_reachable_y()

def part2(raw_target):
    probe_launcher = ProbeLauncher(raw_target)
    return probe_launcher.find_all_velocities()
