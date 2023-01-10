namespace Test

open Microsoft.VisualStudio.TestTools.UnitTesting
open Program

[<TestClass>]
type TestClass () =

  [<TestMethod>]
  member this.ShouldReturnHeightAfter2022Rocks () =
    let expected = 3068
    let actual = PyroclasticFlow.part1 ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>"
    Assert.AreEqual(expected, actual)

  [<TestMethod>]
  member this.ShouldReturnHeightAfter1_000_000_000Rocks () =
    let expected = 1_514_285_714_288L
    let actual = PyroclasticFlow.part2 ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>"
    Assert.AreEqual(expected, actual)
