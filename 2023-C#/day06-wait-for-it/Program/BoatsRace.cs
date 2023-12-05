namespace Namespace;

public class BoatsRace
{
    private static List<long> AllPossibleResults(long time)
    {
        List<long> results = new();
        for (long i = 0; i <= time; i++)
        {
            results.Add(i * (time - i));
        }
        return results;
    }

    private static long NumberOfWaysToBeatTheRecord(Race race)
    {
        return AllPossibleResults(race.Time).Where(r => r > race.Record).Count();
    }

    private readonly List<Race> Races;

    public BoatsRace(string input, bool onlyOneRace = false)
    {
        var parsedInput = input.Split('\n').ToList();
        var times = parsedInput.First().Replace("Time:", "").Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
        var records = parsedInput.Last().Replace("Distance:", "").Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
        Races = new();
        if (onlyOneRace)
        {
            Races.Add(new Race(long.Parse(string.Join("", times)), long.Parse(string.Join("", records))));
        }
        else
        {
            for (int i = 0; i < times.Count; i++)
            {
                Races.Add(new Race(times[i], records[i]));
            }
        }
    }

    public long GetProductOfAllBetterRecords()
    {
        long product = 1;
        foreach (Race race in Races)
        {
            product *= NumberOfWaysToBeatTheRecord(race);
        }
        return product;
    }

    public long GetSumOfAllBetterRecords()
    {
        return Races.Select(r => NumberOfWaysToBeatTheRecord(r)).Sum();
    }

}
