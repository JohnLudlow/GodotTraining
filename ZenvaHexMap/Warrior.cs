using Godot;
using System;

namespace ZenvaHexMap.Game;

public partial class Warrior : Unit
{
  public Warrior(Civilization ownerCivilization) : base(ownerCivilization, "Warrior", 50)
  {

  }
}
