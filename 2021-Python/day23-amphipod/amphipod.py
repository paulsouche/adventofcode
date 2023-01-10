import json
import math


class AmphipodSorter:
    def is_sorted(rooms):
        for i, room in enumerate(rooms):
            for amphipod in room:
                if amphipod == None:
                    return False

        for i, room in enumerate(rooms):
            for amphipod in room:
                if not i == amphipod:
                    return False
        return True

    def compute_state_key(hall, rooms):
        return f'{json.dumps(hall)}|{json.dumps(rooms)}'

    def __init__(self, raw_amphipods_lines):
        self.cost = 0
        self.minimum_cost = float('inf')
        self.states_costs = {}
        # None is a place where you can't go, False, an empty one
        self.hall = [None, None, False, None, False, None, False, None, False, None, None]
        # ###B#C#B#D### => [[1,0],[2,3],[1,2],[3,0]]
        #   #A#D#C#A#
        amphipods_map = { 'A': 0, 'B': 1, 'C': 2, 'D': 3 }
        self.rooms = [[],[],[],[]]
        for raw_amphipods_line in raw_amphipods_lines:
            room_ind = 0
            for char in raw_amphipods_line:
                if char in amphipods_map:
                    self.rooms[room_ind].append(amphipods_map[char])
                    room_ind += 1

    def try_sort_home(self, cost, hall, rooms):
        for hall_cell_ind, hall_cell in enumerate(hall):
            # If spot is empty or non reachable
            if hall_cell == None or (isinstance(hall_cell, bool) and hall_cell == False):
                continue

            amphipod = hall_cell

            # Check if the room is empty or filled with same amphipods
            room_is_available = True
            room_available_ind = None
            for room_ind, room_cell in enumerate(rooms[amphipod]):
                if not room_cell == None and not room_cell == amphipod:
                    room_is_available = False
                    break
                if room_cell == None:
                    room_available_ind = room_ind

            if not room_is_available:
                continue

            # Check if there is any path blocker
            path_is_clear = True
            # Entrance cell of the room
            home_cell_ind = amphipod * 2 + 2
            for cell_index in range(home_cell_ind, hall_cell_ind, 1 if home_cell_ind < hall_cell_ind else -1):
                if isinstance(hall[cell_index], int) and not isinstance(hall[cell_index], bool):
                    path_is_clear = False
                    break

            if not path_is_clear:
                continue

            new_hall = hall.copy()
            new_hall[hall_cell_ind] = None
            new_rooms = list(map(lambda room: room.copy(), rooms))
            new_rooms[amphipod][room_available_ind] = amphipod
            new_cost = int(cost + (abs(hall_cell_ind - home_cell_ind) + room_available_ind + 1) * math.pow(10, amphipod))
            self.do_step(new_cost, new_hall, new_rooms)

    def try_move_on_hall_cell(self, exit_cell_ind, hall_cell_ind, room_ind, depth, cost, hall, rooms):
        new_hall = hall.copy()
        new_hall[hall_cell_ind] = rooms[room_ind][depth]
        new_rooms = list(map(lambda room: room.copy(), rooms))
        new_rooms[room_ind][depth] = None
        new_cost = int(cost + (depth + 1 + abs(exit_cell_ind - hall_cell_ind)) * math.pow(10, rooms[room_ind][depth]))
        self.do_step(new_cost, new_hall, new_rooms)

    def try_all_hall_cell(self, cost, hall, rooms):
        for room_ind, room in enumerate(rooms):
            depth = 0
            # If all of the creatures in this room live here or room is empty, we can skip this room
            skip = True
            for room_cell in room:
                if not room_cell == room_ind and not room_cell == None:
                    skip = False
                if room_cell == None:
                    depth += 1

            if skip:
                continue

            # Exit cell of the room
            exit_cell_ind = room_ind * 2 + 2

            # Left
            for hall_cell_ind in range(exit_cell_ind - 1, -1, -1):
                # Can't move on
                if isinstance(hall[hall_cell_ind], bool) and hall[hall_cell_ind] == False:
                    continue
                # Occupied => can't pass
                if not hall[hall_cell_ind] == None:
                    break
                self.try_move_on_hall_cell(exit_cell_ind,hall_cell_ind, room_ind, depth, cost, hall, rooms)

            # Right
            for hall_cell_ind in range(exit_cell_ind + 1, len(hall)):
                # Can't move on
                if isinstance(hall[hall_cell_ind], bool) and hall[hall_cell_ind] == False:
                    continue
                # Occupied => can't pass
                if not hall[hall_cell_ind] == None:
                    break
                self.try_move_on_hall_cell(exit_cell_ind,hall_cell_ind, room_ind, depth, cost, hall, rooms)

    def do_step(self, cost, hall, rooms):
        # Already found a clever path
        if cost > self.minimum_cost:
            return

        # The end is when every amphipod is in its room
        if AmphipodSorter.is_sorted(rooms):
            if cost < self.minimum_cost:
                self.minimum_cost = cost
            return

        # If we were in the same state but with a lower cost, we end
        state_key = AmphipodSorter.compute_state_key(hall, rooms)
        if state_key in self.states_costs:
            if self.states_costs[state_key] <= cost:
                return
        self.states_costs[state_key] = cost

        # See if any amphipod could go home
        self.try_sort_home(cost, hall, rooms)

        # Try to make amphipods go out on every available hall cell
        self.try_all_hall_cell(cost, hall, rooms)

    def find_minimum_cost(self):
        self.minimum_cost = float('inf')
        self.states_costs = {}
        self.do_step(self.cost, self.hall, self.rooms)
        return self.minimum_cost


def part1(raw_input):
    amphipod_sorter = AmphipodSorter(raw_input[2:4])
    return amphipod_sorter.find_minimum_cost()

def part2(raw_input):
    raw_input.insert(3, '#D#C#B#A#')
    raw_input.insert(4, '#D#B#A#C#')
    amphipod_sorter = AmphipodSorter(raw_input[2:6])
    return amphipod_sorter.find_minimum_cost()
