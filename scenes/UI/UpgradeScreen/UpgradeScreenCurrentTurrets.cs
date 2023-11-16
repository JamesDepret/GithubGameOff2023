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
		turretContainers.GetChildren().ToList().ForEach(child => child.QueueFree());
		// sort the turrets by their level
		var turrets = CurrentTurrets.ToArray().OrderBy(turret => turret.Id).OrderBy(turret => turret.PreviousUpgradePointer!= null).ToArray();
		foreach (var item in turrets)
		{
			AddTurretIcon(item);
		}
	}
    private void AddTurretIcon(BaseUpgrade upgrade)
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
        turretContainers.AddChild(turretIcon);
	}
}