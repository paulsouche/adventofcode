import unittest

from sea_cucumber import SeaCucumberArea, part1


class TestSeaCucumber(unittest.TestCase):
    def test_sea_cucumber_area_1(self):
        sea_cucumber_area = SeaCucumberArea(['...>>>>>...'])
        sea_cucumber_area.do_step()
        self.assertEqual(sea_cucumber_area.map, {
            '0,3': '>',
            '0,4': '>',
            '0,5': '>',
            '0,6': '>',
            '0,8': '>',
        })
        sea_cucumber_area.do_step()
        self.assertEqual(sea_cucumber_area.map, {
            '0,3': '>',
            '0,4': '>',
            '0,5': '>',
            '0,7': '>',
            '0,9': '>',
        })

    def test_sea_cucumber_area_2(self):
        sea_cucumber_area = SeaCucumberArea([
            '..........',
            '.>v....v..',
            '.......>..',
            '..........',
        ])
        sea_cucumber_area.do_step()
        self.assertEqual(sea_cucumber_area.map, {
            '1,1': '>',
            '2,8': '>',
            '2,2': 'v',
            '2,7': 'v',
        })

    def test_sea_cucumber_area_3(self):
        sea_cucumber_area = SeaCucumberArea([
            '...>...',
            '.......',
            '......>',
            'v.....>',
            '......>',
            '.......',
            '..vvv..',
        ])
        sea_cucumber_area.do_step()
        self.assertEqual(sea_cucumber_area.map, {
            '0,2': 'v',
            '0,3': 'v',
            '0,4': '>',
            '2,0': '>',
            '3,0': 'v',
            '3,6': '>',
            '4,0': '>',
            '6,4': 'v',
        })
        sea_cucumber_area.do_step()
        self.assertEqual(sea_cucumber_area.map, {
            '4,0': 'v',
            '2,1': '>',
            '4,1': '>',
            '1,2': 'v',
            '1,3': 'v',
            '0,4': 'v',
            '0,5': '>',
            '3,6': '>',
        })

    def test_part1(self):
        self.assertEqual(part1([
            'v...>>.vv>',
            '.vv>>.vv..',
            '>>.>v>...v',
            '>>v>>.>.v.',
            'v>v.vv.v..',
            '>.>>..v...',
            '.vv..>.>v.',
            'v.v..>>v.v',
            '....v..v.>',
        ]), 58)
