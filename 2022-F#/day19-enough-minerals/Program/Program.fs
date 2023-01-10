open System.Collections.Generic
open System.Text.RegularExpressions
open System.IO

module EnoughMinerals =
  type Blueprint = {
    Id: int;
    OreBotOreCost: int;
    ClayBotOreCost: int;
    ObsidianBotOreCost: int;
    ObsisdianBotOreCost: int;
    GeodeBotOreCost: int;
    GeodeBotObsidianCost: int;
  }

  type Resources = {
    Ore: int;
    Clay: int;
    Obsidian: int;
    Geode: int;
    OreBots: int;
    ClayBots: int;
    ObsidianBots: int;
    GeodeBots: int;
  }

  type State = {
    Resources: Resources;
    MinutesLeft: int;
  }

  let private bluePrintRx() = Regex(@"Blueprint\s(\d+):\sEach\sore\srobot\scosts\s(\d+)\sore\.\sEach\sclay\srobot\scosts\s(\d+)\sore\.\sEach\sobsidian\srobot\scosts\s(\d+)\sore\sand\s(\d+)\sclay\.\sEach\sgeode\srobot\scosts\s(\d+)\sore\sand\s(\d+)\sobsidian\.$", RegexOptions.Compiled)

  let private parse (input:string) =
    input.Split('\n')
      |> Array.toList
      |> List.map (fun line ->
        let groups = bluePrintRx().Match(line).Groups
        {
          Id= int groups[1].Value;
          OreBotOreCost = int groups[2].Value;
          ClayBotOreCost = int groups[3].Value;
          ObsidianBotOreCost = int groups[4].Value;
          ObsisdianBotOreCost = int groups[5].Value;
          GeodeBotOreCost = int groups[6].Value;
          GeodeBotObsidianCost = int groups[7].Value;
        }
      )

  let private safeTake3 (list: list<'a>) = if list.Length < 3 then list else list |> List.take 3

  let private findMaxWeight (initialState: State) (isLastState: State -> bool) (getNextStates: State -> State seq) (stateWeight: State -> int) (stateResources: State -> Resources) =
    let queue = PriorityQueue() // mutable
    queue.Enqueue((initialState, [initialState], 0), 0)
    let visited = [] |> HashSet // mutable
    seq {
      while queue.Count > 0 do
        let state, history, weightSoFar = queue.Dequeue()
        if visited.Add (state |> stateResources) then
          if isLastState state then yield (history, weightSoFar)
          let nextStates = state |> getNextStates
          queue.EnqueueRange (nextStates |> Seq.map (fun (nextState) ->
                                                        let weight = stateWeight nextState
                                                        (nextState, nextState::[], weight), weight))
    }
    |> Seq.head

  let private stateResources (state: State) = state.Resources
  let private stateMinutesLeft (state: State) = state.MinutesLeft
  let private isLastState (state: State) = (state |> stateMinutesLeft) = 0

  let private getNextStatesBuider (bluePrint: Blueprint) (state:State) : (State) seq =
    let resources = state |> stateResources
    let minedResources = {
      resources with
        Ore = resources.Ore + resources.OreBots;
        Clay = resources.Clay + resources.ClayBots;
        Obsidian = resources.Obsidian + resources.ObsidianBots;
        Geode = resources.Geode + resources.GeodeBots;
    }
    let minedState = {
      state with
        Resources = minedResources;
        MinutesLeft = state.MinutesLeft - 1;
    }
    seq {
      yield minedState
      if resources.Ore >= bluePrint.OreBotOreCost then
        yield {
          minedState with
            Resources = {
              minedResources with
                Ore = minedResources.Ore - bluePrint.OreBotOreCost;
                OreBots = minedResources.OreBots + 1
            }
        }
      if resources.Ore >= bluePrint.ClayBotOreCost then
        yield {
          minedState with
            Resources = {
              minedResources with
                Ore = minedResources.Ore - bluePrint.ClayBotOreCost;
                ClayBots = minedResources.ClayBots+1
            }
        }
      if resources.Ore >= bluePrint.ObsidianBotOreCost && resources.Clay >= bluePrint.ObsisdianBotOreCost then
        yield {
          minedState with
            Resources = {
              minedResources with
                Ore = minedResources.Ore - bluePrint.ObsidianBotOreCost;
                Clay = minedResources.Clay - bluePrint.ObsisdianBotOreCost;
                ObsidianBots = minedResources.ObsidianBots + 1
            }
        }
      if resources.Ore >= bluePrint.GeodeBotOreCost && resources.Obsidian >= bluePrint.GeodeBotObsidianCost then
        yield {
          minedState with
            Resources = {
              minedResources with
                Ore = minedResources.Ore - bluePrint.GeodeBotOreCost;
                Obsidian = minedResources.Obsidian - bluePrint.GeodeBotObsidianCost;
                GeodeBots = minedResources.GeodeBots + 1
            }
        }
    }

  let private stateWeight (state: State) =
    let resources = state |> stateResources
    let minutesLeft = state |> stateMinutesLeft
    let initialWeight = 100000 - resources.Geode

    let weight =
      if minutesLeft > 1 then
        let potentialGeoBots = [2..minutesLeft] |> List.map (fun anotherGeoBot -> anotherGeoBot - 1) |> List.sum
        initialWeight - potentialGeoBots
      else initialWeight - minutesLeft

    if minutesLeft > 0 then weight - (minutesLeft * resources.GeodeBots) else weight

  let private blueprintMaxGeodes (totalMins: int) (bluePrints: list<Blueprint>) =
    let initialState = {
      Resources = {
        Ore = 0;
        Clay = 0;
        Obsidian = 0;
        Geode = 0;
        OreBots = 1;
        ClayBots = 0;
        ObsidianBots = 0;
        GeodeBots = 0;
      };
      MinutesLeft = totalMins;
    }

    bluePrints
      |> List.map (fun blueprint -> blueprint, findMaxWeight initialState isLastState (getNextStatesBuider blueprint) stateWeight stateResources)
      |> List.map (fun (inp, (finalState::_,_)) -> (inp, finalState))

  let part1 (text: string) =
    text
      |> parse
      |> blueprintMaxGeodes 24
      |> List.map (fun (blueprint, finalState) -> blueprint.Id * (finalState |> stateResources).Geode)
      |> List.sum

  let part2 (text: string) =
    text
      |> parse
      |> safeTake3
      |> blueprintMaxGeodes 32
      |> List.map (fun (_, finalState) -> (finalState |> stateResources).Geode)
      |> List.reduce (*)

let input = File.ReadAllText(Path.Combine(__SOURCE_DIRECTORY__, "input.txt"))

let resultPart1 = EnoughMinerals.part1 input
printfn "Result part1: %i" resultPart1

let resultPart2 = EnoughMinerals.part2 input
printfn "Result part2: %i" resultPart2
