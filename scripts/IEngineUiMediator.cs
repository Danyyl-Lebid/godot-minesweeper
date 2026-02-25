using Minesweeper.scripts.engine.settings;

namespace Minesweeper.scripts;

public interface IEngineUiMediator
{
    public void OnNewGame(GameSettings settings);
    public void OnScaleChange(int scale);
    public void OnGameStarted();
    public void OnGameOver();
    public void OnGameWon();
    public void OnFlagAdded();
    public void OnFlagRemoved();
}