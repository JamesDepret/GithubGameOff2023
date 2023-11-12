namespace Manager;
public partial class UpgradeManager : Node
{
	[Export] BaseUpgrade[] upgradePool = Array.Empty<BaseUpgrade>();
	[Export] ArenaManager arenaManager;
	[Export] PackedScene upgradesScreen;
	private Godot.Collections.Array<BaseUpgrade> currentUpgrades = new();
	private UpgradesScreen currentUpgradeScreen;

	public override void _Ready()
	{
		arenaManager.WaveCleared += OnWaveCleared;
	}

	public void InitFirstUpgrades()
	{
		SetupUpgradeScreen();
	}

	private void OnWaveCleared()
	{
		SetupUpgradeScreen();
	}

	private void SetupUpgradeScreen()
	{
		currentUpgradeScreen = upgradesScreen.Instantiate() as UpgradesScreen;
		AddChild(currentUpgradeScreen);
		var chosenUpgrades = PickRandomTurretPerTier();
		currentUpgradeScreen.SetAbilityUpgrades(chosenUpgrades.ToArray());
		currentUpgradeScreen.UpgradeSelected += OnUpgradeBought;
		currentUpgradeScreen.SetTurrets(currentUpgrades.ToArray());
		if(arenaManager.WaveNumber>=1) currentUpgradeScreen.HelpPanel.QueueFree();
	}

	void OnUpgradeBought(BaseUpgrade upgrade)
	{
		if (upgrade.PreviousUpgradePointer != null)
		{
			currentUpgrades.Remove(upgrade.PreviousUpgradePointer);
		}
		ApplyUpgrade(upgrade);
	}

	void ApplyUpgrade(BaseUpgrade upgrade)
	{
		currentUpgrades.Add(upgrade);
		GameEvents.Instance.EmitShipUpgradeAdded(upgrade, currentUpgrades);
	}

	List<BaseUpgrade> PickRandomTurretPerTier()
    {
        var chosenUpgrades = new List<BaseUpgrade>();
        for (int i = 0; i < 6; i++)
        {
        	var filteredUpgrades = upgradePool.Where(upg => upg.Tier == i).ToList();
            if (filteredUpgrades.Count != 0)
				chosenUpgrades.Add(PickRandom(filteredUpgrades));
        }
        return chosenUpgrades;
    }

	BaseUpgrade PickRandom(List<BaseUpgrade> upgrades)
	{
		if (upgrades.Count == 0) return null;
		int index = (int) (GD.Randi() % upgrades.Count);
		return upgrades[index];
	}

}
