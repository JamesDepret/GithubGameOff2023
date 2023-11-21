namespace UI;
public partial class UpgradesScreen : CanvasLayer
{
	[Signal] public delegate void UpgradeSelectedEventHandler(BaseUpgrade upgrade);
	public List<BaseUpgrade> CurrentTurrets { get; set; }
	public Control HelpPanel { get; set; }
	private HarvestManager harvestManager;
	private ShipUpgradeCard selectedCard;
	private BaseUpgrade selectedUpgrade;
	private HFlowContainer CardContainer;
	private VBoxContainer PermaCardsContainer;
	private Button StartWaveButton;
	private BaseUpgrade[] availableUpgrades;
	private HFlowContainer turretContainersLevel1;
	private HFlowContainer turretContainersLevel2;
	private bool canAddSupply = true;
	private BaseUpgrade? previousUpgradePointer = null;
	private RichTextLabel waveInfo;
	public override void _Ready()
	{
		waveInfo = GetNode<RichTextLabel>("WaveInfoPanel/MarginContainer/WaveInfo");
		HelpPanel = GetNode<Control>("HelpPanel");
		selectedCard = GetNode<ShipUpgradeCard>("ShipUpgradeCard");
		CardContainer = GetNode<HFlowContainer>("UpgradeContainer");
		PermaCardsContainer = GetNode<VBoxContainer>("PermanentUpgrades");
		harvestManager = GetNode<HarvestManager>("/root/Main/Managers/HarvestManager");

		normalUpgradeBuyButton = GetNode<Button>("BuyButton");
		incomeSpeedBuyButton = GetNode<Button>("SpeedIncomeBuyButton");
		incomeAmountBuyButton = GetNode<Button>("AddIncomeButton");
		roomUpgradeBuyButton = GetNode<Button>("RoomUpgradeBuyButton");
		salvageButton = GetNode<Button>("SalvageButton");
		confirmSalvageButton = GetNode<Button>("ConfirmSalvageButton");
		StartWaveButton = GetNode<Button>("StartWaveButton");

		normalUpgradeBuyButton.Pressed += OnUpgradeCardBought;
		incomeSpeedBuyButton.Pressed += CleanUpDetailsCard;
		incomeAmountBuyButton.Pressed += CleanUpDetailsCard;
		roomUpgradeBuyButton.Pressed += CleanUpDetailsCard;
		salvageButton.Pressed += Salvage;
		confirmSalvageButton.Pressed += ConfirmSalvage;
		StartWaveButton.Pressed += OnNextWaveStartSelected;

		turretContainersLevel1 = GetNode<HFlowContainer>("VBoxContainer/TurretContainers");
		turretContainersLevel2 = GetNode<HFlowContainer>("VBoxContainer/TurretContainers2");
		turretContainersLevel1.GetChildren().ToList().ForEach(child => child.QueueFree());
		turretContainersLevel2.GetChildren().ToList().ForEach(child => child.QueueFree());

		GetTree().Paused = true;
		BuyButtonSetup(BuyButtonEnum.NormalUpgrade, true);
		var enemyManager = GetNode<EnemyManager>("/root/Main/Managers/EnemyManager");
		NextWaveInfo(enemyManager.GetWaveInfo());
	}

	public override void _ExitTree()
	{
		normalUpgradeBuyButton.Pressed -= OnUpgradeCardBought;
		incomeSpeedBuyButton.Pressed -= CleanUpDetailsCard;
		incomeAmountBuyButton.Pressed -= CleanUpDetailsCard;
		roomUpgradeBuyButton.Pressed -= CleanUpDetailsCard;
		salvageButton.Pressed -= Salvage;
		confirmSalvageButton.Pressed -= ConfirmSalvage;
		StartWaveButton.Pressed -= OnNextWaveStartSelected;
	}

	public void SetAbilityUpgrades(BaseUpgrade[] upgrades)
	{
		availableUpgrades = upgrades;
		selectedUpgrade = null;
		selectedCard.Visible = false;
		BuyButtonSetup(BuyButtonEnum.NormalUpgrade, true);
		SetupCards();
	}

	private void OnNextWaveStartSelected()
	{
		GetTree().Paused = false;
		QueueFree();
	}

	private void NextWaveInfo(string info)
	{
		waveInfo.Text = $"Next: [color=\"Steelblue\"]{info}[/color]";
	}
}
