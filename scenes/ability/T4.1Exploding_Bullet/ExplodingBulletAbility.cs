namespace Ability;
public partial class ExplodingBulletAbility : BulletAbility
{
	[Export] private PackedScene explosionScene;
	[Export] private int explodingDamage = 10;
	[Export] private int HitsBeforeDestroyed = 10;
	public override void _Ready()
	{
		base._Ready();
		HitboxComponent.OnHitEffect += OnHitEffect;
	}

	private void OnHitEffect()
	{
		var explosion = explosionScene.Instantiate() as Node2D;
        
		CallDeferred(nameof(DeferredHitboxSettings), explosion);
	}

	private void DeferredHitboxSettings(Node2D explosion)
	{
		if (GetTree().GetFirstNodeInGroup("foreground_layer") is not Node2D foregroundLayer) throw new Exception("Could not find foreground_layer");
        foregroundLayer.AddChild(explosion);
		var hitbox = explosion.GetChild<HitboxComponent>(1);
		hitbox.Damage = explodingDamage;
		hitbox.HitsBeforeDestroyed = HitsBeforeDestroyed;
		explosion.GlobalPosition = GlobalPosition;
	}

	public override void _ExitTree()
    {
		HitboxComponent.OnHitEffect -= OnHitEffect;
    }
}
