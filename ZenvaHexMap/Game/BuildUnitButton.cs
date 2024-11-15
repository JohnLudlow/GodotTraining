using Godot;
using System;

namespace ZenvaHexMap.Game;

public partial class BuildUnitButton : Button
{
  [Signal]
  public delegate void OnPressedEventHandler(Unit unit);
  public Unit Unit {get; set;}

  public override void _Ready()
  {
    base._Ready();

    Pressed += () => EmitSignal(SignalName.OnPressed, Unit);
  }
}
