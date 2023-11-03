namespace Components;
public partial class HealthComponent : Node
{
	[Signal] public delegate void DiedEventHandler();
    [Signal] public delegate void HealthChangedEventHandler();
	[Export] public float MaxHealth { get; set; } = 10;
	[Export] public AnimatedSprite2D sprite;
	[Export] public ShaderMaterial shaderMaterial;
	public float CurrentHealth { get; set; }
	private Tween hitFlashTween;
	private bool hitFlashObjectsSet = false;
		
	public override void _Ready()
	{
		CurrentHealth = MaxHealth;
		ShaderMaterial shaderMat = new ShaderMaterial();
		if(shaderMaterial != null && sprite != null) {
			shaderMat.Shader = shaderMaterial.Shader;
			sprite.Material = shaderMat;
		}
		hitFlashObjectsSet = shaderMat != null && sprite != null;
	}

	public void TakeDamage(float amount)
    {
        CurrentHealth = MathF.Max(CurrentHealth - amount, 0);
		if (hitFlashObjectsSet) HitFlash();
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

	private void HitFlash()
	{
		if(hitFlashTween != null && hitFlashTween.IsValid()) hitFlashTween.Kill();

		(sprite.Material as ShaderMaterial).SetShaderParameter("lerp_percent", 1f);
		hitFlashTween = CreateTween();
		hitFlashTween.TweenProperty(sprite.Material, "shader_parameter/lerp_percent", 0.0f, 0.2f)
					 .SetEase(Tween.EaseType.In)
					 .SetTrans(Tween.TransitionType.Back);
	}
}
