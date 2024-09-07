using Godot;

namespace Udemy25dRpg.Scenes.Characters.Enemy;

public abstract partial class EnemyState : StateMachineStateBase
{
    protected Vector3 _destination;

    /// <summary>
    /// Returns a global position on this state actor's path based on node index
    /// </summary>
    /// <param name="index">The index of the desired node on the path</param>
    /// <returns>The global position of the node at <paramref name="index"/></returns>
    protected Vector3 GetPointGlobalPosition(int index) => 
        _characterNode.PathNode.Curve.GetPointPosition(index) + 
        _characterNode.PathNode.GlobalPosition;

    protected void Move()
    {
        _characterNode.NavigationAgentNode.GetNextPathPosition();
        _characterNode.Velocity = _characterNode.GlobalPosition.DirectionTo(_destination);
        _characterNode.MoveAndSlide();
        _characterNode.FlipSprite();
    }

    protected void HandleChaseAreaBodyEntered(Node3D node) 
    {
        _characterNode.StateMachineNode.SwitchState<EnemyChaseState>();
    }
}