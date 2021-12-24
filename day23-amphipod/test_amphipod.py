import unittest

from amphipod import part1, part2


class TestAmphipod(unittest.TestCase):
    def test_part1(self):
        self.assertEqual(part1([
            '#############',
            '#...........#',
            '###B#C#B#D###',
            '  #A#D#C#A#  ',
            '  #########  ',
        ]), 12521)

    def test_part2(self):
        self.assertEqual(part2([
            '#############',
            '#...........#',
            '###B#C#B#D###',
            '  #A#D#C#A#  ',
            '  #########  ',
        ]), 44169)
