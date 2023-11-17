namespace UI;
public partial class OptionsMenu : CanvasLayer
{
	HSlider MusicVolumeSlider;
	HSlider SFXVolumeSlider;
	Button BackButton;
	public override void _Ready()
	{
		MusicVolumeSlider = GetNode<HSlider>("MarginContainer/PanelContainer/MarginContainer/VBoxContainer/VBoxContainer/MusicOptionContainer/MusicSlider");
		SFXVolumeSlider = GetNode<HSlider>("MarginContainer/PanelContainer/MarginContainer/VBoxContainer/VBoxContainer/SFXContainer/SFXSlider");
		BackButton = GetNode<Button>("MarginContainer/PanelContainer/MarginContainer/VBoxContainer/BackButton");

		SFXVolumeSlider.ValueChanged += OnSFXVolumeSliderChanged;
		MusicVolumeSlider.ValueChanged += OnMusicVolumeSliderChanged;

		BackButton.Pressed += OnBackButtonPressed;
		UpdateDisplay();
	}

	public override void _ExitTree()
	{
		BackButton.Pressed -= OnBackButtonPressed;
		SFXVolumeSlider.ValueChanged -= OnSFXVolumeSliderChanged;
		MusicVolumeSlider.ValueChanged -= OnMusicVolumeSliderChanged;

	}

	private void UpdateDisplay()
	{
		SFXVolumeSlider.Value = GetBusVolumePercent("sfx");
		MusicVolumeSlider.Value = GetBusVolumePercent("music");
	}

	private void OnBackButtonPressed()
	{
		GetTree().Paused = false;
		QueueFree();
	}

	private float GetBusVolumePercent(string busname)
	{
		var busIndex = AudioServer.GetBusIndex(busname);
		var dbVolume = AudioServer.GetBusVolumeDb(busIndex);
		return Mathf.DbToLinear(dbVolume);
	}

	private void SetBusVolumePercent(string busname, float percent)
	{
		var busIndex = AudioServer.GetBusIndex(busname);
		var dbVolume = Mathf.LinearToDb(percent);
		AudioServer.SetBusVolumeDb(busIndex, dbVolume);
	}

	private void OnSFXVolumeSliderChanged(double value)
	{
		SetBusVolumePercent("sfx", (float) value);
	}

	private void OnMusicVolumeSliderChanged(double value)
	{
		SetBusVolumePercent("music", (float) value);
	}
}
