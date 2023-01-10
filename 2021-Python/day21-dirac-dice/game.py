import re


def build_dirac_dice():
    dice = []
    for i in range(1,4):
        for j in range(1,4):
            for k in range(1,4):
                dice.append([i,j,k])
    return dice

DIRAC_DICE = build_dirac_dice()

def parse_player_position(raw_player):
    return int(re.search('^Player\s\d+\sstarting\sposition:\s(\d+)$', raw_player).group(1)) - 1

def play_practice(position_player_1, position_player_2):
    rolls = 0
    players = [
        { 'position': position_player_1, 'score': 0 },
        { 'position': position_player_2, 'score': 0 },
    ]
    curr_player, other_player = players
    while True:
        for i in range(3):
            rolls += 1
            curr_player['position'] = (curr_player['position'] + rolls) % 10
        curr_player['score'] += curr_player['position'] + 1
        if curr_player['score'] >= 1000:
            return rolls * other_player['score']
        # Cannot find a way to make it work with reverse...
        players = [other_player, curr_player]
        curr_player, other_player = players

# Doing memoization with my poor skills in python (miss JavaScript so much)
def cached(func):
    cache = {}

    def inner(position_player_1, position_player_2, score_player_1, score_player_2):
        key = f'{position_player_1},{position_player_2},{score_player_1},{score_player_2}'
        if key in cache:
            return cache[key]

        result = func(position_player_1, position_player_2, score_player_1, score_player_2)
        cache[key] = result
        return result

    return inner

def play_with_dirac_dice(position_player_1, position_player_2, score_player_1, score_player_2):
    if score_player_1 >= 21:
        return [1, 0]
    if score_player_2 >= 21:
        return [0, 1]
    totalP1Wins = 0
    totalP2Wins = 0
    for roll in DIRAC_DICE:
        roll1, roll2, roll3 = roll
        position = (position_player_1 + roll1 + roll2 + roll3) % 10
        score = score_player_1 + position + 1
        P2Wins, P1Wins = cached_play_with_dirac_dice(position_player_2, position, score_player_2, score)
        totalP1Wins += P1Wins
        totalP2Wins += P2Wins
    return [totalP1Wins, totalP2Wins]

cached_play_with_dirac_dice = cached(play_with_dirac_dice)

def part1(raw_players):
    p1, p2 = list(map(lambda player: parse_player_position(player), raw_players))
    return play_practice(p1, p2)

def part2(raw_players):
    p1, p2 = list(map(lambda player: parse_player_position(player), raw_players))
    return max(cached_play_with_dirac_dice(p1, p2, 0, 0))
