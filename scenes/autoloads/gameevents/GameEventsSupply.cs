namespace Manager;
public partial class GameEvents : Node
{
    
	[Signal] public delegate void SupplyChangedEventHandler ();
	public int Supply { get; set; } = 0;
	public int MaxSupply { get; set; } = 10;
	public int MaxSupplyUpgraded { get; set; } = 50;
	public int SupplyUpgradePrice { get; set; } = 50;
    

	public void BuySupply()
	{
		if (Parts >= SupplyUpgradePrice)
		{
			MaxSupply += 5;
            EmitPartsCollected(-SupplyUpgradePrice);
			EmitSignal(SignalName.SupplyChanged);
		}
	}
}