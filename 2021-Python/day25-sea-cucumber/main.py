from sea_cucumber import part1

input = open("input.txt", "r")
raw_sea_cucumbers = list(map(lambda x: x.strip(), input.readlines()))
print(part1(raw_sea_cucumbers))
input.close()
