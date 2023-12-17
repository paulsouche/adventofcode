namespace Namespace;

public interface ICrucible
{
    string ToHash();
    bool IsOutOfGrid(int[][] grid);
    bool IsAtEnd((int, int) end);
    int HeatLoss(int[][] grid);
    int ManhattanDistance((int, int) pos);
    Directions Flip();
    ICrucible Walk(Directions d);
    ICrucible? Neighbour(Directions cardinal, int[][] grid);
}
