namespace Upgrades;
public partial class BounceBullet : BaseUpgrade
{
	[Export] public float Damage { get; set; } = 0f;
	[Export] public int Bounces { get; set; } = 0;

}
