namespace Character;

public partial class Player : CharacterBody2D
{    
    List<BaseAbilityController> shipTurrets_Abilities = new();
    void OnNewShipTurretAdded(BaseUpgrade upgrade, Godot.Collections.Array<BaseUpgrade> currentUpgrades)
    {
        if (upgrade.AbilityControllerScene == null) return;
        if(upgrade.PreviousUpgradePointer != null)
        {
            // all the upgrades point to the same resource so when they get get upgraded, the next upgrade would throw
            // a disposed error because we deleted it's pointer. So we just find one controller to delete instead
            var upg = shipTurrets_Abilities.Find(x => x.SubName == upgrade.PreviousUpgradePointer?.Id);
            if (upg != null)
            {
                upg.QueueFree();
                shipTurrets_Abilities.Remove(upg);
            }
        }
        var controller = upgrade.AbilityControllerScene.Instantiate() as BaseAbilityController;
        controller.SubName = upgrade.Id;
        controller.Upgrade = upgrade;
        shipTurrets_AbilitiesNode.AddChild(controller);
        controller.Init();
        upgrade.ControllerPointer = controller;
        shipTurrets_Abilities.Add(controller);
    }
}