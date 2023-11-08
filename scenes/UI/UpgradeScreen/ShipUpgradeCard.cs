namespace UI;
public partial class ShipUpgradeCard : PanelContainer
{
	[Signal] public delegate void SelectedEventHandler();
	// TODO: ADD price label in the scene and make it red if price is too high
	public bool DisabledByPrice { get; set; } = false;
	public bool DisabledBySupply { get; set; } = false;
	Label NameLabel;
	RichTextLabel DescriptionLabel;
	Label PriceLabel;
	Label SupplyLabel;
	public override void _Ready()
	{
		NameLabel = GetNode<Label>("MarginContainer/VBoxContainer/NameLabel");
		DescriptionLabel = GetNode<RichTextLabel>("MarginContainer/VBoxContainer/DescriptionLabel");
		PriceLabel = GetNode<Label>("MarginContainer/VBoxContainer/PanelContainer/PriceLabel");
		SupplyLabel = GetNode<Label>("MarginContainer/VBoxContainer/PanelContainer/SupplyLabel");
		GuiInput += OnGuiInput;
		SetDisabledForPrice(false);
		SetDisabledForSupply(false);
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
		DescriptionLabel.Text = upgrade.Description;
	}

	public void OnGuiInput(InputEvent @event)
	{
		if (@event.IsActionPressed("left_click") && !DisabledByPrice && !DisabledBySupply)
		{
			EmitSignal(SignalName.Selected);
		}
	}
}
