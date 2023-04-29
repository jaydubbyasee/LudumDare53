using Godot;
using System;

public partial class GameManager : Node
{
	[Export] public int BabyCount { get; set; }
	public GameState CurrentState { get => currentState; set { currentState = value; GD.Print("Game State changed to: ", value); } }
	public Baby LaunchedBaby { get; set; }

	public static GameManager Instance;

	public enum GameState
	{
		MainMenu,
		Ready,
		Play,
		Waiting,
		End,
	}

	[Export] private Label ScoreValueLabel;
	[Export] private Label BabyCountLabel;
	[Export] private PanelContainer GameOverPanel;
	[Export] private Button RestartButton;
	[Export] private Button ExitButton;

	private int playerScore;
	private int babyCount;
	private int initialBabyCount;
	private GameState currentState;
	private Baby launchedBaby;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GameOverPanel.Visible = false;

		if(Instance != null)
		{
			throw new Exception("Duplicate game manager detected!");
		}
		Instance = this;
		if (BabyCount <= 0)
		{
			throw new Exception("BabyCount not defined in inspector");
		}
		RestartButton.ButtonDown += RestartGame;
		ExitButton.ButtonDown += ExitGame;

		initialBabyCount = BabyCount;
		// TODO: Figure out state flow
		CurrentState = GameState.Play;
		playerScore = 0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		UpdateUI();
		switch (CurrentState)
		{
			case GameState.Play:
				if (IsInstanceValid(LaunchedBaby))
				{
					CurrentState = GameState.Waiting;
					return;
				}
				if (BabyCount <= 0)
				{
					GD.Print("ENDING GAME");
					CurrentState = GameState.End;
					GameOverPanel.Visible = true;
				}
			break;

			case GameState.Waiting:
				if(!IsInstanceValid(LaunchedBaby))
				{
					CurrentState = GameState.Play;
					return;
				}
			break;
		}
		
	}
	private void UpdateUI()
	{
		ScoreValueLabel.Text = playerScore.ToString();
		BabyCountLabel.Text = BabyCount.ToString();
	}
	public void UpdateScore(int amount)
	{
		playerScore += amount;
	}

	private void RestartGame()
	{
		playerScore = 0;
		BabyCount = initialBabyCount;
		GameOverPanel.Visible = false;
		CurrentState = GameState.Play;
	}
	private void ExitGame()
	{
		GetTree().Quit();
	}
	
}
