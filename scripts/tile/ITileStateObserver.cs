namespace Minesweeper.scripts.tile;

public interface ITileStateObserver
{
    public void OnTileOpened(Tile tile);
    public void OnTileExploded(Tile tile);
    public void OnFlagAdded(Tile tile);
    public void OnFlagRemoved(Tile tile);
    public bool OnFlagPermission(Tile tile);
}