using Godot;

namespace GodotTraining.SpaceRocks;

public partial class Bullet : Area2D
{
	[Export]
	public int Speed { get; set; } = 1000;

	private Vector2 _velocity = Vector2.Zero;

	public void Start(Transform2D transform)	
	{
		Transform = transform;
		_velocity = transform.X * Speed;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position += _velocity * (float)delta;
	}

	public void OnExitedScreen()
	{
		QueueFree();		
	}

	public void OnBodyEntered(Node2D body)
	{
		if (body.IsInGroup("rocks") && body is Rock rock)
		{			
			rock.Explode();
			QueueFree();
		}
	}
}
