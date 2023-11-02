namespace UI;
public partial class FloatingText : Node2D
{
	Label label;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		label = GetNode<Label>("Label");
	}

	public void Start (string text)
	{
		label.Text = text;
		var tween = CreateTween();
		tween.SetParallel();
		tween.TweenProperty(this, "global_position", GlobalPosition + (Vector2.Up * 32), .2f)
			 .SetEase(Tween.EaseType.Out)
			 .SetTrans(Tween.TransitionType.Cubic);
		tween.Chain();

		tween.TweenProperty(this, "global_position", GlobalPosition + (Vector2.Up * 48), .3f)
			 .SetEase(Tween.EaseType.Out)
			 .SetTrans(Tween.TransitionType.Cubic);
		tween.TweenProperty(this, "scale", Vector2.Zero, .3f)
			 .SetEase(Tween.EaseType.In)
			 .SetTrans(Tween.TransitionType.Cubic);
		tween.Chain();
		
		tween.TweenCallback(Callable.From(QueueFree));

		
		var scaleTween = CreateTween();
		scaleTween.TweenProperty(this, "scale", Vector2.One * 1.5f , .1f)
				  .SetEase(Tween.EaseType.Out)
				  .SetTrans(Tween.TransitionType.Cubic);
		scaleTween.TweenProperty(this, "scale", Vector2.One, .1f)
				  .SetEase(Tween.EaseType.In)
				  .SetTrans(Tween.TransitionType.Cubic);
	}
}
