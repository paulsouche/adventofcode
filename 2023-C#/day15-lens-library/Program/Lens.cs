namespace Namespace;

public class Lens
{
    public readonly string Label;
    public int FocalLength { get; private set; }
    public Lens(string label, int focalLength)
    {
        Label = label;
        FocalLength = focalLength;
    }
    public void ChangeFocalLength(int focalLength)
    {
        FocalLength = focalLength;
    }
}
