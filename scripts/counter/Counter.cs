using System;
using System.Collections.Generic;
using Godot;

namespace Minesweeper.scripts.counter;

public partial class Counter : Control
{
	[Export] public int Value { get; set; }
	[Export] public int MaxValue { get; set; }

	private const int Margin = 5;

	private HBoxContainer _digitsContainer;

	private TextureRect[] _sprites;
	private int _digits;

	private int _originalValue;

	private static readonly Dictionary<int, Texture2D> Textures = new()
	{
		{ -1, GD.Load<Texture2D>("uid://cbide6kvnmrt5") }, //empty tableau
		{ 0, GD.Load<Texture2D>("uid://c71xpb6j55eyx") },
		{ 1, GD.Load<Texture2D>("uid://xtkaxl5q4lhb") },
		{ 2, GD.Load<Texture2D>("uid://cl7gumtqohtxn") },
		{ 3, GD.Load<Texture2D>("uid://dlxtyqw1gdai1") },
		{ 4, GD.Load<Texture2D>("uid://cocu6mhnw0oun") },
		{ 5, GD.Load<Texture2D>("uid://jliteol3uunt") },
		{ 6, GD.Load<Texture2D>("uid://cgoqg33a7bym") },
		{ 7, GD.Load<Texture2D>("uid://c56jivpona8i6") },
		{ 8, GD.Load<Texture2D>("uid://c2xx70gtp1wxi") },
		{ 9, GD.Load<Texture2D>("uid://b3igr1okrn2l4") },
	};

	private static readonly int TextureWidth = Textures[-1].GetWidth();
	private static readonly int TextureHeight = Textures[-1].GetHeight();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_originalValue = Value;
		
		_digitsContainer = GetNode<HBoxContainer>("MarginContainer/HBoxContainer");

		Redraw();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Redraw()
	{
		_digits = 0;

		if (_sprites != null)
		{
			foreach (var sprite in _sprites)
			{
				_digitsContainer.RemoveChild(sprite);
				sprite.QueueFree();
			}
		}
		
		for (int tmp = MaxValue; tmp > 0; tmp /= 10)
		{
			_digits++;
		}

		CustomMinimumSize = new Vector2(2 * Margin + TextureWidth * _digits, 2 * Margin + TextureHeight);

		_sprites = new TextureRect[_digits];
		for (int i = 0; i < _digits; i++)
		{
			_sprites[i] = new TextureRect();
			_digitsContainer.AddChild(_sprites[i]);
		}

		DisplayNewValue();
	}

	public void NewValues(int maxValue, int value)
	{
		Value = value;
		MaxValue = maxValue;
		_originalValue = Value;
		Redraw();
	}

	public void Increment()
	{
		Value = Value >= MaxValue ? MaxValue : Value + 1;
		DisplayNewValue();
	}

	public void Decrement()
	{
		Value = Value <= 0 ? 0 : Value - 1;
		DisplayNewValue();
	}

	public void Reset()
	{
		Value = _originalValue;
		DisplayNewValue();
	}

	private void DisplayNewValue()
	{
		for (int i = 0; i < _digits; i++)
		{
			int digit = (int)(Value / Math.Pow(10, _digits - i - 1) % 10);
			_sprites[i].Texture = Textures[digit];
		}
	}
}
