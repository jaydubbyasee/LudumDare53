using System;
using Godot;

public partial class Launcher : Node2D
{
	private TextureProgressBar _launchAngleRadial;
	private TextureProgressBar _powerBar;

	public enum LaunchState
	{
		ANGLE_SELECT,
		POWER_SELECT,
		LAUNCHED,
	}

	// Default initial launch state to ANGLE_SELECT
	public LaunchState CurrentLaunchState = LaunchState.ANGLE_SELECT;

	[Export]
	public float AngleSelectSpeed { get; set; } = 30.0f;
	
	[Export]
	public float LaunchAngle { get; set; }

	[Export]
	public float MaxLaunchAngle { get; set; } = 90;

	[Export]
	public float MinLaunchAngle { get; set; } = 0;
	private int _angleDirection = 1;

	[Export]
	public float LaunchPowerSelectSpeed { get; set; } = 30.0f;
	
	[Export]
	public float LaunchPower { get; set; }
	
	[Export]
	public float MinLaunchPower { get; set; }

	[Export]
	public float MaxLaunchPower { get; set; } = 100;
	
	[Export]
	public float BabyAngularVelocity { get; set; } = 3.14f * 5;

	private int _powerDirection = 1;
	
	[Signal]
	public delegate void OnBabyLaunchedEventHandler(Baby baby);

	[Export]
	private PackedScene _babyScene;



	public override void _Ready()
	{
		_launchAngleRadial = GetNode<TextureProgressBar>("LaunchAngleRadial");
		_launchAngleRadial.MinValue = MinLaunchAngle;
		_launchAngleRadial.MaxValue = MaxLaunchAngle;

		_powerBar = GetNode<TextureProgressBar>("PowerBar");
		_powerBar.MinValue = MinLaunchPower;
		_powerBar.MaxValue = MaxLaunchPower;
		_powerBar.Hide();
	}

	public override void _Process(double delta)
	{
		if(GameManager.Instance.CurrentState == GameManager.GameState.End)
		{
			return;
		}

		_launchAngleRadial.Value = LaunchAngle;
		_powerBar.Value = LaunchPower;
		switch (CurrentLaunchState)
		{
			case LaunchState.LAUNCHED:
				if (GameManager.Instance.CurrentState == GameManager.GameState.Play)
				{
						CurrentLaunchState = LaunchState.ANGLE_SELECT;
				}
			break;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if (GameManager.Instance.CurrentState == GameManager.GameState.End)
		{
			return;
		}
		switch (CurrentLaunchState)
		{
			case LaunchState.ANGLE_SELECT:
				//GD.Print($"Angle: {LaunchAngle}");
				LaunchAngle += (float)(delta * AngleSelectSpeed * _angleDirection);
				// Toggle angle direction
				if (LaunchAngle > MaxLaunchAngle || LaunchAngle < MinLaunchAngle)
				{
					// Clamp to avoid going beyond ranges
					LaunchAngle = Mathf.Clamp(LaunchAngle, MinLaunchAngle, MaxLaunchAngle);
					// Toggle the increment direction
					_angleDirection *= -1;
				}
				
				// If the spacebar is pressed, then lock in the current angle selection and
				// transition launcher states
				if (Input.IsActionJustPressed("Accept"))
				{
					GD.Print($"Angle Selected: {LaunchAngle}");
					GD.Print("accept");
					CurrentLaunchState = LaunchState.POWER_SELECT;
					_powerBar.Show();
				}
				break;
			case LaunchState.POWER_SELECT:
				//GD.Print($"Power: {LaunchPower}");
				LaunchPower += (float)(delta * LaunchPowerSelectSpeed * _powerDirection);
				if (LaunchPower > MaxLaunchPower || LaunchPower < MinLaunchPower)
				{
					// Clamp to avoid going beyond ranges
					LaunchPower = Mathf.Clamp(LaunchPower, MinLaunchPower, MaxLaunchPower);
					// Toggle increment direction
					_powerDirection *= -1;
				}
				
				// If the spacebar is pressed, then lock in the current power selection and
				// transition launcher states
				if (Input.IsActionJustPressed("Accept"))
				{
					CurrentLaunchState = LaunchState.LAUNCHED;
					GD.Print($"Power Selected: {LaunchPower}");
					GD.Print("LAUNCHING BABY");
					GameManager.Instance.BabyCount -= 1;
					var launchVector = GetBabyLaunchVector();
					var baby = _babyScene.Instantiate<Baby>();
					GetParent().AddChild(baby);
					baby.LinearVelocity = launchVector;
					baby.AngularVelocity = BabyAngularVelocity;

					EmitSignal(SignalName.OnBabyLaunched, baby);
					GameManager.Instance.LaunchedBaby = baby;
				}
				
				break;
			case LaunchState.LAUNCHED:
				// no-op
				LaunchAngle = 0;
				LaunchPower = 0;
				_powerBar.Hide();

				break;
		}
	}

	public Vector2 GetBabyLaunchVector()
	{
		return Vector2.Right.Rotated(-Mathf.DegToRad(LaunchAngle)) * LaunchPower * 20;	
	}
}
