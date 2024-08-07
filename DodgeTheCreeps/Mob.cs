using Godot;

public partial class Mob : RigidBody2D
{
	private void OnVisbleOnScreenNotifier2DScreenExited()
	{
		QueueFree();
	}

    public override void _Ready()
    {
        var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		var mobTypes = animatedSprite2D.SpriteFrames.GetAnimationNames();
		animatedSprite2D.Play(mobTypes[GD.RandRange(0, mobTypes.Length - 1)]);
    }	
}
