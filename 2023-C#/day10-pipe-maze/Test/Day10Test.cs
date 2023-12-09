using Namespace;

namespace Test;

[TestClass]
public class Day10Test
{
    [TestMethod]
    public void ItShouldReturnPart1ResultExample1()
    {
        var part1 = new Part1(".....\n.S-7.\n.|.|.\n.L-J.\n.....");

        Assert.AreEqual(4, part1.Solve());
    }

    [TestMethod]
    public void ItShouldReturnPart1ResultExample2()
    {
        var part1 = new Part1("..F7.\n.FJ|.\nSJ.L7\n|F--J\nLJ...");

        Assert.AreEqual(8, part1.Solve());
    }

    [TestMethod]
    public void ItShouldReturnPart2ResultExample1()
    {
        var part2 = new Part2("...........\n.S-------7.\n.|F-----7|.\n.||.....||.\n.||.....||.\n.|L-7.F-J|.\n.|..|.|..|.\n.L--J.L--J.\n...........");

        Assert.AreEqual(4, part2.Solve());
    }

    [TestMethod]
    public void ItShouldReturnPart2ResultExample2()
    {
        var part2 = new Part2(".F----7F7F7F7F-7....\n.|F--7||||||||FJ....\n.||.FJ||||||||L7....\nFJL7L7LJLJ||LJ.L-7..\nL--J.L7...LJS7F-7L7.\n....F-J..F7FJ|L7L7L7\n....L7.F7||L7|.L7L7|\n.....|FJLJ|FJ|F7|.LJ\n....FJL-7.||.||||...\n....L---J.LJ.LJLJ...");

        Assert.AreEqual(8, part2.Solve());
    }

    [TestMethod]
    public void ItShouldReturnPart2ResultExample3()
    {
        var part2 = new Part2("FF7FSF7F7F7F7F7F---7\nL|LJ||||||||||||F--J\nFL-7LJLJ||||||LJL-77\nF--JF--7||LJLJ7F7FJ-\nL---JF-JLJ.||-FJLJJ7\n|F|F-JF---7F7-L7L|7|\n|FFJF7L7F-JF7|JL---7\n7-L-JL7||F7|L7F-7F7|\nL.L7LFJ|||||FJL7||LJ\nL7JLJL-JLJLJL--JLJ.L");

        Assert.AreEqual(10, part2.Solve());
    }
}
