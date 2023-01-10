import re

error_score_map = {
    ')': 3,
    ']': 57,
    '}': 1197,
    '>': 25137,
}

complete_score_map = {
    '(': 1,
    '[': 2,
    '{': 3,
    '<': 4,
}

valid_operations = ['()', '[]', '{}', '<>']

def part1(instructions):
    score = 0
    for instruction in instructions:
        openings = []
        for command in instruction:
            # openings
            if not command in error_score_map:
                openings.append(command)
                continue

            # closings
            last_opening = openings.pop()
            if f'{last_opening}{command}' in valid_operations:
                continue

            score += error_score_map[command]
            break

    return score


def part2(instructions):
    scores = []
    for instruction in instructions:
        score = 0
        openings = []
        for command in instruction:
            # openings
            if not command in error_score_map:
                openings.append(command)
                continue

            # closings
            last_opening = openings.pop()
            if f'{last_opening}{command}' in valid_operations:
                continue

            # Invalid line
            openings = []
            break

        # Completing the instruction
        while len(openings) > 0:
            score = score * 5 + complete_score_map[openings.pop()]

        # Saving score only for valid lines
        if score > 0:
            scores.append(score)

    return sorted(scores)[int((len(scores) - 1) / 2)]
