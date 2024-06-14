using Godot;
using System;

public partial class Main : Node
{
	[Export]
	public PackedScene AsteroidScene { get; set; }

	private bool _spawned = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Timer>("AsteroidTimer").Start();
	}

	private void OnAsteroidTimerTimeout()
	{
		// if (_spawned) return;
		_spawned = true;

		var asteroid = AsteroidScene.Instantiate<Asteroid>();
		var asteroidSpawnLocation = GetNode<PathFollow2D>("AsteroidPath/AsteroidSpawnLocation");
		asteroidSpawnLocation.ProgressRatio = GD.Randf();
		
		asteroid.Position = asteroidSpawnLocation.Position;

		var direction = asteroidSpawnLocation.Rotation + Mathf.Pi / 2;
		direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
		asteroid.LinearVelocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0).Rotated(direction);
		
		// AddChild(asteroid);
	}
}
