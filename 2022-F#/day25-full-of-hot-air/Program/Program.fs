open System.IO
open System

module FullOfHotAir =
  let private parse (input: string) = input.Split('\n') |> Array.toList |> List.map (fun line -> line.ToCharArray() |> Array.toList)

  let private snafuDigit char =
    match char with
      | '-' -> -1
      | '=' -> -2
      | c -> int (c.ToString())

  let private toDecimal chars =
    chars
      |> List.indexed
      |> List.map (fun (i, char) -> snafuDigit char |> int64, chars.Length - 1 - i)
      |> List.sumBy (fun (value, exp) -> value * (pown 5L exp))

  let private toSnafu number =
    Seq.unfold
      (fun rem ->
        if rem = 0L then
          None
        else
          match rem % 5L with
            | 3L -> '=', (rem + 2L) / 5L
            | 4L -> '-', (rem + 1L) / 5L
            | x -> '0' + char x, rem / 5L
            |> Some)
        number
    |> Seq.rev
    |> Seq.toArray
    |> String

  let part1 (input: string) = input |> parse |> List.sumBy toDecimal |> toSnafu


let input = File.ReadAllText(Path.Combine(__SOURCE_DIRECTORY__, "input.txt"))

let resultPart1 = FullOfHotAir.part1 input
printfn "Result part1: %s" resultPart1
