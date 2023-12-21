using Namespace;

namespace Test;

[TestClass]
public class Day21Test
{
    [TestMethod]
    public void ItShouldReturnPart1Result()
    {
        var part1 = new Part1("...........\n.....###.#.\n.###.##..#.\n..#.#...#..\n....#.#....\n.##..S####.\n.##..#...#.\n.......##..\n.##.#.####.\n.##..##.##.\n...........");

        Assert.AreEqual(16, part1.Solve(6));
    }

    [TestMethod]
    public void ItShouldReturnPart2Result()
    {
        var part2 = new Part2("");

        Assert.AreEqual(0, part2.Solve());
    }
}
