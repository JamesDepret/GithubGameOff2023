namespace Character;
public partial class HealthComponent : Node
{
	[Signal] public delegate void DiedEventHandler();
    [Signal] public delegate void HealthChangedEventHandler();
	[Export] public float MaxHealth { get; set; } = 10;
	public float CurrentHealth { get; set; }
	public override void _Ready()
	{
		CurrentHealth = MaxHealth;
	}

	public void TakeDamage(float amount)
    {
        CurrentHealth = MathF.Max(CurrentHealth - amount, 0);
        EmitSignal(SignalName.HealthChanged);
        Callable.From(CheckDeath).CallDeferred();
    }

    public float GetHealthPercentage()
    {
        if (MaxHealth <= 0) return 0;
        return Mathf.Min(CurrentHealth / MaxHealth,1);
    }

    private void CheckDeath()
    {
        if (CurrentHealth <= 0)
        {
            EmitSignal(SignalName.Died);
            Owner.QueueFree();
        }
    }
}
