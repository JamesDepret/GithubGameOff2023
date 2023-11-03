
namespace Character;
public partial class EnemyAttackComponent : Node
{
	[Export] PackedScene EnemyAttackAbility;
	[Export] public float Damage = 10;
	[Export] public float AttackSpeed = 1;
	[Export] public float AttackRange = 50;
	[Export] public float BulletSpeed = 300;
	private Godot.Timer attackTimer;
	public override void _Ready()
	{
		attackTimer = GetNode<Godot.Timer>("Timer");
		attackTimer.WaitTime = AttackSpeed;
		attackTimer.Timeout += AttackSpeedTimerTimeout;
	}

	private void AttackSpeedTimerTimeout()
	{		
        if (GetParent() is not Enemy enemy) return;
        var players = GetTree().GetNodesInGroup("player").ToList();
		if (players.Count == 0) return;

		var playerNodes = players.ConvertAll(x => x as Player);
		playerNodes.Sort((a, b) => a.PlayerPrio.CompareTo(b.PlayerPrio));
		
		// get the first playerNode that is in the attackrange
		var playerNode = playerNodes.Find(x => x.GlobalPosition.DistanceSquaredTo(enemy.GlobalPosition) <= Mathf.Pow(AttackRange, 2));
		if(playerNode == null) return;

		var enemyAttackInstance = EnemyAttackAbility.Instantiate() as EnemyAttackAbility;
        if (GetTree().GetFirstNodeInGroup("foreground_layer") is not Node2D foregroundLayer) throw new Exception("Could not find foreground_layer");

        foregroundLayer.AddChild(enemyAttackInstance);
		enemyAttackInstance.HitboxComponent.Damage = Damage;

		enemyAttackInstance.GlobalPosition = (GetParent() as Node2D).GlobalPosition;

		var enemyDirection = (players[0] as Node2D).GlobalPosition - enemyAttackInstance.GlobalPosition;
		enemyAttackInstance.Velocity = enemyDirection.Normalized() * BulletSpeed;
	}


	
}
