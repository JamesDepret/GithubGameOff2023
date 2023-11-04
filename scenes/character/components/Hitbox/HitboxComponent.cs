namespace Components;
public partial class HitboxComponent : Area2D
{
	[Signal] public delegate void OnHitEffectEventHandler();
	public float Damage { get; set; } = 0f;
	public int HitsBeforeDestroyed { get; set; } = 1;
	public List<Enemy> EnemiesHit { get; set; } = new List<Enemy>();
}
