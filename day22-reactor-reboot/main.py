from reactor import part1, part2

input = open("input.txt", "r")
raw_reboot_steps = list(map(lambda x: x.strip(), input.readlines()))
print(part1(raw_reboot_steps))
print(part2(raw_reboot_steps))
input.close()
