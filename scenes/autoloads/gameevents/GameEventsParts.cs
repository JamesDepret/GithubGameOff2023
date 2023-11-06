namespace Manager;
public partial class GameEvents : Node
{

	[Signal] public delegate void PartsCollectedEventHandler (int number);
	public int Parts { get; set; } = 0;
    
	public void EmitPartsCollected(int number)
	{
		Parts += number;
		EmitSignal(SignalName.PartsCollected, number);
	}
}