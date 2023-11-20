namespace UI;
public partial class UpgradesScreen : CanvasLayer
{
	[Export] PackedScene UpgradeCardScene;
	[Export] PackedScene IncomeSpeedScene;
	[Export] PackedScene IncomeAmountScene;
	[Export] PackedScene SupplyScene;
	private void SetupCards()
	{
		CardContainer.GetChildren().ToList().ForEach(child => child.QueueFree());
		var parts = GameEvents.Instance.Parts;
		var currentSupply = GameEvents.Instance.Supply;
		var maxSupply = GameEvents.Instance.MaxSupply;
		var maxSupplyUpgraded = GameEvents.Instance.MaxSupplyUpgraded;
		foreach (var upgrade in availableUpgrades)
        {
            var upgradeCard = UpgradeCardScene.Instantiate() as UpgradeCard;
            CardContainer.AddChild(upgradeCard);
            upgradeCard.UpgradesScreen = this;
			upgradeCard.SetupCard(upgrade.Price, upgrade.SupplyCost, upgrade, parts < upgrade.Price, currentSupply + upgrade.SupplyCost > maxSupply);
        }
		
		SetupIncomeCards();
		if (selectedUpgrade != null ) SetupSelectedCard();
		canAddSupply = parts < GameEvents.Instance.SupplyUpgradePrice;
    }

    private void SetupIncomeCards()
	{		
		PermaCardsContainer.GetChildren().ToList().ForEach(child => child.QueueFree());
        Label label = new()
        {
            Text = "Permanent\nUpgrades"
        };
        PermaCardsContainer.AddChild(label);
		if(harvestManager.HarvestTimeLevel < 5)
		{
			var speedCard = IncomeSpeedScene.Instantiate() as SpeedIncomeButton;
			speedCard.UpgradesScreen = this;
			PermaCardsContainer.AddChild(speedCard);
		}
		
		if(harvestManager.BaseIncome < harvestManager.MaxIncome)
		{
			var amountCard = IncomeAmountScene.Instantiate() as IncomeUpgradeCard;
			amountCard.UpgradesScreen = this;
			PermaCardsContainer.AddChild(amountCard);
		}

		var maxSupply = GameEvents.Instance.MaxSupply;
		var maxSupplyUpgraded = GameEvents.Instance.MaxSupplyUpgraded;
		if (maxSupply < maxSupplyUpgraded){
			var supplyCard = SupplyScene.Instantiate() as RoomIncreaseCard;
			supplyCard.UpgradesScreen = this;
			PermaCardsContainer.AddChild(supplyCard);
		}
	}
}