namespace Test

open Microsoft.VisualStudio.TestTools.UnitTesting
open Program

[<TestClass>]
type TestClass () =
  let walk3D (map: list<list<char>>) (row, column, nextRow, nextColumn, direction) =
    let cube = [
      [0; 0; 1; 0];
      [2; 3; 4; 0];
      [0; 0; 5; 6];
    ]

    let cubeSide = 4
    let face = cube[row / cubeSide][column / cubeSide]
    let rCube, cCube = row % cubeSide, column % cubeSide

    match direction with
      | MonkeyMap.Direction.Right ->
        if nextColumn >= map[nextRow].Length then
          match face with
            | 1 ->
              // Face 6, from right
              (3 * cubeSide - 1 - rCube, 4 * cubeSide - 1, MonkeyMap.Direction.Left)
            | 4 ->
              // Face 6, from top
              (2 * cubeSide, 4 * cubeSide - 1 - rCube, MonkeyMap.Direction.Down)
            | 6 ->
              // Face 1, from right
              (cubeSide - 1 - rCube, 2 * cubeSide - 1, MonkeyMap.Direction.Left)
            | _ -> failwith "Invalid"
        else
          (nextRow, nextColumn, direction)
      | MonkeyMap.Direction.Left ->
        if nextColumn < 0 || map[nextRow][nextColumn] = ' ' then
          match face with
            | 1 ->
              // Face 3, from top
              (cubeSide, cubeSide + rCube, MonkeyMap.Direction.Down)
            | 2 ->
              // Face 6, from bottom
              (3 * cubeSide - 1, 3 * cubeSide - rCube, MonkeyMap.Direction.Up)
            | 5 ->
              // Face 3, from bottom
              (2 * cubeSide - 1, 2 * cubeSide - 1 - rCube, MonkeyMap.Direction.Up)
            | _ -> failwith "Invalid"
        else
          (nextRow, nextColumn, direction)
      | MonkeyMap.Direction.Up ->
        if nextRow < 0 || map[nextRow][nextColumn] = ' ' then
          match face with
            | 2 ->
              // Face 1, from top
              (0, 3 * cubeSide - 1 - cCube, MonkeyMap.Direction.Down)
            | 3 ->
              // Face 1, from left
              (cCube, 2 * cubeSide, MonkeyMap.Direction.Right)
            | 1 ->
              // Face 2, from top
              (cubeSide, cubeSide - 1 - cCube, MonkeyMap.Direction.Down)
            | 6 ->
              // Face 4, from right
              (2 * cubeSide - 1 - cCube, 3 * cubeSide - 1, MonkeyMap.Direction.Left)
            | _ -> failwith "Invalid"
        else
          (nextRow, nextColumn, direction)
      | MonkeyMap.Direction.Down ->
        if nextRow >= map.Length || (nextRow >= 0 && nextColumn >= map[nextRow].Length) then
          match face with
            | 2 ->
              // Face 5, from bottom
              (3 * cubeSide - 1, 3 * cubeSide - 1 - cCube, MonkeyMap.Direction.Up)
            | 3 ->
              // Face 5, from left
              (3 * cubeSide - 1 - cCube, 2 * cubeSide, MonkeyMap.Direction.Right)
            | 5 ->
              // Face 2, from bottom
              (2 * cubeSide - 1, cubeSide - 1 - cCube, MonkeyMap.Direction.Up)
            | 6 ->
              // Face 2, from left
              (2* cubeSide - 1 - cCube, 0, MonkeyMap.Direction.Right)
            | _ -> failwith "Invalid"
        else
          (nextRow, nextColumn, direction)

  [<TestMethod>]
  member this.ShouldReturnPasswordWithWrapAround () =
    let expected = 6032;
    let actual = MonkeyMap.part1 "        ...#\n        .#..\n        #...\n        ....\n...#.......#\n........#...\n..#....#....\n..........#.\n        ...#....\n        .....#..\n        .#......\n        ......#.\n\n10R5L5R10L4R5L5"
    Assert.AreEqual(expected, actual)

  [<TestMethod>]
  member this.ShouldReturnPasswordWithWrap3D () =
    let expected = 5031;
    let actual = MonkeyMap.part2 "        ...#\n        .#..\n        #...\n        ....\n...#.......#\n........#...\n..#....#....\n..........#.\n        ...#....\n        .....#..\n        .#......\n        ......#.\n\n10R5L5R10L4R5L5" walk3D
    Assert.AreEqual(expected, actual)
