from parser import part1, part2

input = open("input.txt", "r")
instructions = list(map(lambda x: x.strip(), input.readlines()))
print(part1(instructions))
print(part2(instructions))
input.close()
