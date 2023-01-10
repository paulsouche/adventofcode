import unittest

from probe import ProbeLauncher, part1, part2


class TestProbe(unittest.TestCase):
    def test_calc_min_x_speed(self):
        self.assertEqual(ProbeLauncher('target area: x=20..30, y=-10..-5').calc_min_x_speed(), 6)

    def test_launch_probe_1(self):
        self.assertEqual(ProbeLauncher('target area: x=20..30, y=-10..-5').launch_probe(7,2), 3)

    def test_launch_probe_2(self):
        self.assertEqual(ProbeLauncher('target area: x=20..30, y=-10..-5').launch_probe(6,3), 6)

    def test_launch_probe_3(self):
        self.assertEqual(ProbeLauncher('target area: x=20..30, y=-10..-5').launch_probe(9,0), 0)

    def test_launch_probe_4(self):
        self.assertEqual(ProbeLauncher('target area: x=20..30, y=-10..-5').launch_probe(17,-4), None)

    def test_launch_probe_5(self):
        self.assertEqual(ProbeLauncher('target area: x=20..30, y=-10..-5').launch_probe(6,10), None)

    def test_part1(self):
        self.assertEqual(part1('target area: x=20..30, y=-10..-5'), 45)

    def test_part2(self):
        self.assertEqual(part2('target area: x=20..30, y=-10..-5'), 112)
