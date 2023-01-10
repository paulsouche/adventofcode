open System.IO
open System.Collections.Generic

module BlizzardBasin =
  type Direction =
    | Up
    | Right
    | Down
    | Left

  let private directions() = [ Up; Right; Down; Left ]

  let private move direction (y, x) =
    match direction with
      | Up -> y - 1, x
      | Right -> y, x + 1
      | Down -> y + 1, x
      | Left -> y, x - 1

  let private moveBlizzardsBuilder (height, width) =
    Set.map (fun (y, x, direction) ->
      let (targetY, targetX) = move direction (y, x)
      if targetY < 1 then (height - 2, targetX, direction)
      elif targetX < 1 then (targetY, width - 2, direction)
      elif targetY > height - 2 then (1, targetX, direction)
      elif targetX > width - 2 then (targetY, 1, direction)
      else (targetY, targetX, direction))

  let private parse (input: string) =
    input.Split('\n')
      |> Array.toList
      |> List.map (fun line -> line.ToCharArray() |> Array.toList)
      |> fun grid ->
        let charToDirection =
          directions()
            |> List.zip [ '^'; '>'; 'v'; '<' ]
            |> Map.ofList

        let height = grid.Length
        let width = grid[0].Length

        let initialBlizzards = [
          for y in 0 .. height - 1 do
            for x in 0 .. width - 1 do
              let char = grid[y][x]
              if charToDirection |> Map.containsKey char then (y, x, charToDirection[char])] |> Set.ofList

        let moveBlizzard = moveBlizzardsBuilder (height, width)

        // Pre-calculate all blizzard positions
        let blizzards =
          Seq.unfold (fun b -> Some(b, moveBlizzard b)) initialBlizzards
            |> Seq.map (Set.map (fun (y, x, _) -> (y, x)))
            |> Seq.take 1000
            |> List.ofSeq

        ((height, width), blizzards)

  let private isInboundBuilder (height, width) (y, x) =
    (y, x) = (0, 1) || (y, x) = (height - 1, width - 2) || (y >= 1 && x >= 1 && y < height - 1 && x < width - 1)

  let private adjacent (y, x) =
    seq {
      yield (y, x)
      yield! directions() |> Seq.map (fun direction -> move direction (y, x))
    }

  let private crossBlizzardsBuilder ((height, width), blizzards: list<Set<int * int>>) time source target =
    let isInbound = isInboundBuilder (height, width)

    let visited = HashSet<int * (int * int)>()
    let queue = PriorityQueue<int * (int * int), int>()
    queue.Enqueue((time, source), 0)

    let rec visit () =
      match queue.Dequeue() with
        | (time, position) when position = target -> time
        | state when visited.Contains state -> visit ()
        | (time, position) ->
            visited.Add(time, position) |> ignore

            adjacent position
              |> Seq.filter (fun pos -> isInbound pos && not (blizzards[time + 1] |> Set.contains pos))
              |> Seq.iter (fun n ->
                if not (visited.Contains(time + 1, n)) then
                  queue.Enqueue((time + 1, n), time + 1))

            visit ()

    visit ()

  let part1 (input: string) =
    input
      |> parse
      |> fun ((height, width), blizzards) ->
        let crossBlizzards = crossBlizzardsBuilder ((height, width), blizzards)
        let source = 0, 1
        let target = height - 1, width - 2

        crossBlizzards 0 source target

  let part2 (input: string) =
    input
      |> parse
      |> fun ((height, width), blizzards) ->
        let crossBlizzards = crossBlizzardsBuilder ((height, width), blizzards)
        let source = 0, 1
        let target = height - 1, width - 2

        crossBlizzards (crossBlizzards (crossBlizzards 0 source target) target source) source target


let input = File.ReadAllText(Path.Combine(__SOURCE_DIRECTORY__, "input.txt"))

let resultPart1 = BlizzardBasin.part1 input
printfn "Result part1: %A" resultPart1

let resultPart2 = BlizzardBasin.part2 input
printfn "Result part2: %i" resultPart2
