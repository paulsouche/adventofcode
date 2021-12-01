import unittest

from sonar import part1, part2


class TestSonar(unittest.TestCase):
    def test_part1(self):
        self.assertEqual(part1([199, 200, 208, 210, 200, 207, 240, 269, 260, 263]), 7)

    def test_part2(self):
        self.assertEqual(part2([199, 200, 208, 210, 200, 207, 240, 269, 260, 263]), 5)
