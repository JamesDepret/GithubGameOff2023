namespace UI;
public partial class IncomeUpgradeCard : PanelContainer
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

	private void OnHarvestIncomeAmountUpgraded()
	{
		var upgradeResult = !harvestManager.UpgradeHarvestIncome();
		Visible = upgradeResult;
		ResetLabels();
	}

	private void ResetLabels(int number = 0)
	{
		priceLabel.Text = $"{harvestManager.HarvestIncomeUpgradeCost}";
		name.Text = $"Income Amount [color=Coral]{harvestManager.BaseIncome + 1}[/color]";
		SetDisabledForPrice(harvestManager.HarvestIncomeUpgradeCost > GameEvents.Instance.Parts);
	}

	public void OnGuiInput(InputEvent @event)
	{
		if (@event.IsActionPressed("left_click"))
		{
			UpgradesScreen.SetupSelectedIncomeUpgrade(
				harvestManager.HarvestIncomeUpgradeCost, 
				icon,
				name: "Income Amount",
				description: "Increase the amount for which the parts harvester ticks.\nEffects: [color=Khaki]Increases tick amount by 1[/color]\n\nParts income is indicated by the progressbar in the top right corner, left from the total parts available.",
				UpgradesScreen.BuyButtonEnum.IncomeAmount
			);
		}
	}
}
