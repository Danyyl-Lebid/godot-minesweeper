namespace Minesweeper.scripts.engine.settings;

public class GameSettings(int width, int height, int mines, int tileSize)
{
    private const int DefaultWidth = 8;
    private const int DefaultHeight = 8;
    private const int DefaultMines = 16;
    private const int DefaultTileSize = 32;
    
    public static readonly GameSettings Default = new GameSettings(DefaultWidth, DefaultHeight, DefaultMines, DefaultTileSize);
    
    public GameSettings() : this(DefaultWidth, DefaultHeight, DefaultMines, DefaultTileSize)
    {
    }

    public int Width { get; private set; } = width;
    public int Height { get; private set; } = height;
    public int Mines { get; private set; } = mines;
    public int TileSize { get; private set; } = tileSize;

    public GameSettings SetWidth(int width)
    {
        Width = width;
        return this;
    }

    public GameSettings SetHeight(int height)
    {
        Height = height;
        return this;
    }

    public GameSettings SetMines(int mines)
    {
        Mines = mines;
        return this;
    }

    public GameSettings SetTileSize(int tileSize)
    {
        TileSize = tileSize;
        return this;
    }
}