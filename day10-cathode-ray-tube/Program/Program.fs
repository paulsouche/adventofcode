open System.IO

module CathodeRayTube =
  let private parse (input: string) = Seq.toList(input.Split '\n')

  let private cycles (instructions: list<string>) =
    let mutable X = 1
    let mutable signals = List.empty

    for instruction in instructions do
      match instruction with
      | "noop" ->
        signals <- X::signals
      | _ ->
        let command = instruction.Split(' ')
        signals <- X::signals
        signals <- X::signals
        X <- X + int command[1]

    signals |> List.rev

  let private signalStrength (signals: list<int>) =
    let mutable strength = 0

    for i = 1 to signals.Length do
      if ((i - 20) % 40) = 0 then strength <- strength + i * signals[i - 1]

    strength

  let private print (signals: list<int>) =
    let mutable line = ""
    let mutable printing = List.Empty
    for y = 0 to 5 do
      for x = 0 to 39 do
        let spitePos = signals[y * 40 + x]
        line <- line + if [spitePos - 1; spitePos; spitePos + 1] |> List.contains x then "#" else " "

      printing <- line::printing
      line <- ""

    printing |> List.rev |> String.concat "\n"

  let part1 (input: string) = input |> parse |> cycles |> signalStrength

  let part2 (input: string) = input |> parse |> cycles |> print

let input = File.ReadAllText(Path.Combine(__SOURCE_DIRECTORY__, "input.txt"))

let resultPart1 = CathodeRayTube.part1 input
printfn "Result part1: %i" resultPart1

let resultPart2 = CathodeRayTube.part2 input
printfn "Result part2: %s" ("\n" + resultPart2)
