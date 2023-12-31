namespace Character;

public partial class Player : CharacterBody2D
{
	[Export] public float MaxSpeed { get; set; } = 125f;
	[Export] public float Acceleration { get; set; } = 25f;
    [Export] public float RotationSpeed { get; set; } = 2.0f; 
    [Export] public float RotationAngle { get; set; } = 45f; 
	[Export] public bool IsMainPlayer { get; set; } = false;
	[Export] public int PlayerPrio { get; set; } = 1;
    [Export] public Timer BoostTimer { get; set; }
	[Export] private UpgradeManager upgradeManager;
	[Export] private ArenaManager arenaManager;
	private Node shipTurrets_AbilitiesNode;
	private HealthComponent healthComponent;
	private ProgressBar healthBar;
	private AnimationPlayer animationPlayer;
	private Node2D visuals;
	private CpuParticles2D particles;
	private float maxRotation;
	private float currentRotation = 0f; 
	private Area2D CollisionArea;

	public override void _Ready()
	{
		shipTurrets_AbilitiesNode = GetNode<Node>("Abilities");
		GameEvents.Instance.ShipUpgradeAdded += OnNewShipTurretAdded;

		// Health
		healthComponent = GetNode<HealthComponent>("HealthComponent");
		healthBar = GetNode<ProgressBar>("HealthBar");
		healthComponent.HealthChanged += OnHealthChanged;
		healthComponent.Died += OnDied;
		UpdateHealthDisplay();

		// Getting Damaged
		CollisionArea = GetNode<Area2D>("DamageArea");
		CollisionArea.AreaEntered += OnAreaEntered;

		// Movement
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		visuals = GetNode<Node2D>("Visuals");
		particles = visuals.GetNode<CpuParticles2D>("Particles");
		maxRotation = Mathf.DegToRad(RotationAngle);
		BoostTimer = GetNode<Timer>("BoostTimer");
		BoostTimer.Timeout += OnBoostTimerTimeout;
		
		upgradeManager.InitFirstUpgrades();
	}

	
	public override void _ExitTree()
    {
		GameEvents.Instance.ShipUpgradeAdded -= OnNewShipTurretAdded;
		healthComponent.HealthChanged -= OnHealthChanged;
		healthComponent.Died -= OnDied;
		CollisionArea.AreaEntered -= OnAreaEntered;
		BoostTimer.Timeout -= OnBoostTimerTimeout;
    }

	
	public override void _Process(double delta)
	{
		Move(delta);
	}
}
