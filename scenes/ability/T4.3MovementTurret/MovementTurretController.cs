namespace Ability;
public partial class MovementTurretController : BulletAbilityController
{
	[Export] private float movementBoostChance = 0.1f;
	[Export] private int movementBoost = 20;
	private Player player;
	public override void _Ready()
	{
		base._Ready();
		instantHit = true;
		player = GetParent().GetParent() as Player;
	}

	protected override void InstantHitBullet(List<Node> enemies)
	{
		for (int i = 0; i < bounces; i++){
			if (i > enemies.Count) return;
			var enemy = enemies[i] as Enemy;
			var enemyHealth = enemy.GetNode<HealthComponent>("HealthComponent");
			if(enemyHealth.CurrentHealth > damage) continue;

			var randomRoll = ((float)GD.RandRange(0, 100)) / 100;
			if(randomRoll > movementBoostChance) continue;

			player.AddMovementBoost(movementBoost);
		}
		base.InstantHitBullet(enemies);
	}
}
