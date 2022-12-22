open System.IO

module MonkeyMap =
  type Direction =
    | Right
    | Down
    | Left
    | Up

  let private pair list = match list with | a::b::_ -> (a,b) | _ -> failwith "List length should be 2"

  let private parse (input: string) =
    let inputBoard, inputPath = input.Split("\n\n") |> Array.toList |> pair
    (
      inputBoard.Split('\n') |> Array.toList |> List.map (fun line -> line.ToCharArray() |> Array.toList),
      inputPath
        |> fun line ->
          List.zip
            (line.Split('L') |> Array.collect (fun line -> line.Split('R') |> Array.map int) |> Array.toList)
            (line + "X"
              |> fun line -> line.ToCharArray()
              |> Array.toList
              |> List.filter (fun c -> c = 'R' || c = 'L' || c = 'X'))
    )

  let private password (directionsMap: Map<Direction, int>) (row, column, direction) =
    1000 * (row + 1) + 4 * (column + 1) + directionsMap[direction]

  let private isValidSpace cell = cell = '.' || cell = '#'

  let private turnBuilder (directions: list<Direction>) (directionsMap: Map<Direction, int>) turnTo direction =
    match turnTo with
      | 'L' -> directions[(4 + directionsMap[direction] - 1) % 4]
      | 'R' -> directions[(4 + directionsMap[direction] + 1) % 4]
      | _ -> direction

  let private walk (map: list<list<char>>) (_r, _c, nextRow, nextColumn, direction) =
    let targetRow =
      if direction = Right || direction = Left then
        nextRow
      elif nextRow >= map.Length || (nextRow >= 0 && nextColumn >= map[nextRow].Length) || (nextRow >= 0 && map[nextRow][nextColumn] = ' ' && direction = Down) then
        map |> List.findIndex (fun row -> nextColumn < row.Length && isValidSpace row[nextColumn])
      elif nextRow < 0 || map[nextRow][nextColumn] = ' ' then
        map |> List.findIndexBack (fun row -> nextColumn < row.Length && isValidSpace row[nextColumn])
      else
        nextRow

    let targetColumn =
      if direction = Up || direction = Down then
        nextColumn
      elif nextColumn >= map[nextRow].Length then
        map[nextRow] |> List.findIndex isValidSpace
      elif nextColumn < 0 || map[nextRow][nextColumn] = ' ' then
        map[nextRow] |> List.findIndexBack isValidSpace
      else
        nextColumn

    targetRow, targetColumn, direction

  let rec private step (map: list<list<char>>) walker num (row, column, direction) =
    match num with
      | 0 -> (row, column, direction)
      | _ ->
        (match direction with
           | Right -> row, column + 1
           | Down -> row + 1, column
           | Left -> row, column - 1
           | Up -> row - 1, column)
        |> fun (nextRow, nextColumn) ->
            let (targetRow, targetColumn, nextDirection) = walker map (row, column, nextRow, nextColumn, direction)

            if map[targetRow][targetColumn] = '#' then
              row, column, direction
            else
              step map walker (num - 1) (targetRow, targetColumn, nextDirection)

  let private getPassword (map: list<list<char>>) (path: list<int * char>) walker =
    let initialColumn = map[0] |> List.findIndex (fun cell -> cell = '.')
    let directions = [Right; Down; Left; Up]
    let directionsMap =
      directions
        |> List.indexed
        |> List.map (fun (i, dir) -> dir, i)
        |> Map.ofList
    let turn = turnBuilder directions directionsMap

    path
      |> List.fold
        (fun position (num, turnTo) ->
          step map walker num position
            |> fun (r, c, dir) -> r, c, turn turnTo dir)
        (0, initialColumn, Right)
    |> password directionsMap

  let part1 (input: string) =
    input
      |> parse
      |> fun (map, path) -> getPassword  map path walk

  let part2 (input: string) walker =
    input
      |> parse
      |> fun (map, path) -> getPassword  map path walker

let input = File.ReadAllText(Path.Combine(__SOURCE_DIRECTORY__, "input.txt"))

// To lazy to parse today
let walk3D (map: list<list<char>>) (row, column, nextRow, nextColumn, direction) =
  let cube = [
    [0; 1; 2];
    [0; 3; 0];
    [4; 5; 0];
    [6; 0; 0];
  ]

  let cubeSide = 50
  let face = cube[row / cubeSide][column / cubeSide]
  let rCube, cCube = row % cubeSide, column % cubeSide

  match direction with
    | MonkeyMap.Direction.Right ->
      if nextColumn >= map[nextRow].Length then
        match face with
          | 2 ->
              // Face 5, from right
              (3 * cubeSide - 1 - rCube, 2 * cubeSide - 1, MonkeyMap.Direction.Left)
          | 3 ->
              // Face 2, from bottom
              (cubeSide - 1, 2 * cubeSide + rCube, MonkeyMap.Direction.Up)
          | 5 ->
              // Face 2, from right
              (cubeSide - 1 - rCube, 3 * cubeSide - 1, MonkeyMap.Direction.Left)
          | 6 ->
              // Face 5, from bottom
              (3 * cubeSide - 1, cubeSide + rCube, MonkeyMap.Direction.Up)
          | _ -> failwith "Invalid"
      else
        (nextRow, nextColumn, direction)
    | MonkeyMap.Direction.Left ->
      if nextColumn < 0 || map[nextRow][nextColumn] = ' ' then
        match face with
          | 1 ->
              // Face 4, from left
              (3 * cubeSide - 1 - rCube, 0, MonkeyMap.Direction.Right)
          | 3 ->
              // Face 4, from top
              (2 * cubeSide, rCube, MonkeyMap.Direction.Down)
          | 4 ->
              // Face 1, from left
              (cubeSide - 1 - rCube, cubeSide, MonkeyMap.Direction.Right)
          | 6 ->
              // Face 1, from top
              (0, cubeSide + rCube, MonkeyMap.Direction.Down)
          | _ -> failwith "Invalid"
      else
        (nextRow, nextColumn, direction)
    | MonkeyMap.Direction.Up ->
      if nextRow < 0 || map[nextRow][nextColumn] = ' ' then
        match face with
          | 1 ->
              // Face 6, from left
              (3 * cubeSide + cCube, 0, MonkeyMap.Direction.Right)
          | 2 ->
              // Face 6, from bottom
              (4 * cubeSide - 1, cCube, MonkeyMap.Direction.Up)
          | 4 ->
              // Face 3, from left
              (cubeSide + cCube, cubeSide, MonkeyMap.Direction.Right)
          | _ -> failwith "Invalid"
      else
        (nextRow, nextColumn, direction)
    | MonkeyMap.Direction.Down ->
      if nextRow >= map.Length || (nextRow >= 0 && nextColumn >= map[nextRow].Length) then
        match face with
          | 2 ->
              // Face 3, from right
              (cubeSide + cCube, 2 * cubeSide - 1, MonkeyMap.Direction.Left)
          | 5 ->
              // Face 6, from right
              (3 * cubeSide + cCube, cubeSide - 1, MonkeyMap.Direction.Left)
          | 6 ->
              // Face 2, from top
              (0, 2 * cubeSide + cCube, MonkeyMap.Direction.Down)
          | _ -> failwith "Invalid"
      else
        (nextRow, nextColumn, direction)

let resultPart1 = MonkeyMap.part1 input
printfn "Result part1: %i" resultPart1

let resultPart2 = MonkeyMap.part2 input walk3D
printfn "Result part2: %i" resultPart2
