namespace Manager;
public partial class GameEvents : Node
{
	[Signal] public delegate void ShipUpgradeAddedEventHandler (BaseUpgrade upgrade, Godot.Collections.Array<BaseUpgrade> currentUpgrades);
    [Signal] public delegate void NewControllerAdded(BaseUpgrade upgrade, BaseAbilityController controller);
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
}
