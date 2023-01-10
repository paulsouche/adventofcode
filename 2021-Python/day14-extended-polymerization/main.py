from polymer import polymerization

input = open("input.txt", "r")

raw_polymer = input.readline().strip()
input.readline()

recipes = []
line = input.readline()
while line:
    recipes.append(line.strip())
    line = input.readline()

print(polymerization(raw_polymer, recipes, 10))
print(polymerization(raw_polymer, recipes, 40))
input.close()
