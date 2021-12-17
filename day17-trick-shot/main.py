from probe import part1, part2

input = open("input.txt", "r")
raw_target = input.readline().strip()
print(part1(raw_target))
print(part2(raw_target))
input.close()
