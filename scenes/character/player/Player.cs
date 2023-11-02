namespace Character;

public partial class Player : CharacterBody2D
{
	[Export] public float MaxSpeed { get; set; } = 125f;
	[Export] public float Acceleration { get; set; } = 25f;
    [Export] public float RotationSpeed { get; set; } = 2.0f; 
    [Export] public float RotationAngle { get; set; } = 45f; 
	private HealthComponent healthComponent;
	private ProgressBar healthBar;
	private Godot.Timer damageIntervalTimer;
	private AnimationPlayer animationPlayer;
	private Node2D visuals;
	private CpuParticles2D particles;
	private float maxRotation;
	private float currentRotation = 0f; 


	public override void _Ready()
	{
		// Health
		healthComponent = GetNode<HealthComponent>("HealthComponent");
		healthBar = GetNode<ProgressBar>("HealthBar");
		healthComponent.HealthChanged += OnHealthChanged;
		UpdateHealthDisplay();

		// Getting Damaged
		var CollisionArea = GetNode<Area2D>("DamageArea");
		CollisionArea.BodyEntered += OnBodyEntered;
		CollisionArea.BodyExited += OnBodyExited;
		damageIntervalTimer = GetNode<Godot.Timer>("DamageIntervalTimer");
		damageIntervalTimer.Timeout += OnDamageIntervalTimerTimeout;

		// Movement
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		visuals = GetNode<Node2D>("Visuals");
		particles = visuals.GetNode<CpuParticles2D>("Particles");
		maxRotation = Mathf.DegToRad(RotationAngle);
	}

	
	public override void _Process(double delta)
	{
		Move(delta);
	}
}
