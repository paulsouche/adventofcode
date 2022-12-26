open System.IO
open System.Text

module PyroclasticFlow =
  type Direction =
    | Left
    | Right
    | Down

  type Shape =
    | Line
    | Plus
    | Lshape
    | Pipe
    | Square

  let private parse (input: string) =
    input.ToCharArray()
      |> Array.toList
      |> List.map (function | '<' -> Left | '>' -> Right | _ -> failwith "Invalid character")

  let private getShape shapeInd = [Line; Plus; Lshape; Pipe; Square][shapeInd % 5]

  let private getTopRock (area : Set<int*int>) =
    if (area |> Set.isEmpty) then None else area |> Set.toList |> List.map snd |> List.max |> Some

  let private spawnShape (shape: Shape) (area: Set<int*int>) =
    let y = match getTopRock area with | Some y -> y + 4 | None -> 4
    let x = -1

    match shape with
      | Line -> [(x, y); (x + 1, y); (x + 2, y); (x + 3, y)]
      | Plus -> [(x, y + 1); (x + 1, y); (x + 1, y + 1); (x + 1,y + 2); (x + 2, y + 1)]
      | Lshape -> [(x, y); (x + 1, y); (x + 2, y); (x + 2,y + 1); (x + 2, y + 2)]
      | Pipe -> [(x, y); (x, y + 1); (x, y + 2); (x, y + 3)]
      | Square -> [(x, y); (x, y + 1); (x + 1, y); (x + 1, y + 1)]

  let private pushShape direction (shape: (int*int) list) (area: Set<int*int>) =
    let newCoords =
      match direction with
        | Left -> shape |> List.map (fun (x, y) -> (x - 1, y))
        | Right -> shape |> List.map (fun (x, y) -> (x + 1, y))
        | Down -> shape |> List.map (fun (x, y) -> (x, y - 1))

    let wouldHitWall = newCoords |> List.exists (fun (x,y) -> abs x > 3 || y <= 0)
    let wouldHitOccupied = newCoords |> List.exists (fun element -> area |> Set.contains element)
    if (wouldHitWall || wouldHitOccupied) then None else Some newCoords

  let private addShapeToAreaBuilder (hotGasJets: list<Direction>) (shape: Shape) ((area: Set<int*int>), hotGasJetInd) =
    let spawnedShape = spawnShape shape area

    let rec moveShape coords hotGasJetInd =
      let wind = hotGasJets[hotGasJetInd % hotGasJets.Length]
      let pushedByWindCoords = pushShape wind coords area |> Option.defaultValue coords
      match pushShape Down pushedByWindCoords area with
        | Some nextCoords -> moveShape nextCoords ((hotGasJetInd + 1) % hotGasJets.Length)
        | None -> pushedByWindCoords, (hotGasJetInd + 1) % hotGasJets.Length

    let (movedShape, nextHotGasJetInd) = moveShape spawnedShape hotGasJetInd
    let areaBeforePrune = (movedShape |> List.fold (fun s t -> Set.add t s) area)
    let areaAfterPrune =
      let topRock = getTopRock areaBeforePrune |> Option.get
      areaBeforePrune |> Set.filter (fun v -> topRock < (snd v) + 50)
    areaAfterPrune, nextHotGasJetInd

  let private addShapeBuilder (hotGasJets: list<Direction>) ((area: Set<int*int>), hotGasJetInd) shapeInd =
    let addShapeToArea = addShapeToAreaBuilder(hotGasJets)
    addShapeToArea (getShape shapeInd) (area, hotGasJetInd)

  let private areaToString (area: Set<int*int>) =
    let topRock = getTopRock area |> Option.get
    let sb = new StringBuilder(7*50)
    for y = topRock - 49 to topRock do
      for x = -3 to 3 do
        (if (area |> Set.contains (x,y)) then sb.Append("#") else sb.Append(".")) |> ignore

    sb.ToString()

  let part1 (input: string) =
    let addShape = addShapeBuilder (input |> parse)

    [0..2021]
      |> List.fold addShape (Set.empty, 0)
      |> (fst >> Set.toArray)
      |> Array.maxBy snd
      |> snd

  let part2 (input: string) =
    let addShape = addShapeBuilder (input |> parse)

    let rec visit shapeInd hotGasJetInd area (cache: Map<string, int * int>) =
        let (nextArea, nextHotGasJetInd) = addShape (area, hotGasJetInd) shapeInd
        let nextAreaKey = areaToString nextArea
        let nextAreaTopRock = getTopRock nextArea |> Option.get
        match cache |> Map.tryFind (nextAreaKey) with
          | None -> visit (shapeInd + 1) nextHotGasJetInd nextArea (cache |> Map.add (nextAreaKey) (shapeInd + 1, nextAreaTopRock))
          | Some (cacheShapeInd,cacheTopRock) ->
            let startTime, startHeight = (int64 cacheShapeInd, int64 cacheTopRock)
            let endTime, endHeight = (int64 shapeInd + 1L, int64 nextAreaTopRock)
            let period = endTime - startTime
            let height = endHeight - startHeight
            let timeToSimulate = 1_000_000_000_000L - startTime
            let extraHeight = (timeToSimulate / period) * height
            if (float (timeToSimulate / period) = (float timeToSimulate / float period)) then (startHeight + extraHeight)
            else visit (shapeInd + 1) nextHotGasJetInd nextArea (cache |> Map.add (nextAreaKey) (shapeInd + 1, nextAreaTopRock))

    visit 0 0 Set.empty Map.empty


let input = File.ReadAllText(Path.Combine(__SOURCE_DIRECTORY__, "input.txt"))

let resultPart1 = PyroclasticFlow.part1 input
printfn "Result part1: %i" resultPart1

let resultPart2 = PyroclasticFlow.part2 input
printfn "Result part2: %i" resultPart2
