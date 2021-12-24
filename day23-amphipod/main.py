from amphipod import part1, part2

input = open("input.txt", "r")
raw_input = list(map(lambda x: x.strip(), input.readlines()))
print(part1(raw_input))
print(part2(raw_input))
input.close()
