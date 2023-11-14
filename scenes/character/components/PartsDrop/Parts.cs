namespace Components;
public partial class Parts : Node2D
{
	[Export] public float MaxLifeTime = 5f;
	public int Value { get; set; } = 1;
	private Vector2 startPosition = Vector2.Zero;
	private AnimatedSprite2D sprite;
	private Godot.Timer timer;
	private Tween blinkTween;
	private Area2D CollectionArea;
	public override void _Ready()
	{
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		timer = GetNode<Godot.Timer>("Timer");
		timer.WaitTime = MaxLifeTime;
		timer.Timeout += OnTimeout;
		CollectionArea = GetNode<Area2D>("Area2D");
		CollectionArea.AreaEntered += OnBodyEntered;

		blinkTween = CreateTween();
		var callable = Callable.From<float>(TweenBlink);
		blinkTween.TweenMethod(callable,0f,1.0f,MaxLifeTime);
	}

	
    public override void _ExitTree()
    {
		timer.Timeout -= OnTimeout;
		CollectionArea.AreaEntered -= OnBodyEntered;
    }



	void OnBodyEntered(Node2D body)
	{
		StopBlinkingTween();
		Tween tween = CreateTween();
		var callable = Callable.From<float>(TweenCollect);
		tween.SetParallel();
		tween.TweenMethod(callable,0f,1.0f,0.5f)
			 .SetEase(Tween.EaseType.In)
			 .SetTrans(Tween.TransitionType.Cubic);

		tween.TweenProperty(sprite, "scale", Vector2.Zero, .20f).SetDelay(.5f);
		tween.Chain();

		tween.TweenCallback(Callable.From(Collect));
	}

	void TweenCollect(float percent)
	{
        if (GetTree().GetFirstNodeInGroup("player") is not Player player) return;

        if (startPosition == Vector2.Zero) startPosition = GlobalPosition;
				
		GlobalPosition = startPosition.Lerp(player.GlobalPosition, percent);
	}

	void Collect()
	{
		sprite.Visible = false;
		GameEvents.Instance.EmitPartsCollected(Value);
		QueueFree();
	}

	void TweenBlink(float percent)
	{
		int value = (int) (percent *100);
		if(value > 0 && value % 10 == 0) sprite.Modulate = new Color(1,1,1,0);
		else if(value > 50 && value % 5 == 0)  sprite.Modulate = new Color(1,1,1,0);
		else if(value > 80 && value % 3 == 0)  sprite.Modulate = new Color(1,1,1,0);
		else sprite.Modulate = new Color(1,1,1,1);
	}

	void StopBlinkingTween()
	{
		blinkTween.Stop();
		timer.Stop();
		sprite.Modulate = new Color(1,1,1,1);
	}

	void OnTimeout()
	{
		timer.Stop();
		QueueFree();
	}
}
