namespace UI;
public partial class VictoryScreen : CanvasLayer
{
	public Label EndingLabel { get; set; }
	private Label scoreLabel;
	public override void _Ready()
	{
		GetTree().Paused = true;
		EndingLabel = GetNode<Label>("MarginContainer/PanelContainer/MarginContainer/VBoxContainer/EndingLabel");
		scoreLabel = GetNode<Label>("MarginContainer/PanelContainer/MarginContainer/VBoxContainer/ScoreLabel");
		scoreLabel.Text = $"Score: {GameEvents.Instance.TotalScore}";
		var restartButton = GetNode<Button>("MarginContainer/PanelContainer/MarginContainer/VBoxContainer/Restart");
		var quitButton = GetNode<Button>("MarginContainer/PanelContainer/MarginContainer/VBoxContainer/Quit");
		restartButton.Pressed += OnRestartButtonPressed;
		quitButton.Pressed += OnQuitButtonPressed;
	}

	private void OnRestartButtonPressed()
	{
		GetTree().Paused = false;
		GameEvents.Instance.Restart();
		GetTree().ChangeSceneToFile("res://scenes/main/main.tscn");
	}

	private void OnQuitButtonPressed()
	{
		GetTree().Quit();
	}
}
