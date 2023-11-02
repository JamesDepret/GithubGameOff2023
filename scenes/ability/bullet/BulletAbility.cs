namespace Ability;

public partial class BulletAbility : CharacterBody2D
{
	
	public override void _Process(double delta)
	{
		MoveAndSlide();
	}
}
