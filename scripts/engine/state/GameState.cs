using System.Collections.Generic;
using System.Linq;
using Minesweeper.scripts.engine.settings;
using Minesweeper.scripts.tile;
using Minesweeper.scripts.tile.grid;

namespace Minesweeper.scripts.engine.state;

public class GameState (TileGrid tileGrid, GameSettings settings)
{
    public TileGrid TileGrid { get; } = tileGrid;
    public GameSettings Settings { get; } = settings;
    public bool IsGameStarted { get; set; } = false;
    public bool IsGameOver { get; set; } = false;
    public HashSet<Tile> Tiles { get; } = tileGrid.TileSet;
    public HashSet<Tile> MinedTiles { get; } = [];
    public int FlaggedTilesCount { get; set; } = 0;

    public void UpdateMinedTiles()
    {
        foreach (var tile in TileGrid.TileSet.Where(tile => tile.State.HasMine))
        {
            MinedTiles.Add(tile);
        }
    }
}