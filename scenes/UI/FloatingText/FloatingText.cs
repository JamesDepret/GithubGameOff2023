namespace UI;
public partial class FloatingText : Node2D
{
	Label label;
	int randomDirection;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		label = GetNode<Label>("Label");
		GD.Randomize();
		randomDirection = GD.Randi() % 2 == 0 ? 1 : -1;
	}

	public void Start (string text)
	{
		label.Text = text;
		var tween = CreateTween();
		
		var callable = Callable.From<float>(MoveText);
		tween.TweenMethod(callable,0f,2f,0.6f);
		

		// tween.TweenProperty(this, "global_position.y", GlobalPosition.Y + 48 , .9f)
		// 	 .SetEase(Tween.EaseType.Out)
		// 	 .SetTrans(Tween.TransitionType.Back);
		// tween.TweenProperty(this, "global_position.x", GlobalPosition.Y + 48 , .9f)
		// 	 .SetEase(Tween.EaseType.Out)
		// 	 .SetTrans(Tween.TransitionType.Quart);
		// tween.Chain();

		// tween.TweenProperty(this, "global_position", GlobalPosition + (Vector2.Up * 32) + direction*2, .4f)
		//  	 .SetEase(Tween.EaseType.Out)
		//  	 .SetTrans(Tween.TransitionType.Cubic);
		// tween.TweenProperty(this, "scale", Vector2.Zero, .3f)
		// 	 .SetEase(Tween.EaseType.In)
		// 	 .SetTrans(Tween.TransitionType.Cubic);
		// tween.Chain();
		
		tween.TweenCallback(Callable.From(QueueFree));

		
		var scaleTween = CreateTween();
		scaleTween.TweenProperty(this, "scale", Vector2.One * 1.5f , .1f)
				  .SetEase(Tween.EaseType.Out)
				  .SetTrans(Tween.TransitionType.Cubic);
		scaleTween.TweenProperty(this, "scale", Vector2.One, .1f)
				  .SetEase(Tween.EaseType.In)
				  .SetTrans(Tween.TransitionType.Cubic);
		
	}

	private void MoveText(float value)
	{				
		//random number of 1 or -1
		var step = 0.4f;
		GlobalPosition = new Vector2(GlobalPosition.X + value * 1.4f *  randomDirection, GlobalPosition.Y - GetParabolicPosition(value)*step);
	}

	private float GetParabolicPosition(float x)
	{
		return ((-2 * x * x) + (6 * x));
	}
}
