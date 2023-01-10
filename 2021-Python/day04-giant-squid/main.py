import re

from bingo import part1, part2

input = open("input.txt", "r")

draws = list(map(int, input.readline().strip().split(',')))

raw_boards = []
line = input.readline()
while line:
    if line == '\n':
        board = []
    else:
        board.append(list(map(int, re.sub('\s+', ',', line.strip()).split(','))))

    line = input.readline()
    if line == '\n':
        raw_boards.append(board)
raw_boards.append(board)

print(part1(draws, raw_boards))
print(part2(draws, raw_boards))
input.close()
