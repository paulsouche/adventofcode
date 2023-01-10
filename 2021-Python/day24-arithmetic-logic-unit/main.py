from program import part1, part2

input = open("input.txt", "r")
raw_instructions = list(map(lambda x: x.strip(), input.readlines()))
print(part1(raw_instructions))
print(part2(raw_instructions))
input.close()
