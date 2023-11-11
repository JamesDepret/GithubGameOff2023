namespace Ability;
public partial class BulletAbilityController : BaseAbilityController
{
	[Export] PackedScene bulletAbility;
	[Export] float maxRange = 150.0f;
	[Export] protected double baseWaitTime = 1.5f;
	[Export] float bulletSpeed = 300.0f;
	[Export] bool instantHit = false;
	[Export] float damage = 5f;
	[Export] int bounces = 1;
	[Export] float damageReductionOnBounce = 0f;
	[Export] float critChance = 0f;
	protected Godot.Timer cooldownTimer;
	float baseDamage = 5f;
	float timerDeviation = 0.05f;
	int damageDone = 0;
	float startLifeTime = 0f;
	

	public override void _Ready()
	{
		GD.Print("BulletAbilityController");
		cooldownTimer = GetNode<Godot.Timer>("Timer");
		cooldownTimer.WaitTime = baseWaitTime;
		cooldownTimer.Timeout += OnCooldownTimerTimeout;
		cooldownTimer.Start();
		baseDamage = damage;
		//GameEvents.Instance.AbilityUpgradeAdded += OnAbilityUpgradeAdded;
	}

	void OnCooldownTimerTimeout()
    {
        if (GetParent().GetParent() is not Player player) return;
		
		// random number between -1 and 1
		var randomDeviation = (float) GD.RandRange(-1, 1) * timerDeviation;
		cooldownTimer.WaitTime = baseWaitTime + randomDeviation;
		GD.Print("cooldownTimer.WaitTime", cooldownTimer.WaitTime);

        var enemies = GetTree().GetNodesInGroup("enemy")
                               .Where(enemy => FilterEnemyInRange(enemy, player))
                               .ToList();
        if (enemies.Count == 0) return;

        enemies.Sort((a, b) => SortByDistanceTo(a as Node2D, b as Node2D, player.GlobalPosition));

		if(instantHit) InstantHitBullet(enemies);
		else SpawnBullet(player, enemies);
    }

    private void SpawnBullet(Player player, List<Node> enemies)
    {
        var bulletInstance = bulletAbility.Instantiate() as BulletAbility;
        if (GetTree().GetFirstNodeInGroup("foreground_layer") is not Node2D foregroundLayer) throw new Exception("Could not find foreground_layer");

        foregroundLayer.AddChild(bulletInstance);
		bulletInstance.HitboxComponent.SetupHitboxComponent(damage, bounces, damageReductionOnBounce, critChance);
        bulletInstance.GlobalPosition = player.GlobalPosition;
        var enemyDirection = (enemies[0] as Node2D).GlobalPosition - bulletInstance.GlobalPosition;
        bulletInstance.Velocity = enemyDirection.Normalized() * bulletSpeed;
    }

	private void InstantHitBullet(List<Node> enemies)
	{
		for (int i = 0; i < bounces; i++){
			if (i > enemies.Count) return;
			var enemy = enemies[i] as Enemy;
			enemy.HurtboxComponent.GetHit(damage, critChance);
		}
	}

    bool FilterEnemyInRange(Node enemy, Player player)
    {
        if (enemy is not Node2D enemyNode) return false;
        var distance = enemyNode.GlobalPosition.DistanceSquaredTo(player.GlobalPosition);
        return distance <= Mathf.Pow(maxRange, 2);
    }

    int SortByDistanceTo(Node2D a, Node2D b, Vector2 positionToCompare)
	{
		var aDistance = a.GlobalPosition.DistanceSquaredTo(positionToCompare);
		var bDistance = b.GlobalPosition.DistanceSquaredTo(positionToCompare);
		return aDistance.CompareTo(bDistance);
	}
}
