namespace Test

open Microsoft.VisualStudio.TestTools.UnitTesting
open Program

[<TestClass>]
type TestClass () =

  [<TestMethod>]
  member this.ShouldReturnGameScoreForPart1 () =
    let expected = 15;
    let actual = RockPaperScissors.part1 "A Y\nB X\nC Z"
    Assert.AreEqual(expected, actual)

  [<TestMethod>]
  member this.ShouldReturnGameScoreForPart2 () =
    let expected = 12;
    let actual = RockPaperScissors.part2 "A Y\nB X\nC Z"
    Assert.AreEqual(expected, actual)
