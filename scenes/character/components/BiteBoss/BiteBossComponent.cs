namespace Character;
public partial class BiteBossComponent : Node
{
	[Export] public float MaxSpeed { get; set; } = 40.0f;
    [Export] public float Acceleration { get; set; } = 5f;
    [Export] Node2D visualsNode;
    [Export] float SpeedUpAmount = 100f;
	public Vector2 Direction { get; set; } = new Vector2(1, 0);
	Vector2 velocity = Vector2.Zero;
	Vector2 target = Vector2.Zero;
	Vector2 previousTarget = Vector2.Zero;

    public override void _Ready()
    {
        previousTarget = (Owner as Node2D).GlobalPosition;
    }
    
    public void Move(CharacterBody2D body){
        if((Owner as Node2D).GlobalPosition.DistanceTo(target) < 5)
            Direction = GetPointInSpace();
        else Direction = target;

		if (visualsNode != null){
			var movesign = Mathf.Sign(Direction.X);
			if (movesign != 0) 
				visualsNode.Scale = new Vector2(movesign, 1);
		}

		visualsNode.Rotation = Mathf.Atan2(target.Y - previousTarget.Y, target.X - previousTarget.X);

        body.Velocity = velocity;
        body.MoveAndSlide();
		velocity = body.Velocity;
    }

    public Vector2 GetPointInSpace()
    {
        if (Owner is not Node2D owner) return Vector2.Zero;
		var XPosition = (float) GD.RandRange(0, 640);
		var YPosition = target.Y <= 0 ? 400 : -40;
		previousTarget = target;
		target = new Vector2(XPosition, YPosition);
		return target;
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
        MaxSpeed += SpeedUpAmount;
    }
}
