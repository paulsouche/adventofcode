namespace Test

open Microsoft.VisualStudio.TestTools.UnitTesting
open Program

[<TestClass>]
type TestClass () =

  [<TestMethod>]
  member this.ShouldReturnOrderedPackets () =
    let expected = 24
    let actual = RegolithReservoir.part1 "498,4 -> 498,6 -> 496,6\n503,4 -> 502,4 -> 502,9 -> 494,9"
    Assert.AreEqual(expected, actual)

  [<TestMethod>]
  member this.ShouldReturnDecoderKey () =
    let expected: int = 93
    let actual = RegolithReservoir.part2 "498,4 -> 498,6 -> 496,6\n503,4 -> 502,4 -> 502,9 -> 494,9"
    Assert.AreEqual(expected, actual)
