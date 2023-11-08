namespace Character;
public partial class BiteBossComponent : VelocityComponent
{
	Vector2 target = Vector2.Zero;
	Vector2 previousTarget = Vector2.Zero;

    public override void _Ready()
    {
        target = (GetParent() as Node2D).GlobalPosition;
        previousTarget = (GetParent() as Node2D).GlobalPosition;
        VelocityModifier = (float) GD.RandRange(0.97f, 1.03f);
    }
    
    public override void Move(CharacterBody2D body)
    {
        if((Owner as Node2D).GlobalPosition.DistanceTo(target) < 20  || (previousTarget == Vector2.Zero)) 
            Direction = GetPointInSpace();
        else Direction = target;

		visualsNode.Rotation = Mathf.Atan2(target.Y - previousTarget.Y, target.X - previousTarget.X) - Mathf.Pi / 2;

        body.Velocity = velocity * VelocityModifier;
        body.MoveAndSlide();
		velocity = body.Velocity;
    }

    public Vector2 GetPointInSpace()
    {
        int min = 320;
        int max = 640;
        if (Owner is not Node2D owner) return Vector2.Zero;
        var playerNodes = GetTree().GetNodesInGroup("player");
        if (playerNodes.Count > 0)
        {
            var player = playerNodes[0] as Player;
            int extra = 0;
            int distance = (int) player.GlobalPosition.DistanceTo(owner.GlobalPosition);
            if (distance > 200) extra = (int) (100 * GetDirectionToTarget().X);
            switch (player.GlobalPosition.X)
            {
                case < 100:
                    min = -50;
                    max = 150 + extra;
                    break;
                case < 200:
                    min = 50 + extra;
                    max = 250 + extra;
                    break;
                case < 300:
                    min = 150 + extra;
                    max = 350 + extra;
                    break;
                case < 400:
                    min = 250 + extra;
                    max = 450 + extra;
                    break;
                case < 500:
                    min = 350 + extra;
                    max = 550 + extra;
                    break;
                case < 600:
                    min = 450 + extra;
                    max = 650 + extra;
                    break;
                default:
                    min = 550 + extra;
                    max = 700;
                    break;
            }
        }
		var XPosition = (float) GD.RandRange(min, max);
		var YPosition = target.Y <= 0 ? 400 : -40;
		previousTarget = target;
		target = new Vector2(XPosition, YPosition);
        return target;
    }

    public override Vector2 GetDirectionToTarget()
    {
        if (Owner is not Node2D owner) return Vector2.Zero;
        return (target - owner.GlobalPosition).Normalized();
    }
}
