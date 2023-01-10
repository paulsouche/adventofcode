from decoder import part1, part2

input = open("input.txt", "r")
message = input.readline().strip()
print(part1(message))
print(part2(message))
input.close()
