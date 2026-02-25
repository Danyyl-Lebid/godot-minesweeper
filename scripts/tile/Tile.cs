using Godot;
using Minesweeper.scripts.tile.sprite;
using Minesweeper.scripts.tile.state;

namespace Minesweeper.scripts.tile;

public partial class Tile(int x, int y, int size = 16) : Node2D
{
	private static readonly PackedScene Scene = GD.Load<PackedScene>("uid://c313yn1vc8ntb");

	public const int DefaultSize = 16;

	private Button _button;
	private Sprite2D _sprite;
	private Sprite2D _markSprite;

	public int X { get; private set; } = x;
	public int Y { get; private set; } = y;
	public TileState State { get; } = new();
	private int Size { get; set; } = size;

	private delegate void TileEvent(Tile tile);
	private delegate bool FlagPermissionEvent(Tile tile);

	private event TileEvent TileOpenedEvent;
	private event TileEvent TileExplodedEvent;
	private event TileEvent FlagAddedEvent;
	private event TileEvent FlagRemovedEvent;

	private event FlagPermissionEvent AddFlagPermissionEvent;
	
	public Tile() : this(0, 0)
	{
	}

	public static Tile Create(int x, int y, int size = DefaultSize)
	{
		Tile tile = Scene.Instantiate<Tile>();
		tile.X = x;
		tile.Y = y;
		tile.Size = size;
		tile.Scale = new Vector2((float)size / DefaultSize, (float)size / DefaultSize);
		return tile;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_button = GetNode<Button>("Button");
		_sprite = GetNode<Sprite2D>("Sprite");
		_markSprite = GetNode<Sprite2D>("MarkSprite");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Subscribe(ITileStateObserver observer)
	{
		TileOpenedEvent += observer.OnTileOpened;
		TileExplodedEvent += observer.OnTileExploded;
		FlagAddedEvent += observer.OnFlagAdded;
		FlagRemovedEvent += observer.OnFlagRemoved;

		AddFlagPermissionEvent += observer.OnFlagPermission;
	}

	private void OnInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			if (!mouseEvent.Pressed && IsMouseEventOnButton(mouseEvent))
			{
				if (mouseEvent.ButtonIndex == MouseButton.Left)
				{
					OnOpenPressed();
				}
				else if (mouseEvent.ButtonIndex == MouseButton.Right)
				{
					OnMarkedPressed();
				}
			}
		}
	}

	private bool IsMouseEventOnButton(InputEventMouseButton @event)
	{
		return
			@event.Position.X >= 0 &&
			@event.Position.X < Size &&
			@event.Position.Y >= 0 &&
			@event.Position.Y < Size;
	}

	private void OnMarkedPressed()
	{
		if (State.IsFlagged())
		{
			FlagRemovedEvent?.Invoke(this);
		}

		if (AddFlagPermissionEvent != null)
		{
			if (!AddFlagPermissionEvent.Invoke(this))
			{
				GD.Print("Too many flags on board.");
				return;
			}
		}
		
		_markSprite.Texture = TextureDictionary.GetMarkSprite(State.NextMark());

		if (State.IsFlagged())
		{
			FlagAddedEvent?.Invoke(this);
		}
	}

	public void OnOpenPressed()
	{
		if (State.IsFlagged())
		{
			return;
		}

		State.ResetMark();
		_markSprite.Texture = null;

		if (State.HasMine)
		{
			Explode();
		}
		else
		{
			TileOpenedEvent?.Invoke(this);
		}
	}

	public void Open(int neighborsCount)
	{
		ResetMark();
		_sprite.Texture = TextureDictionary.GetTileSprite(neighborsCount);
		ToggleVisibility();
		State.IsOpened = true;
	}

	public void OpenMined()
	{
		ResetMark();
		_sprite.Texture = TextureDictionary.GetMinedSprite();
		ToggleVisibility();
		State.IsOpened = true;
	}

	public void Explode()
	{
		ResetMark();
		_sprite.Texture = TextureDictionary.GetExplodedSprite();
		ToggleVisibility();
		State.IsOpened = true;
		TileExplodedEvent?.Invoke(this);
	}

	public void SetSize(int tileSize)
	{
		Scale = new Vector2((float)tileSize / DefaultSize, (float)tileSize / DefaultSize);
	}

	private void ToggleVisibility()
	{
		_button.Visible = false;
		_sprite.Visible = true;
	}

	private void ResetMark()
	{
		State.ResetMark();
		_markSprite.Texture = null;
	}
}
