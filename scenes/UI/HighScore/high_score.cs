namespace UI;
public partial class high_score : Control
{
	Label scoreLabel;
	public override void _Ready()
	{
		scoreLabel = GetNode<Label>("ScoreLabel");
		GameEvents.Instance.PartsCollected += OnPartsCollected;
	}

	private void OnPartsCollected(int number)
	{
		scoreLabel.Text = $"high score: {GameEvents.Instance.TotalScore}";
	}
}