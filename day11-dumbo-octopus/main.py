from octopus import part1, part2

input = open("input.txt", "r")
raw_grid = list(map(lambda x: x.strip(), input.readlines()))
print(part1(raw_grid, 100))
print(part2(raw_grid))
input.close()
