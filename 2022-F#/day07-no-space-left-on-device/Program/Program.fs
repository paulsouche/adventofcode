open System.IO
open System.Collections.Generic

module NoSpaceLeftOnDevice =
  type Folder = {
    mutable Size: int
  }

  let private parse (input: string) =
    let lines = input.Split '\n' |> Array.toList |> List.filter(fun line -> line <> "$ cd /" && line <> "$ ls" && not (line.StartsWith("dir")))
    let stack = new Stack<Folder>()
    let mutable folders = List.empty
    stack.Push({ Size = 0 })

    for line in lines do
      let command = line.Split(' ')
      match command[0] with
      | "$" -> match command[2] with
               | ".." -> folders <- stack.Pop()::folders
                         stack.Peek().Size <- stack.Peek().Size + folders[0].Size
               | _ -> stack.Push({ Size=0})
      | _ -> stack.Peek().Size <- stack.Peek().Size + int command[0]

    let mutable total = 0
    while stack.Count > 1 do
      let folder = stack.Pop()
      total <- total + folder.Size
      folders <- folder::folders
      stack.Peek().Size <- stack.Peek().Size + total

    (folders, stack.Peek().Size)

  let private foldersAtMost100000 (folders: list<Folder>, _: int) =
    folders
      |> List.filter (fun folder -> folder.Size <= 100_000)
      |> List.sumBy (fun folder -> folder.Size)

  let private folderToDelete (folders: list<Folder>, totalUsed: int) =
    let goal = 30_000_000
    let unused = 70_000_000 - totalUsed
    folders
      |> List.map(fun folder -> folder.Size)
      |> List.sort
      |> List.find(fun size -> size + unused >= goal)


  let part1 (input: string) =
    input
      |> parse
      |> foldersAtMost100000

  let part2 (input: string) =
    input
      |> parse
      |> folderToDelete

let input = File.ReadAllText(Path.Combine(__SOURCE_DIRECTORY__, "input.txt"))

let resultPart1 = NoSpaceLeftOnDevice.part1 input
printfn "Result part1: %i" resultPart1

let resultPart2 = NoSpaceLeftOnDevice.part2 input
printfn "Result part2: %i" resultPart2
