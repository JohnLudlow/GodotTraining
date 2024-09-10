using Godot;

namespace Udemy25dRpg.Scenes.Characters.Enemy;

public partial class EnemyPatrolState : EnemyState
{
    [Export]
    public Timer IdleTimerNode { get; set; }

    [Export(PropertyHint.Range, "0, 20, .1")]
    public float MaxIdleTime { get; set; } = 4;

    private int _pathNodeIndex = 0;

    protected override void ExitState()
    {
        _characterNode.NavigationAgentNode.NavigationFinished -= HandleNavigationFinished;
        _characterNode.ChaseAreaNode.BodyEntered -= HandleChaseAreaBodyEntered;
        
        IdleTimerNode.Timeout -= HandleTimerTimeout;
        IdleTimerNode.Stop();
    }

    protected override void EnterState()
    {
        _characterNode.AnimatedSprite3DNode.Play(nameof(Enemy.EnemyAnimations.Move));
        _characterNode.NavigationAgentNode.NavigationFinished += HandleNavigationFinished;
        _characterNode.ChaseAreaNode.BodyEntered += HandleChaseAreaBodyEntered;

        _pathNodeIndex = 1;
        _destination = GetPointGlobalPosition(_pathNodeIndex);                

        _characterNode.NavigationAgentNode.TargetPosition = _destination;

        IdleTimerNode.Timeout += HandleTimerTimeout;
    }

    private void HandleTimerTimeout()
    {
        IdleTimerNode.Stop();

        _characterNode.AnimatedSprite3DNode.Play(nameof(Enemy.EnemyAnimations.Move));

        _pathNodeIndex = Mathf.Wrap(_pathNodeIndex + 1, 0, _characterNode.PathNode.Curve.PointCount);
        _destination = GetPointGlobalPosition(_pathNodeIndex);
        _characterNode.NavigationAgentNode.TargetPosition = _destination;
    }

    private void HandleNavigationFinished()
    {
        _characterNode.AnimatedSprite3DNode.Play(nameof(Enemy.EnemyAnimations.Idle));

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
