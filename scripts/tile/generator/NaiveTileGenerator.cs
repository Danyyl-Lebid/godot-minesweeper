using System;
using Minesweeper.scripts.engine.settings;
using Minesweeper.scripts.tile.grid;
using Minesweeper.scripts.tile.state;

namespace Minesweeper.scripts.tile.generator;

internal class NaiveTileGenerator : ITileGenerator
{
    public TileGrid Generate(TileGrid tileGrid, GameSettings settings, Tile firstTile)
    {
        settings ??= new GameSettings();

        Random random = new Random();
        
        for (int i = 0; i < settings.Mines; i++)
        {
            tileGrid.Tiles[random.Next(0, settings.Width), random.Next(0, settings.Height)].State.HasMine = true;
        }

        return tileGrid;
    }
}