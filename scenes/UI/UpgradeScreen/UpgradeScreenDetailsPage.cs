namespace UI;
public partial class UpgradesScreen : CanvasLayer
{

	public void SetSelectedUpgrade(BaseUpgrade upgrade, BaseUpgrade? previousUpgrade = null)
	{
		if (previousUpgrade != null) upgrade.PreviousUpgradePointer = previousUpgrade;
		selectedUpgrade = upgrade;
		selectedCard.Visible = true;
		SetupSelectedCard();
		CardContainer.GetChildren().ToList().ForEach(child => (child as Control).Modulate = new Color(1, 1, 1, 1));
	}

	public void SetupSelectedIncomeOrSupplyUpgrade(int price, Texture2D icon, string name, string description, BuyButtonEnum button)
    {
		var parts = GameEvents.Instance.Parts;
		selectedCard.Visible = true;
		selectedCard.SetupCard(price, 0, null, parts < price, false, icon);
		selectedCard.SetTexts(name, description);
		BuyButtonSetup(button, parts < price);
		CardContainer.GetChildren().ToList().ForEach(child => (child as Control).Modulate = new Color(1, 1, 1, 1));
    }

    private void SetupSelectedCard()
    {
		var parts = GameEvents.Instance.Parts;
		var currentSupply = GameEvents.Instance.Supply;
		var maxSupply = GameEvents.Instance.MaxSupply;
		selectedCard.SetupCard(selectedUpgrade.Price, 
							   selectedUpgrade.SupplyCost, 
							   selectedUpgrade, 
							   parts < selectedUpgrade.Price, 
							   currentSupply + selectedUpgrade.SupplyCost > maxSupply, 
							   selectedUpgrade.Icon);
        BuyButtonSetup(BuyButtonEnum.NormalUpgrade, parts < selectedUpgrade.Price || currentSupply + selectedUpgrade.SupplyCost > maxSupply);
    }
    
	private void CleanUpDetailsCard()
	{
		selectedCard.Visible = false;
		BuyButtonSetup(BuyButtonEnum.NormalUpgrade, true);
		SetupCards();
	}
}