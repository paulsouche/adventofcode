
namespace Namespace;

public record Supports(
    Dictionary<Brick, HashSet<Brick>> BricksAbove,
    Dictionary<Brick, HashSet<Brick>> BricksBelow
);
