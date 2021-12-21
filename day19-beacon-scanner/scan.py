import math
import re

ROTATIONS = [
    lambda x,y,z: [x, y, z],
    lambda x,y,z: [y, z, x],
    lambda x,y,z: [z, x, y],
    lambda x,y,z: [-x, z, y],
    lambda x,y,z: [z, y, -x],
    lambda x,y,z: [y, -x, z],
    lambda x,y,z: [x, z, -y],
    lambda x,y,z: [z, -y, x],
    lambda x,y,z: [-y, x, z],
    lambda x,y,z: [x, -z, y],
    lambda x,y,z: [-z, y, x],
    lambda x,y,z: [y, x, -z],
    lambda x,y,z: [-x, -y, z],
    lambda x,y,z: [-y, z, -x],
    lambda x,y,z: [z, -x, -y],
    lambda x,y,z: [-x, y, -z],
    lambda x,y,z: [y, -z, -x],
    lambda x,y,z: [-z, -x, y],
    lambda x,y,z: [x, -y, -z],
    lambda x,y,z: [-y, -z, x],
    lambda x,y,z: [-z, x, -y],
    lambda x,y,z: [-x, -z, -y],
    lambda x,y,z: [-z, -y, -x],
    lambda x,y,z: [-y, -x, -z],
]

class Match:
    def __init__(self, scanner_id, beacon_id, votes):
        self.scanner_id = scanner_id
        self.beacon_id = beacon_id
        self.votes = votes

class Beacon:
    def __init__(self, id, scanner_id, raw_beacon):
        self.id = id
        self.scanner_id = scanner_id
        self.coordinates = list(map(int, raw_beacon.split(',')))
        self.distances = []
        self.matches = {}

    def add_votes_for_match(self, scanner_id, beacon_id, votes):
        if not scanner_id in self.matches:
            self.matches[scanner_id] = []
        self.matches[scanner_id].append(Match(scanner_id, beacon_id, votes))

class Scanner:
    def pythagore_distance(point_a, point_b):
        xa, ya, za = point_a
        xb, yb, zb = point_b
        return math.sqrt(math.pow(xa - xb, 2) + math.pow(ya - yb, 2) + math.pow(za - zb, 2))

    def manhattan_distance(point_a, point_b):
        xa, ya, za = point_a
        xb, yb, zb = point_b
        return abs(xa - xb) + abs(ya - yb) + abs(za - zb)

    def set_matches_overlappings_and_connections(scanners):
        counted = set()
        count = 0
        for i, scanner_a in enumerate(scanners):
            for beacon_a in scanner_a.beacons:
                for j in range(i + 1, len(scanners)):
                    scanner_b = scanners[j]
                    beacons_matched = 0
                    max_votes = 0
                    for beacon_b in scanner_b.beacons:
                        votes = len(list(filter(lambda distance: distance in beacon_b.distances, beacon_a.distances )))
                        if votes > 0:
                            beacon_a.add_votes_for_match(scanner_b.id, beacon_b.id, votes)
                            beacon_b.add_votes_for_match(scanner_a.id, beacon_a.id, votes)
                            beacons_matched += 1
                            max_votes = max(max_votes, votes)
                        if beacons_matched >= 12:
                            scanner_a.overlappings.add(scanner_b.id)
                            scanner_b.overlappings.add(scanner_a.id)

                    if not scanner_b.id in beacon_a.matches:
                        continue

                    matches = list(filter(lambda match: match.votes == max_votes, beacon_a.matches[scanner_b.id]))
                    beacon_a.matches[scanner_b.id] = matches

                    if not len(matches) == 1:
                        continue

                    match = matches[0]
                    match_key = f'{match.scanner_id}:{match.beacon_id}'
                    counted.add(match_key)
                    beacon_b = scanners[match.scanner_id].beacons[match.beacon_id]
                    scanner_a.add_connection(scanner_b.id, beacon_a.coordinates, beacon_b.coordinates)
                    scanner_b.add_connection(scanner_a.id, beacon_b.coordinates, beacon_a.coordinates)

                beacon_key = f'{beacon_a.scanner_id}:{beacon_a.id}'
                if not beacon_key in counted:
                    counted.add(beacon_key)
                    count += 1

        return count

    def find_largest_manhattan_distance(scanners):
        queue = [[0, ROTATIONS[0]]]
        visited = set()
        while len(queue) > 0 and len(visited) <= len(scanners):
            scanner_id, last_rotation = queue.pop(0)
            scanner = scanners[scanner_id]
            visited.add(scanner_id)
            for overlap_id in scanner.overlappings:
                if overlap_id in visited:
                    continue
                connection = scanner.connections[overlap_id]
                selected_rotation = None
                for rotation in ROTATIONS:
                    unique_differences = set()
                    for beacon_a_coords, beacon_b_coords in connection:
                        ax, ay, az = beacon_a_coords
                        sx, sy, sz = last_rotation(ax, ay, az)
                        bx, by, bz = beacon_b_coords
                        dx, dy, dz = rotation(bx, by, bz)
                        unique_differences.add(f'{sx - dx},{sy - dy},{sz - dz}')

                    if not len(unique_differences) == 1:
                        continue

                    sx, sy, sz = scanner.coordinates
                    dx, dy, dz = list(map(int, list(unique_differences).pop().split(',')))
                    scanners[overlap_id].coordinates = [sx + dx, sy + dy, sz + dz]
                    selected_rotation = rotation
                    break

                queue.append([overlap_id, selected_rotation])

        max_dist = 0
        for i, scanner_a in enumerate(scanners):
            for j in range(i+1, len(scanners)):
                scanner_b = scanners[j]
                max_dist = max(max_dist, Scanner.manhattan_distance(scanner_a.coordinates, scanner_b.coordinates))

        return max_dist

    def __init__(self, raw_scanner):
        self.id = int(re.search('^---\sscanner\s(\d+)\s---$', raw_scanner.pop(0)).group(1))
        self.coordinates = [0,0,0]
        self.overlappings = set()
        self.connections = {}
        self.beacons = []
        for i, raw_beacon in enumerate(raw_scanner):
            self.beacons.append(Beacon(i, self.id, raw_beacon))

        for i, beacon in enumerate(self.beacons):
            for j in range(i + 1, len(self.beacons)):
                other_beacon = self.beacons[j]
                distance = Scanner.pythagore_distance(beacon.coordinates, other_beacon.coordinates)
                beacon.distances.append(distance)
                other_beacon.distances.append(distance)

    def add_connection(self, scanner_id, beacon_a_coords, beacon_b_coords):
        if not scanner_id in self.connections:
            self.connections[scanner_id] = []

        self.connections[scanner_id].append([beacon_a_coords, beacon_b_coords])

def part1(raw_scanners):
    scanners = list(map(lambda raw_scanner: Scanner(raw_scanner.copy()), raw_scanners))
    return Scanner.set_matches_overlappings_and_connections(scanners)

def part2(raw_scanners):
    scanners = list(map(lambda raw_scanner: Scanner(raw_scanner.copy()), raw_scanners))
    Scanner.set_matches_overlappings_and_connections(scanners)
    return Scanner.find_largest_manhattan_distance(scanners)
