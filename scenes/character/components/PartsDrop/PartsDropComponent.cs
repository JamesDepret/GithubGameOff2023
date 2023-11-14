namespace Components;

public partial class PartsDropComponent : Node
{
	[Export] public int Value = 5;
	[Export] public PackedScene PartsScene;
	[Export(PropertyHint.Range, "0,1")] public float DropPercentage = .5f;
	public HealthComponent HealthComponent;
	bool isDead = false;
	public override void _Ready()
	{
		HealthComponent = GetParent().GetNode<HealthComponent>("HealthComponent");
		HealthComponent.Died += OnDied;
	}

    public override void _ExitTree()
    {
		HealthComponent.Died -= OnDied;
    }

	void OnDied(string name)
	{
		if (PartsScene == null 
			|| Owner is not Node2D 
			|| GD.Randf() > DropPercentage) return;
		if (isDead) return;
		var spawnPosition = (Owner as Node2D).GlobalPosition;
		var partInstance = PartsScene.Instantiate() as Parts;
        if (GetTree().GetFirstNodeInGroup("entities_layer") is not Node2D entitiesLayer) throw new Exception("Could not find entities layer");

        entitiesLayer.AddChild(partInstance);
		partInstance.GlobalPosition = spawnPosition;
		partInstance.Value = Value;
		isDead = true;
	}
}
