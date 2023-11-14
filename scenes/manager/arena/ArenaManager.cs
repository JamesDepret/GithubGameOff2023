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
		GameEvents.Instance.EmitPartsCollected(StartingParts, false);
	}

	public double GetTimeElapsed()
	{
		return timer.WaitTime - timer.TimeLeft;
	}

	public void OnWaveCleared()
	{
		WaveNumber++;
		GameEvents.Instance.EmitSignal(SignalName.WaveCleared, WaveNumber);
	    EmitSignal(SignalName.WaveCleared);
		if(WaveNumber > enemyManager.EnemyScenePool.Count) EndGame("Victory!");
	}
	public void EndGame(string text){
		var EndScreen = EndScreenScene.Instantiate() as VictoryScreen;
		AddChild(EndScreen);
		EndScreen.EndingLabel.Text = text;
	}

}
