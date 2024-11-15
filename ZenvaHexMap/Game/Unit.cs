using Godot;
using System;
using System.Collections.Generic;

namespace ZenvaHexMap.Game;

public partial class Unit() : Node2D
{
  private Civilization? _ownerCivilization;

  public string UnitName { get; protected set; }
  public Vector2I UnitCoordinates {get;set;}

  public Civilization? OwnerCivilization
  {
    get => _ownerCivilization;
    set
    {
      _ownerCivilization = value;
      Modulate = OwnerCivilization?.CivilizationTerritoryColor ?? default;
    }
  }
  public int ProductionRequired { get; protected set; }

  public static Dictionary<Type, PackedScene> UnitSceneResources {get;} = new () {
    [typeof(Settler)] = ResourceLoader.Load<PackedScene>("res://Settler.tscn"),
    [typeof(Warrior)] = ResourceLoader.Load<PackedScene>("res://Warrior.tscn")
  };
}
