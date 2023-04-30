using Godot;
using System;

public partial class GameManager : Node2D
{
	[Export] public int BabyCount { get; set; }
	public GameState CurrentState { get => currentState; set { currentState = value; GD.Print("Game State changed to: ", value); } }
	public Baby LaunchedBaby { get => launchedBaby; set { launchedBaby = value; if (value != null) { BabyLaunchSound.Play(); } } }
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
	[Export] private Label HighScoreLabel;
	[Export] private Label MaxDistanceLabel;
	[Export] private Label MaxHeightLabel;
	[Export] private PanelContainer GameOverPanel;
	[Export] private Button RestartButton;
	[Export] private Button ExitButton;
	[Export] private Camera2D Camera;
	[Export] private Vector2 LaunchCameraPosition;
	[Export] private Vector2 LaunchCameraZoom;
	[Export] private float CameraSpeed;
	[Export] private float CameraZoomSpeed;
	[Export] private AudioStreamPlayer2D BabyLaunchSound;
	[Export] private AudioStreamPlayer2D BabyMissSound;
	[Export] private AudioStreamPlayer2D BabyCatchSound;
	[Export] private AudioStreamPlayer2D BGM;

	private int playerScore;
	private int highScore;
	private float maxDistance;
	private float maxHeight;
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

		CurrentState = GameState.Play;
		BGM.Play();
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
					CurrentState = GameState.End;
					return;
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

				if (PlayerPosition.X < maxPosition.X && PlayerPosition.Y < maxPosition.Y)
				{
					Camera.Zoom = Camera.Zoom + new Vector2(CameraZoomSpeed, CameraZoomSpeed);
				}
				else
				{
					Camera.Zoom = Camera.Zoom - new Vector2(CameraZoomSpeed, CameraZoomSpeed);
				}

			break;
			case GameState.End:
				BGM.Stop();
				if(playerScore > highScore) { highScore = playerScore; }
				MaxDistanceLabel.Text = maxDistance.ToString("F");
				MaxHeightLabel.Text = maxHeight.ToString("F");
				HighScoreLabel.Text = highScore.ToString();
				GameOverPanel.Visible = true;
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
		if(amount > 0)
		{
			BabyCatchSound.Play();
		} else
		{
			BabyMissSound.Play();
		}
		playerScore += amount;
	}
	public void SubmitBabyStats(float distance, float height)
	{
		GD.Print("Submitted baby stats: ", distance, height);
		if(distance > maxDistance) { maxDistance = distance; }
		if(height > maxHeight) { maxHeight = height; }
		playerScore = CalcScore(distance, height);
	}
	private int CalcScore(float distance, float height)
	{
		// TODO: Figure out scoring
		float heightScore = height / 2;
		float distanceScore = distance / 2;

		return (int)(heightScore + distanceScore);
	}
	private void RestartGame()
	{
		playerScore = 0;
		BabyCount = initialBabyCount;
		GameOverPanel.Visible = false;
		CurrentState = GameState.Play;
		BGM.Play();
	}
	private void ExitGame()
	{
		GetTree().Quit();
	}
	
}
