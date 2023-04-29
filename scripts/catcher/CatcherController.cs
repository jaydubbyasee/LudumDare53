using Godot;
using System;

public partial class CatcherController : Node2D
{
	[Export] private float moveSpeed;
	private GameManager gameManager;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//GD.Print(GetTree().CurrentScene.FindChild("GameManager", true));
		gameManager = GetTree().CurrentScene.FindChild("GameManager", true) as GameManager;
		if (gameManager == null)
		{
			throw new Exception("Cannot find GameManager at /");
		}
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
			GD.Print(body.Name);
			GD.Print("And it was a baby!");
			Baby baby = body as Baby;

			// Update score here
			GD.Print("Max Height was: ", baby.MaxHeight);
			GD.Print("Max Distance was: ", baby.MaxDistance);
			int score = CalcScore(baby.MaxHeight, baby.MaxDistance);
			gameManager.PlayerScore += score;
			baby.Destroy();
		}
	}

	private int CalcScore(float height, float distance)
	{
		// TODO: Figure out scoring
		float heightScore = height / 2;
		float distanceScore = distance / 2;

		return (int)(heightScore + distanceScore);
	}
}
