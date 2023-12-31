namespace Upgrades;
public partial class BaseUpgrade : Resource
{
    [Export] public string Id { get; set; } = "";
	[Export] public string Name { get; set; } = "";
	[Export] public Texture2D Icon { get; set; }
	[Export(PropertyHint.MultilineText)] public string Description { get; set; } = "";
	[Export] public int Price { get; set; } = 10;
    [Export] public int Tier { get; set; } = 1;
    [Export] public int SupplyCost { get; set; } = 1;
	[Export] public Resource UpgradesTo { get; set; }
	[Export] public PackedScene AbilityControllerScene { get; set; }
	public BaseUpgrade PreviousUpgradePointer { get; set; }
	public Node ControllerPointer { get; set; }
}