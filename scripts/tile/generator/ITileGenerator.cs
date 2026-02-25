using Minesweeper.scripts.engine.settings;
using Minesweeper.scripts.tile.grid;
using Minesweeper.scripts.tile.state;

namespace Minesweeper.scripts.tile.generator;

public interface ITileGenerator
{
    TileGrid Generate(TileGrid tileGrid, GameSettings settings, Tile firstTile);
}