namespace UI;
public partial class UpgradesScreen : CanvasLayer
{
	public void InitTurretList(BaseUpgrade[] turrets)
	{
		CurrentTurrets = new(turrets);
		SetTurrets();
	}
	public void SetTurrets()
	{
		turretContainersLevel1.GetChildren().ToList().ForEach(child => child.QueueFree());
		turretContainersLevel2.GetChildren().ToList().ForEach(child => child.QueueFree());
		// sort the turrets by their level
		var turrets = CurrentTurrets.ToArray().OrderBy(turret => turret.Id).OrderBy(turret => turret.PreviousUpgradePointer!= null).ToArray();
		foreach (var item in turrets)
		{
            if ( item.PreviousUpgradePointer != null)
            {
			    AddTurretIcon(item, 2);
            }
            else
            {
			    AddTurretIcon(item, 1);
            }
		}
	}
    private void AddTurretIcon(BaseUpgrade upgrade, int level)
	{
        CurrentTurret turretIcon = new()
        {
            TextureNormal = upgrade.Icon,
            TextureHover = upgrade.Icon,
            TexturePressed = upgrade.Icon,
            TextureFocused = upgrade.Icon,
            Tower = upgrade,
            UpgradesScreen = this
        };
        if(level == 1) turretContainersLevel1.AddChild(turretIcon);
        else if(level == 2) turretContainersLevel2.AddChild(turretIcon);
	}
}