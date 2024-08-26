using Godot;

namespace Udemy25dRpg.Scenes.Characters;

public partial class Player : CharacterBody3D
{
	public enum PlayerInputs {
		MoveLeft,
		MoveRight,
		MoveForward,
		MoveBackward
	}

	public enum PlayerAnimations {
		Idle,
		Move,
	}

	[Export]
	public StateMachine StateMachineNode { get; private set; }

	[Export, ExportGroup("Required Nodes")] 
	public AnimationPlayer AnimationPlayerNode {get; private set;}
	
	[Export, ExportGroup("Required Nodes")] 
	public Sprite3D SpriteNode {get; private set;}

	public Vector2 Direction {get; private set;} = Vector2.Zero;


    public override void _Ready()
    {
        StateMachineNode.SwitchState<PlayerIdleState>();
    }

    public override void _PhysicsProcess(double delta)
	{
		Velocity = new(
			Direction.X * 5, 
			0, 
			Direction.Y * 5
		);

		MoveAndSlide();
	}

    public override void _Input(InputEvent @event)
    {
		Direction = Input.GetVector(
			nameof(PlayerInputs.MoveLeft), 
			nameof(PlayerInputs.MoveRight), 
			nameof(PlayerInputs.MoveForward), 
			nameof(PlayerInputs.MoveBackward)
		);

		FlipSprite();
    }

    private void FlipSprite()
    {
        if (Direction.X < 0)
        {
            SpriteNode.FlipH = true;
        }
        else if (Direction.X > 0)
        {
            SpriteNode.FlipH = false;
        }
    }

}
