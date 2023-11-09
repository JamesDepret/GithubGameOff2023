namespace Ability;
public partial class LootBoostController : BaseAbilityController
{
	[Export] float lootBoost = 0.5f;
	[Export] float doublePartsChance = 0f;
	public override void Init()
	{
		CircleShape2D pickupArea = GetNode<CollisionShape2D>("../../PickupArea/CollisionShape2D").Shape as CircleShape2D;
		pickupArea.Radius *= (1 + lootBoost);
		GameEvents.Instance.LootCritChance += doublePartsChance;
	}
}
