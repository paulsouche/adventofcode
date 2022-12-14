open System.IO

module RegolithReservoir =
  type Space =
    | Air
    | Rock
    | Sand

  let private pair list = match list with | a::b::_ -> (a,b) | _ -> failwith "List length should be 2"
  let private parse (input: string) =
    input.Split('\n')
      |> Array.toList
      |> List.map (fun line -> line.Split(" -> ") |> Array.toList |> List.map (fun coords -> coords.Split(',') |> Array.toList |> List.map int |> pair))

  let private maxX (coords: list<list<int * int>>) =
    coords
      |> List.map (List.map fst >> List.max)
      |> List.max

  let private maxY (coords: list<list<int * int>>) =
    coords
      |> List.map (List.map snd >> List.max)
      |> List.max

  let private rocks (coords: list<list<int * int>>) =
    let xLen = maxX coords
    let yLen = maxY coords
    let graph = Array2D.create (xLen + yLen) (yLen + 3) Air

    coords
      |> List.iter (fun paths ->
         paths
         |> List.pairwise
         |> List.iter (fun ((prevX, prevY), (ptX, ptY)) ->
              for x in min prevX ptX .. max prevX ptX do
                for y in min prevY ptY .. max prevY ptY do
                  Array2D.set graph x y Rock))

    graph

  let private buidRocks (input: string) = input |> parse |> rocks

  let private buidRocksWithFloor (input: string) =
    let coords = input |> parse
    let rocksWithFloor = coords |> rocks

    seq { 0 .. Array2D.length1 rocksWithFloor - 1 }
    |> Seq.iter (fun x -> Array2D.set rocksWithFloor x (maxY coords + 2) Rock)

    rocksWithFloor

  let rec private addSand (x, y) graph =
    if y >= (Array2D.length2 graph) - 1
       || graph[x, y] = Sand then
        None
    else
        match graph[x, y + 1], graph[x - 1, y + 1], graph[x + 1, y + 1] with
        | Air, _, _ -> addSand (x, y + 1) graph
        | _, Air, _ -> addSand (x - 1, y + 1) graph
        | _, _, Air -> addSand (x + 1, y + 1) graph
        | _ ->
            Array2D.set graph x y Sand
            Some(graph[x,y], graph)

  let part1 (input: string) =
    Seq.unfold (addSand (500, 0)) (buidRocks input) |> Seq.length

  let part2 (input: string) =
    Seq.unfold (addSand (500, 0)) (buidRocksWithFloor input) |> Seq.length


let input = File.ReadAllText(Path.Combine(__SOURCE_DIRECTORY__, "input.txt"))

let resultPart1 = RegolithReservoir.part1 input
printfn "Result part1: %i" resultPart1

let resultPart2 = RegolithReservoir.part2 input
printfn "Result part2: %i" resultPart2
