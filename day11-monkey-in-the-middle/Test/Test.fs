namespace Test

open Microsoft.VisualStudio.TestTools.UnitTesting
open Program

[<TestClass>]
type TestClass () =
  let monkeys = [
    new MonkeyInTheMiddle.Monkey({
      Items = [79; 98];
      TestValue = 23;
      ThrowMonkeys = (2,3);
      Transform = fun x -> x * 19L
    });

    new MonkeyInTheMiddle.Monkey({
      Items = [54; 65; 75; 74];
      TestValue = 19;
      ThrowMonkeys = (2,0);
      Transform = fun x -> x + 6L
    });

    new MonkeyInTheMiddle.Monkey({
      Items = [79; 60; 97];
      TestValue = 13;
      ThrowMonkeys = (1,3);
      Transform = fun x -> x * x
    });

    new MonkeyInTheMiddle.Monkey({
      Items = [74];
      TestValue = 17;
      ThrowMonkeys = (0,1);
      Transform = fun x -> x + 3L
    });
  ]

  [<TestMethod>]
  member this.ShouldReturnMonkeyBusinessAfter20rounds () =
    let expected = 10605L
    let actual = MonkeyInTheMiddle.part1 monkeys
    Assert.AreEqual(expected, actual)

  [<TestMethod>]
  member this.ShouldReturnMonkeyBusinessAfter10000rounds () =
    let expected = 2713310158L
    let actual = MonkeyInTheMiddle.part2 monkeys
    Assert.AreEqual(expected, actual)

