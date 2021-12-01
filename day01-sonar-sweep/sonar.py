def part1(scan):
    increases = 0
    pivot = scan[0]
    for i, depth in enumerate(scan, start=1):
        if depth > pivot:
            increases += 1
        pivot = depth
    return increases

def part2(scan):
    increases = 0
    pivot = scan[0] + scan[1] + scan[2]
    i = 1
    while i < len(scan) - 2:
        sum = scan[i] + scan[i + 1] + scan[i + 2]
        if sum > pivot:
            increases += 1
        pivot = sum
        i += 1
    return increases
