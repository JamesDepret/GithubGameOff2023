namespace Components;
public partial class HitboxComponent : Area2D
{
	[Signal] public delegate void OnHitEffectEventHandler();
	public int HitsBeforeDestroyed { get; set; } = 1;
	public float Damage { get; set; } = 0f;
	public List<Enemy> EnemiesHit { get; set; } = new List<Enemy>();
	public float DamageReductionOnPierce { get; set; } = 0f;
	public float CritChance { get; set; } = 0f;

	public void SetupHitboxComponent(float damage, int hitsBeforeDestroyed, float damageReductionOnPierce, float critChance)
	{
		Damage = damage;
		HitsBeforeDestroyed = hitsBeforeDestroyed;
		DamageReductionOnPierce = damageReductionOnPierce;
		CritChance = critChance;
	}
}
