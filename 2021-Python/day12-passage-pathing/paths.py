import re


class Maze:
    def can_go_back(start, end):
        if start == 'start':
            return False
        if end == 'end':
            return False
        return True

    def __init__(self, raw_maze):
        self.maze = {}
        self.paths = []
        for path_line in raw_maze:
            start, end = path_line.split('-')
            if end == 'start':
                end = start
                start = 'start'
            if start == 'end':
                start = end
                end = 'end'
            if start in self.maze:
                self.maze[start].append(end)
            else:
                self.maze[start] = [end]

            if not Maze.can_go_back(start, end):
                continue

            if end in self.maze:
                self.maze[end].append(start)
            else:
                self.maze[end] = [start]

    def walk(self, pos, small_caves_count, prev_path = ''):
        if pos in prev_path and re.match('[a-z]+', pos):
            small_caves_count -= 1

        path = prev_path + pos + ','

        if not pos in self.maze:
            if pos == 'end':
                self.paths.append(path)
            return

        for next_pos in self.maze[pos]:
            if next_pos in path:
                if re.match('[a-z]+', next_pos):
                    if path.count(next_pos) >= small_caves_count:
                        continue
            self.walk(next_pos, small_caves_count, path)

    def find_possible_paths(self, small_caves_count = 1):
        self.paths = []
        self.walk('start', small_caves_count)
        return self.paths

def part1(raw_maze):
    maze = Maze(raw_maze)
    return len(maze.find_possible_paths())

def part2(raw_maze):
    maze = Maze(raw_maze)
    return len(maze.find_possible_paths(2))
