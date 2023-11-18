namespace Manager;
public partial class RandomRewardManager : Node
{
    [Export] public PackedScene HealthRewardScene;
    private float healthPercentage = 1f;

	private void OnPlayerHealthChanged()
	{
		healthPercentage = playerHealthComponent.GetHealthPercentage();
	}

    private float HealthDropChance()
    {
        return 1f;
        if (healthPercentage > 0.75f) return 0f;
        if (healthPercentage > 0.25f) return 0.025f;
        if (healthPercentage > 0.1f) return 0.1f;
        return 0.15f;
    }

    private void PlaceHealthReward()
    {
        
        if (GetTree().GetFirstNodeInGroup("entities_layer") is not Node2D entitiesLayer) throw new Exception("Could not find entities layer");
        var healthReward = HealthRewardScene.Instantiate() as HealthReward;
        entitiesLayer.AddChild(healthReward);
    }
}