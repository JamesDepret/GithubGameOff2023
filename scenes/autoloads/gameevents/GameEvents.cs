namespace Manager;
public partial class GameEvents : Node
{

	[Signal] public delegate void PartsCollectedEventHandler (int number);
	[Signal] public delegate void ShipUpgradeAddedEventHandler (BaseUpgrade upgrade, Godot.Collections.Array<BaseUpgrade> currentUpgrades);
	
	public static GameEvents Instance { get; private set; }
	public int Parts { get; set; } = 0;
	public int Supply { get; set; } = 0;
	public int MaxSupply { get; set; } = 10;
	public int MaxSupplyUpgraded { get; set; } = 50;
	public override void _Ready()
	{
		Instance = this;
	}

	public void EmitPartsCollected(int number)
	{
		Parts += number;
		EmitSignal(SignalName.PartsCollected, number);
	}
	
	public void EmitShipUpgradeAdded(BaseUpgrade upgrade, Godot.Collections.Array<BaseUpgrade> currentUpgrades)
	{
		Supply += upgrade.SupplyCost;
		EmitSignal(SignalName.ShipUpgradeAdded, upgrade, currentUpgrades);
	}
}
