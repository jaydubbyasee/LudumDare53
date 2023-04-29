using Godot;
using System;

public partial class Baby : RigidBody2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyShapeEntered += OnBodyShapeEntered;
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node body)
	{
		GD.Print("I Body Entered", body.Name);
	}

	private void OnBodyShapeEntered(Rid bodyRid, Node body, long bodyShapeIndex, long localShapeIndex)
	{
		GD.Print("I entered something!", body.Name);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	//private void OnBodyEntered(Node body)
	//{
	//	GD.Print("I entered something!", body.Name);
	//}
	//private void OnShapeEntered(Rid body_rid, Node body, long body_shape_index, long local_shape_index)
	//{
	//	GD.Print("My shape entered?");
		 
	//}
}





