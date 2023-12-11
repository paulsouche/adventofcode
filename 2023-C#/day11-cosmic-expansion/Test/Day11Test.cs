using Namespace;

namespace Test;

[TestClass]
public class Day11Test
{
    [TestMethod]
    public void ItShouldReturnPart1Result()
    {
        var part1 = new Part1("...#......\n.......#..\n#.........\n..........\n......#...\n.#........\n.........#\n..........\n.......#..\n#...#.....");

        Assert.AreEqual(374, part1.Solve());
    }

    [TestMethod]
    public void ItShouldReturnPart2ResultExample1()
    {
        var part2 = new Part2("...#......\n.......#..\n#.........\n..........\n......#...\n.#........\n.........#\n..........\n.......#..\n#...#.....");

        Assert.AreEqual(1030, part2.Solve(10));
    }

    [TestMethod]
    public void ItShouldReturnPart2ResultExample2()
    {
        var part2 = new Part2("...#......\n.......#..\n#.........\n..........\n......#...\n.#........\n.........#\n..........\n.......#..\n#...#.....");

        Assert.AreEqual(8410, part2.Solve(100));
    }
}
