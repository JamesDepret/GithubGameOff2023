namespace Ability;
public partial class VampireTurretController : BulletAbilityController
{
	[Export] public int HealthGainedPerHit = 0;
	HealthComponent playerHealthComponent;
	public override void _Ready()
	{
		base._Ready();
		var player = GetNode<Player>("../../../Player");
		playerHealthComponent = player.GetNode<HealthComponent>("HealthComponent");
	}

	protected override void InstantHitBullet(List<Node> enemies)
	{
		base.InstantHitBullet(enemies);
		playerHealthComponent.HealDamage(HealthGainedPerHit);
	}
}
