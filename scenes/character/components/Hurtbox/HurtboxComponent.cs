namespace Components;
public partial class HurtboxComponent : Area2D
{
	[Export] public HealthComponent HealthComponent { get; set; }
	[Export] public PackedScene FloatingTextScene { get; set; }
	public override void _Ready()
	{
		AreaEntered += OnBodyEntered;
	}

	void OnBodyEntered(Node2D body)
	{
		if (body is not HitboxComponent hitbox || HealthComponent == null) return;
		var parent = GetParent() as Enemy;
		if(hitbox.EnemiesHit.Contains(parent)) return;
		
		HealthComponent.TakeDamage(hitbox.Damage);
		hitbox.HitsBeforeDestroyed--;
		if (hitbox.HitsBeforeDestroyed == 0) hitbox.GetParent().QueueFree();
		if (hitbox.HitsBeforeDestroyed < 0) return;

		hitbox.EnemiesHit.Add(parent);
		hitbox.EmitSignal(nameof(HitboxComponent.OnHitEffect));

		var floatingText = FloatingTextScene.Instantiate() as FloatingText;
		GetTree().GetFirstNodeInGroup("foreground_layer").AddChild(floatingText);
		floatingText.GlobalPosition = GlobalPosition + (Vector2.Up * 8);
		
		if (Mathf.RoundToInt(hitbox.Damage) == hitbox.Damage) floatingText.Start(hitbox.Damage.ToString("0"));
		else floatingText.Start(hitbox.Damage.ToString("0.0"));
	}
}
