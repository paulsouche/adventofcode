def breed_lanternfishes(fishes, days):
    fish_map = {
        0: 0,
        1: 0,
        2: 0,
        3: 0,
        4: 0,
        5: 0,
        6: 0,
        7: 0,
        8: 0,
    }

    for fish in fishes:
        fish_map[fish] += 1

    for day in range(days):
        pregnants = fish_map[0]
        for age in range(8):
            fish_map[age] = fish_map[age + 1]
        fish_map[6] += pregnants
        fish_map[8] = pregnants

    return sum(fish_map.values())
