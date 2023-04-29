using Godot;
using System;

public partial class CatcherController : Node2D
{
	[Export] private float moveSpeed;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("MoveRight"))
		{
			GlobalPosition = new Vector2(GlobalPosition.X + (moveSpeed * Convert.ToSingle(delta)), GlobalPosition.Y);
		}
		if (Input.IsActionPressed("MoveLeft"))
		{
			GlobalPosition = new Vector2(GlobalPosition.X - (moveSpeed * Convert.ToSingle(delta)), GlobalPosition.Y);
		}
	}
	
	private void BodyEntered(Node2D body)
	{
		GD.Print("Body entered the basket!");
		if (body.GetGroups().Contains("Baby"))
		{
			GD.Print("And it was a baby!");
		}
	}
}



