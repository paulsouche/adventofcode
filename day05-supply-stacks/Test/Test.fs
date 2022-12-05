namespace Test

open Microsoft.VisualStudio.TestTools.UnitTesting
open Program

[<TestClass>]
type TestClass () =

  [<TestMethod>]
  member this.ShouldReturnWhichCrateWillBeOnTopForCrane9000 () =
    let expected = "CMZ"
    let actual = SupplyStacks.part1 "    [D]    \n[N] [C]    \n[Z] [M] [P]\n 1   2   3 \n\nmove 1 from 2 to 1\nmove 3 from 1 to 3\nmove 2 from 2 to 1\nmove 1 from 1 to 2"
    Assert.AreEqual(expected, actual)

  [<TestMethod>]
  member this.ShouldReturnWhichCrateWillBeOnTopForCrane9001 () =
    let expected = "MCD"
    let actual = SupplyStacks.part2 "    [D]    \n[N] [C]    \n[Z] [M] [P]\n 1   2   3 \n\nmove 1 from 2 to 1\nmove 3 from 1 to 3\nmove 2 from 2 to 1\nmove 1 from 1 to 2"
    Assert.AreEqual(expected, actual)
