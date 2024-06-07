using Godot;
using System;

public partial class Main : Node
{
	private int _score;

	[Export]
	public PackedScene MobScene {get;set;}

	private void OnPlayerHit()
	{
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();

		GetNode<HeadsUpDisplay>("HeadsUpDisplay").ShowGameOver();
		GetNode<AudioStreamPlayer2D>("Music").Stop();
		GetNode<AudioStreamPlayer2D>("DeathSound").Play();
	}

	private void OnMobTimerTimeout()
	{
		var mob = MobScene.Instantiate<Mob>();
		var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
		mobSpawnLocation.ProgressRatio = GD.Randf();
		
		mob.Position = mobSpawnLocation.Position;
		
		var direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;
		direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
		mob.Rotation = direction;

        mob.LinearVelocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0).Rotated(direction);

		AddChild(mob);
	}

	private void OnScoreTimerTimeout()
	{
		GetNode<HeadsUpDisplay>("HeadsUpDisplay").UpdateScore(++_score);
	}

	private void OnStartTimerTimeout()
	{
		GetNode<Timer>("MobTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();
	}

	protected void NewGame()
	{
		_score = 0;
		var player = GetNode<Player>("Player");
		var startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);

		GetNode<Timer>("StartTimer").Start();

		var hud = GetNode<HeadsUpDisplay>("HeadsUpDisplay");
		hud.UpdateScore(_score);
		hud.ShowMessage("GET READY!");

		GetTree().CallGroup("mobs", Node.MethodName.QueueFree);

		GetNode<AudioStreamPlayer2D>("Music").Play();
	}
}
