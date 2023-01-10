from lanternfish import breed_lanternfishes

input = open("input.txt", "r")
fishes = list(map(int, input.readline().strip().split(',')))
print(breed_lanternfishes(fishes, 80))
print(breed_lanternfishes(fishes, 256))
input.close()
