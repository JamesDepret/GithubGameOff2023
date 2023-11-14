namespace Manager;
public partial class GameEvents : Node
{
	[Signal] public delegate void ShipUpgradeAddedEventHandler (BaseUpgrade upgrade, Godot.Collections.Array<BaseUpgrade> currentUpgrades);
	[Signal] public delegate void WaveClearedEventHandler (int waveNumber);
	public static GameEvents Instance { get; private set; }
	public override void _Ready()
	{
		Instance = this;
	}

	public void EmitShipUpgradeAdded(BaseUpgrade upgrade, Godot.Collections.Array<BaseUpgrade> currentUpgrades)
	{
		Supply += upgrade.SupplyCost;
		EmitSignal(SignalName.ShipUpgradeAdded, upgrade, currentUpgrades);
		EmitSignal(SignalName.SupplyChanged);
	}

	public void Restart()
	{
		TotalScore = 0;
		Parts = 0;
		Supply = 0;
		MaxSupply = 10;
		LootCritChance = 0f;
	}
}
