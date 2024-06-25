using Godot;
using System;

public partial class Rock : RigidBody2D
{
    float _scaleFactor = 0.2f;

    public Vector2 ScreenSize { get; set; } = Vector2.Zero;
    public int Size { get; set; } = 0;
    public float Radius { get; set; } = 0;


    [Signal]
	public delegate void ExplodedEventHandler(Rock rock);

    public void Start(Vector2 position, Vector2 velocity, int size)
	{
		Position = position;
		Size = size;

        GetNode<Sprite2D>("Sprite2D").Scale = Vector2.One * _scaleFactor * size;

		var background = GetNode<Sprite2D>("Sprite2D");
		Radius = background.Texture.GetSize().X / 2 * background.Scale.X;

        GetNode<CollisionShape2D>("CollisionShape2D").Shape = new CircleShape2D(){ Radius = Radius };
		
		LinearVelocity = velocity * 100;
		AngularVelocity = (float)GD.RandRange(-Math.PI, Math.PI);

		GetNode<Sprite2D>("Explosion").Scale = Vector2.One * .75f * size;
	}	

	public void Explode()
	{
		var explosionPlayer = GetNode<AnimationPlayer>("Explosion/AnimationPlayer");
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
		GetNode<Sprite2D>("Sprite2D").Hide();
		GetNode<Sprite2D>("Explosion").Show();

		explosionPlayer.AnimationFinished += new AnimationMixer.AnimationFinishedEventHandler(_ => QueueFree());
		explosionPlayer.Play("Explosion");

		LinearVelocity = Vector2.Zero;
		AngularVelocity = 0;
		EmitSignal(SignalName.Exploded, this);
	}

	public override void _IntegrateForces(PhysicsDirectBodyState2D state)
	{
		var xform = state.Transform;
		xform.Origin.X = Mathf.Wrap(xform.Origin.X, 0 -Radius, ScreenSize.X + Radius);
		xform.Origin.Y = Mathf.Wrap(xform.Origin.Y, 0 -Radius, ScreenSize.Y + Radius);
		state.Transform = xform;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
