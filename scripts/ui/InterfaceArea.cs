using Godot;
using Minesweeper.scripts.counter;
using Minesweeper.scripts.engine.settings;

namespace Minesweeper.scripts.ui;

public partial class InterfaceArea : Control
{
	private const float MaxMinesToBoardSizeRatio = 0.4f;

	private Button _newGameButton;
	private SpinBox _widthInput;
	private SpinBox _heightInput;
	private SpinBox _minesInput;
	private SpinBox _scaleInput;

	private Counter _minesCounter;
	private Counter _timeCounter;
	private Timer _timer;

	private delegate void NewGameHandler(GameSettings settings);

	private delegate void ScaleHandler(int scale);

	private event NewGameHandler NewGameEvent;
	private event ScaleHandler ScaleEvent;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_newGameButton = GetNode<Button>("MarginContainer/HBoxContainer/NewGameButton");
		_widthInput = GetNode<SpinBox>("MarginContainer/HBoxContainer/GameSettingsGrid/WidthInput");
		_heightInput = GetNode<SpinBox>("MarginContainer/HBoxContainer/GameSettingsGrid/HeightInput");
		_minesInput = GetNode<SpinBox>("MarginContainer/HBoxContainer/GameSettingsGrid/MinesInput");
		_scaleInput = GetNode<SpinBox>("MarginContainer/HBoxContainer/ScaleGrid/ScaleInput");

		_minesCounter = GetNode<Counter>("MarginContainer/MinesCounter");
		_timeCounter = GetNode<Counter>("MarginContainer/TimeCounter");
		_timer = GetNode<Timer>("Timer");

		_widthInput.Value = GameSettings.Default.Width;
		_heightInput.Value = GameSettings.Default.Height;
		_minesInput.Value = GameSettings.Default.Mines;
		_scaleInput.Value = GameSettings.Default.TileSize;
		
		_minesInput.MaxValue = (int)(_widthInput.Value * _heightInput.Value * MaxMinesToBoardSizeRatio);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Subscribe(IEngineUiMediator mediator)
	{
		NewGameEvent += mediator.OnNewGame;
		ScaleEvent += mediator.OnScaleChange;
	}

	public void OnNewGameButtonPressed()
	{
		GameSettings settings = new GameSettings()
			.SetWidth((int)_widthInput.Value)
			.SetHeight((int)_heightInput.Value)
			.SetMines((int)_minesInput.Value)
			.SetTileSize((int)_scaleInput.Value);

		_timeCounter.Reset();

		NewGameEvent?.Invoke(settings);

		_minesCounter.NewValues(settings.Mines, settings.Mines);
	}

	public void OnGameStarted()
	{
		_timer.Start();
	}

	public void OnScaleChanged(float scale)
	{
		_scaleInput.Value = (int)scale;
		ScaleEvent?.Invoke((int)scale);
	}

	public void OnBoardSizeChanged(float value)
	{
		GD.Print("Board size changed");
		_minesInput.MaxValue = (int)(_widthInput.Value * _heightInput.Value * MaxMinesToBoardSizeRatio);
		GD.Print("Max mined: " + _minesInput.MaxValue);
		if (_minesInput.Value > _minesInput.MaxValue)
		{
			GD.Print("Mines overflowing");
			_minesInput.Value = _minesInput.MaxValue;
		}
	}

	public void OnTimerTick()
	{
		_timeCounter.Increment();
	}

	public void OnGameOver()
	{
		_timer.Stop();
	}

	public void OnGameWon()
	{
		_timer.Stop();
	}

	public void OnFlagAdded()
	{
		_minesCounter.Decrement();
	}

	public void OnFlagRemoved()
	{
		_minesCounter.Increment();
	}
}
