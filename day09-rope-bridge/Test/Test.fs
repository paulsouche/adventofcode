namespace Test

open Microsoft.VisualStudio.TestTools.UnitTesting
open Program

[<TestClass>]
type TestClass () =

  [<TestMethod>]
  member this.ShouldCountVisitedPositionByTail () =
    let expected = 13
    let actual = RopeBridge.part1 "R 4\nU 4\nL 3\nD 1\nR 4\nD 1\nL 5\nR 2"
    Assert.AreEqual(expected, actual)

  [<TestMethod>]
  member this.ShouldCountVisitedPositionByTailForLongerStrings () =
    let expected = 1
    let actual = RopeBridge.part2 "R 4\nU 4\nL 3\nD 1\nR 4\nD 1\nL 5\nR 2"
    Assert.AreEqual(expected, actual)

    let expected = 36
    let actual = RopeBridge.part2 "R 5\nU 8\nL 8\nD 3\nR 17\nD 10\nL 25\nU 20"
    Assert.AreEqual(expected, actual)
