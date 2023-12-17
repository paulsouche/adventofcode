using Namespace;

namespace Test;

[TestClass]
public class Day17Test
{
    [TestMethod]
    public void ItShouldReturnPart1Result()
    {
        var part1 = new Part1("2413432311323\n3215453535623\n3255245654254\n3446585845452\n4546657867536\n1438598798454\n4457876987766\n3637877979653\n4654967986887\n4564679986453\n1224686865563\n2546548887735\n4322674655533");

        Assert.AreEqual(102, part1.Solve());
    }

    [TestMethod]
    public void ItShouldReturnPart2ResultExample1()
    {
        var part2 = new Part2("2413432311323\n3215453535623\n3255245654254\n3446585845452\n4546657867536\n1438598798454\n4457876987766\n3637877979653\n4654967986887\n4564679986453\n1224686865563\n2546548887735\n4322674655533");

        Assert.AreEqual(94, part2.Solve());
    }

    [TestMethod]
    public void ItShouldReturnPart2ResultExample2()
    {
        var part2 = new Part2("111111111111\n999999999991\n999999999991\n999999999991\n999999999991");

        Assert.AreEqual(71, part2.Solve());
    }
}
