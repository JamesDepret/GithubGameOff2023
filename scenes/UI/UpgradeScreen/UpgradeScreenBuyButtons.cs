namespace UI;
public partial class UpgradesScreen : CanvasLayer
{
    
	private Button NormalUpgradeBuyButton;
	private Button IncomeSpeedBuyButton;
	private Button IncomeAmountBuyButton;
	private Button RoomUpgradeBuyButton;
	public enum BuyButtonEnum
	{
		IncomeSpeed,
		IncomeAmount,
		RoomUpgrade,
		NormalUpgrade
	}

	// We have multiple buy buttons, this method will show the correct one
	public void BuyButtonSetup(BuyButtonEnum button, bool disabled = false)
	{
		NormalUpgradeBuyButton.Visible = false;
		IncomeSpeedBuyButton.Visible = false;
		IncomeAmountBuyButton.Visible = false;
		RoomUpgradeBuyButton.Visible = false;
        bool canBuy;
        switch (button)
		{
			case BuyButtonEnum.NormalUpgrade:
				NormalUpgradeBuyButton.Visible = !disabled;
				break;
			case BuyButtonEnum.IncomeSpeed:
				canBuy = harvestManager.HarvestTimeLevel < 5;
				IncomeSpeedBuyButton.Visible = !disabled && canBuy;
				break;
			case BuyButtonEnum.IncomeAmount:
				canBuy = harvestManager.BaseIncome < harvestManager.MaxIncome;
				IncomeAmountBuyButton.Visible = !disabled && canBuy;
				break;
			case BuyButtonEnum.RoomUpgrade:
				RoomUpgradeBuyButton.Visible = !disabled;
				break;
		}
	}

    // On Normal upgrade purchase 
    private void OnUpgradeCardBought()
	{
		CurrentTurrets.Add(selectedUpgrade);
		GameEvents.Instance.EmitPartsCollected(-selectedUpgrade.Price);
		EmitSignal(SignalName.UpgradeSelected, selectedUpgrade);
		SetupCards();

		if (selectedUpgrade.PreviousUpgradePointer != null)
		{
			CurrentTurrets.Remove(selectedUpgrade.PreviousUpgradePointer);
			selectedCard.Visible = false;
			BuyButtonSetup(BuyButtonEnum.NormalUpgrade, true);
		}
		SetTurrets();
	}
}