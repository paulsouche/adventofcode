namespace Test

open Microsoft.VisualStudio.TestTools.UnitTesting
open Program

[<TestClass>]
type TestClass () =
  [<TestMethod>]
  member this.ShouldReturnMinMovesToAvoidBlizzards () =
    let expected = 18;
    let actual = BlizzardBasin.part1 "#.######\n#>>.<^<#\n#.<..<<#\n#>v.><>#\n#<^v^^>#\n######.#"
    Assert.AreEqual(expected, actual)

  [<TestMethod>]
  member this.ShouldReturnMinMovesToAvoidBlizzardsWithBackAMdForth () =
    let expected = 54;
    let actual = BlizzardBasin.part2 "#.######\n#>>.<^<#\n#.<..<<#\n#>v.><>#\n#<^v^^>#\n######.#"
    Assert.AreEqual(expected, actual)
