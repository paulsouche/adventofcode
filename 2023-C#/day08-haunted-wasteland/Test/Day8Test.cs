using Namespace;

namespace Test;

[TestClass]
public class Day8Test
{
    [TestMethod]
    public void ItShouldReturnPart1ResultExample1()
    {
        var part1 = new Part1("RL\n\nAAA = (BBB, CCC)\nBBB = (DDD, EEE)\nCCC = (ZZZ, GGG)\nDDD = (DDD, DDD)\nEEE = (EEE, EEE)\nGGG = (GGG, GGG)\nZZZ = (ZZZ, ZZZ)");

        Assert.AreEqual(2, part1.Solve());
    }

    [TestMethod]
    public void ItShouldReturnPart1ResultExample2()
    {
        var part1 = new Part1("LLR\n\nAAA = (BBB, BBB)\nBBB = (AAA, ZZZ)\nZZZ = (ZZZ, ZZZ)");

        Assert.AreEqual(6, part1.Solve());
    }

    [TestMethod]
    public void ItShouldReturnPart2Result()
    {
        var part2 = new Part2("LR\n\n11A = (11B, XXX)\n11B = (XXX, 11Z)\n11Z = (11B, XXX)\n22A = (22B, XXX)\n22B = (22C, 22C)\n22C = (22Z, 22Z)\n22Z = (22B, 22B)\nXXX = (XXX, XXX)");

        Assert.AreEqual(6, part2.Solve());
    }
}
