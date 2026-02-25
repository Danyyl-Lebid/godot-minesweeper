namespace Minesweeper.scripts.tile.generator;

public static class TileGeneratorProvider
{
    private static ITileGenerator _tileGenerator;

    public static ITileGenerator Get()
    {
        _tileGenerator ??= new SimpleTileGenerator();
        return _tileGenerator;
    }
}