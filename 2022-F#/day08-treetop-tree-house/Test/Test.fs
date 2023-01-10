namespace Test

open Microsoft.VisualStudio.TestTools.UnitTesting
open Program

[<TestClass>]
type TestClass () =

  [<TestMethod>]
  member this.SouldCountVisibleTrees () =
    let expected = 21
    let actual = TreeTopTreeHouse.part1 "30373\n25512\n65332\n33549\n35390"
    Assert.AreEqual(expected, actual)

  [<TestMethod>]
  member this.SouldReturnHighestScenicScore () =
    let expected = 8
    let actual = TreeTopTreeHouse.part2 "30373\n25512\n65332\n33549\n35390"
    Assert.AreEqual(expected, actual)
