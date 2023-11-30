namespace Character;
public partial class FireballBoss : Enemy
{
    public override void _Ready()
    {
		lifeTimer = GetNode<Godot.Timer>("Lifetime");
		lifeTimer.Timeout += OnSpeedUp;
		velocityComponent = GetNode<FireballBossComponent>("FireballBossComponent");
		HurtboxComponent = GetNode<HurtboxComponent>("HurtboxComponent");
		particles = GetNode<CpuParticles2D>("Visuals/Particles");
		visuals = GetNode<Node2D>("Visuals");
    }
}