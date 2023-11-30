namespace Manager;
public partial class ArenaManager : Node
{
	[Signal] public delegate void WaveClearedEventHandler();
	[Export] PackedScene EndScreenScene;
	[Export] public int StartingParts { get; set; } = 100;
	[Export] public float WaitTime { get; set; } = 4f;
	public EnemyManager enemyManager;
	public int WaveNumber  { get; set; } = 0;
	private Godot.Timer timer;
	private Timer waveDelayTimer;
	private AnimatedSprite2D player;
	public override void _Ready()
	{
		player = GetNode<AnimatedSprite2D>("Player");
		timer = GetNode<Godot.Timer>("Timer");
		GameEvents.Instance.EmitPartsCollected(StartingParts, false);
		waveDelayTimer = new Timer();
		AddChild(waveDelayTimer);
		waveDelayTimer.Timeout += DelayedWaveCleared;
		waveDelayTimer.Stop();
	}

	public double GetTimeElapsed()
	{
		return timer.WaitTime - timer.TimeLeft;
	}

	public void OnWaveCleared()
	{
		waveDelayTimer.WaitTime = WaitTime;
		waveDelayTimer.Start();
		player.Visible = true;
		player.Frame = 0;
		player.Modulate = new Color(1, 1, 1, 0);
		player.Scale = new Vector2(0.2f, 0.2f);
		Tween tween = player.CreateTween();
		tween.SetParallel();
		tween.TweenProperty(player, "scale", new Vector2(1f, 1f), 1f);
		tween.TweenProperty(player, "modulate:a", 1, 1f);
		tween.Chain();
		var callable = Callable.From(StartAnimation);
		tween.TweenCallback(callable);
	}

	private void StartAnimation()
	{
		player.Play();
	}

	private void DelayedWaveCleared()
	{
		waveDelayTimer.Stop();
		player.Stop();
		player.Visible = false;
		WaveNumber++;
		GameEvents.Instance.EmitSignal(SignalName.WaveCleared, WaveNumber);
	    EmitSignal(SignalName.WaveCleared);
		if(WaveNumber >= enemyManager.EnemyScenePool.Count) EndGame("Victory!");
	}

	public void EndGame(string text){
		var EndScreen = EndScreenScene.Instantiate() as VictoryScreen;
		AddChild(EndScreen);
		EndScreen.EndingLabel.Text = text;
	}

}
