namespace Ability;

public partial class BulletAbility : Node2D
{
	public HitboxComponent HitboxComponent { get; private set; }
	public Vector2 Velocity { get; set; } = Vector2.Zero;
	private int hitsBeforeDestroyed = 1;

	public override void _Ready()
	{
		HitboxComponent = GetNode<HitboxComponent>("HitboxComponent");
		HitboxComponent.HitsBeforeDestroyed = hitsBeforeDestroyed;
	}
	public override void _Process(double delta)
	{
		GlobalPosition += Velocity * (float) delta;
	}
}
