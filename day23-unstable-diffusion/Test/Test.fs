namespace Test

open Microsoft.VisualStudio.TestTools.UnitTesting
open Program

[<TestClass>]
type TestClass () =
  [<TestMethod>]
  member this.ShouldReturnEmptyCellsAfter10Rounds () =
    let expected = 110;
    let actual = UnstableDiffusion.part1 "..............\n..............\n.......#......\n.....###.#....\n...#...#.#....\n....#...##....\n...#.###......\n...##.#.##....\n....#..#......\n..............\n..............\n.............."
    Assert.AreEqual(expected, actual)

  [<TestMethod>]
  member this.ShouldReturnRoundWithNoMoreMoves () =
    let expected = 20;
    let actual = UnstableDiffusion.part2 "..............\n..............\n.......#......\n.....###.#....\n...#...#.#....\n....#...##....\n...#.###......\n...##.#.##....\n....#..#......\n..............\n..............\n.............."
    Assert.AreEqual(expected, actual)
