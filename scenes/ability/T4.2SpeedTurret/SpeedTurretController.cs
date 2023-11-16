namespace Ability;
public partial class SpeedTurretController : BulletAbilityController
{
	[Export] private float speedModifier = 0.05f;
	public SpeedModifier SpeedModifier {get; set; }


	public override void _Ready()
	{
		base._Ready();
        SpeedModifier = new SpeedModifier
        {
            ModifierValue = speedModifier
        };
    }
}
