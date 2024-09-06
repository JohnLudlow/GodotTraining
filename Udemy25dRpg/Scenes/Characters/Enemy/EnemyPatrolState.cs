namespace Udemy25dRpg.Scenes.Characters.Enemy;

public partial class EnemyPatrolState : StateMachineStateBase
{
    private int _pathNodeIndex = 0;

    protected override void EnterState()
    {
        _characterNode.AnimatedSprite3DNode.Play(nameof(Enemy.EnemyAnimations.Move));
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (_characterNode.NavigationAgentNode.IsNodeReady() && _characterNode.NavigationAgentNode.IsNavigationFinished())
        {
            if (_pathNodeIndex >= _characterNode.PathNode.Curve.PointCount)
            {
                _pathNodeIndex = 0;
            }

            _characterNode.NavigationAgentNode.TargetPosition =
                _characterNode.PathNode.Curve.GetPointPosition(_pathNodeIndex++) +
                _characterNode.PathNode.GlobalPosition;
        }

        _characterNode.Velocity = _characterNode.GlobalPosition.DirectionTo(_characterNode.NavigationAgentNode.TargetPosition);
        _characterNode.MoveAndSlide();
    }
}
