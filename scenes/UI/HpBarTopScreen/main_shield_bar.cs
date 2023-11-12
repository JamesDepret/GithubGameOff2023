namespace UI;
public partial class main_shield_bar : ProgressBar
{
	[Export] public Player player;
	HealthComponent healthComponent;
	Label ShieldLabel;
	public override void _Ready()
	{
		ShieldLabel = GetNode<Label>("ShieldLabel");
		healthComponent = player.GetNode<HealthComponent>("HealthComponent");
		healthComponent.HealthChanged += OnHealthChanged;
	}

	private void OnHealthChanged()
	{
		Visible = healthComponent.MaxShields > 0;
		MaxValue = healthComponent.MaxShields;
		Value = healthComponent.CurrentShields;
		ShieldLabel.Text = $"{Value}/{MaxValue}";
	}
}
