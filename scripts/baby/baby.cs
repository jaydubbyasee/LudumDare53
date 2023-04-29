using Godot;
using System;

public partial class Baby : RigidBody2D
{
	private float maxHeight;
	private float maxDistance;

	public float MaxHeight { get => maxHeight; set => maxHeight = value; }
	public float MaxDistance { get => maxDistance; set => maxDistance = value; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		MaxHeight = float.MaxValue;
		MaxDistance = -float.MaxValue;
		// Record the initial height of the baby here
		// Take math.abs of initial - height (which is low since the highest height is 0
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GD.Print(string.Format("Current Position: {0}", GlobalPosition));
		// Coords start at the top left so the higher the object, the smaller the Y value
		if (GlobalPosition.Y < MaxHeight)
		{
			MaxHeight = GlobalPosition.Y;
		}
		if (GlobalPosition.X > MaxDistance)
		{
			MaxDistance = GlobalPosition.X;
		}
	}

	public void Destroy()
	{
		QueueFree();
	}
}





