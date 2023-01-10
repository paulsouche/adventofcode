import unittest

from lanternfish import breed_lanternfishes


class TestLanternFish(unittest.TestCase):
    def test_breed_lanternfishes_18(self):
        self.assertEqual(breed_lanternfishes([3,4,3,1,2], 18), 26)

    def test_breed_lanternfishes_80(self):
        self.assertEqual(breed_lanternfishes([3,4,3,1,2], 80), 5934)

    def test_breed_lanternfishes_256(self):
        self.assertEqual(breed_lanternfishes([3,4,3,1,2], 256), 26984457539)
