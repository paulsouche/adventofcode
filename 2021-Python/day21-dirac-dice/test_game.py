import unittest

from game import part1, part2


class TestGame(unittest.TestCase):
    def test_part1(self):
        self.assertEqual(part1([
            'Player 1 starting position: 4',
            'Player 2 starting position: 8',
        ]), 739785)

    def test_part2(self):
        self.assertEqual(part2([
            'Player 1 starting position: 4',
            'Player 2 starting position: 8',
        ]), 444356092776315)
