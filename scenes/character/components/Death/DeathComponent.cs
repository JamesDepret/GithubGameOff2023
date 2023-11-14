namespace Components;
public partial class DeathComponent : Node2D
{
	[Export] HealthComponent healthComponent;
	[Export] AnimatedSprite2D sprite;
	AnimationPlayer animationPlayer;
	GpuParticles2D particles;
	
	public override void _Ready()
	{
		healthComponent.Died += OnDied;

		particles = GetNode<GpuParticles2D>("GPUParticles2D");
		particles.Texture = sprite.SpriteFrames.GetFrameTexture("default",0);

		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
	}
	public override void _ExitTree()
    {
		healthComponent.Died -= OnDied;
    }

	void OnDied(string name)
	{
		if (Owner == null) return;

		var spawnPosition = (Owner as Node2D).GlobalPosition;
		var entities = GetTree().GetFirstNodeInGroup("entities_layer");
		GetParent().RemoveChild(this);
		entities.AddChild(this);
		GlobalPosition = spawnPosition;
		animationPlayer.Play("default");
	}
}
