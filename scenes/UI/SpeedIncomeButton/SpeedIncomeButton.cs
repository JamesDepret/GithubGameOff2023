namespace UI;
public partial class SpeedIncomeButton : PanelContainer
{
	public UpgradesScreen UpgradesScreen { get; set; }
	private HarvestManager harvestManager;
	Label priceLabel;
	RichTextLabel name;
	Texture2D icon;
	
	public override void _Ready()
	{
		harvestManager = GetNode<HarvestManager>("/root/Main/Managers/HarvestManager");
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

	private void OnHarvestSpeedIncomeUpgraded()
	{
		var upgradeResult = !harvestManager.UpgradeHarvestTime();
		Visible = upgradeResult;
		ResetLabels();
	}

	private void ResetLabels(int number = 0)
	{
		priceLabel.Text = $"{harvestManager.HarvestSpeedUpgradeCost}";
		name.Text = $"Income Speed [color=Coral]{harvestManager.HarvestTimeLevel + 1}[/color]";
		SetDisabledForPrice(harvestManager.HarvestSpeedUpgradeCost > GameEvents.Instance.Parts);
	}

	public void OnGuiInput(InputEvent @event)
	{
		if (@event.IsActionPressed("left_click"))
		{
			UpgradesScreen.SetupSelectedIncomeOrSupplyUpgrade(
				harvestManager.HarvestSpeedUpgradeCost, 
				icon,
				name: "Income Speed",
				description: "Increase the speed at which the parts harvester ticks.\nEffects: [color=Khaki]Reduces the time it takes to get free parts by 1 second[/color]\n\nParts income is indicated by the progressbar in the top right corner, left from the total parts available.",
				UpgradesScreen.BuyButtonEnum.IncomeSpeed
			);
			Modulate = new Color(1, 1, 1, 0.8f);
		}
	}
}
