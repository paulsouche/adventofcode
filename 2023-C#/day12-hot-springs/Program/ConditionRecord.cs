namespace Namespace;

public class ConditionRecord {
    public readonly string[] RawRecord;
    public ConditionRecord(string input, bool foldedUp = false)
    {
        if (foldedUp) {
            RawRecord = input.Split('\n').Select(l => {
                var rawLine = l.Split(' ');
                var record = string.Join('?', Enumerable.Range(0, 5).Select(i => rawLine.First()));
                var contigousDamagedSprings = string.Join(',', Enumerable.Range(0, 5).Select(i => rawLine.Last()));
                return $"{record} {contigousDamagedSprings}";
            }).ToArray();
        } else {
            RawRecord = input.Split('\n');
        }
    }

    private static List<int> ContigousDamagedSprings(string record)
    {
        return record.Split('.', StringSplitOptions.RemoveEmptyEntries).Select(g => g.Length).ToList();
    }

    private int PossibleArrangments(string record)
    {
        var possibleArrangments = 0;
        var rawRecord = record.Split(' ');
        var validContigousDamagedSprings = rawRecord.Last().Split(',').Select(int.Parse).ToList();
        var validDamagedSpringCount = validContigousDamagedSprings.Count;
        Queue<string> queue = new();
        queue.Enqueue(rawRecord.First());
        while (queue.Any()) {
            var testRecord = queue.Dequeue();
            var unknownIndex = testRecord.IndexOf('?');
            if (unknownIndex >= 0)
            {
                var stringUntilNow = testRecord.Remove(unknownIndex, testRecord.Length - unknownIndex);
                if (stringUntilNow.Contains('#')) {
                    var contigousDamagedSpringsUntilNow = ContigousDamagedSprings(stringUntilNow);
                    var damagedSpringCountUntilNow = contigousDamagedSpringsUntilNow.Count;
                    if (damagedSpringCountUntilNow > validDamagedSpringCount) continue;
                    if (contigousDamagedSpringsUntilNow.Where((v, i) => (i == (damagedSpringCountUntilNow - 1)) ? v > validContigousDamagedSprings[i] : v != validContigousDamagedSprings[i]).Any()) continue;
                }

                queue.Enqueue(testRecord.Remove(unknownIndex, 1).Insert(unknownIndex, "."));
                queue.Enqueue(testRecord.Remove(unknownIndex, 1).Insert(unknownIndex, "#"));
                continue;
            }

            var contigousDamagedSprings = ContigousDamagedSprings(testRecord);
            var damagedSpringCount = contigousDamagedSprings.Count;
            if (damagedSpringCount != validDamagedSpringCount) continue;
            if (contigousDamagedSprings.Where((v, i) => v != validContigousDamagedSprings[i]).Any()) continue;

            possibleArrangments++;
        }

        return possibleArrangments;
    }

    public int PossibleArrangmentsSum {
        get => RawRecord.Select(PossibleArrangments).Sum();
    }
}
