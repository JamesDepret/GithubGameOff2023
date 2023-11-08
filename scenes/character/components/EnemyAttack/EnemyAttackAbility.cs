namespace Ability;

public partial class EnemyAttackAbility : Node2D
{
	public HitboxComponent HitboxComponent { get; private set; }
	public Vector2 Velocity { get; set; } = Vector2.Zero;
	private Godot.Timer timer;

	public override void _Ready()
	{
		HitboxComponent = GetNode<HitboxComponent>("HitboxComponent");
		AddLifeTimeTimer();
	}
	public override void _Process(double delta)
	{
		GlobalPosition += Velocity * (float) delta;
	}
	
	private void AddLifeTimeTimer()
	{
        var lifeTimeTimer = new Godot.Timer
        {
            WaitTime = 3f,
            OneShot = true
        };
		AddChild(lifeTimeTimer);
        lifeTimeTimer.Start();
		lifeTimeTimer.Timeout += OnLifeTimeTimerTimeout;
	}

	private void OnLifeTimeTimerTimeout()
	{
		QueueFree();
	}
}
