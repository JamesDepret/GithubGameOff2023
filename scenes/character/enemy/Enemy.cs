namespace Character;
public partial class Enemy : CharacterBody2D
{
	[Export] public bool IsBiteBoss { get; set; } = false;
	[Export] public bool IsBiteBossPhase2 { get; set; } = false;
	public HurtboxComponent HurtboxComponent { get; set; }
	protected Godot.Timer lifeTimer;
	protected VelocityComponent velocityComponent;
	protected CpuParticles2D particles;
	protected bool isMoving = true;
	protected Node2D visuals;

    public override void _Ready()
    {
		lifeTimer = GetNode<Godot.Timer>("Lifetime");
		lifeTimer.Timeout += OnSpeedUp;
		if(IsBiteBoss) velocityComponent = GetNode<BiteBossComponent>("BiteBossComponent");
		else velocityComponent = GetNode<VelocityComponent>("VelocityComponent");
		HurtboxComponent = GetNode<HurtboxComponent>("HurtboxComponent");
		particles = GetNode<CpuParticles2D>("Visuals/Particles");
		visuals = GetNode<Node2D>("Visuals");
    }

	public override void _ExitTree()
    {
		lifeTimer.Timeout -= OnSpeedUp;
    }

	public override void _Process(double delta)
	{
		if(velocityComponent != null) {
			if (isMoving) velocityComponent.AccelerateInDirection(velocityComponent.GetDirectionToTarget());
			else velocityComponent.Decelerate();
			velocityComponent.Move(this);
			var dir = velocityComponent.Direction;
			particles.Direction = new Vector2(dir.X * -(particles.GetParent() as Node2D).Scale.X, -dir.Y);
			if(IsBiteBossPhase2)
			{
				visuals.Rotation = Mathf.Atan2(dir.Y, dir.X) - Mathf.Pi / 2;
			}
		}
	}

	public void SetMoving(bool moving)
	{
		isMoving = moving;
	}

	protected void OnSpeedUp()
	{
		velocityComponent.SpeedUp();
	}
}