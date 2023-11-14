namespace Ability;
public partial class BulletAbilityController : BaseAbilityController
{
	[Export] protected double baseWaitTime = 1.5f;
	[Export] protected int bounces = 1;
	[Export] protected bool instantHit = false;
	[Export] PackedScene bulletAbility;
	[Export] float maxRange = 150.0f;
	[Export] float bulletSpeed = 300.0f;
	[Export] float damage = 5f;
	[Export] float damageReductionOnBounce = 0f;
	[Export] float critChance = 0f;
	[Export] int maxSpeedModifiers = 2;
	protected Godot.Timer cooldownTimer;
	private float baseDamage = 5f;
	private float timerDeviation = 0.05f;
	private int damageDone = 0;
	private double startLifeTime = 0f;
	private ArenaManager arenaManager;
	private List<SpeedModifier> speedModifiers = new();
	
	public override void _Ready()
	{
		cooldownTimer = GetNode<Godot.Timer>("Timer");
		cooldownTimer.WaitTime = baseWaitTime;
		cooldownTimer.Timeout += OnCooldownTimerTimeout;
		cooldownTimer.Start();
		baseDamage = damage;
		arenaManager = GetNode<ArenaManager>("/root/Main/Managers/ArenaManager");
		startLifeTime = arenaManager.GetTimeElapsed();
		arenaManager.WaveCleared += OnWaveCleared;
	}

	public void AddSpeedModifier(SpeedModifier speedModifier)
	{
		speedModifiers.Add(speedModifier);
		if(speedModifiers.Count > maxSpeedModifiers){
			var lowestValue = speedModifiers.Min(speedModifier => speedModifier.ModifierValue);
			speedModifiers.Remove(speedModifiers.Find(speedModifier => speedModifier.ModifierValue == lowestValue));
		}
	}

	public void RemoveSpeedModifier(SpeedModifier speedModifier)
	{
		speedModifiers.Remove(speedModifier);
	}

	void OnCooldownTimerTimeout()
    {
        if (GetParent().GetParent() is not Player player) return;
		
		// random number between -1 and 1
		var randomDeviation = (float) GD.RandRange(-1, 1) * timerDeviation;
		var speedModifier = 1f + speedModifiers.Sum(speedModifier => -speedModifier.ModifierValue);
		speedModifier = Mathf.Max(speedModifier, 0.1f);
		cooldownTimer.WaitTime = (baseWaitTime + randomDeviation) * speedModifier;

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

	protected virtual void InstantHitBullet(List<Node> enemies)
	{
		for (int i = 0; i < bounces; i++){
			if (i > enemies.Count) return;
			var enemy = enemies[i] as Enemy;
			enemy.HurtboxComponent.GetHit(damage, critChance);
			damageDone += (int) damage;
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

	void OnWaveCleared()
	{
		double endLife = arenaManager.GetTimeElapsed();
		double lifeTime = endLife - startLifeTime;
		GD.Print($"BulletAbilityController ${SubName} - damageDone: {damageDone} lifeTime: {lifeTime} DPS: {damageDone / lifeTime}");
	}
}
