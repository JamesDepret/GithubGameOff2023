namespace Manager;
public partial class ArenaManager : Node
{

	[Signal] public delegate void StarNextLevelEventHandler();
	[Export] PackedScene EndScreenScene;
	public EnemyManager enemyManager;
	public int WaveNumber  { get; set; } = 0;
	private Godot.Timer timer;
	public override void _Ready()
	{
		timer = GetNode<Godot.Timer>("Timer");
		timer.Timeout += OnTimerTimeout;
	}

	public double GetTimeElapsed()
	{
		return timer.WaitTime - timer.TimeLeft;
	}

	public void OnWaveCleared()
	{
		WaveNumber++;

		// TODO: should start AFTER upgrades
	    EmitSignal(SignalName.StarNextLevel);
	}
	void OnTimerTimeout(){
		// var EndScreen = EndScreenScene.Instantiate() as VictoryScreen;
		// AddChild(EndScreen);

		// TODO: Add end screen
		GD.Print("Timer timeout");
	}

}
