namespace UI;
public partial class BuyRoomButton : Button
{	
	public override void _Ready()
	{
		Pressed += OnBuySupply;
	}

	public override void _ExitTree()
    {
		Pressed -= OnBuySupply;
    }

	private void OnBuySupply()
	{
		GameEvents.Instance.BuySupply();
	}
}
