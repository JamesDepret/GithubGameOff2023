namespace UI;
public partial class CurrencyGainText : Control
{
	Label label;
	public override void _Ready()
	{
		label = GetNode<Label>("Label");
	}

	public void Start (string text)
	{
		if(label == null) 
		{
			QueueFree();
			return;
		}
		label.Text = text;
		var tween = CreateTween();
		tween.SetParallel();
		tween.TweenProperty(this, "global_position", GlobalPosition + (Vector2.Up * 16), .4f)
			 .SetEase(Tween.EaseType.Out)
			 .SetTrans(Tween.TransitionType.Cubic);
		tween.Chain();

		tween.TweenProperty(this, "global_position", GlobalPosition + (Vector2.Up * 24), .6f)
			 .SetEase(Tween.EaseType.Out)
			 .SetTrans(Tween.TransitionType.Cubic);
		tween.TweenProperty(this, "scale", Vector2.Zero, .6f)
			 .SetEase(Tween.EaseType.In)
			 .SetTrans(Tween.TransitionType.Cubic);
		tween.Chain();
		
		tween.TweenCallback(Callable.From(QueueFree));

		
		var scaleTween = CreateTween();
		scaleTween.TweenProperty(this, "scale", Vector2.One * 1.5f , .3f)
				  .SetEase(Tween.EaseType.Out)
				  .SetTrans(Tween.TransitionType.Cubic);
		scaleTween.TweenProperty(this, "scale", Vector2.One, .3f)
				  .SetEase(Tween.EaseType.In)
				  .SetTrans(Tween.TransitionType.Cubic);
	}
}
