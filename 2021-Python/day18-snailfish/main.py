from snailfish import part1, part2

input = open("input.txt", "r")
numbers = list(map(lambda x: x.strip(), input.readlines()))
print(part1(numbers))
print(part2(numbers))
input.close()
