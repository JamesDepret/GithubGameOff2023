namespace UI;
public partial class high_score : Control
{
	Label scoreLabel;
	public override void _Ready()
	{
		scoreLabel = GetNode<Label>("ScoreLabel");
		GameEvents.Instance.PartsCollected += OnPartsCollected;
	}
	public override void _ExitTree()
    {
		GameEvents.Instance.PartsCollected -= OnPartsCollected;
    }

	private void OnPartsCollected(int number)
	{
		scoreLabel.Text = $"Score: {GameEvents.Instance.TotalScore}";
	}
}
