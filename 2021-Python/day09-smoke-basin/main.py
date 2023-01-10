from lava_tubes import part1, part2

input = open("input.txt", "r")
tubes = list(map(lambda x: x.strip(), input.readlines()))
print(part1(tubes))
print(part2(tubes))
input.close()
