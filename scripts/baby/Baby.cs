using Godot;
using System;

public partial class Baby : RigidBody2D
{
	private float maxHeight;
	private float maxDistance;
	private float initialYCoord;
	private float initialXCoord;

	public float MaxHeight { get; set; }
	public float MaxDistance { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		MaxHeight = 0;
		MaxDistance = 0;
		initialXCoord = GlobalPosition.X;
		initialYCoord = GlobalPosition.Y;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		float currentHeight = Math.Abs(GlobalPosition.Y) + Math.Abs(initialYCoord);
		float currentDistance = Math.Abs(GlobalPosition.X) + Math.Abs(initialXCoord);

		if (currentHeight > MaxHeight)
		{
			MaxHeight = currentHeight;
		}
		if (currentDistance > MaxDistance)
		{
			MaxDistance = currentDistance;
		}
	}

	public void Destroy()
	{
		QueueFree();
	}
}





