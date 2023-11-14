namespace UI;
public partial class CurrencyIndicator : Control
{
	[Export] private PackedScene CurrencyGainTextScene;
	private Label myLabel;
	private GameEvents gameEvents;
	public override void _Ready()
	{
		myLabel = GetNode<Label>("Label");
		myLabel.Text = "10";
		GameEvents.Instance.PartsCollected += OnPartsCollected;
	}
	public override void _ExitTree()
    {
		GameEvents.Instance.PartsCollected -= OnPartsCollected;
    }

	void OnPartsCollected(int number)
	{
		myLabel.Text = GameEvents.Instance.Parts.ToString();
		
		var currencyGainText = CurrencyGainTextScene.Instantiate() as CurrencyGainText;
		GetTree().GetFirstNodeInGroup("foreground_layer").AddChild(currencyGainText);
		currencyGainText.GlobalPosition = myLabel.GlobalPosition + new Vector2(4, 54);
		currencyGainText.Start(number.ToString("0"));
	}

}
