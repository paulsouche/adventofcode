using Namespace;

namespace Test;

[TestClass]
public class Day20Test
{
    [TestMethod]
    public void ItShouldReturnPart1ResultExample1()
    {
        var part1 = new Part1("broadcaster -> a, b, c\n%a -> b\n%b -> c\n%c -> inv\n&inv -> a");

        Assert.AreEqual(32000000, part1.Solve());
    }

    [TestMethod]
    public void ItShouldReturnPart1ResultExample2()
    {
        var part1 = new Part1("broadcaster -> a\n%a -> inv, con\n&inv -> b\n%b -> con\n&con -> output");

        Assert.AreEqual(11687500, part1.Solve());
    }
}
