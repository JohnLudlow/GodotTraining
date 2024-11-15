using System;

using Godot;

namespace ZenvaHexMap.Game;

public partial class CityUI : Panel
{
  private City? _city;
  private Label? _cityNameLabel, _cityPopLabel, _cityFoodLabel, _cityProdLabel;

  public City? City
  {
    get
    {
      return _city;
    }
    set
    {
      if (_city is not null)
        _city.PropertyChanged -= City_PropertyChanged;

      _city = value;

      if (_city is not null)
        _city.PropertyChanged += City_PropertyChanged;

      City_PropertyChanged(_city, null);
      PopulateBuildQueue();
      ConnectUnitBuildSignals();
    }
  }

  public void ConnectUnitBuildSignals()
  {
    if (_city?.OwnerCivilization is null) return;

    var buttons = GetNode<VBoxContainer>("BuildMenuContainer/VBoxContainer");

    var settlerButton = buttons.GetNode<BuildUnitButton>("BuildSettlerButton");
    settlerButton.Unit = new Settler();
    settlerButton.OnPressed += _city.AddUnitToBuildQueue;
    
    var warriorButton = buttons.GetNode<BuildUnitButton>("BuildWarriorButton");
    warriorButton.Unit = new Warrior();
    warriorButton.OnPressed += _city.AddUnitToBuildQueue;
  }

  private void City_PropertyChanged(object? sender, EntityUpdatedEventArgs<City>? e)
  {
    if (_cityNameLabel is not null)
      _cityNameLabel.Text = _city?.CityName;

    if (_cityPopLabel is not null)
      _cityPopLabel.Text  = $"Population : {_city?.Population}";
  
    if (_cityFoodLabel is not null)
      _cityFoodLabel.Text = $"Food : {_city?.TotalFood}";
    
    if (_cityProdLabel is not null)
      _cityProdLabel.Text = $"Production : {_city?.TotalProduction}";

    PopulateBuildQueue();
  }

  public void PopulateBuildQueue()
  {
    if (_city is null) return;

    var queueVboxContainer = GetNode<VBoxContainer>("BuildQueueContainer/VBoxContainer");
    foreach (var node in queueVboxContainer.GetChildren())
    {
      queueVboxContainer.RemoveChild(node);
      node.QueueFree();
    }

    for (var i = 0; i < _city.BuildQueue.Count; i++)
    {
      var queueItem = _city.BuildQueue[i];
      queueVboxContainer.AddChild(
        new Label {
          Text = $"{queueItem.UnitName} {(i == 0 ? _city.UnitProductionCompleted : 0)} / {queueItem.ProductionRequired}",
        });
    }
  }
}
