using Godot;
using System;

public partial class GameManager : Node
{
	public int PlayerScore { get => playerScore; set => playerScore = value; }
	public int BabyCount { get => babyCount; set => babyCount = value; }

	public GameManager Instance;

	[Export] private Label ScoreValueLabel;

	private int playerScore;
	private int babyCount;

	// Probably general game States here
	// Ready countdown
	// Playing
	// Game ended

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
		playerScore = 0;
		babyCount = 5;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		UpdateUI();
	}

	private void UpdateUI()
	{
		ScoreValueLabel.Text = PlayerScore.ToString();
	}
}
