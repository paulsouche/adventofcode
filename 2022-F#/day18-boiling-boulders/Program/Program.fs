open System.Collections.Generic
open System.IO
open System

module BoilingBoulders =
  let private coords list = match list with | a::b::c::_ -> (a,b,c) | _ -> failwith "List length should be 3"
  let private parse (input: string) = input.Split '\n' |> Array.toList |> List.map (fun line -> line.Split ',' |> Array.toList |> List.map int |> coords)

  let private sides() = [
    ( 1,  0,  0);
    (-1,  0,  0);
    ( 0,  1,  0);
    ( 0, -1,  0);
    ( 0,  0,  1);
    ( 0,  0, -1);
  ]

  let private countFreeSides boulders =
    let bouldersSet = boulders |> Set.ofList

    boulders
      |> List.map (fun (bx, by, bz) ->
        sides()
          |> List.map (fun (sx, sy, sz) -> (bx + sx, by + sy, bz + sz))
          |> List.filter (fun side -> not (bouldersSet.Contains side))
          |> List.length)
      |> List.sum

  let private countExternalSides boulders =
    let ((minx, miny, minz), (maxx, maxy, maxz)) =
      boulders
        |> List.fold (fun ((minx, miny, minz), (maxx, maxy, maxz)) (x, y, z) ->
          (((min minx (x - 1)), (min miny (y - 1)), (min minz (z - 1))), ((max maxx (x + 1)), (max maxy (y + 1)), (max maxz (z + 1)))))
          ((Int32.MaxValue, Int32.MaxValue, Int32.MaxValue), (Int32.MinValue, Int32.MinValue, Int32.MinValue))

    let xRange = [minx..maxx]
    let yRange = [miny..maxy]
    let zRange = [minz..maxz]

    let isInRange (x, y, z) = (xRange |> List.contains x) && (yRange |> List.contains y) && (zRange |> List.contains z)

    let bouldersSet = boulders |> Set.ofList
    let mutable outside = Set.empty
    let mutable visited = Set.empty
    let queue = Queue<int * int * int>()
    let startCell = (minx, miny, minz)
    queue.Enqueue startCell
    visited <- visited.Add startCell

    while queue.Count > 0 do
      let cell = queue.Dequeue()
      outside <- outside.Add cell
      let cx, cy, cz = cell
      let nexts =
        sides()
          |> List.map (fun (sx, sy, sz) -> (cx + sx, cy + sy, cz + sz))
          |> List.filter (fun cell -> not (visited.Contains cell) && not (bouldersSet.Contains cell) && isInRange cell)
      for next in nexts do
        visited <- visited.Add next
        queue.Enqueue next

    let mutable bouldersSetWithoutInnerCells = List.empty
    for x in xRange do
      for y in yRange do
        for z in zRange do
          if not (outside.Contains (x, y, z)) then bouldersSetWithoutInnerCells <- (x,y,z)::bouldersSetWithoutInnerCells

    countFreeSides(bouldersSetWithoutInnerCells)

  let part1 (input: string) = input |> parse |> countFreeSides

  let part2 (input: string) = input |> parse |> countExternalSides


let input = File.ReadAllText(Path.Combine(__SOURCE_DIRECTORY__, "input.txt"))

let resultPart1 = BoilingBoulders.part1 input
printfn "Result part1: %i" resultPart1

let resultPart2 = BoilingBoulders.part2 input
printfn "Result part2: %i" resultPart2
