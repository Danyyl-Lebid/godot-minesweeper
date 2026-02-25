using System.Linq;
using Godot;
using Minesweeper.scripts.engine.settings;
using Minesweeper.scripts.engine.state;
using Minesweeper.scripts.tile;
using Minesweeper.scripts.tile.generator;
using Minesweeper.scripts.tile.grid;

namespace Minesweeper.scripts.engine;

public partial class GameEngine : Node, ITileStateObserver
{
	private delegate void GameEventHandler();
	
	private event GameEventHandler GameStartedEvent;
	private event GameEventHandler GameOverEvent;
	private event GameEventHandler GameWonEvent;
	private event GameEventHandler FlagAddedEvent;
	private event GameEventHandler FlagRemovedEvent;

	private GameState _gameState;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Subscribe(IEngineUiMediator mediator)
	{
		GameStartedEvent += mediator.OnGameStarted;
		GameOverEvent += mediator.OnGameOver;
		GameWonEvent += mediator.OnGameWon;
		FlagAddedEvent += mediator.OnFlagAdded;
		FlagRemovedEvent += mediator.OnFlagRemoved;
	}

	public void NewGame(TileGrid tileGrid, GameSettings settings)
	{
		_gameState = new GameState(tileGrid, settings);
	}

	public void OnTileOpened(Tile tileOpened)
	{
		if (!_gameState.IsGameStarted)
		{
			TileGeneratorProvider.Get().Generate(_gameState.TileGrid, _gameState.Settings, tileOpened);
			_gameState.UpdateMinedTiles();
			GameStartedEvent?.Invoke();
			_gameState.IsGameStarted = true;
		}

		var neighbors = _gameState.TileGrid.GetNeighbors(tileOpened);
		int minedNeighbors = neighbors.Count(neighbor => neighbor.State.HasMine);

		tileOpened.Open(minedNeighbors);

		if (minedNeighbors == 0)
		{
			foreach (var neighbor in neighbors.Where(neighbor => !neighbor.State.IsOpened))
			{
				neighbor.OnOpenPressed();
			}
		}
		
		if (_gameState.Tiles
			.Where(tile => !tile.State.IsOpened)
			.Any(tile => !_gameState.MinedTiles.Contains(tile))
			) return;
		
		if (_gameState.IsGameOver) return;
		
		_gameState.IsGameOver = true;
		GameWonEvent?.Invoke();
	}

	public void OnTileExploded(Tile tile)
	{
		_gameState.IsGameOver = true;
		GameOverEvent?.Invoke();
		OpenAll();
	}

	public void OnFlagAdded(Tile tile)
	{
		_gameState.FlaggedTilesCount++;
		FlagAddedEvent?.Invoke();
	}

	public void OnFlagRemoved(Tile tile)
	{
		_gameState.FlaggedTilesCount--;
		FlagRemovedEvent?.Invoke();
	}

	public bool OnFlagPermission(Tile tile)
	{
		return _gameState.FlaggedTilesCount < _gameState.Settings.Mines;
	}

	public void OpenAll()
	{
		foreach (var tile in _gameState.Tiles.Where(tile => !tile.State.IsOpened))
		{
			if (tile.State.HasMine)
			{
				tile.OpenMined();
				continue;
			}

			var neighbors = _gameState.TileGrid.GetNeighbors(tile);

			int minedNeighbors = neighbors.Count(neighbor => neighbor.State.HasMine);

			tile.Open(minedNeighbors);
		}
	}
}
