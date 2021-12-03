import unittest

from diagnostic import part1, part2


class TestDiagnostic(unittest.TestCase):
    def test_part1(self):
        self.assertEqual(part1([
            '00100',
            '11110',
            '10110',
            '10111',
            '10101',
            '01111',
            '00111',
            '11100',
            '10000',
            '11001',
            '00010',
            '01010',
        ]), 198)

    def test_part2(self):
        self.assertEqual(part2([
            '00100',
            '11110',
            '10110',
            '10111',
            '10101',
            '01111',
            '00111',
            '11100',
            '10000',
            '11001',
            '00010',
            '01010',
        ]), 230)
