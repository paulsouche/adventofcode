from risk import part1, part2

input = open("input.txt", "r")
raw_cave = list(map(lambda x: x.strip(), input.readlines()))
print(part1(raw_cave))
print(part2(raw_cave))
input.close()
