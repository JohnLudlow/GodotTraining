using Godot;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GodotTraining.SpaceRocks;
public partial class HeadsUpDisplay : CanvasLayer
{
	[Signal]
	public delegate void StartGameEventHandler();

	private IEnumerable<TextureRect> LivesCounter {get;set;}
	private Label ScoreLabel {get;set;}
	private Timer MessageTimer {get;set;}
	private Label MessageLabel {get;set;}
	private TextureButton StartButton {get;set;}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		LivesCounter = GetNode<HBoxContainer>("MarginContainer/HBoxContainer/LivesCounter").GetChildren().Cast<TextureRect>();
		ScoreLabel = GetNode<Label>("MarginContainer/HBoxContainer/ScoreLabel");
		MessageLabel = GetNode<Label>("VBoxContainer/Message");
		MessageTimer = GetNode<Timer>("Timer");
		StartButton = GetNode<TextureButton>("VBoxContainer/StartButton");
	}

	public async Task ShowMessage(string text)
	{
		MessageLabel.Text = text;
		MessageLabel.Show();
		MessageTimer.Start();
		await ToSignal(MessageTimer, Timer.SignalName.Timeout);
	}

	public void UpdateScore(int score)
	{
		ScoreLabel.Text = score.ToString();
	}

	public void UpdateLives(int lives)
	{
		for(int i = 0; i < lives; i++)
		{
			LivesCounter.ElementAt(i).Visible = lives > i;
		}
	}

	public async void GameOver()
	{
        await ShowMessage("Game Over");
		await ToSignal(MessageTimer, Timer.SignalName.Timeout);
		StartButton.Show(); 
	}

	public void OnStartButtonPressed()
	{
		StartButton.Hide();
		EmitSignal(SignalName.StartGame);
	}

	private void OnMessageTimerTimeout() {
		MessageLabel.Hide();
		MessageLabel.Text = "";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		UpdateScore(GetTree().GetNodesInGroup("rocks").Count);
	}
}
