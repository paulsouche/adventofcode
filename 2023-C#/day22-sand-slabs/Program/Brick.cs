
namespace Namespace;

public record Brick(Range X, Range Y, Range Z)
{
    public int Top => Z.End.Value;
    public int Bottom => Z.Start.Value;
}
