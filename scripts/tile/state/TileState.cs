using Godot;

namespace Minesweeper.scripts.tile.state;

public partial class TileState : Node
{
	public enum Mark
	{
		None = 0,
		Flagged = 1,
		Question = 2
	}

	public bool HasMine { get; set; } = false;
	public bool IsOpened { get; set; } = false;
	private Mark _mark = Mark.None;

	public Mark GetMark()
	{
		return _mark;
	}

	public bool IsFlagged()
	{
		return _mark.Equals(Mark.Flagged);
	}

	public void ResetMark()
	{
		_mark = Mark.None;
	}

	public Mark GetNextMark()
	{
		if (_mark == Mark.Question) return Mark.None;
		return _mark + 1;
	}
	
	public Mark NextMark()
	{
		return _mark = GetNextMark();
	}
}
