namespace Ability;
public partial class ShieldBoostController : BaseAbilityController
{
	[Export] int shieldsRegen = 0;
	[Export] int maxShieldsIncrease = 25;
	private HealthComponent healthComponent;
	public override void Init()
	{
		healthComponent = GetNode<HealthComponent>("../../HealthComponent");
		DoEffect();
	}

	public override void DoEffect()
	{
		healthComponent.IncreaseMaxShields(maxShieldsIncrease);
		healthComponent.ShieldRegenAmount += shieldsRegen;
	}

	
}
