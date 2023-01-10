from sonar import part1, part2

input = open("input.txt", "r")
lines = list(map(int, input.readlines()))
print(part1(lines))
print(part2(lines))
input.close()
