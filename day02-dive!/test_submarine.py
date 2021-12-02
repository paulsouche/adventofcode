import unittest

from submarine import part1, part2


class TestSubmarine(unittest.TestCase):
     def test_part1(self):
          self.assertEqual(part1([
              'forward 5',
              'down 5',
              'forward 8',
              'up 3',
              'down 8',
              'forward 2',
          ]), 150)

     def test_part2(self):
          self.assertEqual(part2([
              'forward 5',
              'down 5',
              'forward 8',
              'up 3',
              'down 8',
              'forward 2',
          ]), 900)
