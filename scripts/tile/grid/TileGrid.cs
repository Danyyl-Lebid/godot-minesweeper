using System.Collections.Generic;
using Godot;
using Minesweeper.scripts.engine.settings;

namespace Minesweeper.scripts.tile.grid;

public partial class TileGrid : Control
{
	private static readonly PackedScene Scene = GD.Load<PackedScene>("uid://clufwg8r40hbh");
	
	public const int DefaultWidth = 16;
	public const int DefaultHeight = 16;

	public Tile[,] Tiles { get; private set; }
	public HashSet<Tile> TileSet { get; } = []; 
	public int Width { get; private set; }
	public int Height { get; private set; }

	public int TileSize { get; private set; }

	public TileGrid() : this(DefaultWidth, DefaultHeight)
	{
	}

	private TileGrid(int width, int height, int tileSize = Tile.DefaultSize)
	{
		Width = width;
		Height = height;
		TileSize = tileSize;
	}

	public static TileGrid Create(GameSettings settings, ITileStateObserver observer)
	{
		var tileGrid = Scene.Instantiate<TileGrid>();
		tileGrid.Width = settings.Width;
		tileGrid.Height = settings.Height;
		tileGrid.TileSize = settings.TileSize;
		tileGrid.Tiles = new Tile[tileGrid.Width, tileGrid.Height];


		for (int i = 0; i < tileGrid.Width; i++)
		{
			for (int j = 0; j < tileGrid.Height; j++)
			{
				Tile tile = Tile.Create(i, j, tileGrid.TileSize);
				tile.SetPosition(new Vector2(i * tileGrid.TileSize, j * tileGrid.TileSize));
				tileGrid.Tiles[i, j] = tile;
				tileGrid.TileSet.Add(tile);

				tile.Subscribe(observer);
				
				tileGrid.AddChild(tile);
			}
		}

		return tileGrid;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void UpdateScale(int scale)
	{
		TileSize = scale;

		foreach (var tile in TileSet)
		{
			tile.SetPosition(new Vector2(tile.X * TileSize, tile.Y * TileSize));
			tile.SetSize(scale);
		}
	}

	public HashSet<Tile> GetNeighbors(Tile tile)
	{
		HashSet<Tile> neighbors = [];
		for (int i = -1; i < 2; i++)
		{
			for (int j = -1; j < 2; j++)
			{
				if (i == 0 && j == 0) continue;

				if (tile.X + i >= 0
					&& tile.X + i < Width
					&& tile.Y + j >= 0
					&& tile.Y + j < Height)
				{
					neighbors.Add(Tiles[tile.X + i, tile.Y + j]);
				}
			}
		}

		return neighbors;
	}
}
