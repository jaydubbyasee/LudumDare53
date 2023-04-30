using Godot;
using System;

public partial class GameManager : Node2D
{
	[Export] public int BabyCount { get; set; }
	public GameState CurrentState { get => currentState; set { currentState = value; GD.Print("Game State changed to: ", value); } }
	public Baby LaunchedBaby { get; set; }
	public Vector2 PlayerPosition { get; set; }

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
	[Export] private Camera2D Camera;
	[Export] private Vector2 LaunchCameraPosition;
	[Export] private Vector2 LaunchCameraZoom;
	[Export] private float CameraSpeed;
	[Export] private float CameraZoomSpeed;

	private int playerScore;
	private int babyCount;
	private int initialBabyCount;
	private Vector2 playerPosition;
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

		Camera.PositionSmoothingEnabled = true;
		Camera.PositionSmoothingSpeed = CameraSpeed;

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
				Camera.GlobalPosition = LaunchCameraPosition;
				Camera.Zoom = new Vector2(LaunchCameraZoom.X, LaunchCameraZoom.Y);
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
				Camera.GlobalPosition = LaunchedBaby.GlobalPosition;
				Transform2D canvasTransform = GetCanvasTransform();
				Vector2 minPosition = -canvasTransform.Origin / canvasTransform.Scale;
				Vector2 viewSize = GetViewportRect().Size / canvasTransform.Scale;
				Vector2 maxPosition = minPosition + viewSize;
				//GD.Print(minPosition, maxPosition);
				//GD.Print("PlayerPosition: ", PlayerPosition);
				//GD.Print("MaxPosition: ", maxPosition);
				if (PlayerPosition.X < maxPosition.X && PlayerPosition.Y < maxPosition.Y)
				{
					GD.Print("PLAYER IS IN VIEW!");
					Camera.Zoom = Camera.Zoom + new Vector2(CameraZoomSpeed, CameraZoomSpeed);
				}
				else
				{
					GD.Print("ZOOMING");
					Camera.Zoom = Camera.Zoom - new Vector2(CameraZoomSpeed, CameraZoomSpeed);
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
