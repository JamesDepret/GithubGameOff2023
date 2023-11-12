namespace Components;
public partial class HealthComponent : Node
{
	[Signal] public delegate void DiedEventHandler(string name);
    [Signal] public delegate void HealthChangedEventHandler();
	[Export] public float MaxHealth { get; set; } = 10;
	[Export] public AnimatedSprite2D sprite;
	[Export] public ShaderMaterial shaderMaterial;
	[Export] public bool HasShields { get; set; } = false;
	public float CurrentHealth { get; set; }
	private Tween hitFlashTween;
	private bool hitFlashObjectsSet = false;
	public int CurrentShields { get; set; } = 0;
	public int MaxShields { get; set; } = 0;
	public int ShieldRegenAmount { get; set; } = 0;
		
	public override void _Ready()
	{
		CurrentHealth = MaxHealth;
		EmitSignal(SignalName.HealthChanged);
		
		ShaderMaterial shaderMat = new ShaderMaterial();
		if(shaderMaterial != null && sprite != null) {
			shaderMat.Shader = shaderMaterial.Shader;
			sprite.Material = shaderMat;
		}
		hitFlashObjectsSet = shaderMat != null && sprite != null;

		if(HasShields) 
		{
			SetupShieldRegenTimer();
			GameEvents.Instance.WaveCleared += ResetShields;
		}
	}

	public void TakeDamage(float amount)
    {
		float remainder = amount - CurrentShields;
		CurrentShields = (int) MathF.Max(CurrentShields - (int) amount, 0);
		if(remainder > 0)
		{
			CurrentHealth = MathF.Max(CurrentHealth - amount, 0);
			if (hitFlashObjectsSet) HitFlash();
		}
        EmitSignal(SignalName.HealthChanged);
        Callable.From(CheckDeath).CallDeferred();
    }

	public void HealDamage(float amount)
	{
        CurrentHealth = MathF.Max(CurrentHealth + amount, 0);
        EmitSignal(SignalName.HealthChanged);
	}

    public float GetHealthPercentage()
    {
        if (MaxHealth <= 0) return 0;
        return Mathf.Min(CurrentHealth / MaxHealth,1);
    }

	public void IncreaseMaxHealth(int amount)
	{
		MaxHealth += amount;
		CurrentHealth += amount;
		EmitSignal(SignalName.HealthChanged);
	}

	public void IncreaseMaxShields(int amount)
	{
		MaxShields += amount;
		CurrentShields += amount;
		EmitSignal(SignalName.HealthChanged);
	}

	public void HealShields(int amount)
	{
		if(CurrentShields >= MaxShields) return;
		CurrentShields = (int) MathF.Min(CurrentShields + amount, MaxShields);
		EmitSignal(SignalName.HealthChanged);
	}
	
	private void ResetShields(int waveNumber)
	{
		CurrentShields = MaxShields;
		EmitSignal(SignalName.HealthChanged);
	}

    private void CheckDeath()
    {
        if (CurrentHealth <= 0)
        {
            EmitSignal(SignalName.Died, GetParent().Name);
            Owner.QueueFree();
        }
    }

	private void HitFlash()
	{
		if(hitFlashTween != null && hitFlashTween.IsValid()) hitFlashTween.Kill();

		(sprite.Material as ShaderMaterial).SetShaderParameter("lerp_percent", 1f);
		hitFlashTween = CreateTween();
		hitFlashTween.TweenProperty(sprite.Material, "shader_parameter/lerp_percent", 0.0f, 0.2f)
					 .SetEase(Tween.EaseType.In)
					 .SetTrans(Tween.TransitionType.Back);
	}

	private void SetupShieldRegenTimer()
	{
        var shieldRegenTimer = new Godot.Timer
        {
            OneShot = false,
            WaitTime = 5f
        };
		AddChild(shieldRegenTimer);
		shieldRegenTimer.Start();
        shieldRegenTimer.Timeout += RegenerateShields;
	}

	private void RegenerateShields()
	{
		HealShields(ShieldRegenAmount);
	}
}
