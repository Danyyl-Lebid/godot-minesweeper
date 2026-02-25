using Godot;
using Minesweeper.scripts.tile.grid;

namespace Minesweeper.scripts.ui;

public partial class PlayingArea : Control
{
	private const int Margin = 2;
	private const string TileGridName = "TileGrid";
	
	private bool _isInitialized = false;
	private MarginContainer _tilesMarginContainer = new();
		
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		_tilesMarginContainer.AddThemeConstantOverride("margin_top", Margin);
		_tilesMarginContainer.AddThemeConstantOverride("margin_bottom", Margin);
		_tilesMarginContainer.AddThemeConstantOverride("margin_left", Margin);
		_tilesMarginContainer.AddThemeConstantOverride("margin_right", Margin);
		_tilesMarginContainer.SetAnchorsPreset(LayoutPreset.FullRect);
		
		AddChild(_tilesMarginContainer);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SetTileGrid(TileGrid tileGrid)
	{
		if (_isInitialized)
		{
			var oldTileGrid = _tilesMarginContainer.GetNode(TileGridName);
			_tilesMarginContainer.RemoveChild(oldTileGrid);
			oldTileGrid.Free();
		}
		
		CustomMinimumSize = new Vector2(tileGrid.Width *  tileGrid.TileSize + Margin * 2, tileGrid.Height * tileGrid.TileSize + Margin * 2);
		
		tileGrid.SetName(TileGridName);
		_tilesMarginContainer.AddChild(tileGrid);
		
		_isInitialized = true;
	}

	public void UpdateScale(int scale)
	{
		if (!_isInitialized) return;
		TileGrid tileGrid = _tilesMarginContainer.GetNode<TileGrid>(TileGridName);
		tileGrid.UpdateScale(scale);
		CustomMinimumSize = new Vector2(tileGrid.Width *  tileGrid.TileSize + Margin * 2, tileGrid.Height * tileGrid.TileSize + Margin * 2);
	}
}
