namespace Ability;
public partial class RooterTurretController : BulletAbilityController
{
	[Export] public int SpeedUpFactor = 0;
	
	protected override void InstantHitBullet(List<Node> enemies)
	{
		base.InstantHitBullet(enemies);
		if(SpeedUpFactor > 0 && enemies.Count > 0)
		{
			for (int i = 0; i < bounces; i++){
				if (i > enemies.Count) return;
				VelocityComponent velocityComponent = enemies[i].GetNode<VelocityComponent>("VelocityComponent");
				velocityComponent.AdjustSpeed(SpeedUpFactor);
			}
		}
		
	}
}
