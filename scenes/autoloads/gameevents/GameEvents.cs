namespace Manager;
public partial class GameEvents : Node
{
	[Signal] public delegate void ShipUpgradeAddedEventHandler (BaseUpgrade upgrade, Godot.Collections.Array<BaseUpgrade> currentUpgrades);
	[Signal] public delegate void WaveClearedEventHandler (int waveNumber);
	public static GameEvents Instance { get; private set; }
	private PackedScene optionsMenu;
	private bool wasGamePaused = false;
	public override void _Ready()
	{
		Instance = this;
		optionsMenu = GD.Load<PackedScene>("res://scenes/UI/OptionsMenu/options_menu.tscn");
	}

	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("ui_esc"))
		{
			OnOptionsButtonPressed();
		}
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
		//Instance = new GameEvents();
		MusicPlayer.Instance.ConnectToSignals();
		MusicPlayer.Instance.StartGame();
		Instance.Supply = 0;
		Instance.Parts = 0;
		Instance.TotalScore = 0;
		Instance.MaxSupply = 10;
	}

	public void OnOptionsBackPressed()
	{
		GetTree().Paused = wasGamePaused;
	}

	private void OnOptionsButtonPressed()
	{
		// validate if the game has an optionmenu open
		var optionMenu = GetNodeOrNull<OptionsMenu>("/root/Main/OptionsMenu");
		if(optionMenu != null) 
		{
			GetTree().Paused = wasGamePaused;
			optionMenu.QueueFree();
			return;
		}
		
		wasGamePaused = GetTree().Paused;
		GetTree().Paused = true;
		var optionsMenuInstance = optionsMenu.Instantiate() as OptionsMenu;
		GetNode<Node>("/root/Main").AddChild(optionsMenuInstance);
	}
}
