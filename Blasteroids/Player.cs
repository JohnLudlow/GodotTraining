using Godot;
using System;

public partial class Player : Area2D
{
	[Export]
	public float TurnSpeed { get; set; } = 3.0f;

	[Export]
	public PackedScene BulletScene { get; set; }

	Vector2 _screenSize = Vector2.Zero;



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var vw = GetViewportRect();
		_screenSize = vw.Size;
		Position = vw.GetCenter();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var velocity = Vector2.Zero;

		if (Input.IsActionPressed("turn_right"))
		{
			Rotation += TurnSpeed * (float)delta;
		}

		if (Input.IsActionPressed("turn_left"))
		{
			Rotation -= TurnSpeed * (float)delta;
		}

		if (Input.IsActionPressed("accelerate"))
		{
		}

		if (Input.IsActionJustPressed("fire"))
		{
			var bullet = BulletScene.Instantiate<Bullet>();
			bullet.Position = GetViewportRect().GetCenter();
			bullet._IntegrateForces()
		
			AddChild(bullet);
		}
	}
}
