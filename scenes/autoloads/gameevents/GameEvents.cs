namespace Manager;
public partial class GameEvents : Node
{

	[Signal] public delegate void PartsCollectedEventHandler (float number);
	public static GameEvents Instance { get; private set; }
	public override void _Ready()
	{
		Instance = this;
	}

	public void EmitPartsCollected(float number)
	{
		EmitSignal(SignalName.PartsCollected, number);
	}
}
