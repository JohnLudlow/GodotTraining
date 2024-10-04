using System;

using Godot;

namespace Udemy25dRpg.Resources;

public enum StatType { Health, Strength }

[GlobalClass]
public partial class StatsResource : Resource
{
  private float _statValue;

  [Export] 
  public StatType StatType { get; private set; }

  [Export]
  public float StatValue
  {
    get => _statValue;
    set => _statValue = Mathf.Clamp(value, 0, Mathf.Inf);
  }
}