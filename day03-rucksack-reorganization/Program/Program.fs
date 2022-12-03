open System.IO

module RucksackReorganization =
  let private flip f a b = f b a
  let private rucksacks (input: string) = Seq.toList(input.Split '\n')
  let private compartiments (rucksack: string) =
    rucksack
      |> List.ofSeq
      |> List.splitAt (rucksack.Length / 2)
  let private commonItem (first: list<char>, second: list<char>) = List.find (flip List.contains second) first
  let private badge (rucksacks: list<string>) =
    rucksacks
      |> Seq.map Set
      |> Set.intersectMany
      |> Set.minElement
  let private priority (item: char) = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(item) + 1

  let part1 (input: string) =
    input
      |> rucksacks
      |> List.map compartiments
      |> List.map commonItem
      |> List.map priority
      |> List.sum

  let part2 (input: string) =
    input
      |> rucksacks
      |> List.chunkBySize 3
      |> List.map badge
      |> List.map priority
      |> List.sum

let input = File.ReadAllText(Path.Combine(__SOURCE_DIRECTORY__, "input.txt"))

let resultPart1 = RucksackReorganization.part1 input
printfn "Result part1: %i" resultPart1

let resultPart2 = RucksackReorganization.part2 input
printfn "Result part2: %i" resultPart2
