using Namespace;

namespace Test;

[TestClass]
public class Day1Test
{
    [TestMethod]
    public void ItShouldReturnPart1Result()
    {
        var part1 = new Part1("1abc2\npqr3stu8vwx\na1b2c3d4e5f\ntreb7uchet");

        Assert.AreEqual(142, part1.Solve());
    }

    [TestMethod]
    public void ItShouldReturnPart2Result()
    {
        var part2 = new Part2("two1nine\neightwothree\nabcone2threexyz\nxtwone3four\n4nineeightseven2\nzoneight234\n7pqrstsixteen");

        Assert.AreEqual(281, part2.Solve());
    }
}