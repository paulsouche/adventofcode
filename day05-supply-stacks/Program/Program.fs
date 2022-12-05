open System.IO
open System.Text.RegularExpressions
open System.Globalization

module SupplyStacks =
  let private pair list = match list with | a::b::_ -> (a,b) | _ -> failwith "List length should be 2"

  let private instructionsRx() = Regex(@"^move\s(\d+)\sfrom\s(\d+)\sto\s(\d+)$", RegexOptions.Compiled)

  type Instruction =
    struct
      val Move: int
      val From: int
      val To: int

      new (move, fromColumn, toColumn) = {Move = move; From = fromColumn; To = toColumn;}
    end

  let instructions (instruction: string) =
    let m = instructionsRx().Match(instruction)
    new Instruction(int m.Groups[1].Value, int m.Groups[2].Value, int m.Groups[3].Value)

  [<AbstractClass>]
  type Stack() =
    let mutable crates = ""

    member this.Crates with get () = crates and set (cratesVal) = crates <- cratesVal
    member this.TopCrate with get () = crates[crates.Length - 1]

    member this.pushCrates(add: string) = crates <- crates + add

    abstract member popCrates: int -> string

  type Stack9000() =
    inherit Stack()
    override this.popCrates(count: int) =
      let len = this.Crates.Length
      let removed =
          seq {
              let tee = StringInfo.GetTextElementEnumerator(this.Crates[len - count..len])
              while tee.MoveNext() do
                yield tee.GetTextElement()
          } |> List.ofSeq |> List.rev |> String.concat ""

      this.Crates <- this.Crates[0..len - count - 1]
      removed

  type Stack9001() =
    inherit Stack()
    override this.popCrates(count: int) =
      let len = this.Crates.Length
      let removed = this.Crates[len - count..len]
      this.Crates <- this.Crates[0..len - count - 1]
      removed

  let private stacks9000 (_: int) = new Stack9000() :> Stack

  let private stacks9001 (_: int) = new Stack9001() :> Stack

  let private stacks (ctor: int -> Stack) (input: list<string>) =
    let stackLen = input[0].Split "   " |> Array.length
    let stacks = List.init stackLen ctor

    for stack in (List.tail input) do
      for i in [0..stackLen - 1] do
        let charIndex = i * 4 + 1
        let char = if charIndex <= stack.Length then stack[charIndex] else ' '
        if char <> ' ' then stacks[i].pushCrates(stack[charIndex].ToString())

    stacks

  let private program (ctor: int -> Stack) (crates: string, commands: string) = (
    Seq.toList(crates.Split "\n")
      |> List.rev
      |> stacks ctor,
    Seq.toList(commands.Split "\n")
      |> List.map instructions
    )
  let private parse (ctor: int -> Stack) (input: string) = Seq.toList(input.Split "\n\n") |> pair |> program ctor

  let rec private move (stacks: list<Stack>) (instruction: Instruction) =
    let removed = stacks[instruction.From - 1].popCrates(instruction.Move)
    stacks[instruction.To - 1].pushCrates(removed)
    stacks

  let private run (stacks: list<Stack>, instructions: list<Instruction>) = instructions |> List.fold move stacks

  let private result (stacks: list<Stack>) = stacks |> List.map (fun stack -> stack.TopCrate.ToString()) |> String.concat ""

  let part1 (input: string) =
    input
      |> parse stacks9000
      |> run
      |> result

  let part2 (input: string) =
    input
      |> parse stacks9001
      |> run
      |> result

let input = File.ReadAllText(Path.Combine(__SOURCE_DIRECTORY__, "input.txt"))

let resultPart1 = SupplyStacks.part1 input
printfn "Result part1: %s" resultPart1

let resultPart2 = SupplyStacks.part2 input
printfn "Result part2: %s" resultPart2
