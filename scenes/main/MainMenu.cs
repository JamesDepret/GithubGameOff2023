namespace UI;
public partial class MainMenu : CanvasLayer
{
	[Export] private PackedScene optionsMenu;

	public override void _Ready()
	{
		var restartButton = GetNode<Button>("MarginContainer/PanelContainer/MarginContainer/VBoxContainer/PlayButton");
		var optionsButton = GetNode<Button>("MarginContainer/PanelContainer/MarginContainer/VBoxContainer/OptionsButton");
		var quitButton = GetNode<Button>("MarginContainer/PanelContainer/MarginContainer/VBoxContainer/QuitButton");
		restartButton.Pressed += OnPlayButtonPressed;
		optionsButton.Pressed += OnOptionsButtonPressed;
		quitButton.Pressed += OnQuitButtonPressed;
	}

	public override void _ExitTree()
    {
		var restartButton = GetNode<Button>("MarginContainer/PanelContainer/MarginContainer/VBoxContainer/PlayButton");
		var optionsButton = GetNode<Button>("MarginContainer/PanelContainer/MarginContainer/VBoxContainer/OptionsButton");
		var quitButton = GetNode<Button>("MarginContainer/PanelContainer/MarginContainer/VBoxContainer/QuitButton");
		restartButton.Pressed -= OnPlayButtonPressed;
		optionsButton.Pressed -= OnOptionsButtonPressed;
		quitButton.Pressed -= OnQuitButtonPressed;
    }


	private void OnPlayButtonPressed()
	{
		GetTree().Paused = false;
        GameEvents.Restart();
		GetTree().ChangeSceneToFile("res://scenes/main/main.tscn");
	}

	private void OnQuitButtonPressed()
	{
		GetTree().Quit();
	}

	private void OnOptionsButtonPressed()
	{
		var optionsMenuInstance = optionsMenu.Instantiate() as OptionsMenu;
		AddChild(optionsMenuInstance);
	}
}
