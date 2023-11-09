namespace Ability;
public partial class HealthBoostController : BaseAbilityController
{
	[Export] int healthOnWaveClear = 0;
	[Export] int maxHealthIncrease = 25;
	private HealthComponent healthComponent;
	public override void Init()
	{
		healthComponent = GetNode<HealthComponent>("../../HealthComponent");
		healthComponent.IncreaseMaxHealth(25);
		var WaveManager = GetNode<ArenaManager>("/root/Main/Managers/ArenaManager");
		WaveManager.WaveCleared += OnWaveCleared;
	}

	private void OnWaveCleared()
	{
		healthComponent.IncreaseMaxHealth(healthOnWaveClear);
	}
}
