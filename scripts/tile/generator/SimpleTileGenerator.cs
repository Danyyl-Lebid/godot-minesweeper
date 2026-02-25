using System;
using System.Collections.Generic;
using Minesweeper.scripts.engine.settings;
using Minesweeper.scripts.tile.grid;

namespace Minesweeper.scripts.tile.generator;

public class SimpleTileGenerator : ITileGenerator
{
    public TileGrid Generate(TileGrid tileGrid, GameSettings settings, Tile firstTile)
    {
        HashSet<Tile> minedTiles = [firstTile];
        foreach (var tile in tileGrid.GetNeighbors(firstTile))
        {
            minedTiles.Add(tile);
        }

        for (int i = 0; i < settings.Mines;)
        {
            Random random = new Random();
            int x = random.Next(0, settings.Width);
            int y = random.Next(0, settings.Height);
            Tile tile = tileGrid.Tiles[x, y];
            if (minedTiles.Add(tile))
            {
                tile.State.HasMine = true;
                i++;
            }
        }

        return tileGrid;
    }
}