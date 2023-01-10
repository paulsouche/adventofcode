import unittest

from lava_tubes import part1, part2


class TestLavaTubes(unittest.TestCase):
    def test_part1(self):
        self.assertEqual(part1([
            '2199943210',
            '3987894921',
            '9856789892',
            '8767896789',
            '9899965678',
        ]), 15)

    def test_part2(self):
        self.assertEqual(part2([
            '2199943210',
            '3987894921',
            '9856789892',
            '8767896789',
            '9899965678',
        ]), 1134)
