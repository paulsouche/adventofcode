open System.IO

module GrovePositioningSystem =
  let private parse (input:string) = input.Split('\n') |> Array.toList |> List.map int64

  let private coordinates list =
    let zeroIndex = List.findIndex (fun value -> value = 0L) list

    [ 1000; 2000; 3000 ]
    |> List.sumBy (fun i -> list[(zeroIndex + i) % list.Length])

  let private move (list: (int * int64) list) (i, value) =
    let index = list |> List.findIndex (fun item -> item = (i, value))
    let front, back = List.splitAt index list
    let newList = front @ (List.tail back)
    let length = newList.Length |> int64
    let target = int ((length + ((int64 index) + value) % length) % length)
    newList |> List.insertAt target (i, value)

  let part1 (text: string) =
    text
      |> parse
      |> List.indexed
      |> fun list -> list |> List.fold move list
      |> List.map snd
      |> coordinates

  let part2 (text: string) =
    text
      |> parse
      |> List.map (fun x -> x * 811589153L)
      |> List.indexed
      |> fun list ->
          let tenTimes = Seq.init 10 (fun _ -> list) |> List.concat
          tenTimes |> List.fold move list
      |> List.map snd
      |> coordinates

let input = File.ReadAllText(Path.Combine(__SOURCE_DIRECTORY__, "input.txt"))

let resultPart1 = GrovePositioningSystem.part1 input
printfn "Result part1: %i" resultPart1

let resultPart2 = GrovePositioningSystem.part2 input
printfn "Result part2: %i" resultPart2
