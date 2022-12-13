open System.IO

module DistressSignal =
  type Node =
    | Value of int
    | Children of Node list

  let private pair list = match list with | a::b::_ -> (a,b) | _ -> failwith "List length should be 2"
  let private chars (line: string) = line.ToCharArray() |> Array.toList
  let private blocks (input: string) = input.Split("\n\n") |> Array.toList |> List.map (fun line -> line.Split('\n') |> Array.toList |> pair)
  let private lines (input: string) = input.Split("\n\n") |> Array.toList |> List.collect (fun line -> line.Split('\n') |> Array.toList)

  let private parse (line: string) =
    let rec parseRec charsList acc =
      match charsList, acc with
      | [], _ -> List.rev acc, []
      | '[' :: rest, _ ->
          let children, rest = parseRec rest []
          parseRec rest (Children children :: acc)
      | ']' :: rest, _ -> List.rev acc, rest
      | ',' :: rest, _ -> parseRec rest acc
      | _ ->
          let value = charsList |> List.takeWhile (fun char->
            match char with
              | '0' | '1' | '2' | '3' | '4' | '5' | '6' | '7' | '8' | '9' -> true
              | _ -> false)
          parseRec charsList[value.Length..charsList.Length] (Value (int (String.concat "" <| List.map string value)) :: acc)

    parseRec (line |> chars) [] |> fst |> List.head

  let rec private compare l r =
    match l, r with
    | Value vl, Value vr -> vl.CompareTo vr
    | Value _, Children _ -> compare (Children [ l ]) r
    | Children _, Value _ -> compare l (Children [ r ])
    | Children cl, Children cr ->
        let minLength = min cl.Length cr.Length

        List.zip (List.take minLength cl) (List.take minLength cr)
        |> List.map (fun (x, y) -> compare x y)
        |> List.skipWhile (fun c -> c = 0)
        |> List.tryHead
        |> function
            | Some c -> c
            | None -> (List.length cl).CompareTo(List.length cr)


  let part1 (input: string) =
    input
      |> blocks
      |> List.map (fun (line1, line2) -> parse line1, parse line2)
      |> List.mapi (fun i (l, r) -> i + 1, compare l r)
      |> List.filter (fun (_, c) -> c < 0)
      |> List.sumBy fst

  let part2 (input: string) =
    let divider1 = parse "[[2]]"
    let divider2 = parse "[[6]]"
    input
      |> lines
      |> List.map parse
      |> List.append [divider1; divider2]
      |> List.sortWith compare
      |> List.indexed
      |> List.filter (fun (_, c) -> c = divider1 || c = divider2)
      |> List.map (fun (i, _) -> i + 1)
      |> List.reduce (*)


let input = File.ReadAllText(Path.Combine(__SOURCE_DIRECTORY__, "input.txt"))

let resultPart1 = DistressSignal.part1 input
printfn "Result part1: %i" resultPart1

let resultPart2 = DistressSignal.part2 input
printfn "Result part2: %i" resultPart2
