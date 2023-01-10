open System.IO

module RockPaperScissors =
  exception InnerError of string
  let private rounds (input: string) = input.Split '\n'

  let rec private scorePart1 totalScore round =
    match round with
      // Rock Rock
      | "A X" -> totalScore + 4
      // Rock Paper
      | "A Y" -> totalScore + 8
      // Rock Scissors
      | "A Z" -> totalScore + 3
      // Paper Rock
      | "B X" -> totalScore + 1
      // Paper Paper
      | "B Y" -> totalScore + 5
      // Paper Scissors
      | "B Z" -> totalScore + 9
      // Scissors Rock
      | "C X" -> totalScore + 7
      // Scissors Paper
      | "C Y" -> totalScore + 2
      // Scissors Scissors
      | "C Z" -> totalScore + 6
      // Break
      | _ -> raise (InnerError "Invalid round")

  let rec private scorePart2 totalScore round =
    match round with
      // Rock Lose
      | "A X" -> totalScore + 3
      // Rock Draw
      | "A Y" -> totalScore + 4
      // Rock Win
      | "A Z" -> totalScore + 8
      // Paper Lose
      | "B X" -> totalScore + 1
      // Paper Draw
      | "B Y" -> totalScore + 5
      // Paper Win
      | "B Z" -> totalScore + 9
      // Scissors Lose
      | "C X" -> totalScore + 2
      // Scissors Draw
      | "C Y" -> totalScore + 6
      // Scissors Win
      | "C Z" -> totalScore + 7
      // Break
      | _ -> raise (InnerError "Invalid round")

  let part1 (input: string) =
    input
      |> rounds
      |> Array.fold scorePart1 0

  let part2 (input: string) =
    input
      |> rounds
      |> Array.fold scorePart2 0


let input = File.ReadAllText(Path.Combine(__SOURCE_DIRECTORY__, "input.txt"))

let resultPart1 = RockPaperScissors.part1 input
printfn "Result part1: %i" resultPart1

let resultPart2 = RockPaperScissors.part2 input
printfn "Result part2: %i" resultPart2
