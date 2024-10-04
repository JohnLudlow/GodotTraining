using Godot;

namespace Udemy25dRpg.Scenes.Characters.Player;

public partial class PlayerIdleState : PlayerState
{
    public override void _Ready()
    {
        base._Ready();

        CharacterNode.AnimatedSprite3DNode.AnimationFinished += () => {             
            CharacterNode.StateMachineNode.SwitchState<PlayerIdleState>(); 
        };
    }

    public override void _PhysicsProcess(double delta)
    {
        if (CharacterNode.Direction != Vector2.Zero)
        {
            CharacterNode.StateMachineNode.SwitchState<PlayerMoveState>();
        }
    }

    protected override void EnterState()
    {
        CharacterNode.AnimatedSprite3DNode.Play(nameof(Player.PlayerAnimations.Idle));
    }

    public override void _Input(InputEvent @event)
    {
        HandleInput();
    }
}
