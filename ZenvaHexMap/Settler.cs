using Godot;
using System;

namespace ZenvaHexMap.Game;

public partial class Settler : Unit
{
  public Settler(Civilization ownerCivilization) : base(ownerCivilization, "Settler", 100)
  {

  }
}
