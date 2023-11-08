namespace UI;
public partial class CurrentTurret : TextureButton
{
	public UpgradesScreen UpgradesScreen { get; set; }
	public BaseUpgrade Tower { get; set; }

	public override void _Ready()
	{
		Pressed += OnPressed;
	}
	public void OnPressed()
	{
		if (Tower.UpgradesTo != null)
		{
			UpgradesScreen.SetSelectedUpgrade(Tower.UpgradesTo as BaseUpgrade, Tower);
		}
	}
}
