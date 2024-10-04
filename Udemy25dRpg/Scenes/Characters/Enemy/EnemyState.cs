using Godot;

namespace Udemy25dRpg.Scenes.Characters.Enemy;

public abstract partial class EnemyState : StateMachineStateBase
{
    protected Vector3 Destination {get; set;}

    /// <summary>
    /// Returns a global position on this state actor's path based on node index
    /// </summary>
    /// <param name="index">The index of the desired node on the path</param>
    /// <returns>The global position of the node at <paramref name="index"/></returns>
    protected Vector3 GetPointGlobalPosition(int index) 
      =>
        CharacterNode.PathNode.Curve.GetPointPosition(index) +
        CharacterNode.PathNode.GlobalPosition;

    protected void Move()
    {
        CharacterNode.NavigationAgentNode.GetNextPathPosition();
        CharacterNode.Velocity = CharacterNode.GlobalPosition.DirectionTo(Destination);
        CharacterNode.MoveAndSlide();
        CharacterNode.FlipSprite();
    }

  protected void HandleChaseAreaBodyEntered(Node3D node) 
    => CharacterNode.StateMachineNode.SwitchState<EnemyChaseState>();
}