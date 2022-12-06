namespace Test

open Microsoft.VisualStudio.TestTools.UnitTesting
open Program

[<TestClass>]
type TestClass () =

  [<TestMethod>]
  member this.ShouldReturnFirstMarkerOf4Chars () =
    let expected = 7
    let actual = TuningTrouble.part1 "mjqjpqmgbljsphdztnvjfqwrcgsmlb"
    Assert.AreEqual(expected, actual)

    let expected = 5
    let actual = TuningTrouble.part1 "bvwbjplbgvbhsrlpgdmjqwftvncz"
    Assert.AreEqual(expected, actual)

    let expected = 6
    let actual = TuningTrouble.part1 "nppdvjthqldpwncqszvftbrmjlhg"
    Assert.AreEqual(expected, actual)

    let expected = 10
    let actual = TuningTrouble.part1 "nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg"
    Assert.AreEqual(expected, actual)

    let expected = 11
    let actual = TuningTrouble.part1 "zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw"
    Assert.AreEqual(expected, actual)

  [<TestMethod>]
  member this.ShouldReturnFirstMarkerOf14Chars () =
    let expected = 19
    let actual = TuningTrouble.part2 "mjqjpqmgbljsphdztnvjfqwrcgsmlb"
    Assert.AreEqual(expected, actual)

    let expected = 23
    let actual = TuningTrouble.part2 "bvwbjplbgvbhsrlpgdmjqwftvncz"
    Assert.AreEqual(expected, actual)

    let expected = 23
    let actual = TuningTrouble.part2 "nppdvjthqldpwncqszvftbrmjlhg"
    Assert.AreEqual(expected, actual)

    let expected = 29
    let actual = TuningTrouble.part2 "nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg"
    Assert.AreEqual(expected, actual)

    let expected = 26
    let actual = TuningTrouble.part2 "zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw"
    Assert.AreEqual(expected, actual)
