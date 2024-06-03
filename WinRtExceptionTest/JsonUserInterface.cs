namespace WinRtExceptionTest;

internal class JsonUserInterface
{ 
    public required List<Container> Containers { get; set; }
    public int ColumnsCount { get; set; }
    public required string CellAspectRatio { get; set; }
}

internal class Container
{
    public int StartX { get; set; }
    public int StartY { get; set; }
    public int SizeX { get; set; }
    public int SizeY { get; set; }
    public required string BackgroundColor { get; set; }
}
