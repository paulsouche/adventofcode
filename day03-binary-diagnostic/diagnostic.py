def analyse(lines):
    diagnostic = { '0': 0, '1': 0}

    for digit in lines:
        diagnostic[digit] += 1

    return diagnostic


def part1(scans):
    scans_len = len(scans[0])
    diagnostics = list(map(lambda i: analyse(list(map(lambda scan: scan[i], scans))), range(scans_len)))
    gamma = ''
    epsilon = ''

    for column in diagnostics:
        if column['0'] > column['1']:
            gamma += '0'
            epsilon += '1'
        else:
            gamma += '1'
            epsilon += '0'

    return int(gamma, 2) * int(epsilon, 2)

def part2(scans):
    scan_len = len(scans[0])
    oxygen = ''
    filtered_scans = scans
    for i in range(scan_len):
        diagnostic = analyse(list(map(lambda scan: scan[i], filtered_scans)))
        digit = '0' if diagnostic['0'] > diagnostic['1'] else '1'
        oxygen += digit
        filtered_scans = list(filter(lambda scan: scan[i] == digit, filtered_scans))
        if (len(filtered_scans) == 1):
            oxygen = filtered_scans[0]
            break

    co2 = ''
    filtered_scans = scans
    for i in range(scan_len):
        diagnostic = analyse(list(map(lambda scan: scan[i], filtered_scans)))
        digit = '1' if diagnostic['0'] > diagnostic['1'] else '0'
        co2 += digit
        filtered_scans = list(filter(lambda scan: scan[i] == digit, filtered_scans))
        if (len(filtered_scans) == 1):
            co2 = filtered_scans[0]
            break

    return int(oxygen, 2) * int(co2, 2)
