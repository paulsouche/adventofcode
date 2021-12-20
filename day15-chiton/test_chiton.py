import unittest

from risk import part1, part2


class TestRisk(unittest.TestCase):
    def test_part1(self):
        self.assertEqual(part1([
            '1163751742',
            '1381373672',
            '2136511328',
            '3694931569',
            '7463417111',
            '1319128137',
            '1359912421',
            '3125421639',
            '1293138521',
            '2311944581',
        ]), 40)

    def test_part2(self):
        self.assertEqual(part2([
            '1163751742',
            '1381373672',
            '2136511328',
            '3694931569',
            '7463417111',
            '1319128137',
            '1359912421',
            '3125421639',
            '1293138521',
            '2311944581',
        ]), 315)
