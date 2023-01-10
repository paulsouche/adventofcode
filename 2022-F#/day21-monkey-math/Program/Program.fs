open System.IO

module MonkeyMath =
  let private parse (input:string) =
    input.Split('\n')
      |> Array.map (fun line -> line.Split(": ") |> Array.collect (fun part -> part.Split(' ')))
      |> Array.map (fun arr -> Array.head arr, Array.tail arr)
      |> Map.ofArray

  let private calculate (map: Map<string, string array>) (resolvedNumbers: Map<string, int64>) =
    map
      |> Map.filter (fun key value -> not (resolvedNumbers |> Map.containsKey key) && (resolvedNumbers |> Map.containsKey value[0]) && (resolvedNumbers |> Map.containsKey value[2]))
      |> Map.map (fun _ value ->
        let a, b = resolvedNumbers[value[0]], resolvedNumbers[value[2]]

        match value[1] with
          | "+" -> a + b
          | "-" -> a - b
          | "*" -> a * b
          | "/" -> a / b
          | _ -> failwith "Invalid operation")
      |> Map.fold (fun acc key value -> acc |> Map.add key value) resolvedNumbers

  let private doTheMath map =
    map
      |> Map.filter (fun _ value -> Array.length value = 1)
      |> Map.map (fun _ value -> int64 value[0])
      |> Seq.unfold (fun resolvedNumbers ->
        let newMap = calculate map resolvedNumbers
        if Map.count resolvedNumbers = Map.count newMap then None else Some(newMap, newMap))
      |> Seq.last

  let setHumn inputMap value =
    inputMap
      |> Map.add "humn" [| string value |]
      |> doTheMath

  let findHumnThatSatisfiesEquality inputMap map leftRootKey rightRootKey =
    let low = 0L
    let high = 10_000_000_000_000L
    let valuesLow = setHumn inputMap low

    let compareFn =
      if (valuesLow[leftRootKey] < valuesLow[rightRootKey]) then compare
      else (fun a b -> compare b a)

    let rec binarySearch low high =
      let mid = low + ((high - low) / 2L)
      let values = setHumn map mid

      match compareFn values[leftRootKey] values[rightRootKey] with
        | n when n < 0 -> binarySearch (mid + 1L) high
        | n when n > 0 -> binarySearch low (mid - 1L)
        | _ -> mid

    binarySearch low high

  let part1 (text: string) =
    text
      |> parse
      |> doTheMath
      |> Map.find "root"

  let part2 (text: string) =
    let inputMap = text |> parse
    let leftRootKey, rightRootKey = inputMap["root"][0], inputMap["root"][2]
    let map = inputMap |> Map.remove "root" |> Map.remove "humn"
    findHumnThatSatisfiesEquality inputMap map leftRootKey rightRootKey

let input = File.ReadAllText(Path.Combine(__SOURCE_DIRECTORY__, "input.txt"))

let resultPart1 = MonkeyMath.part1 input
printfn "Result part1: %i" resultPart1

let resultPart2 = MonkeyMath.part2 input
printfn "Result part2: %i" resultPart2
