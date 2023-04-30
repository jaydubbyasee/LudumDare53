using Godot;
using System;

public partial class CatcherController : Node2D
{
	[Export] private float moveSpeed;
	[Export] private Vector2 InitialPosition;
	//private GameManager gameManager;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(GameManager.Instance.CurrentState == GameManager.GameState.End)
		{
			GlobalPosition = InitialPosition;
			return;
		}
		if (Input.IsActionPressed("MoveRight"))
		{
			GlobalPosition = new Vector2(GlobalPosition.X + (moveSpeed * Convert.ToSingle(delta)), GlobalPosition.Y);
		}
		if (Input.IsActionPressed("MoveLeft"))
		{
			GlobalPosition = new Vector2(GlobalPosition.X - (moveSpeed * Convert.ToSingle(delta)), GlobalPosition.Y);
		}
		GameManager.Instance.PlayerPosition = GlobalPosition;
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
			//int score = CalcScore(baby.MaxHeight, baby.MaxDistance);
			//GameManager.Instance.UpdateScore(score);
			GameManager.Instance.SubmitBabyStats(baby.MaxDistance, baby.MaxHeight);
			baby.Destroy();
		}
	}


}
