namespace UI;
public partial class CloseHelp : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Pressed += OnPressed;
	}

	private void OnPressed()
	{
		GetParent().QueueFree();
	}
}
