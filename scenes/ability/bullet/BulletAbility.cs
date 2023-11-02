namespace Ability;

public partial class BulletAbility : Node2D
{
	public HitboxComponent HitboxComponent { get; private set; }
	public Vector2 Velocity { get; set; } = Vector2.Zero;

	public override void _Ready()
	{
		HitboxComponent = GetNode<HitboxComponent>("HitboxComponent");
	}
	public override void _Process(double delta)
	{
		GlobalPosition += Velocity * (float) delta;
	}
}
