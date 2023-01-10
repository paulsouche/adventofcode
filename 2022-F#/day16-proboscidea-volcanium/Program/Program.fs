open System.IO
open System.Text.RegularExpressions
open System

module ProboscideaVolcanium =
  let private lineRx() = Regex(@"^Valve\s([A-Z]+)\shas\sflow\srate=(\d+);\stunnel(?:s?)\slead(?:s?)\sto\svalve(?:s?)\s(.*)$", RegexOptions.Compiled)

  let private parse (input: string) =
    input.Split('\n')
      |> Array.toList
      |> List.map (fun line ->
        let m = lineRx().Match(line)
        (m.Groups[1].Value, (int m.Groups[2].Value, Seq.toList(m.Groups[3].Value.Split(',', StringSplitOptions.TrimEntries)))))
      |> Map.ofList

  let private openedValves valves =
    valves
      |> Map.toSeq
      |> Seq.filter (fun (_, (flowRate, _)) -> flowRate = 0)
      |> Seq.map (fun (valve, _) -> valve)
      |> Set.ofSeq

  let private getNextStatesAlone valves remainingMinutes (currentPressure, currentValve, openedValves) =
    let valve = valves |> Map.find currentValve
    [
      if not (openedValves |> Set.contains currentValve) then
        let releasedPressure = currentPressure + (remainingMinutes - 1) * (valve |> fst)
        (releasedPressure, currentValve, openedValves |> Set.add currentValve)
      yield!
        valve
          |> snd
          |> List.map (fun valve -> (currentPressure, valve, openedValves))
    ]

  let private getNextStatesWithElephant valves remainingMinutes (releasedPressure, (valve1, valve2), openedValves) =
    let getActions valve =
      [
        if not <| Set.contains valve openedValves then
          (valve, Set.singleton valve)
        yield!
          valves
            |> Map.find valve
            |> snd
            |> List.map (fun valve -> (valve, Set.empty))
      ]

    List.allPairs (getActions valve1) (getActions valve2)
      |> List.map (fun ((valve1, openedValves1), (valve2, openedValves2)) ->
        let additionalOpenedValves = Set.union openedValves1 openedValves2
        let additionalReleasedPressure =
          additionalOpenedValves
            |> Seq.sumBy (fun valve ->
                let flowRate =
                  valves
                    |> Map.find valve
                    |> fst
                (remainingMinutes - 1) * flowRate
            )
        (releasedPressure + additionalReleasedPressure, (valve1, valve2), Set.union openedValves additionalOpenedValves)
    )

  let private maxPressure minutes start getNextStatesBuilder valves =
    let getNextStates = getNextStatesBuilder valves

    let rec visit remainingMinutes states =
      if remainingMinutes = 0 then
        states |> List.map (fun (pressure, _, _) -> pressure) |> List.max
      else
        states
          |> List.collect (getNextStates remainingMinutes)
          |> List.sortByDescending (fun (pressure, _, _) -> pressure)
          |> List.truncate 516
          |> visit (remainingMinutes - 1)

    visit minutes [(0, start, valves |> openedValves)]

  let part1 (input: string) = input |> parse |> maxPressure 30 "AA" getNextStatesAlone

  let part2 (input: string) = input |> parse |> maxPressure 26 ("AA", "AA") getNextStatesWithElephant


let input = File.ReadAllText(Path.Combine(__SOURCE_DIRECTORY__, "input.txt"))

let resultPart1 = ProboscideaVolcanium.part1 input
printfn "Result part1: %i" resultPart1

let resultPart2 = ProboscideaVolcanium.part2 input
printfn "Result part2: %i" resultPart2
