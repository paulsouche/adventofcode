open System.IO

module UnstableDiffusion =
  type Direction =
    | North
    | South
    | West
    | East

  let directions () = [ North; South; West; East ]

  let private parse (input: string) =
    input.Split('\n')
      |> Array.toList
      |> List.map (fun line -> line.ToCharArray() |> Array.toList)
      |> fun grid ->
        seq {
          for y in 0 .. grid.Length - 1 do
            for x in 0 .. grid[y].Length - 1 do
              if grid[y][x] = '#' then y, x
        }
        |> Set.ofSeq

  let private neighbors direction (y, x) =
    seq {
      for i in -1 .. 1 do
        match direction with
         | North -> y - 1, x + i
         | South -> y + 1, x + i
         | West -> y + i, x - 1
         | East -> y + i, x + 1
    }

  let private hasNeighbors elves (y, x) =
    seq {
      for dy in -1 .. 1 do
        for dx in -1 .. 1 do
          if dy <> 0 || dx <> 0 then (y + dy, x + dx)
    }
    |> Seq.exists (fun element -> elves |> Set.contains element)

  let private move direction (y, x) =
    match direction with
      | North -> y - 1, x
      | South -> y + 1, x
      | West -> y, x - 1
      | East -> y, x + 1

  let private step elves round =
    let sortedDirections =
      seq { round .. round + 3 }
      |> Seq.map (fun i -> directions()[i % 4])

    let proposals =
      elves
        |> Seq.filter (fun position -> hasNeighbors elves position)
        |> Seq.map (fun position ->
          sortedDirections
            |> Seq.map (fun direction -> direction, neighbors direction position)
            |> Seq.tryFind (fun (_, ns) -> not (Seq.exists (fun n -> Set.contains n elves) ns))
            |> Option.map (fun (direction, _) -> position, move direction position))
        |> Seq.filter Option.isSome
        |> Seq.map Option.get
        |> Seq.fold
            (fun map (source, target) ->
              Map.change
                target
                  (function
                    | None -> Some [ source ]
                    | Some l -> Some(source :: l))
                    map)
            Map.empty

    proposals
      |> Map.filter (fun _ v -> List.length v = 1)
      |> Map.map (fun _ v -> List.exactlyOne v)
      |> Map.fold (fun nextElves target source -> nextElves |> Set.remove source |> Set.add target) elves

  let private countEmpty elves =
    let ySet = elves |> Set.map fst
    let xSet = elves |> Set.map snd

    seq {
      for y in Seq.min ySet .. Seq.max ySet do
        for x in Seq.min xSet .. Seq.max xSet do
          (y, x)
    }
    |> Seq.filter (fun element -> not (elves |> Set.contains element))
    |> Seq.length

  let part1 (input: string) =
    let initialElves = input |> parse
    [ 0..9 ]
      |> Seq.fold step initialElves
      |> countEmpty

  let part2 (input: string) =
    let initialElves = input |> parse
    Seq.unfold (fun (elves, i) ->
      let nextElves = step elves i

      if nextElves = elves then None
      else Some(i + 1, (nextElves, i + 1)))
      (initialElves, 0)
    |> Seq.last
    |> (+) 1

let input = File.ReadAllText(Path.Combine(__SOURCE_DIRECTORY__, "input.txt"))

let resultPart1 = UnstableDiffusion.part1 input
printfn "Result part1: %i" resultPart1

let resultPart2 = UnstableDiffusion.part2 input
printfn "Result part2: %i" resultPart2
