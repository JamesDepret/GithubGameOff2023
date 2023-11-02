namespace Character;

public partial class Player : CharacterBody2D
{
	[Export] public float MaxSpeed { get; set; } = 125f;
	[Export] public float Acceleration { get; set; } = 25f;
	AnimationPlayer animationPlayer;
	Node2D visuals;

	public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		visuals = GetNode<Node2D>("Visuals");
	}

	
	public override void _Process(double delta)
	{
		Move(delta);
	}
}
