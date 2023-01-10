import unittest

from snailfish import (add, explode, magnitude, parse, part1, part2, reduce,
                       split, sum)


class TestSnailfish(unittest.TestCase):
    def test_add(self):
        self.assertEqual(add(parse('[1,2]'), parse('[[3,4],5]')), parse('[[1,2],[[3,4],5]]'))

    def test_explode_1(self):
        number = parse('[[[[[9,8],1],2],3],4]')
        self.assertEqual(explode(number), True)
        self.assertEqual(number, parse('[[[[0,9],2],3],4]'))

    def test_explode_2(self):
        number = parse('[7,[6,[5,[4,[3,2]]]]]')
        self.assertEqual(explode(number), True)
        self.assertEqual(number, parse('[7,[6,[5,[7,0]]]]'))

    def test_explode_3(self):
        number = parse('[[6,[5,[4,[3,2]]]],1]')
        self.assertEqual(explode(number), True)
        self.assertEqual(number, parse('[[6,[5,[7,0]]],3]'))

    def test_explode_4(self):
        number = parse('[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]')
        self.assertEqual(explode(number), True)
        self.assertEqual(number, parse('[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]'))

    def test_explode_5(self):
        number = parse('[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]')
        self.assertEqual(explode(number), True)
        self.assertEqual(number, parse('[[3,[2,[8,0]]],[9,[5,[7,0]]]]'))

    def test_explode_6(self):
        number = parse('[[[[0,7],4],[7,[[8,4],9]]],[1,1]]')
        self.assertEqual(explode(number), True)
        self.assertEqual(number, parse('[[[[0,7],4],[15,[0,13]]],[1,1]]'))

    def test_split_1(self):
        number = parse('[[[[0,7],4],[15,[0,13]]],[1,1]]')
        self.assertEqual(split(number), True)
        self.assertEqual(number, parse('[[[[0,7],4],[[7,8],[0,13]]],[1,1]]'))

    def test_split_2(self):
        number = parse('[[[[0,7],4],[[7,8],[0,13]]],[1,1]]')
        self.assertEqual(split(number), True)
        self.assertEqual(number, parse('[[[[0,7],4],[[7,8],[0,[6,7]]]],[1,1]]'))

    def test_reduce_1(self):
        self.assertEqual(reduce(parse('[[[[[4,3],4],4],[7,[[8,4],9]]],[1,1]]')), parse('[[[[0,7],4],[[7,8],[6,0]]],[8,1]]'))

    def test_reduce_2(self):
        self.assertEqual(reduce(parse('[[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]],[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]]')), parse('[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,9],[5,0]]]]'))

    def test_magnitude_1(self):
        self.assertEqual(magnitude(parse('[[1,2],[[3,4],5]]')), 143)

    def test_magnitude_2(self):
        self.assertEqual(magnitude(parse('[[[[0,7],4],[[7,8],[6,0]]],[8,1]]')), 1384)

    def test_magnitude_3(self):
        self.assertEqual(magnitude(parse('[[[[1,1],[2,2]],[3,3]],[4,4]]')), 445)

    def test_magnitude_4(self):
        self.assertEqual(magnitude(parse('[[[[3,0],[5,3]],[4,4]],[5,5]]')), 791)

    def test_magnitude_5(self):
        self.assertEqual(magnitude(parse('[[[[5,0],[7,4]],[5,5]],[6,6]]')), 1137)

    def test_magnitude_6(self):
        self.assertEqual(magnitude(parse('[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]')), 3488)

    def test_sum_1(self):
        self.assertEqual(sum([
            '[1,1]',
            '[2,2]',
            '[3,3]',
            '[4,4]',
        ]), parse('[[[[1,1],[2,2]],[3,3]],[4,4]]'))

    def test_sum_2(self):
        self.assertEqual(sum([
            '[1,1]',
            '[2,2]',
            '[3,3]',
            '[4,4]',
            '[5,5]',
        ]), parse('[[[[3,0],[5,3]],[4,4]],[5,5]]'))

    def test_sum_3(self):
        self.assertEqual(sum([
            '[1,1]',
            '[2,2]',
            '[3,3]',
            '[4,4]',
            '[5,5]',
            '[6,6]',
        ]), parse('[[[[5,0],[7,4]],[5,5]],[6,6]]'))

    def test_sum_4(self):
        self.assertEqual(sum([
            '[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]',
            '[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]',
            '[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]',
            '[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]]',
            '[7,[5,[[3,8],[1,4]]]]',
            '[[2,[2,2]],[8,[8,1]]]',
            '[2,9]',
            '[1,[[[9,3],9],[[9,0],[0,7]]]]',
            '[[[5,[7,4]],7],1]',
            '[[[[4,2],2],6],[8,7]]',
        ]), parse('[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]'))

    def test_part1(self):
        self.assertEqual(part1([
            '[[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]',
            '[[[5,[2,8]],4],[5,[[9,9],0]]]',
            '[6,[[[6,2],[5,6]],[[7,6],[4,7]]]]',
            '[[[6,[0,7]],[0,9]],[4,[9,[9,0]]]]',
            '[[[7,[6,4]],[3,[1,3]]],[[[5,5],1],9]]',
            '[[6,[[7,3],[3,2]]],[[[3,8],[5,7]],4]]',
            '[[[[5,4],[7,7]],8],[[8,3],8]]',
            '[[9,3],[[9,9],[6,[4,9]]]]',
            '[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]',
            '[[[[5,2],5],[8,[3,7]]],[[5,[7,5]],[4,4]]]',
        ]), 4140)

    def test_part2(self):
        self.assertEqual(part2([
            '[[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]',
            '[[[5,[2,8]],4],[5,[[9,9],0]]]',
            '[6,[[[6,2],[5,6]],[[7,6],[4,7]]]]',
            '[[[6,[0,7]],[0,9]],[4,[9,[9,0]]]]',
            '[[[7,[6,4]],[3,[1,3]]],[[[5,5],1],9]]',
            '[[6,[[7,3],[3,2]]],[[[3,8],[5,7]],4]]',
            '[[[[5,4],[7,7]],8],[[8,3],8]]',
            '[[9,3],[[9,9],[6,[4,9]]]]',
            '[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]',
            '[[[[5,2],5],[8,[3,7]]],[[5,[7,5]],[4,4]]]',
        ]), 3993)
