open System.IO

module CalorieCounting =
  let private calories (elf: string) = elf.Split '\n'
  let private caloriesSum (calories: string[]) = Array.sumBy int calories
  let private caloriesByElf (input: string) =
    input.Split "\n\n"
      |> Array.map calories
      |> Array.map caloriesSum

  let part1 (input: string) =
    input
      |> caloriesByElf
      |> Array.max

  let part2 (input: string) =
    input
      |> caloriesByElf
      |> Array.sortDescending
      |> Array.take 3
      |> Array.sum

let input = File.ReadAllText(Path.Combine(__SOURCE_DIRECTORY__, "input.txt"))

let resultPart1 = CalorieCounting.part1 input
printfn "Result part1: %i" resultPart1

let resultPart2 = CalorieCounting.part2 input
printfn "Result part2: %i" resultPart2
