using Godot;

namespace Udemy25dRpg.Scenes.Characters.Player;

public partial class PlayerDashState : StateMachineStateBase
{
    [Export]
    public Timer DashTimer {get; private set;}

    [Export(PropertyHint.Range, "0, 20, .1")]
    public float DashSpeed {get; private set;} = 10f;

    public override void _Ready()
    {
        base._Ready();

        DashTimer.Timeout += () => { 
            CharacterNode.StateMachineNode.SwitchState<PlayerIdleState>();
            CharacterNode.Velocity = Vector3.Zero;
        };
    }

    public override void _PhysicsProcess(double delta)
    {
        CharacterNode.MoveAndSlide();
        CharacterNode.ApplyFloorSnap();
        CharacterNode.FlipSprite();
    }

    protected override void EnterState()
    {
        CharacterNode.AnimatedSprite3DNode.Play(nameof(Player.PlayerAnimations.Dash));

        if (CharacterNode.Velocity == Vector3.Zero)
        {
            CharacterNode.Velocity = CharacterNode.AnimatedSprite3DNode.FlipH ? Vector3.Left : Vector3.Right;
        }

        CharacterNode.Velocity = new (
            CharacterNode.Direction.X * DashSpeed, 
            0, 
            CharacterNode.Direction.Y * DashSpeed
        );
        
        DashTimer.Start();
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustReleased(nameof(Player.PlayerInputs.Dash)))
        {
            CharacterNode.StateMachineNode.SwitchState<PlayerIdleState>();
        }
    }
}
