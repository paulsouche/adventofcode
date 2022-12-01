namespace Test

open Microsoft.VisualStudio.TestTools.UnitTesting
open Program

[<TestClass>]
type TestClass () =

  [<TestMethod>]
  member this.ShouldReturnMaxCalories () =
    let expected = 24000;
    let actual = CalorieCounting.part1 "1000\n2000\n3000\n\n4000\n\n5000\n6000\n\n7000\n8000\n9000\n\n10000"
    Assert.AreEqual(expected, actual)

  [<TestMethod>]
  member this.ShouldReturnMostCalories() =
    let expected = 45000;
    let actual = CalorieCounting.part2 "1000\n2000\n3000\n\n4000\n\n5000\n6000\n\n7000\n8000\n9000\n\n10000"
    Assert.AreEqual(expected, actual)
