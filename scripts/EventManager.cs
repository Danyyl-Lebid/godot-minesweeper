using Godot;
using Minesweeper.scripts.engine;
using Minesweeper.scripts.engine.settings;
using Minesweeper.scripts.tile.grid;
using Minesweeper.scripts.ui;

namespace Minesweeper.scripts;

public partial class EventManager : Node, IEngineUiMediator
{

	private InterfaceArea _interfaceArea;
	private PlayingArea _playingArea;
	private GameEngine _engine;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public EventManager SetInterfaceArea(InterfaceArea interfaceArea)
	{
		_interfaceArea = interfaceArea;
		return this;
	}
	
	public EventManager SetPlayingArea(PlayingArea playingArea)
	{
		_playingArea = playingArea;
		return this;
	}

	public EventManager SetEngine(GameEngine engine)
	{
		_engine = engine;
		return this;
	}

	public EventManager Build()
	{
		_interfaceArea.Subscribe(this);
		_engine.Subscribe(this);
		return this;
	}

	public void OnScaleChange(int scale)
	{
		_playingArea.UpdateScale(scale);
	}

	public void OnNewGame(GameSettings settings)
	{
		TileGrid tileGrid = TileGrid.Create(settings, _engine);
		
		_playingArea.SetTileGrid(tileGrid);
		_engine.NewGame(tileGrid, settings);
		
		_playingArea.ProcessMode = ProcessModeEnum.Always;
	}

	public void OnGameStarted()
	{
		_interfaceArea.OnGameStarted();
	}

	public void OnGameOver()
	{
		_playingArea.ProcessMode = ProcessModeEnum.Disabled;
		_interfaceArea.OnGameOver();
		GD.Print("Better luck next time!");
	}

	public void OnGameWon()
	{
		_playingArea.ProcessMode = ProcessModeEnum.Disabled;
		_interfaceArea.OnGameWon();
		GD.Print("Good job!");
	}

	public void OnFlagAdded()
	{
		_interfaceArea.OnFlagAdded();
	}

	public void OnFlagRemoved()
	{
		_interfaceArea.OnFlagRemoved();
	}
}
