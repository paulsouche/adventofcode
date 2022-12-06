open System.IO

module TuningTrouble =
  let firstMarker (input: string) (charCount: int) = (seq { charCount - 1..input.Length } |> Seq.find (fun i -> (Set.ofArray (input[i - charCount + 1..i].ToCharArray())).Count = charCount))  + 1

  let part1 (input: string) = firstMarker input 4

  let part2 (input: string) = firstMarker input 14


let input = File.ReadAllText(Path.Combine(__SOURCE_DIRECTORY__, "input.txt"))

let resultPart1 = TuningTrouble.part1 input
printfn "Result part1: %i" resultPart1

let resultPart2 = TuningTrouble.part2 input
printfn "Result part2: %i" resultPart2
