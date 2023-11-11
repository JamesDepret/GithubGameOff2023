using System.Runtime.Serialization;

namespace UI;
public partial class ShipUpgradeCard : PanelContainer
{
	[Signal] public delegate void SelectedEventHandler();
	public bool DisabledByPrice { get; set; } = false;
	public bool DisabledBySupply { get; set; } = false;
	Label NameLabel;
	RichTextLabel DescriptionLabel;
	Label PriceLabel;
	Label SupplyLabel;
	TextureRect Background;
	public override void _Ready()
	{
		NameLabel = GetNode<Label>("MarginContainer/VBoxContainer/NameLabel");
		DescriptionLabel = GetNode<RichTextLabel>("MarginContainer/VBoxContainer/DescriptionLabel");
		PriceLabel = GetNode<Label>("MarginContainer/VBoxContainer/PanelContainer/PriceLabel");
		SupplyLabel = GetNode<Label>("MarginContainer/VBoxContainer/PanelContainer/SupplyLabel");
		Background = GetNode<TextureRect>("Border/Background/Icon");
		GuiInput += OnGuiInput;
		SetDisabledForPrice(false);
		SetDisabledForSupply(false);
	}

	public void SetupCard(int price, int supplyCost, BaseUpgrade upgrade, bool disabledByPrice, bool disabledBySupply, Texture2D icon)
	{
		SetPrice(price);
		SetSupply(supplyCost);
		SetAbilityUpgrade(upgrade);
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
		NameLabel.Text = $"{upgrade.Name} - {level}";
		DescriptionLabel.Text = upgrade.Description;
	}

	public void SetTexture(Texture2D texture)
	{
		Background.Texture = texture;
	}

	public void OnGuiInput(InputEvent @event)
	{
		if (@event.IsActionPressed("left_click") && !DisabledByPrice && !DisabledBySupply)
		{
			EmitSignal(SignalName.Selected);
		}
	}
}
