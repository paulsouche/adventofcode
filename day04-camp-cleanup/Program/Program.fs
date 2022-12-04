open System.IO

module CampCleanup =
  let private pair (a::b::_) = (a,b)
  let private check f =
    function
      | a,b when f a b -> true
      | a,b when f b a -> true
      | _ -> false
  let private contains (a1,b1) (a2,b2) = a1 >= a2 && b1 <= b2
  let private overlaps (_,b1) (a2,b2) = b1 >= a2 && b1 <= b2

  let private sections (section: string) = section.Split '-' |> Array.toList |> List.map int |> pair
  let private assignments (assignment: string) = assignment.Split ',' |> Array.toList |> List.map sections |> pair
  let private parse (input: string) = input.Split '\n' |> Array.toList |> List.map assignments

  let part1 (input: string) =
    input
      |> parse
      |> List.filter (check contains)
      |> List.length

  let part2 (input: string) =
    input
      |> parse
      |> List.filter (check overlaps)
      |> List.length

let input = File.ReadAllText(Path.Combine(__SOURCE_DIRECTORY__, "input.txt"))

let resultPart1 = CampCleanup.part1 input
printfn "Result part1: %i" resultPart1

let resultPart2 = CampCleanup.part2 input
printfn "Result part2: %i" resultPart2
