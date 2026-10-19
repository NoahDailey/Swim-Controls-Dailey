using Godot;
using System;

public partial class SwimmingController : Node3D
{
	[Export] public CharacterBody3D player;
	public Node3D playerHead;
	
	[ExportGroup("Movement Values")]
	[Export(PropertyHint.Range, "0,20,0.1,or_greater")]
	public float swimSpeed             { get; set; } = 2.0f;
	[Export(PropertyHint.Range, "0,20,0.1,or_greater")]
	public float sprintSwimSpeed             { get; set; } = 3.5f;
	[Export(PropertyHint.Range, "0,20,0.1,or_greater")]
	public float verticalSwimSpeed             { get; set; } = 3.0f;
	[Export(PropertyHint.Range, "0,20,0.1,or_greater")]
	public float waterGravity          { get; set; } = 0.25f;
	
	[ExportGroup("Input")]
	[Export] public string moveForward;
	[Export] public string moveBackward;
	[Export] public string strafeLeft;
	[Export] public string strafeRight;
	[Export] public string swimUp;
	[Export] public string swimDown;
	[Export] public string sprint;
	
	private float gravity;
	
	public override void _Ready()
	{
		playerHead = player.GetChild("Head");
		gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");
	}
	
	public override void _PhysicsProcess(double delta)
	{
		float forward = Strength(moveBackward, Key.S) - Strength(moveForward, Key.W);
		float sideways = Strength(strafeRight, Key.D) - Strength(strafeLeft, Key.A);
		
		Vector3 input = new Vector3(sideways, 0, forward).LimitLength();
		Vector3 direction = (player.GlobalTransform.Basis * input).Normalized();
		
		 Vector3 velocity = player.Velocity;
		velocity.X = direction.X * swimSpeed;
		velocity.Z = direction.Z * swimSpeed;
		
		float verticalInput = Strength(swimUp, Key.Space) - Strength(swimDown, Key.Ctrl);
		
		 velocity.Y += gravity * waterGravity * (float)delta;
		velocity.Y = Mathf.Clamp(velocity.Y + verticalInput * verticalSwimSpeed,
		-verticalSwimSpeed, verticalSwimSpeed);
		
		player.Velocity = velocity;
		player.MoveAndSlide();
	}
	
	 private float Strength(string action, Key fallback)
	{
		if (!string.IsNullOrEmpty(action) && InputMap.HasAction(action))
			return Input.GetActionStrength(action);
		
		return Input.IsPhysicalKeyPressed(fallback) ? 1.0f : 0.0f;
	}
}
