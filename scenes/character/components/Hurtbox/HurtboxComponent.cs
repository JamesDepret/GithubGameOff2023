namespace Components;
public partial class HurtboxComponent : Area2D
{
	[Export] public HealthComponent HealthComponent { get; set; }
	[Export] public PackedScene floatingTextScene { get; set; }
	public override void _Ready()
	{
		AreaEntered += OnBodyEntered;
	}

	void OnBodyEntered(Node2D body)
	{
		if (body is not HitboxComponent hitbox || HealthComponent == null) return;
		HealthComponent.TakeDamage(hitbox.Damage);

		var floatingText = floatingTextScene.Instantiate() as FloatingText;
		GetTree().GetFirstNodeInGroup("foreground_layer").AddChild(floatingText);
		floatingText.GlobalPosition = GlobalPosition + (Vector2.Up * 8);
		
		if (Mathf.RoundToInt(hitbox.Damage) == hitbox.Damage) floatingText.Start(hitbox.Damage.ToString("0"));
		else floatingText.Start(hitbox.Damage.ToString("0.0"));
	}
}
