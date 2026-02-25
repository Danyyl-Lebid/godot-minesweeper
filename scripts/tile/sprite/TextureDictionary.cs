using Godot;
using Godot.Collections;
using Minesweeper.scripts.tile.state;

namespace Minesweeper.scripts.tile.sprite;

public static class TextureDictionary
{
    private static readonly Dictionary<int, Texture2D> TileSpriteDictionary = new()
    {
        { -2, GD.Load<Texture2D>("uid://br8m1bmt4ywci") }, // exploded tile
        { -1, GD.Load<Texture2D>("uid://4kgevr7b6aqh") },
        { 0, GD.Load<Texture2D>("uid://bvwmhlg46rggq") },
        { 1, GD.Load<Texture2D>("uid://bxefjgodn2tyy") },
        { 2, GD.Load<Texture2D>("uid://bcnr6711j1s25") },
        { 3, GD.Load<Texture2D>("uid://capcfaosr5m4c") },
        { 4, GD.Load<Texture2D>("uid://ddnrir8jhdwqn") },
        { 5, GD.Load<Texture2D>("uid://m3gj0w41txpr") },
        { 6, GD.Load<Texture2D>("uid://dhqm5whf5atit") },
        { 7, GD.Load<Texture2D>("uid://cry4lmybpkte8") },
        { 8, GD.Load<Texture2D>("uid://jthmjwkuxd6h") },
    };

    private static readonly Dictionary<TileState.Mark, Texture2D> MarkSpriteDictionary = new()
    {
        { TileState.Mark.None, null },
        { TileState.Mark.Flagged, GD.Load<Texture2D>("uid://dnih3hgn7ply7") },
        { TileState.Mark.Question, GD.Load<Texture2D>("uid://dx3ry8j0emxvk") }
    };


    public static Texture2D GetTileSprite(int neighbors)
    {
        return TileSpriteDictionary[neighbors];
    }

    public static Texture2D GetMinedSprite()
    {
        return GetTileSprite(-1);
    }

    public static Texture2D GetExplodedSprite()
    {
        return GetTileSprite(-2);
    }

    public static Texture2D GetMarkSprite(TileState.Mark mark)
    {
        return MarkSpriteDictionary[mark];
    }
}