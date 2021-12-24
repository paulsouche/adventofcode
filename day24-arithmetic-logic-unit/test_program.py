import unittest

from program import Program


class TestProgram(unittest.TestCase):
    def test_program_1(self):
        program = Program([
            'inp x',
            'mul x -1',
        ])
        self.assertEqual(program.run(['3']).memory['x'], -3)
        self.assertEqual(program.reset().run(['5']).memory['x'], -5)

    def test_program_2(self):
        program = Program([
            'inp z',
            'inp x',
            'mul z 3',
            'eql z x',
        ])
        self.assertEqual(program.run(['3','4']).memory['z'], 0)
        self.assertEqual(program.reset().run(['6', '18']).memory['z'], 1)

    def test_program_3(self):
        program = Program([
            'inp w',
            'add z w',
            'mod z 2',
            'div w 2',
            'add y w',
            'mod y 2',
            'div w 2',
            'add x w',
            'mod x 2',
            'div w 2',
            'mod w 2',
        ])
        self.assertEqual(program.run('1').memory, {
            'w': 0,
            'x': 0,
            'y': 0,
            'z': 1,
        })
        self.assertEqual(program.reset().run('2').memory, {
            'w': 0,
            'x': 0,
            'y': 1,
            'z': 0,
        })
        self.assertEqual(program.reset().run(['15']).memory, {
            'w': 1,
            'x': 1,
            'y': 1,
            'z': 1,
        })
