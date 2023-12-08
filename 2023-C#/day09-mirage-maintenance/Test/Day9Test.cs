using Namespace;

namespace Test;

[TestClass]
public class Day9Test
{
    [TestMethod]
    public void ItShouldReturnPart1Result()
    {
        var part1 = new Part1("0 3 6 9 12 15\n1 3 6 10 15 21\n10 13 16 21 30 45");

        Assert.AreEqual(114, part1.Solve());
    }

    [TestMethod]
    public void ItShouldReturnPart2Result()
    {
        var part2 = new Part2("0 3 6 9 12 15\n1 3 6 10 15 21\n10 13 16 21 30 45");

        Assert.AreEqual(2, part2.Solve());
    }
}
