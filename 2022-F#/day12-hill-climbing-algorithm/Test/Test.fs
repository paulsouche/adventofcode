namespace Test

open Microsoft.VisualStudio.TestTools.UnitTesting
open Program

[<TestClass>]
type TestClass () =

  [<TestMethod>]
  member this.ShouldReturnTargetDistance () =
    let expected = 31
    let actual = HillClimbingAlgorithm.part1 "Sabqponm\nabcryxxl\naccszExk\nacctuvwj\nabdefghi"
    Assert.AreEqual(expected, actual)

  [<TestMethod>]
  member this.ShouldReturnMinTargetDistanceFromAnyStartingPoint () =
    let expected = 29
    let actual = HillClimbingAlgorithm.part2 "Sabqponm\nabcryxxl\naccszExk\nacctuvwj\nabdefghi"
    Assert.AreEqual(expected, actual)
