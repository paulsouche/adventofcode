using System.Text;

namespace Namespace;

public class Engine
{
    private readonly IEnumerable<string> RawCells;
    private readonly List<PartNumber> PartNumbers;
    public Engine(string input)
    {
        RawCells = input.Split('\n');
        StringBuilder sb = new();
        PartNumbers = RawCells.Select((line, y) => (line, y)).Aggregate(new List<PartNumber>(), (accy, nexty) =>
        {
            var lineLength = nexty.line.Length;
            return accy.Concat(nexty.line.ToCharArray().Select((c, x) => (c, x)).Aggregate(new List<PartNumber>(), (accx, nextx) =>
            {
                if (char.IsNumber(nextx.c))
                {
                    sb.Append(nextx.c);
                    if (nextx.x == lineLength - 1)
                    {
                        accx.Add(new PartNumber(sb.ToString(), lineLength, nexty.y));
                        sb = new();
                    }
                }
                else if (sb.Length > 0)
                {
                    accx.Add(new PartNumber(sb.ToString(), nextx.x, nexty.y));
                    sb = new();
                }

                return accx;
            })).ToList();
        });
    }

    private Dictionary<string, Cell> Cells
    {
        get => RawCells.SelectMany((l, y) => l.ToCharArray().Select((c, x) => new Cell(x, y, c))).ToDictionary(c => c.Coordinates, c => c);
    }

    private IEnumerable<string> GearCoordinates
    {
        get => Cells.Values.Where(c => c.IsGear).Select(c => c.Coordinates);
    }

    public int PartNumbersSum
    {
        get => PartNumbers.Where(pn => pn.IsAdjacentToSymbol(Cells)).Select(pn => pn.Value).Sum();
    }

    public int GearsRatioSum
    {
        get => GearCoordinates.Select(gc =>
        {
            var partNumbers = PartNumbers.Where(pn => pn.IsAdjacentToGear(gc));
            return partNumbers.Count() == 2 ? partNumbers.First().Value * partNumbers.Last().Value : 0;
        }).Sum();
    }
}
