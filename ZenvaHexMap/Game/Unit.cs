using Godot;
using System;
using System.Collections.Generic;

namespace ZenvaHexMap.Game;

public partial class Unit(string unitName, int productionRequired) : Node2D
{
  private Civilization? _ownerCivilization;

  public string UnitName { get; } = unitName;
  public Civilization? OwnerCivilization
  {
    get => _ownerCivilization;
    set
    {
      _ownerCivilization = value;
      Modulate = OwnerCivilization?.CivilizationTerritoryColor ?? default;
    }
  }
  public int ProductionRequired { get; } = productionRequired;

  public static Dictionary<Type, PackedScene> UnitSceneResources {get;} = new () {
    [typeof(Settler)] = ResourceLoader.Load<PackedScene>("res://Settler.tscn"),
    [typeof(Warrior)] = ResourceLoader.Load<PackedScene>("res://Warrior.tscn")
  };
}
