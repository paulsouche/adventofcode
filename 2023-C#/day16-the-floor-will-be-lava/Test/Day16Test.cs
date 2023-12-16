using Namespace;

namespace Test;

[TestClass]
public class Day16Test
{
    [TestMethod]
    public void ItShouldReturnPart1Result()
    {
        var part1 = new Part1(".|...\\....\n|.-.\\.....\n.....|-...\n........|.\n..........\n.........\\\n..../.\\\\..\n.-.-/..|..\n.|....-|.\\\n..//.|....");

        Assert.AreEqual(46, part1.Solve());
    }

    [TestMethod]
    public void ItShouldReturnPart2Result()
    {
        var part2 = new Part2(".|...\\....\n|.-.\\.....\n.....|-...\n........|.\n..........\n.........\\\n..../.\\\\..\n.-.-/..|..\n.|....-|.\\\n..//.|....");

        Assert.AreEqual(51, part2.Solve());
    }
}
