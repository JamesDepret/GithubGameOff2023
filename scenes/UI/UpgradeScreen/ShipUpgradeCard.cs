namespace UI;
public partial class ShipUpgradeCard : PanelContainer
{
	[Signal] public delegate void SelectedEventHandler();
	public bool DisabledByPrice { get; set; } = false;
	public bool DisabledBySupply { get; set; } = false;
	RichTextLabel NameLabel;
	RichTextLabel DescriptionLabel;
	Label PriceLabel;
	Label SupplyLabel;
	TextureRect Icon;
	public override void _Ready()
	{
		NameLabel = GetNode<RichTextLabel>("MarginContainer/VBoxContainer/HBoxContainer/VBoxContainer/NameLabel");
		DescriptionLabel = GetNode<RichTextLabel>("MarginContainer/VBoxContainer/MarginContainer/DescriptionLabel");
		PriceLabel = GetNode<Label>("MarginContainer/VBoxContainer/HBoxContainer/VBoxContainer/PanelContainer/PriceLabel");
		SupplyLabel = GetNode<Label>("MarginContainer/VBoxContainer/HBoxContainer/VBoxContainer/PanelContainer/SupplyLabel");
		Icon = GetNode<TextureRect>("MarginContainer/VBoxContainer/HBoxContainer/MarginContainer/PanelContainer/Icon");
		GuiInput += OnGuiInput;
		SetDisabledForPrice(false);
		SetDisabledForSupply(false);
	}

	
	public override void _ExitTree()
    {
		GuiInput -= OnGuiInput;
    }

	public void SetupCard(int price, int supplyCost, BaseUpgrade? upgrade, bool disabledByPrice, bool disabledBySupply, Texture2D icon)
	{
		SetPrice(price);
		SetSupply(supplyCost);
		if(upgrade != null) SetAbilityUpgrade(upgrade);
		SetDisabledForPrice(disabledByPrice);
		SetDisabledForSupply(disabledBySupply);
		SetTexture(icon);
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
		int level = upgrade.PreviousUpgradePointer == null ? 1 : 2;
		string color = level == 1 ? "Coral" : "Gold";
		NameLabel.Text = $"{upgrade.Name} -  [color={color}]{level}[/color]";
		DescriptionLabel.Text = upgrade.Description;
	}

	public void SetTexts(string name, string description)
	{
		NameLabel.Text = name;
		DescriptionLabel.Text = description;
	}

	public void SetTexture(Texture2D texture)
	{
		Icon.Texture = texture;
	}

	public void OnGuiInput(InputEvent @event)
	{
		if (@event.IsActionPressed("left_click") && !DisabledByPrice && !DisabledBySupply)
		{
			EmitSignal(SignalName.Selected);
		}
	}
}
