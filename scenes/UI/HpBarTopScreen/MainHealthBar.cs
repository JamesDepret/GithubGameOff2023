namespace UI;
public partial class MainHealthBar : ProgressBar
{
	[Export] public Player player;
	HealthComponent healthComponent;
	Label hpLabel;
	public override void _Ready()
	{
		hpLabel = GetNode<Label>("HpLabel");
		healthComponent = player.GetNode<HealthComponent>("HealthComponent");
		healthComponent.HealthChanged += OnHealthChanged;
	}

	public override void _ExitTree()
    {
		healthComponent.HealthChanged -= OnHealthChanged;
    }

	private void OnHealthChanged()
	{
		MaxValue = healthComponent.MaxHealth;
		Value = healthComponent.CurrentHealth;
		hpLabel.Text = $"{Value}/{MaxValue}";
	}
}
