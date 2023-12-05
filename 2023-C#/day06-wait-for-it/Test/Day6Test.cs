using Namespace;

namespace Test;

[TestClass]
public class Day6Test
{
    [TestMethod]
    public void ItShouldReturnPart1Result()
    {
        var part1 = new Part1("Time:      7  15   30\nDistance:  9  40  200");

        Assert.AreEqual(288, part1.Solve());
    }

    [TestMethod]
    public void ItShouldReturnPart2Result()
    {
        var part2 = new Part2("Time:      7  15   30\nDistance:  9  40  200");

        Assert.AreEqual(71503, part2.Solve());
    }
}
