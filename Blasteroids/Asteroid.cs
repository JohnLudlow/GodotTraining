using Godot;
using System;

public partial class Asteroid : RigidBody2D
{
	public float Spin {get; set;} = 1.0f;
	public Guid _id = Guid.NewGuid();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Spin = (float)GD.RandRange(-2.0f, 2.0f);
		var scale = (float)GD.RandRange(0.1f, 0.5f);
		ApplyScale(new Vector2(scale, scale));
		GetNode<Sprite2D>("Sprite2D").ApplyScale(new Vector2(scale, scale));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	private void OnVisbleOnScreenNotifier2DScreenExited()
	{
		QueueFree();
	}

    public override void _Process(double delta)
    {
        GetNode<Sprite2D>("Sprite2D").Rotate((float)(Spin * delta));
    }
}
