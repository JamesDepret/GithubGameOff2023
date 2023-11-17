namespace Manager;
public partial class GameEvents : Node
{
	[Signal] public delegate void ShipUpgradeAddedEventHandler (BaseUpgrade upgrade, Godot.Collections.Array<BaseUpgrade> currentUpgrades);
	[Signal] public delegate void WaveClearedEventHandler (int waveNumber);
	public static GameEvents Instance { get; private set; }
	private PackedScene optionsMenu;
	public override void _Ready()
	{
		Instance = this;
		optionsMenu = GD.Load<PackedScene>("res://scenes/UI/OptionsMenu/options_menu.tscn");
	}

	public void EmitShipUpgradeAdded(BaseUpgrade upgrade, Godot.Collections.Array<BaseUpgrade> currentUpgrades)
	{
		Supply += upgrade.SupplyCost;
		EmitSignal(SignalName.ShipUpgradeAdded, upgrade, currentUpgrades);
		EmitSignal(SignalName.SupplyChanged);
	}

	public static void Restart()
	{
		MusicPlayer.Instance.DisconnectFromSignals();
		Instance = new GameEvents();
		MusicPlayer.Instance.ConnectToSignals();
		MusicPlayer.Instance.StartGame();
	}

	private void OnOptionsButtonPressed()
	{
		GetTree().Paused = true;
		var optionsMenuInstance = optionsMenu.Instantiate() as OptionsMenu;
		AddChild(optionsMenuInstance);
	}
}
