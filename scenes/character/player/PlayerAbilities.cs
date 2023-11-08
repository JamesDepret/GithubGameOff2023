namespace Character;

public partial class Player : CharacterBody2D
{
    
    void OnNewShipTurretAdded(BaseUpgrade upgrade, Godot.Collections.Array<BaseUpgrade> currentUpgrades)
    {
        if (upgrade.AbilityControllerScene == null) return;
        if (upgrade.PreviousUpgradePointer != null) 
        {
            shipTurrets.RemoveChild(upgrade.PreviousUpgradePointer.ControllerPointer);
        }
        var controller = upgrade.AbilityControllerScene.Instantiate() as BulletAbilityController;
        shipTurrets.AddChild(controller);
        upgrade.ControllerPointer = controller;

    }
}