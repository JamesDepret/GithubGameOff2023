namespace Manager;
public partial class ArenaManager : Node
{
	[Export] PackedScene EndScreenScene;
	public int WaveNumber  { get; set; } = 1;
	private Godot.Timer timer;
	public override void _Ready()
	{
		EnemyManager enemyManager = GetNode<EnemyManager>("EnemyManager");
		enemyManager.WaveCleared += OnWaveCleared;
		timer = GetNode<Godot.Timer>("Timer");
		timer.Timeout += OnTimerTimeout;
	}

	public double GetTimeElapsed()
	{
		return timer.WaitTime - timer.TimeLeft;
	}

	void OnTimerTimeout(){
		// var EndScreen = EndScreenScene.Instantiate() as VictoryScreen;
		// AddChild(EndScreen);

		// TODO: Add end screen
		GD.Print("Timer timeout");
	}

	void OnWaveCleared()
	{
		WaveNumber++;
	}
}
