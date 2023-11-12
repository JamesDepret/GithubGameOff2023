namespace Character;

public partial class Player : CharacterBody2D
{
    
    void OnNewShipTurretAdded(BaseUpgrade upgrade, Godot.Collections.Array<BaseUpgrade> currentUpgrades)
    {
        if (upgrade.AbilityControllerScene == null) return;
        upgrade.PreviousUpgradePointer?.ControllerPointer.QueueFree();
        var controller = upgrade.AbilityControllerScene.Instantiate() as BaseAbilityController;
        controller.SubName = upgrade.Id;
        shipTurrets_AbilitiesNode.AddChild(controller);
        controller.Init();
        upgrade.ControllerPointer = controller;

    }
}