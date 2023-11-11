namespace Character;

public partial class Player : CharacterBody2D
{
    
    void OnNewShipTurretAdded(BaseUpgrade upgrade, Godot.Collections.Array<BaseUpgrade> currentUpgrades)
    {
        if (upgrade.AbilityControllerScene == null) return;
        if (upgrade.PreviousUpgradePointer != null) 
        {
            try
            {
                shipTurrets.RemoveChild(upgrade.PreviousUpgradePointer.ControllerPointer);
            } 
            catch (Exception e)
            {
                GD.Print(e, "Error removing child of " + shipTurrets.Name + " - child - " + upgrade.PreviousUpgradePointer.ControllerPointer.Name);
                GD.PrintErr(e, "Error removing child of " + shipTurrets.Name + " - child - " + upgrade.PreviousUpgradePointer.ControllerPointer.Name);
            }
            
        }
        var controller = upgrade.AbilityControllerScene.Instantiate() as BaseAbilityController;
        shipTurrets.AddChild(controller);
        controller.Init();
        upgrade.ControllerPointer = controller;

    }
}