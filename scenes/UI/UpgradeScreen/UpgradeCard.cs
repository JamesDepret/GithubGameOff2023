namespace UI;
public partial class UpgradeCard : PanelContainer
{
	public UpgradesScreen UpgradesScreen { get; set; }
	public bool DisabledByPrice { get; set; } = false;
	public bool DisabledBySupply { get; set; } = false;
	TextureRect Icon;
	BaseUpgrade currentUpgrade;
	Label NameLabel;
	Label PriceLabel;
	Label SupplyLabel;
	public override void _Ready()
	{
		Icon = GetNode<TextureRect>("Border/Background/Icon");
		NameLabel = GetNode<Label>("MarginContainer/VBoxContainer/NameLabel");
		PriceLabel = GetNode<Label>("MarginContainer/VBoxContainer/PanelContainer/PriceLabel");
		SupplyLabel = GetNode<Label>("MarginContainer/VBoxContainer/PanelContainer/SupplyLabel");
		GuiInput += OnGuiInput;
		SetDisabledForPrice(false);
		SetDisabledForSupply(false);
	}

	public void SetupCard(int price, int supplyCost, BaseUpgrade upgrade, bool disabledByPrice, bool disabledBySupply)
	{
		SetPrice(price);
		SetSupply(supplyCost);
		SetAbilityUpgrade(upgrade);
		SetDisabledForPrice(disabledByPrice);
		SetDisabledForSupply(disabledBySupply);
	}

	public void SetPrice(int price)
	{
		PriceLabel.Text = price.ToString();
	}

	public void SetDisabledForPrice(bool disabled)
	{
		DisabledByPrice = disabled;
		PriceLabel.Modulate = disabled ? new Color(255, 0, 0) : new Color(0, 133, 221);
	}

	public void SetSupply(int supply)
	{
		SupplyLabel.Text = supply.ToString();
	}

	public void SetDisabledForSupply(bool disabled)
	{
		DisabledBySupply = disabled;
		SupplyLabel.Modulate = disabled ? new Color(255, 0, 0) : new Color(0, 133, 221);
	}

	public void SetAbilityUpgrade(BaseUpgrade upgrade)
	{
		NameLabel.Text = upgrade.Name;
		Icon.Texture = upgrade.Icon;
		currentUpgrade = upgrade;
	}

	public void OnGuiInput(InputEvent @event)
	{
		if (@event.IsActionPressed("left_click"))
		{
			UpgradesScreen.SetSelectedUpgrade(currentUpgrade);
		}
	}
}

