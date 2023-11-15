namespace UI;
public partial class RoomIncreaseCard : PanelContainer
{
	public UpgradesScreen UpgradesScreen { get; set; }
	Label priceLabel;
	RichTextLabel name;
	Texture2D icon;
	
	public override void _Ready()
	{
		priceLabel = GetNode<Label>("MarginContainer/VBoxContainer/PanelContainer/PriceLabel");
		name = GetNode<RichTextLabel>("MarginContainer/VBoxContainer/NameLabel");
		icon = GetNode<TextureRect>("Border/Background/VBoxContainer/Icon").Texture;
		GuiInput += OnGuiInput;
		ResetLabels();
		GameEvents.Instance.PartsCollected += ResetLabels;
	}

	public override void _ExitTree()
	{
		GuiInput -= OnGuiInput;
		GameEvents.Instance.PartsCollected -= ResetLabels;
	}

	public void SetDisabledForPrice(bool disabled)
	{
		priceLabel.Modulate = disabled ? new Color(255, 0, 0) : new Color(0, 133, 221);
	}

	private void OnRoomIncreased()
	{
		GameEvents.Instance.BuySupply();
		ResetLabels();
	}

	private void ResetLabels(int number = 0)
	{
		priceLabel.Text = $"{GameEvents.Instance.SupplyUpgradePrice}";
		name.Text = $"Room";
		SetDisabledForPrice(GameEvents.Instance.SupplyUpgradePrice > GameEvents.Instance.Parts);
	}

	public void OnGuiInput(InputEvent @event)
	{
		if (@event.IsActionPressed("left_click"))
		{
			UpgradesScreen.SetupSelectedIncomeUpgrade(
				GameEvents.Instance.SupplyUpgradePrice, 
				icon,
				name: "Increase Room Amount",
				description: "Increase the room for turrets so you can install more.\nEffects: [color=Khaki]Increases room amount by 5[/color]\n\nThe max amount of room is 50.",
				UpgradesScreen.BuyButtonEnum.RoomUpgrade
			);
		}
	}
}
