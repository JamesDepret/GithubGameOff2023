namespace UI;
public partial class CurrentTurret : TextureButton
{
	public UpgradesScreen UpgradesScreen { get; set; }
	public BaseUpgrade Tower { get; set; }

	public override void _Ready()
	{
		Pressed += OnPressed;
	}
	public void OnPressed()
	{
		if (Tower.UpgradesTo != null)
		{
			// tower seems to point to the seem turret for all of the same turrets
			UpgradesScreen.SetSelectedUpgrade(Tower.UpgradesTo as BaseUpgrade, Tower, (int)(Tower.Price * UpgradesScreen.SalvagePercentage));
		}
		else
		{
			UpgradesScreen.SetSelectedUpgrade(Tower, Tower.PreviousUpgradePointer, (int)((Tower.Price + Tower.PreviousUpgradePointer.Price) * UpgradesScreen.SalvagePercentage), true);
		}
	}
}
