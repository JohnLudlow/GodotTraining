using Godot;
using System;

public partial class Score : Control
{
	private Label _valueLabel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        _valueLabel = GetNode<Label>("Panel/ScoreLabel");
        Reset();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void AddToScore(int value)
    {
        if(int.TryParse(_valueLabel?.Text, out var currentScore))
        {
            _valueLabel.Text = (currentScore + value).ToString();
        }
    }

    public void Reset()
    {
        _valueLabel.Text = "0";
    }
}
