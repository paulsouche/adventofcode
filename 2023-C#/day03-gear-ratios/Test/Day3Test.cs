using Namespace;

namespace Test;

[TestClass]
public class Day3Test
{
    [TestMethod]
    public void ItShouldReturnPart1Result()
    {
        var part1 = new Part1("467..114..\n...*......\n..35..633.\n......#...\n617*......\n.....+.58.\n..592.....\n......755.\n...$.*....\n.664.598..");

        Assert.AreEqual(4361, part1.Solve());
    }

    [TestMethod]
    public void ItShouldReturnPart2Result()
    {
        var part2 = new Part2("467..114..\n...*......\n..35..633.\n......#...\n617*......\n.....+.58.\n..592.....\n......755.\n...$.*....\n.664.598..");

        Assert.AreEqual(467835, part2.Solve());
    }
}
