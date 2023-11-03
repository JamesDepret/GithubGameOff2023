namespace Components;
public partial class HitboxComponent : Area2D
{
	public float Damage { get; set; } = 0f;
	public int HitsBeforeDestroyed { get; set; } = 1;
}
