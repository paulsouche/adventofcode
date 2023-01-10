open System.IO

module BeaconExclusionZone =
  let private manhattan (ax, ay) (bx, by) = abs (bx - ax) + abs (by - ay)

  let private pair list = match list with | a::b::_ -> (a,b) | _ -> failwith "List length should be 2"
  let private parse (input: string) =
    input.Split('\n')
      |> Array.toList
      |> List.map (fun line ->
        line[line.IndexOf('x')..line.Length - 1].Split(": closest beacon is at ")
          |> Array.toList
          |> List.map (fun coords ->
            coords.Split(", ")
              |> Array.toList
              |> List.map (fun coord -> coord[(coord.IndexOf('=') + 1)..(coord.Length - 1)])
              |> List.map int
              |> pair)
          |> pair)

  let private detectedBuilder sensors pt =
    sensors
      |> Map.exists (fun s d -> manhattan pt s <= d)

  let private noBeaconCells (column: int) (scan: list<((int * int) * (int * int))>) =
    let sensors = scan |> List.map (fun (sensor, beacon) -> sensor, manhattan sensor beacon) |> Map.ofList
    let beacons = scan |> List.map snd |> Set.ofList
    let maxDistance = sensors |> Map.values |> Seq.max
    let sensorsX = sensors |> Map.keys |> Seq.map fst

    let detected = detectedBuilder sensors

    { Seq.min sensorsX - maxDistance .. Seq.max sensorsX + maxDistance }
    |> Seq.map (fun x -> x, column)
    |> Seq.filter (fun pt -> not (beacons |> Set.contains pt) && detected pt)
    |> Seq.length

  let private boundariesBuilder (maxX, maxY) (x,y) = x >= 0 && x <= maxX && y >= 0 && y<= maxY

  let private perimeterBuilder (boundaries) (( x, y), distance) =
    let topRight = List.init (distance + 2) (fun i -> (x + i, y - distance - 1 + i))
    let bottomRight = List.init (distance + 2) (fun i -> (x + i, y + distance + 1 - i))
    let bottomLeft = List.init (distance + 2) (fun i -> (x - i, y + distance + 1 - i))
    let topLeft = List.init (distance + 2) (fun i -> (x - i, y - distance - 1 + i))

    [topRight; bottomRight; bottomLeft; topLeft] |> List.concat |> List.filter boundaries


  let private findDistressBeacon (scan: list<((int * int) * (int * int))>) =
    let sensors = scan |> List.map (fun (sensor, beacon) -> sensor, manhattan sensor beacon)
    let maxX = sensors |> List.map fst |> List.map fst |> List.max
    let maxY = sensors |> List.map fst |> List.map snd |> List.max

    let perimeter = perimeterBuilder (boundariesBuilder (maxX, maxY))

    let undetected = sensors |> List.collect (fun sensor ->
        perimeter sensor |> List.filter (fun pt ->
          match sensors |> List.tryFind (fun (sensor, distance) -> (manhattan sensor pt) <= distance) with
          | None -> true
          | _ -> false
        )
    )

    let distressBeaconX, distressBeaconY = undetected[0]

    int64 distressBeaconX * 4_000_000L + int64 distressBeaconY

  let part1 (column: int) (input: string) = input |> parse |> noBeaconCells column

  let part2 (input: string) = input |> parse |> findDistressBeacon


let input = File.ReadAllText(Path.Combine(__SOURCE_DIRECTORY__, "input.txt"))

let resultPart1 = BeaconExclusionZone.part1 2_000_000 input
printfn "Result part1: %i" resultPart1

let resultPart2 = BeaconExclusionZone.part2 input
printfn "Result part2: %i" resultPart2
