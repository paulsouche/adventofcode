from game import part1, part2

input = open("input.txt", "r")
raw_players = list(map(lambda x: x.strip(), input.readlines()))
print(part1(raw_players))
print(part2(raw_players))
input.close()
