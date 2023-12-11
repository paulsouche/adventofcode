namespace Namespace;

public class Universe
{
    private long ManhattanWithExpansion((Galaxy, Galaxy) galaxies)
    {
        var galaxy1 = galaxies.Item1;
        var galaxy2 = galaxies.Item2;
        var minY = Math.Min(galaxy1.Y, galaxy2.Y);
        var maxY = Math.Max(galaxy1.Y, galaxy2.Y);
        var yExpansion = RawCells.Where((l, y) => y > minY && y < maxY && l.All(c => c == '.')).Count() * (Expansion - 1);
        var minX = Math.Min(galaxy1.X, galaxy2.X);
        var maxX = Math.Max(galaxy1.X, galaxy2.X);
        var xExpansion = RawCells.First().Where((c, x) => x > minX && x < maxX && RawCells.All(l => l[x] == '.')).Count() * (Expansion - 1);

        return Math.Abs(galaxies.Item2.X - galaxies.Item1.X) + xExpansion + Math.Abs(galaxies.Item2.Y - galaxies.Item1.Y) + yExpansion;
    }

    private readonly char[][] RawCells;
    private readonly long Expansion;

    public Universe(string input, long expansion = 2)
    {
        Expansion = expansion;
        RawCells = input.Split('\n').Select((l) => l.ToCharArray()).ToArray();
    }

    private IEnumerable<IEnumerable<Cell>> Cells
    {
        get => RawCells.Select((l, y) => l.Select((c, x) => new Cell(x, y, c)));
    }

    private IEnumerable<Galaxy> Galaxies
    {
        get => Cells.SelectMany(l => l.Where(c => c.Value == '#').Select(c => new Galaxy(c.X, c.Y)));
    }

    private IEnumerable<(Galaxy, Galaxy)> GalaxiesPairs
    {
        get => Galaxies.SelectMany((fst, i) => Galaxies.Skip(i + 1).Select(snd => (fst, snd)));
    }

    public long DistanceBetweenGalaxiesSum
    {
        get => GalaxiesPairs.Select(ManhattanWithExpansion).Sum();
    }
}
