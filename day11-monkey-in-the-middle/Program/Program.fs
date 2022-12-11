
module MonkeyInTheMiddle =
  type MonkeyParams = {
    mutable Items: List<int64>
    Transform: int64 -> int64
    TestValue: int64
    ThrowMonkeys: (int * int)
  }

  type Monkey(monkeyParams: MonkeyParams) =
    let mutable inspects: int64 = 0

    member this.TestValue with get () = monkeyParams.TestValue
    member this.Items with get () = monkeyParams.Items
    member this.Inspects with get () = inspects

    member this.inspect(divideValue: int64, superModulo: int64) =
      let worryLevel = (monkeyParams.Transform monkeyParams.Items[0] % superModulo) / divideValue
      monkeyParams.Items <- monkeyParams.Items[1..monkeyParams.Items.Length - 1]
      inspects <- inspects + 1L
      (worryLevel, if worryLevel % monkeyParams.TestValue = 0 then fst monkeyParams.ThrowMonkeys else snd monkeyParams.ThrowMonkeys)

    member this.receiveItem(item: int64) =
      monkeyParams.Items <- List.append monkeyParams.Items [item]

  let private rounds (roundCount: int, divideValue: int) (monkeys: list<Monkey>) =
    let superModulo = monkeys |> List.map (fun m -> m.TestValue) |> List.reduce (*)
    for i = 1 to roundCount do
      for monkey in monkeys do
        while monkey.Items.Length > 0 do
          let item, monkeyIndex = monkey.inspect(divideValue, superModulo)
          monkeys[monkeyIndex].receiveItem(item)
    monkeys

  let part1 (monkeys: list<Monkey>) =
    monkeys
      |> rounds (20, 3)
      |> List.map (fun m -> m.Inspects)
      |> List.sortDescending
      |> List.take 2
      |> List.reduce (*)

  let part2 (monkeys: list<Monkey>) =
    monkeys
      |> rounds (10000, 1)
      |> List.map (fun m -> m.Inspects)
      |> List.sortDescending
      |> List.take 2
      |> List.reduce (*)

// Too lazy to parse today
let monkeysInput = fun () -> [
  // Monkey 0:
  //   Starting items: 85, 79, 63, 72
  //   Operation: new = old * 17
  //   Test: divisible by 2
  //     If true: throw to monkey 2
  //     If false: throw to monkey 6
  new MonkeyInTheMiddle.Monkey({
    Items = [85; 79; 63; 72];
    TestValue = 2;
    ThrowMonkeys = (2,6);
    Transform = fun x -> x * 17L
  });

  // Monkey 1:
  //   Starting items: 53, 94, 65, 81, 93, 73, 57, 92
  //   Operation: new = old * old
  //   Test: divisible by 7
  //     If true: throw to monkey 0
  //     If false: throw to monkey 2
  new MonkeyInTheMiddle.Monkey({
    Items = [53; 94; 65; 81; 93; 73; 57; 92];
    TestValue = 7;
    ThrowMonkeys = (0,2);
    Transform = fun x -> x * x
  });

  // Monkey 2:
  //   Starting items: 62, 63
  //   Operation: new = old + 7
  //   Test: divisible by 13
  //     If true: throw to monkey 7
  //     If false: throw to monkey 6
  new MonkeyInTheMiddle.Monkey({
    Items = [62; 63];
    TestValue = 13;
    ThrowMonkeys = (7,6);
    Transform = fun x -> x + 7L
  });

  // Monkey 3:
  //   Starting items: 57, 92, 56
  //   Operation: new = old + 4
  //   Test: divisible by 5
  //     If true: throw to monkey 4
  //     If false: throw to monkey 5
  new MonkeyInTheMiddle.Monkey({
    Items = [57; 92; 56];
    TestValue = 5;
    ThrowMonkeys = (4,5);
    Transform = fun x -> x + 4L
  });

  // Monkey 4:
  //   Starting items: 67
  //   Operation: new = old + 5
  //   Test: divisible by 3
  //     If true: throw to monkey 1
  //     If false: throw to monkey 5
  new MonkeyInTheMiddle.Monkey({
    Items = [67];
    TestValue = 3;
    ThrowMonkeys = (1,5);
    Transform = fun x -> x + 5L
  });

  // Monkey 5:
  //   Starting items: 85, 56, 66, 72, 57, 99
  //   Operation: new = old + 6
  //   Test: divisible by 19
  //     If true: throw to monkey 1
  //     If false: throw to monkey 0
  new MonkeyInTheMiddle.Monkey({
    Items = [85; 56; 66; 72; 57; 99];
    TestValue = 19;
    ThrowMonkeys = (1,0);
    Transform = fun x -> x + 6L
  });

  // Monkey 6:
  //   Starting items: 86, 65, 98, 97, 69
  //   Operation: new = old * 13
  //   Test: divisible by 11
  //     If true: throw to monkey 3
  //     If false: throw to monkey 7
  new MonkeyInTheMiddle.Monkey({
    Items = [86; 65; 98; 97; 69];
    TestValue = 11;
    ThrowMonkeys = (3,7);
    Transform = fun x -> x * 13L
  });

  // Monkey 7:
  //   Starting items: 87, 68, 92, 66, 91, 50, 68
  //   Operation: new = old + 2
  //   Test: divisible by 17
  //     If true: throw to monkey 4
  //     If false: throw to monkey 3
  new MonkeyInTheMiddle.Monkey({
    Items = [87; 68; 92; 66; 91; 50; 68];
    TestValue = 17;
    ThrowMonkeys = (4,3);
    Transform = fun x -> x + 2L
  });
]

let resultPart1 = MonkeyInTheMiddle.part1 (monkeysInput())
printfn "Result part1: %i" resultPart1

let resultPart2 = MonkeyInTheMiddle.part2 (monkeysInput())
printfn "Result part2: %i" resultPart2
