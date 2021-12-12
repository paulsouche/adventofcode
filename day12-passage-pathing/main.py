from paths import part1, part2

input = open("input.txt", "r")
raw_maze = list(map(lambda x: x.strip(), input.readlines()))
print(part1(raw_maze))
print(part2(raw_maze))
input.close()
