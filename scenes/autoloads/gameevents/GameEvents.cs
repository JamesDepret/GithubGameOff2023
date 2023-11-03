namespace Manager;
public partial class GameEvents : Node
{

	[Signal] public delegate void PartsCollectedEventHandler (int number);
	public static GameEvents Instance { get; private set; }
	public int Parts { get; set; } = 0;
	public override void _Ready()
	{
		Instance = this;
	}

	public void EmitPartsCollected(int number)
	{
		Parts += number;
		EmitSignal(SignalName.PartsCollected, number);
	}
}
