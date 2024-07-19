using Godot;
using System;
using System.Diagnostics;

public partial class CollisionSensor : RigidBody2D
{
	public void OnBodyEntered(Node body)
	{
		Debug.WriteLine("Collided!");
	}

	public void OnMouseEntered()
	{
		Debug.WriteLine("Collided!");
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
