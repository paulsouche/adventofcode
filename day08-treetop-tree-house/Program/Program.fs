open System.IO

module TreeTopTreeHouse =
  let private parse (input: string) = input.Split '\n' |> Array.toList |> List.map(fun line -> List.ofArray(line.ToCharArray() |> Array.map (fun char -> int (char.ToString()))))

  let private countVisibleTrees (trees: list<list<int>>) =
    let columnSize = trees.Length
    let mutable visibleTrees = 0
    for y = 0 to columnSize - 1 do
      let line = trees[y]
      let lineSize = line.Length
      if y = 0 || y = columnSize - 1 then visibleTrees <- visibleTrees + lineSize
      else
        for x = 0 to lineSize - 1 do
          if x = 0 || x = lineSize - 1 then visibleTrees <- visibleTrees + 1
          else
            let visibleTop = trees[0..y - 1] |> List.map (fun l -> l[x]) |> List.forall (fun tree -> tree < line[x] )
            let visibleRight = line[x+1..lineSize - 1] |> List.forall (fun t -> t < line[x] )
            let visibleBottom = trees[y + 1..trees.Length - 1] |> List.map (fun l -> l[x]) |> List.forall (fun tree -> tree < line[x] )
            let visibleLeft = line[0..x - 1] |> List.forall (fun t -> t < line[x] )
            if visibleLeft || visibleRight || visibleTop || visibleBottom then visibleTrees <- visibleTrees + 1
    visibleTrees

  let private scenicScore (trees: list<list<int>>) =
    let mutable scores = List.empty

    let columnSize = trees.Length
    for y = 1 to columnSize - 2 do
      let line = trees[y]
      let lineSize = line.Length
      for x = 1 to lineSize - 2 do
        let tree = trees[y][x]

        let topColumn = trees[0..y - 1] |> List.map (fun l -> l[x]) |> List.rev
        let topScenicScore = min topColumn.Length ((topColumn |> List.takeWhile (fun t -> t < tree) |> List.length) + 1)

        let rightLine = line[x+1..lineSize - 1]
        let rightScenicScore = min rightLine.Length ((rightLine |> List.takeWhile (fun t -> t < tree) |> List.length) + 1)

        let bottomColumn = trees[y + 1..trees.Length - 1] |> List.map (fun l -> l[x])
        let bottomScenicScore = min bottomColumn.Length ((bottomColumn |> List.takeWhile (fun t -> t < tree) |> List.length) + 1)

        let leftLine = line[0..x - 1] |> List.rev
        let leftScenicScore = min leftLine.Length ((leftLine |> List.takeWhile (fun t -> t < tree) |> List.length) + 1)

        scores <- (topScenicScore * rightScenicScore * bottomScenicScore * leftScenicScore)::scores

    scores

  let part1 (input: string) =
    input
      |> parse
      |> countVisibleTrees

  let part2 (input: string) =
    input
      |> parse
      |> scenicScore
      |> List.max

let input = File.ReadAllText(Path.Combine(__SOURCE_DIRECTORY__, "input.txt"))

let resultPart1 = TreeTopTreeHouse.part1 input
printfn "Result part1: %i" resultPart1

let resultPart2 = TreeTopTreeHouse.part2 input
printfn "Result part2: %i" resultPart2
