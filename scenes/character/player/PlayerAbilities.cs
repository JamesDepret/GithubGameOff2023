namespace Character;

public partial class Player : CharacterBody2D
{
    
    void OnNewShipTurretAdded(BaseUpgrade upgrade, Godot.Collections.Array<BaseUpgrade> currentUpgrades)
    {
        if (upgrade.AbilityControllerScene == null) return;
        shipTurrets.AddChild(upgrade.AbilityControllerScene.Instantiate());
    }
}