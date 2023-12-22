using Namespace;

namespace Test;

[TestClass]
public class Day22Test
{
    [TestMethod]
    public void ItShouldReturnPart1Result()
    {
        var part1 = new Part1("1,0,1~1,2,1\n0,0,2~2,0,2\n0,2,3~2,2,3\n0,0,4~0,2,4\n2,0,5~2,2,5\n0,1,6~2,1,6\n1,1,8~1,1,9");

        Assert.AreEqual(5, part1.Solve());
    }

    [TestMethod]
    public void ItShouldReturnPart2Result()
    {
        var part2 = new Part2("1,0,1~1,2,1\n0,0,2~2,0,2\n0,2,3~2,2,3\n0,0,4~0,2,4\n2,0,5~2,2,5\n0,1,6~2,1,6\n1,1,8~1,1,9");

        Assert.AreEqual(7, part2.Solve());
    }
}
