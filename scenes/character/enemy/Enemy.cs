namespace Character;
public partial class Enemy : CharacterBody2D
{
	VelocityComponent velocityComponent;
	CpuParticles2D particles;
	bool isMoving = true;

    public override void _Ready()
    {
        velocityComponent = GetNode<VelocityComponent>("VelocityComponent");
		particles = GetNode<CpuParticles2D>("Visuals/Particles");
    }
	public override void _Process(double delta)
	{
		if(velocityComponent != null) {
			if(isMoving) velocityComponent.AccelerateInDirection(velocityComponent.GetDirectionToPlayer());
			else velocityComponent.Decelerate();
			velocityComponent.Move(this);
			var dir = velocityComponent.Direction;
			particles.Direction = new Vector2(dir.X * -(particles.Owner as Node2D).Scale.X, -dir.Y);
		}
		
	}

	public void SetMoving(bool moving)
	{
		isMoving = moving;
	}
}