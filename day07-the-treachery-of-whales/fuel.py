def part1(crabs):
    min_crab = min(crabs)
    max_crab = max(crabs)

    min_fuel = float('inf')
    for x_pos in range(min_crab, max_crab):
        fuel = 0
        for crab in crabs:
            fuel += abs(crab - x_pos)
        if fuel < min_fuel:
            min_fuel = fuel
    return min_fuel

def part2(crabs):
    min_crab = min(crabs)
    max_crab = max(crabs)

    min_fuel = float('inf')
    for x_pos in range(min_crab, max_crab):
        fuel = 0
        for crab in crabs:
            for move in range(1, abs(crab - x_pos) + 1):
                fuel += move
        if fuel < min_fuel:
            min_fuel = fuel
    return min_fuel
