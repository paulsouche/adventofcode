from origami import part1, part2

input = open("input.txt", "r")
coordinates = []
while True:
    line = input.readline()
    if line == '\n':
        break
    coordinates.append(line.strip())

instructions = []
line = input.readline()
while line:
    instructions.append(line.strip())
    line = input.readline()

print(part1(coordinates, instructions))
part2(coordinates, instructions)
input.close()
