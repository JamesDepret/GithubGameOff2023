namespace UI;
public partial class ArenaWaveUI : MarginContainer
{
	
	[Export] ArenaManager arenaManager;
	Label timeLabel;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		timeLabel = GetNode<Label>("Label");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (arenaManager == null) return;
		var timeElapsed = arenaManager.GetTimeElapsed();
		timeLabel.Text = $"Wave {arenaManager.WaveNumber + 1} - {FormatSecondsToString(timeElapsed)}";
	}

	string FormatSecondsToString(double seconds)
	{
		var minutes = Mathf.FloorToInt((float) seconds / 60.0f);
		var secondsLeft = Mathf.FloorToInt(seconds - minutes * 60.0f);
		if (secondsLeft >= 60) secondsLeft = 0;
		return $"{minutes}:{secondsLeft:00}";
	}
}
