namespace Test

open Microsoft.VisualStudio.TestTools.UnitTesting
open Program

[<TestClass>]
type TestClass () =

  [<TestMethod>]
  member this.ShouldReturnFreeSidesCount () =
    let expected = 10;
    let actual = BoilingBoulders.part1 "1,1,1\n2,1,1"
    Assert.AreEqual(expected, actual)

    let expected = 64;
    let actual = BoilingBoulders.part1 "2,2,2\n1,2,2\n3,2,2\n2,1,2\n2,3,2\n2,2,1\n2,2,3\n2,2,4\n2,2,6\n1,2,5\n3,2,5\n2,1,5\n2,3,5"
    Assert.AreEqual(expected, actual)

  [<TestMethod>]
  member this.ShouldReturnExternalFreeSidesCount() =
    let expected = 58;
    let actual = BoilingBoulders.part2 "2,2,2\n1,2,2\n3,2,2\n2,1,2\n2,3,2\n2,2,1\n2,2,3\n2,2,4\n2,2,6\n1,2,5\n3,2,5\n2,1,5\n2,3,5"
    Assert.AreEqual(expected, actual)