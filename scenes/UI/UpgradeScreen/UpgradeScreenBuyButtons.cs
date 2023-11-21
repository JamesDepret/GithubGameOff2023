namespace UI;
public partial class UpgradesScreen : CanvasLayer
{
    
	public float SalvagePercentage {get;set;} = 0.5f;
	private Button normalUpgradeBuyButton;
	private Button incomeSpeedBuyButton;
	private Button incomeAmountBuyButton;
	private Button roomUpgradeBuyButton;
	private Button salvageButton;
	private Button confirmSalvageButton;
	public enum BuyButtonEnum
	{
		IncomeSpeed,
		IncomeAmount,
		RoomUpgrade,
		NormalUpgrade
	}

	// We have multiple buy buttons, this method will show the correct one
	public void BuyButtonSetup(BuyButtonEnum button, bool disabled = false, bool rankTwo = false)
	{
		normalUpgradeBuyButton.Visible = false;
		incomeSpeedBuyButton.Visible = false;
		incomeAmountBuyButton.Visible = false;
		roomUpgradeBuyButton.Visible = false;
		salvageButton.Visible = false;
		confirmSalvageButton.Visible = false;
        bool canBuy;
        switch (button)
		{
			case BuyButtonEnum.NormalUpgrade:
				normalUpgradeBuyButton.Visible = !disabled;
				salvageButton.Visible = rankTwo;
				if(rankTwo && selectedUpgrade != null) salvageButton.Text = "Salvage for " + salvagePrice;
				break;
			case BuyButtonEnum.IncomeSpeed:
				canBuy = harvestManager.HarvestTimeLevel < 5;
				incomeSpeedBuyButton.Visible = !disabled && canBuy;
				break;
			case BuyButtonEnum.IncomeAmount:
				canBuy = harvestManager.BaseIncome < harvestManager.MaxIncome;
				incomeAmountBuyButton.Visible = !disabled && canBuy;
				break;
			case BuyButtonEnum.RoomUpgrade:
				roomUpgradeBuyButton.Visible = !disabled;
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

	private void Salvage()
	{
		confirmSalvageButton.Visible = true;
	}

	private void ConfirmSalvage()
	{
		var refundableUpgrade = selectedUpgrade.PreviousUpgradePointer;
		GameEvents.Instance.EmitPartsCollected((int)(refundableUpgrade.Price * SalvagePercentage), false, false);
		GameEvents.Instance.SalvageSupply(refundableUpgrade.SupplyCost);
		var playerNodes = GetTree().GetNodesInGroup("player");
        if (playerNodes.Count > 0)
        {
            var player = playerNodes[0] as Player;
			player.RemoveTurretController(refundableUpgrade);
		}
		else
		{
			GD.PrintErr("No player found to remove turret controller from");
			throw new Exception("No player found to remove turret controller from");
		}
		CurrentTurrets.Remove(refundableUpgrade);
		CleanUpDetailsCard();
		SetTurrets();
	}
}