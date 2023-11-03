namespace Components;
public partial class VelocityComponent : Node
{
	[Export] public float MaxSpeed { get; set; } = 40.0f;
    [Export] public float Acceleration { get; set; } = 5f;
    [Export] Node2D visualsNode;
	public Godot.Vector2 Direction { get; set; } = new Godot.Vector2(1, 0);
	
	Vector2 velocity = Vector2.Zero;
    public void Move(CharacterBody2D body){
        Direction = GetDirectionToPlayer();

		if (visualsNode != null){
			var movesign = Mathf.Sign(Direction.X);
			if (movesign != 0) 
				visualsNode.Scale = new Vector2(movesign, 1);
		}

        body.Velocity = velocity;
        body.MoveAndSlide();
		velocity = body.Velocity;
    }

    public Vector2 GetDirectionToPlayer()
    {
        if (Owner is not Node2D owner) return Vector2.Zero;

        var playerNodes = GetTree().GetNodesInGroup("player");
        if (playerNodes.Count > 0)
        {
            var player = playerNodes[0] as Player;
            return (player.GlobalPosition - owner.GlobalPosition).Normalized();
        }
        return Vector2.Zero;
    }

    public Vector2 AccelerateInDirection(Vector2 direction)
    {
        var desired_velocity = direction * MaxSpeed;
        velocity = velocity.Lerp(desired_velocity, (float) (1 - Mathf.Exp(-Acceleration * GetProcessDeltaTime())));
        return velocity;
    }

	public Vector2 Decelerate()
	{
		return AccelerateInDirection(Vector2.Zero);
	}

    public void SpeedUp()
    {
        MaxSpeed += 100f;
    }
}
