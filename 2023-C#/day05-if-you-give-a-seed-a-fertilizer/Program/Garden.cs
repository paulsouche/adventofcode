namespace Namespace;

public class Garden
{
    private static readonly string StatusStart = "seed";
    private static readonly string StatusEnd = "location";
    private readonly List<long> Seeds;
    private readonly List<(long start, long end)> SeedsRanges;
    private readonly Dictionary<string, StepMap> SeedsStepMaps;
    private readonly Dictionary<string, StepMap> SeedsRangesStepMaps;

    public Garden(string input)
    {
        var inputList = input.Split("\n\n").ToList();
        Seeds = inputList.First().Replace("seeds: ", "").Split(' ').Select(long.Parse).ToList();
        SeedsRanges = Seeds.Chunk(2).Select(chunk => (chunk.First(), chunk.Last())).ToList();
        var maps = inputList.Skip(1).Select(i => new StepMap(i));
        SeedsStepMaps = maps.ToDictionary(sm => sm.From, sm => sm);
        SeedsRangesStepMaps = maps.ToDictionary(sm => sm.To, sm => sm);
    }

    public long LowestLocationNumberForSeeds
    {
        get
        {
            string status = StatusStart;
            var seeds = Seeds;
            while (status != StatusEnd)
            {
                var map = SeedsStepMaps[status];
                seeds = seeds.Select(s =>
                {
                    var range = map.Ranges.Find(r => s >= r.SourceRangeStart && s < r.SourceRangeStart + r.Length);
                    if (range != null) return range.DestinationRangeStart + s - range.SourceRangeStart;
                    return s;
                }).ToList();
                status = map.To;
            }
            return seeds.Min();
        }
    }

    public long GetLowestLocationNumberForSeedsRanges(long seedStart = 0)
    {
        long seed = seedStart;
        while (true)
        {
            string status = StatusEnd;
            var prevSeed = seed;
            while (status != StatusStart)
            {
                var map = SeedsRangesStepMaps[status];
                var range = map.Ranges.Find(r => prevSeed >= r.DestinationRangeStart && prevSeed < r.DestinationRangeStart + r.Length);
                if (range != null) prevSeed = range.SourceRangeStart + prevSeed - range.DestinationRangeStart;
                status = map.From;
            }

            if (SeedsRanges.Any(sr => prevSeed >= sr.start && prevSeed < sr.start + sr.end)) break;

            seed++;
        }

        return seed;
    }
}
