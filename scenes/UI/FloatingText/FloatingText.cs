namespace UI;
public partial class FloatingText : Node2D
{
	private Label label;
	private int randomDirection;
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
		var step = 0.4f;
		GlobalPosition = new Vector2(GlobalPosition.X + value * 1.4f *  randomDirection, GlobalPosition.Y - GetParabolicPosition(value)*step);
	}

	private float GetParabolicPosition(float x)
	{
		return (-2 * x * x) + (6 * x);
	}
}
