using Godot;
using System;

namespace Udemy25dRpg.Scenes.Characters.Enemy;

public partial class Enemy : CharacterBody3D
{
    public enum EnemyAnimations
    {
        Idle,
        Move,
        Attack,
        Death,
        TakeHit
    }
}
