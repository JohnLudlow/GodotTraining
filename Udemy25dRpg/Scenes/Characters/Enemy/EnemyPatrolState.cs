using Godot;

namespace Udemy25dRpg.Scenes.Characters.Enemy;

public partial class EnemyPatrolState : EnemyState
{
    [Export]
    public Timer IdleTimerNode { get; set; }

    [Export(PropertyHint.Range, "0, 20, .1")]
    public float MaxIdleTime { get; set; } = 4;

    private int _pathNodeIndex;

  protected override void ExitState()
    {
        CharacterNode.NavigationAgentNode.NavigationFinished -= HandleNavigationFinished;
        CharacterNode.ChaseAreaNode.BodyEntered -= HandleChaseAreaBodyEntered;

        IdleTimerNode.Timeout -= HandleTimerTimeout;
        IdleTimerNode.Stop();
    }

    protected override void EnterState()
    {
        CharacterNode.AnimatedSprite3DNode.Play(nameof(Enemy.EnemyAnimations.Move));
        CharacterNode.NavigationAgentNode.NavigationFinished += HandleNavigationFinished;
        CharacterNode.ChaseAreaNode.BodyEntered += HandleChaseAreaBodyEntered;

        _pathNodeIndex = 1;
        Destination = GetPointGlobalPosition(_pathNodeIndex);                

        CharacterNode.NavigationAgentNode.TargetPosition = Destination;

        IdleTimerNode.Timeout += HandleTimerTimeout;
    }

    private void HandleTimerTimeout()
    {
        IdleTimerNode.Stop();

        CharacterNode.AnimatedSprite3DNode.Play(nameof(Enemy.EnemyAnimations.Move));

        _pathNodeIndex = Mathf.Wrap(_pathNodeIndex + 1, 0, CharacterNode.PathNode.Curve.PointCount);
        Destination = GetPointGlobalPosition(_pathNodeIndex);
        CharacterNode.NavigationAgentNode.TargetPosition = Destination;
    }

    private void HandleNavigationFinished()
    {
        CharacterNode.AnimatedSprite3DNode.Play(nameof(Enemy.EnemyAnimations.Idle));

        IdleTimerNode.WaitTime = new RandomNumberGenerator().RandfRange(0, MaxIdleTime);
        IdleTimerNode.Start();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (IdleTimerNode.IsStopped())
        {            
            Move();
        }
    }
}
