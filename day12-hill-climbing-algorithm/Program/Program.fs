open System.Collections.Generic
open System.IO
open System

module HillClimbingAlgorithm =
  type Node = { visited: bool; distance: int }

  let private DefaultNode() = { visited = false; distance = Int32.MaxValue }

  let private find2D target arr =
    let row = arr |> Array.findIndex (Array.contains target)
    row, Array.findIndex (fun c -> c = target) arr[row]

  let private elevationBuilder (chars: char[][]) (x, y) =
    match chars[x][y] with
    | 'S' -> 0
    | 'E' -> (int 'z') - (int 'a')
    | c -> (int c) - (int 'a')

  let private distanceBuilder (graph: Node[,]) (x, y) = graph[x, y].distance

  let private isInboundBuilder (graph: Node[,]) (x, y) =
    x >= 0
    && y >= 0
    && x < Array2D.length1 graph
    && y < Array2D.length2 graph

  let private isVisitedBuilder (graph: Node[,]) (x, y) = graph[x, y].visited

  let private adjacent (x, y) = [
    (x - 1, y)
    (x + 1, y)
    (x, y - 1)
    (x, y + 1)
  ]

  let private dijkstra chars cost width height source target =
    let graph = Array2D.create width height (DefaultNode())

    let (sx, sy) = source
    graph.[sx, sy] <- { graph.[sx, sy] with distance = 0 }

    let queue = PriorityQueue<int * int, int>()
    queue.Enqueue(source, 0)

    let elevation = elevationBuilder chars
    let distance = distanceBuilder graph
    let isInbound = isInboundBuilder graph
    let isVisited = isVisitedBuilder graph

    let rec visit () =
        if queue.Count = 0 then
            distance target
        else
            match queue.Dequeue() with
            | node when isVisited node -> visit ()
            | node when node = target -> distance node
            | (x, y) ->
              graph[x, y] <- { graph[x, y] with visited = true }

              adjacent (x, y)
                |> Seq.filter (fun n -> isInbound n && not (isVisited n))
                |> Seq.filter (fun n -> elevation n <= elevation (x, y) + 1)
                |> Seq.map (fun n -> (n, distance (x, y) + cost n))
                |> Seq.filter (fun (n, d) -> d < distance n)
                |> Seq.iter (fun ((xn, yn), d) ->
                    graph[xn, yn] <- { graph[xn, yn] with distance = d }
                    queue.Enqueue((xn, yn), d))

              visit()

    visit()

  let part1 (input: string) =
    let chars = input.Split('\n') |> Array.map (fun line-> line.ToCharArray())
    let start = find2D 'S' chars
    let target = find2D 'E' chars
    let width = chars.Length
    let height = chars[0].Length

    dijkstra chars (fun _ -> 1) width height start target

  let part2 (input: string) =
    let chars = input.Split('\n') |> Array.map (fun line-> line.ToCharArray())
    let target = find2D 'E' chars
    let width = chars.Length
    let height = chars[0].Length

    let elevation = elevationBuilder chars

    [
      for row in 0..width - 1 do
          for col in 0..height - 1 do
              if elevation (row, col) = 0 then
                  dijkstra chars (fun _ -> 1) width height (row, col) target
    ] |> List.min


let input = File.ReadAllText(Path.Combine(__SOURCE_DIRECTORY__, "input.txt"))

let resultPart1 = HillClimbingAlgorithm.part1 input
printfn "Result part1: %i" resultPart1

let resultPart2 = HillClimbingAlgorithm.part2 input
printfn "Result part2: %i" resultPart2
