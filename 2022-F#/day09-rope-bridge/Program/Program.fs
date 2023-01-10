open System.IO

module RopeBridge =
  exception InnerError of string
  let private pair list = match list with | a::b::_ -> (a,b) | _ -> failwith "List length should be 2"
  let private parse (input: string) = input.Split '\n' |> Array.toList |> List.map(fun line -> line.Split(' ') |> Array.toList |> pair)

  type private Node() =
    let mutable x = 0
    let mutable y = 0

    member this.Coordinates with get () = (x,y)

    member this.move(direction: string) =
      match direction with
        | "U" -> y <- y + 1
        | "R" -> x <- x + 1
        | "D" -> y <- y - 1
        | "L" -> x <- x - 1
        | _ -> raise (InnerError ("Invalid move: " + direction))

    member this.followHead(head: Node) =
      let (hx,hy) = head.Coordinates
      let hxs = [hx - 1; hx; hx + 1]
      let hys = [hy - 1; hy; hy + 1]
      let valids = [
        (hx - 1, hy - 1);(hx, hy - 1);(hx + 1, hy - 1);
        (hx - 1, hy    );(hx, hy    );(hx + 1, hy    );
        (hx - 1, hy + 1);(hx, hy + 1);(hx + 1, hy + 1)
      ]
      if x = hx && not (hys |> List.contains y) then
        if hy > y then y <- y + 1
        else y <- y - 1
      elif y = hy && not (hxs |> List.contains x) then
        if hx > x then x <- x + 1
        else x <- x - 1
      elif not (valids |> List.contains (x,y)) then
        if x < hx && y < hy then
          x <- x + 1
          y <- y + 1
        elif x < hx && y > hy then
          x <- x + 1
          y <- y - 1
        elif x > hx && y < hy then
          x <- x - 1
          y <- y + 1
        else
          x <- x - 1
          y <- y - 1

  let private motions (stringSize: int) (instructions: list<string * string>) =
    let head = new Node()
    let tails: list<Node> = List.init stringSize (fun _ -> new Node())
    let mutable visited = List.Empty

    for (direction, count) in instructions do
      for i = 1 to (int count) do
        head.move(direction)
        let mutable node = head
        for j = 0 to tails.Length - 1 do
          tails[j].followHead(node)
          node <- tails[j]
        visited <- node.Coordinates.ToString()::visited

    visited

  let part1 (input: string) = input |> parse |> motions 1 |> Set |> Set.count

  let part2 (input: string) = input |> parse |> motions 9 |> Set |> Set.count


let input = File.ReadAllText(Path.Combine(__SOURCE_DIRECTORY__, "input.txt"))

let resultPart1 = RopeBridge.part1 input
printfn "Result part1: %i" resultPart1

let resultPart2 = RopeBridge.part2 input
printfn "Result part2: %i" resultPart2
