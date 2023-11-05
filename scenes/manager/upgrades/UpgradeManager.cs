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

	private void OnWaveCleared()
	{
		currentUpgradeScreen = upgradesScreen.Instantiate() as UpgradesScreen;
		AddChild(currentUpgradeScreen);
		var chosenUpgrades = PickRandomTurretPerTier();
		currentUpgradeScreen.SetAbilityUpgrades(chosenUpgrades.ToArray());
		currentUpgradeScreen.UpgradeSelected += OnUpgradeSelected;
	}


	void OnUpgradeSelected(BaseUpgrade upgrade)
	{
		ApplyUpgrade(upgrade);
	}

	void ApplyUpgrade(BaseUpgrade upgrade)
	{
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