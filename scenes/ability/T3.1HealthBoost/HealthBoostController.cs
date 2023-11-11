namespace Ability;
public partial class HealthBoostController : BaseAbilityController
{
	[Export] int healthOnWaveClear = 0;
	[Export] int maxHealthIncrease = 25;
	private HealthComponent healthComponent;
	public override void Init()
	{
		healthComponent = GetNode<HealthComponent>("../../HealthComponent");
		DoEffect();
	}

	private void OnWaveCleared()
	{
		healthComponent.IncreaseMaxHealth(healthOnWaveClear);
	}

	public override void DoEffect()
	{
		if(healthOnWaveClear>0)
		{
			var WaveManager = GetNode<ArenaManager>("/root/Main/Managers/ArenaManager");
			WaveManager.WaveCleared += OnWaveCleared;
		}
		
		healthComponent.IncreaseMaxHealth(maxHealthIncrease);
	}
}
