namespace UI;
public partial class ShipUpgradeCard : PanelContainer
{
	[Signal] public delegate void SelectedEventHandler();
	// TODO: ADD price label in the scene and make it red if price is too high
	public bool Disabled { get; set; } = false;
	Label NameLabel;
	Label DescriptionLabel;
	Label PriceLabel;
	public override void _Ready()
	{
		NameLabel = GetNode<Label>("MarginContainer/VBoxContainer/NameLabel");
		DescriptionLabel = GetNode<Label>("MarginContainer/VBoxContainer/DescriptionLabel");
		PriceLabel = GetNode<Label>("MarginContainer/VBoxContainer/PanelContainer/PriceLabel");
		GuiInput += OnGuiInput;
		SetDisabled(false);
	}

	public void SetPrice(int price)
	{
		PriceLabel.Text = price.ToString();
	}

	public void SetDisabled(bool disabled)
	{
		Disabled = disabled;
		PriceLabel.Modulate = disabled ? new Color(255, 0, 0) : new Color(0, 133, 221);
	}

	public void SetAbilityUpgrade(BaseUpgrade upgrade)
	{
		NameLabel.Text = upgrade.Name;
		DescriptionLabel.Text = upgrade.Description;
	}

	public void OnGuiInput(InputEvent @event)
	{
		if (@event.IsActionPressed("left_click") && !Disabled)
		{
			EmitSignal(SignalName.Selected);
		}
	}
}
