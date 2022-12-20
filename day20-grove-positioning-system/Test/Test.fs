namespace Test

open Microsoft.VisualStudio.TestTools.UnitTesting
open Program

[<TestClass>]
type TestClass () =

  [<TestMethod>]
  member this.ShouldReturnGroveCoordinates () =
    let expected = 3L;
    let actual = GrovePositioningSystem.part1 "1\n2\n-3\n3\n-2\n0\n4"
    Assert.AreEqual(expected, actual)

  [<TestMethod>]
  member this.ShouldReturnGroveCoordinatesWithDecryptionKey () =
    let expected = 1623178306L;
    let actual = GrovePositioningSystem.part2 "1\n2\n-3\n3\n-2\n0\n4"
    Assert.AreEqual(expected, actual)
