namespace Manager;
public partial class ArenaManager : Node
{
	[Signal] public delegate void WaveClearedEventHandler();
	[Export] PackedScene EndScreenScene;
	[Export] public int StartingParts { get; set; } = 100;
	public EnemyManager enemyManager;
	public int WaveNumber  { get; set; } = 0;
	private Godot.Timer timer;
	public override void _Ready()
	{
		timer = GetNode<Godot.Timer>("Timer");
		timer.Timeout += OnTimerTimeout;
		GameEvents.Instance.EmitPartsCollected(StartingParts);
	}

	public double GetTimeElapsed()
	{
		return timer.WaitTime - timer.TimeLeft;
	}

	public void OnWaveCleared()
	{
		WaveNumber++;

		// TODO: should start AFTER upgrades
	    EmitSignal(SignalName.WaveCleared);
	}
	void OnTimerTimeout(){
		// var EndScreen = EndScreenScene.Instantiate() as VictoryScreen;
		// AddChild(EndScreen);

		// TODO: Add end screen
		GD.Print("Timer timeout");
	}

}
