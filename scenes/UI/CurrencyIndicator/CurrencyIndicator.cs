namespace UI;
public partial class CurrencyIndicator : Control
{
	[Export] private PackedScene CurrencyGainTextScene;
	private Label label;
	private GameEvents gameEvents;
	public override void _Ready()
	{
		label = GetNode<Label>("Label");
		label.Text = "0";
		GameEvents.Instance.PartsCollected += OnPartsCollected;
	}

	void OnPartsCollected(int number)
	{
		label.Text = GameEvents.Instance.Parts.ToString();
		
		var currencyGainText = CurrencyGainTextScene.Instantiate() as CurrencyGainText;
		GetTree().GetFirstNodeInGroup("foreground_layer").AddChild(currencyGainText);
		currencyGainText.GlobalPosition = label.GlobalPosition + new Vector2(4, 54);
		currencyGainText.Start(number.ToString("0"));
	}

}
