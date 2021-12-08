from fuel import part1, part2

input = open("input.txt", "r")
crabs = list(map(int, input.readline().strip().split(',')))
print(part1(crabs))
print(part2(crabs))
input.close()
