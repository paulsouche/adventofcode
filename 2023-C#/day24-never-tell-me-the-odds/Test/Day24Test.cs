using Namespace;

namespace Test;

[TestClass]
public class Day24Test
{
    [TestMethod]
    public void ItShouldReturnPart1Result()
    {
        var part1 = new Part1("19, 13, 30 @ -2,  1, -2\n18, 19, 22 @ -1, -1, -2\n20, 25, 34 @ -2, -2, -4\n12, 31, 28 @ -1, -2, -1\n20, 19, 15 @  1, -5, -3");

        Assert.AreEqual(2, part1.Solve((7L, 27L)));
    }

    [TestMethod]
    public void ItShouldReturnPart2Result()
    {
        var part2 = new Part2("");

        Assert.AreEqual(0, part2.Solve());
    }
}
