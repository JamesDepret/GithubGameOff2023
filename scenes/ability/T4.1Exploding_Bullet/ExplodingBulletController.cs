namespace Ability;
public partial class ExplodingBulletController : BulletAbilityController
{
	[Export] private PackedScene explosionScene;
	[Export] private int explodingDamage = 10;
	[Export] private int HitsBeforeDestroyed = 10;
	
	public override void _Ready()
	{
		base._Ready();
		instantHit = true;
	}
	protected override void InstantHitBullet(List<Node> enemies)
	{
		base.InstantHitBullet(enemies);
		for (int i = 0; i < bounces; i++){
			if (i > enemies.Count) return;
			OnHitEffect(enemies[i] as Enemy);
		}
	}

	private void OnHitEffect(Enemy enemy)
	{
		var explosion = explosionScene.Instantiate() as Node2D;
        CallDeferred(nameof(DeferredHitboxSettings), explosion, enemy);
	}

	private void DeferredHitboxSettings(Node2D explosion, Enemy enemy)
	{
		if (GetTree().GetFirstNodeInGroup("foreground_layer") is not Node2D foregroundLayer) throw new Exception("Could not find foreground_layer");
        foregroundLayer.AddChild(explosion);
		var hitbox = explosion.GetChild<HitboxComponent>(1);
		hitbox.Damage = explodingDamage;
		hitbox.HitsBeforeDestroyed = HitsBeforeDestroyed;
		explosion.GlobalPosition = enemy.GlobalPosition;
	}
}
