using Namespace;

namespace Test;

[TestClass]
public class Day14Test
{
    [TestMethod]
    public void ItShouldReturnPart1Result()
    {
        var part1 = new Part1("O....#....\nO.OO#....#\n.....##...\nOO.#O....O\n.O.....O#.\nO.#..O.#.#\n..O..#O..O\n.......O..\n#....###..\n#OO..#....");

        Assert.AreEqual(136, part1.Solve());
    }

    [TestMethod]
    public void ItShouldReturnPart2Result()
    {
        var part2 = new Part2("O....#....\nO.OO#....#\n.....##...\nOO.#O....O\n.O.....O#.\nO.#..O.#.#\n..O..#O..O\n.......O..\n#....###..\n#OO..#....");

        Assert.AreEqual(64, part2.Solve());
    }
}
