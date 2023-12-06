using Namespace;

namespace Test;

[TestClass]
public class Day7Test
{
    [TestMethod]
    public void ItShouldReturnPart1Result()
    {
        var part1 = new Part1("32T3K 765\nT55J5 684\nKK677 28\nKTJJT 220\nQQQJA 483");

        Assert.AreEqual(6440, part1.Solve());
    }

    [TestMethod]
    public void ItShouldReturnPart2Result()
    {
        var part2 = new Part2("32T3K 765\nT55J5 684\nKK677 28\nKTJJT 220\nQQQJA 483");

        Assert.AreEqual(5905, part2.Solve());
    }
}