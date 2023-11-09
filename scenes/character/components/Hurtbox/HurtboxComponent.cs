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
        if (hitbox.EnemiesHit.Contains(parent)) return;

        GetHit(hitbox.Damage, hitbox.CritChance);

        hitbox.HitsBeforeDestroyed--;
        hitbox.Damage *= 1 - hitbox.DamageReductionOnPierce;


        if (hitbox.HitsBeforeDestroyed == 0) hitbox.GetParent().QueueFree();
        if (hitbox.HitsBeforeDestroyed < 0) return;

        hitbox.EnemiesHit.Add(parent);
        hitbox.EmitSignal(nameof(HitboxComponent.OnHitEffect));

    }

    public void GetHit(float damage, float CritChance = 0f)
    {
        var critroll = (float) GD.RandRange(0, 100)/100;
        if (critroll < CritChance) damage *= 2;

        HealthComponent.TakeDamage(damage);

        var floatingText = FloatingTextScene.Instantiate() as FloatingText;
        GetTree().GetFirstNodeInGroup("foreground_layer").AddChild(floatingText);
        floatingText.GlobalPosition = GlobalPosition + (Vector2.Up * 8);

        if (Mathf.RoundToInt(damage) == damage) floatingText.Start(damage.ToString("0"));
        else floatingText.Start(damage.ToString("0.0"));
    }
}
