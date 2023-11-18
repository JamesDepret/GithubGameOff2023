namespace Manager;
public partial class RandomRewardManager : Node
{
	[Export] private float WaitTime = 10f;
	[Export] private Player player;
	private HealthComponent playerHealthComponent;
	Timer timer;
	private int waveNumber = 0;

	public override void _Ready()
	{
		timer = GetNode<Timer>("Timer");
		timer.WaitTime = WaitTime;
		timer.Timeout += OnTimeout;
		playerHealthComponent = player.GetNode<HealthComponent>("HealthComponent");
		playerHealthComponent.HealthChanged += OnPlayerHealthChanged;
		GameEvents.Instance.WaveCleared += OnWaveCleared;
	}

	public override void _ExitTree()
	{
		timer.Timeout -= OnTimeout;
		playerHealthComponent.HealthChanged -= OnPlayerHealthChanged;
	}

	private void OnTimeout()
	{
		float randomNumber = ((float) GD.RandRange(0, 10000)) / 10000;
		var healthRewardChance = HealthDropChance();

		if (randomNumber < healthRewardChance)
		{
			PlaceHealthReward();
		}

	}

	private void OnWaveCleared(int number)
	{
		waveNumber = number;
	}
}
