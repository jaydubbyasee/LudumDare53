using Godot;
using System;

public partial class MainMenu : Node2D
{
	[Export] private Button PlayButton;
	[Export] private Button ExitButton;
	[Export] public PackedScene PlayScene;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		PlayButton.ButtonDown += StartGame;
		ExitButton.ButtonDown += EndGame;
		if(PlayScene == null)
		{
			throw new Exception("Play scene not specified!");
		}
	}

	private void StartGame()
	{
		GetTree().ChangeSceneToPacked(PlayScene);
	}
	private void EndGame()
	{
		GetTree().Quit();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
