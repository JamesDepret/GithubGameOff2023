namespace BaseRewards;

public  abstract partial class BaseReward : Node2D
{
    [Export] public float MaxLifeTime = 5f;
    [Export] public float Speed { get; set; } = 10f;
	public int Value { get; set; } = 1;
	private Vector2 startPosition = Vector2.Zero;
	private AnimatedSprite2D sprite;
	private Godot.Timer timer;
	private bool moving = true;
	protected Area2D CollectionArea;
    protected Player player;

    public override void _Ready()
    {
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		timer = GetNode<Godot.Timer>("Timer");
		timer.WaitTime = MaxLifeTime;
		timer.Timeout += OnTimeout;
		CollectionArea = GetNode<Area2D>("Area2D");
		CollectionArea.AreaEntered += OnBodyEntered;

        SetRandomLocation();
    }

    public override void _PhysicsProcess(double delta)
    {
        GlobalPosition = new Vector2(GlobalPosition.X, GlobalPosition.Y + (float) delta * Speed);
    }

    private void SetRandomLocation()
    {
        var x = GD.RandRange(10, 630);
        GlobalPosition = new Vector2(x, -50);
    }

    public override void _ExitTree()
    {
		timer.Timeout -= OnTimeout;
		CollectionArea.AreaEntered -= OnBodyEntered;
    }



	protected virtual void OnBodyEntered(Node2D body)
	{
		moving = false;
		Tween tween = CreateTween();
		var callable = Callable.From<float>(TweenCollect);
		tween.SetParallel();
		tween.TweenMethod(callable,0f,1.0f,0.5f)
			 .SetEase(Tween.EaseType.In)
			 .SetTrans(Tween.TransitionType.Cubic);

		tween.TweenProperty(sprite, "scale", Vector2.Zero, .80f);
		tween.Chain();

		tween.TweenCallback(Callable.From(Collect));
	}

	void TweenCollect(float percent)
	{
        if (GetTree().GetFirstNodeInGroup("player") is not Player collidingPlayer) return;
        player = collidingPlayer;

        if (startPosition == Vector2.Zero) startPosition = GlobalPosition;
			
		GlobalPosition = startPosition.Lerp(player.GlobalPosition, percent);
	
		if(percent > 0.95f) sprite.Visible = false;
	}

	protected virtual void Collect()
	{
		sprite.Visible = false;
	
		QueueFree();
	}


	void OnTimeout()
	{
		timer.Stop();
		QueueFree();
	}
}