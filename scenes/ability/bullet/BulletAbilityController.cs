using Godot;
using System;

public partial class BulletAbilityController : Node
{
	[Export] PackedScene bulletAbility;
	[Export] float maxRange = 150.0f;
	[Export] double baseWaitTime = 1.5f;
	[Export] float bulletSpeed = 300.0f;
	Godot.Timer cooldownTimer;
	float damage = 5f;
	float baseDamage = 5f;
	

	public override void _Ready()
	{
		cooldownTimer = GetNode<Godot.Timer>("Timer");
		cooldownTimer.WaitTime = baseWaitTime;
		cooldownTimer.Timeout += OnCooldownTimerTimeout;
		//GameEvents.Instance.AbilityUpgradeAdded += OnAbilityUpgradeAdded;
	}

	void OnCooldownTimerTimeout()
	{
        if (GetParent().GetParent() is not Player player) return;

        var enemies = GetTree().GetNodesInGroup("enemy")
							   .Where(enemy => FilterEnemyInRange(enemy, player))
							   .ToList();
		if (enemies.Count == 0) return;

		enemies.Sort((a, b) => SortByDistanceTo(a as Node2D, b as Node2D, player.GlobalPosition));

		var bulletInstance = bulletAbility.Instantiate() as BulletAbility;
        if (GetTree().GetFirstNodeInGroup("foreground_layer") is not Node2D foregroundLayer) throw new Exception("Could not find foreground_layer");

        foregroundLayer.AddChild(bulletInstance);
		//bulletInstance.HitboxComponent.Damage = damage;

		bulletInstance.GlobalPosition = player.GlobalPosition;
		var enemyDirection = (enemies[0] as Node2D).GlobalPosition - bulletInstance.GlobalPosition;
		bulletInstance.Velocity = enemyDirection.Normalized() * bulletSpeed;
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
