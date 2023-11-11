namespace Manager;
public partial class EnemyManager : Node
{
	[Signal] public delegate void WaveClearedEventHandler();
	[Export] public Godot.Collections.Array<PackedScene> EnemyScenePool = new();
	[Export] public Godot.Collections.Array<int> EnemiesPerWave = new();
	[Export] public Godot.Collections.Array<float> EnemySpawnratePerWave = new();
	[Export] public ArenaManager arenaManager;
	private List<string> currentWaveEnemies = new();
	private int spawnRadius = 0;
	private Godot.Timer timer;
	private double baseSpawnTime = 0f;
	private int mobLevel = 0;
	private int currentWaveKills = 0;
	private int currentWaveSpawns = 0;
	public override void _Ready()
	{
		timer = GetNode<Godot.Timer>("Timer");
		baseSpawnTime = EnemySpawnratePerWave[arenaManager.WaveNumber] - 1;
		timer.WaitTime = baseSpawnTime;
		timer.Timeout += OnTimerTimeoutSpawnEnemy;
		spawnRadius = (int) (GetViewport().GetVisibleRect().Size.X / 2) + 50;
		arenaManager.enemyManager = this;
		WaveCleared += arenaManager.OnWaveCleared;
		arenaManager.WaveCleared += OnStarNextLevel;
	}

	private void EnemyDied(string enemyName)
	{
		if(!currentWaveEnemies.Contains(enemyName)) return;
		currentWaveEnemies.Remove(enemyName);
		currentWaveKills++;
		//GD.Print("currentKills " + currentWaveKills + " - Enemies this wave: " + EnemiesPerWave[arenaManager.WaveNumber] + "Name: " + enemyName);
		if(currentWaveKills >= EnemiesPerWave[arenaManager.WaveNumber]) 
			WaveIsCleared();
	}
	Vector2 GetSpawnPosition()
	{
        Vector2 basePosition = GetViewport().GetVisibleRect().Size / 2;
        var spawnPosition = Vector2.Zero;
		var randomDirection = Vector2.Right.Rotated(GD.Randf() * Mathf.Pi * 2);

		for (int i = 0; i < 4; i++)
		{
			spawnPosition = basePosition + randomDirection * spawnRadius;

			var query_params = PhysicsRayQueryParameters2D.Create(basePosition, spawnPosition, 1); // 1 << 0: bit at position 0
			var result = GetTree().Root.World2D.DirectSpaceState.IntersectRay(query_params);
			if (result.Count == 0)
			{
				break;
			}
			else
			{
				randomDirection = randomDirection.Rotated(Mathf.DegToRad(90));
			}
		}

		return spawnPosition;
	}


	private void OnTimerTimeoutSpawnEnemy() 
	{
		if(currentWaveSpawns >= EnemiesPerWave[arenaManager.WaveNumber])
		{
			timer.Stop();
			return;
		}
		timer.Start();

		var enemy = EnemyScenePool[arenaManager.WaveNumber].Instantiate() as Node2D;
        if (GetTree().GetFirstNodeInGroup("entities_layer") is not Node2D entitiesLayer) throw new Exception("Could not find entities layer");

        entitiesLayer.AddChild(enemy);
		enemy.GlobalPosition = GetSpawnPosition();
		enemy.GetNode<HealthComponent>("HealthComponent").Died += EnemyDied;
		currentWaveEnemies.Add(enemy.Name);
		currentWaveSpawns++;
	}

	private void WaveIsCleared()
	{
	    EmitSignal(SignalName.WaveCleared);
	}

	private void OnStarNextLevel()
	{
		timer.WaitTime = EnemySpawnratePerWave[arenaManager.WaveNumber] - 1;
		currentWaveKills = 0;
		currentWaveSpawns = 0;
		GD.Print("Starting next wave - " + arenaManager.WaveNumber + " - spawntime " + baseSpawnTime + " - enemies per wave " + EnemiesPerWave[arenaManager.WaveNumber]
		+ " - currentKills " + currentWaveKills + " - currentSpawns " + currentWaveSpawns);
		timer.Start();
	}
}
