namespace Test

open Microsoft.VisualStudio.TestTools.UnitTesting
open Program

[<TestClass>]
type TestClass () =

  [<TestMethod>]
  member this.SouldReturnAssignmentsPairsCountWithOneRangeFullyContainingTheOther () =
    let expected = 2;
    let actual = CampCleanup.part1 "2-4,6-8\n2-3,4-5\n5-7,7-9\n2-8,3-7\n6-6,4-6\n2-6,4-8"
    Assert.AreEqual(expected, actual)

  [<TestMethod>]
  member this.SouldReturnAssignmentsPairsThatOverlaps () =
    let expected = 4;
    let actual = CampCleanup.part2 "2-4,6-8\n2-3,4-5\n5-7,7-9\n2-8,3-7\n6-6,4-6\n2-6,4-8"
    Assert.AreEqual(expected, actual)
