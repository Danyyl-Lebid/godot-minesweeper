using Godot;
using Minesweeper.scripts.engine;
using Minesweeper.scripts.ui;

namespace Minesweeper.scripts;

public partial class MainScene : Control
{

	private EventManager _eventManager;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_eventManager = GetNode<EventManager>("EventManager")
			.SetInterfaceArea(GetNode<InterfaceArea>("UI/MarginContainer/VBoxContainer/InterfaceArea"))
			.SetPlayingArea(GetNode<PlayingArea>("UI/MarginContainer/VBoxContainer/PlayingArea"))
			.SetEngine(GetNode<GameEngine>("GameEngine"))
			.Build();
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
