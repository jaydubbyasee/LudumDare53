using Godot;
using System;

public partial class Launcher : Node2D
{
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
	public float MaxLaunchPower { get; set; } = 10000;

	private int _powerDirection = 1;
	
	[Signal]
	public delegate void OnBabyLaunchedEventHandler();

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		switch (CurrentLaunchState)
		{
			case LaunchState.ANGLE_SELECT:
				GD.Print($"Angle: {LaunchAngle}");
				LaunchAngle += (float)(delta * AngleSelectSpeed * _angleDirection);
				// Toggle angle direction
				if (LaunchAngle > MaxLaunchAngle || LaunchAngle < MinLaunchAngle)
				{
					// Clamp to avoid going beyond ranges
					Mathf.Clamp(LaunchAngle, MinLaunchAngle, MaxLaunchAngle);
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
				}
				break;
			case LaunchState.POWER_SELECT:
				GD.Print($"Angle: {LaunchPower}");
				LaunchPower += (float)(delta * LaunchPowerSelectSpeed * _powerDirection);
				if (LaunchPower > MaxLaunchPower || LaunchAngle < MinLaunchPower)
				{
					// Clamp to avoid going beyond ranges
					Mathf.Clamp(LaunchPower, MinLaunchPower, MaxLaunchPower);
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

					var launchVector = GetBabyLaunchVector();
					// TODO: instantiate babby, and set launch vector
					
					EmitSignal(SignalName.OnBabyLaunched);
				}
				
				break;
			case LaunchState.LAUNCHED:
				// no-op
				break;
		}
	}

	public Vector2 GetBabyLaunchVector()
	{
		return Vector2.Right.Rotated(LaunchAngle) * LaunchPower;	
	}
}
