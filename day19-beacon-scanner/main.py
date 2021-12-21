from scan import part1, part2

input = open("input.txt", "r")

raw_scanners = []
raw_scanner = []
line = input.readline().strip()
while line:
    raw_scanner.append(line)
    line = input.readline().strip()
    if line == '' or not line:
        raw_scanners.append(raw_scanner)
        raw_scanner = []
        line = input.readline().strip() if line == '' else line

print(part1(raw_scanners))
print(part2(raw_scanners))

input.close()
