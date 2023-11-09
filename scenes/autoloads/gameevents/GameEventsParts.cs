namespace Manager;
public partial class GameEvents : Node
{

	[Signal] public delegate void PartsCollectedEventHandler (int number);
	public int Parts { get; set; } = 0;
	public float LootCritChance { get; set; } = 0f;
    
	public void EmitPartsCollected(int number)
	{
		// get random number between 0 and 1
		float critRoll = (float) GD.RandRange(0, 100)/100;
		if (critRoll <= LootCritChance && number > 0)
		{
			number *= 2;
		}
		Parts += number;
		EmitSignal(SignalName.PartsCollected, number);
	}
}