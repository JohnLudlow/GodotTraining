using Godot;
using System;

public partial class Game : Node
{
    private Score _score;
    private Grid _grid;
    private Node2D _overlay;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        _score = GetNode<Score>("Score");
        _grid = GetNode<Grid>("Grid");
        _overlay = GetNode<Node2D>("GameOverOverlay");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void GameOver()
    {
        _overlay.Visible = true;
    }

    
    public void Restart()
    {
        _grid.Reset();
        _score.Reset();
        _overlay.Visible = false;
    }
}
