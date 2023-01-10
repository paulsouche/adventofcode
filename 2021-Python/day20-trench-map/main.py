from trench_map import enhance

input = open("input.txt", "r")

raw_rule = input.readline().strip()
input.readline()

raw_input = []
line = input.readline()
while line:
    raw_input.append(line.strip())
    line = input.readline()

print(enhance(raw_rule, raw_input, 2))
print(enhance(raw_rule, raw_input, 50))
input.close()
