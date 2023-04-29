using Godot;
using System;

public partial class Boundary : Node2D
{
	[Export] private int pointPenalty;
	private void BodyEntered(Node2D body)
	{
		if (body.GetGroups().Contains("Baby"))
		{
			GD.Print("Baby hit the floor!");
			GameManager.Instance.UpdateScore(-pointPenalty);
			Baby baby = body as Baby;
			baby.Destroy();
		}
	}
}
