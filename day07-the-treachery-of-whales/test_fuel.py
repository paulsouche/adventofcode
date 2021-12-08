import unittest

from fuel import part1, part2


class TestFuel(unittest.TestCase):
    def test_part1(self):
        self.assertEqual(part1([16,1,2,0,4,2,7,1,2,14]), 37)

    def test_part2(self):
        self.assertEqual(part2([16,1,2,0,4,2,7,1,2,14]), 168)
