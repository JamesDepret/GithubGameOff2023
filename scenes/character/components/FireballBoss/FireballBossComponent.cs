
namespace Components;
public partial class FireballBossComponent : VelocityComponent
{
	[Export] private float rotationSpeed = 1f;
	[Export] private float maxRotation = 0f;
	[Export] private float maxSpeed = 0f;
	[Export] private float radius = 20f;
	Vector2 target;
	Vector2 centerPoint;
	bool inPosition = false;
	bool firstTimeInPosition = true;
	float distanceOnCircomferenceToAngle = 0f;
	List<FireballBossComponent> allies = new List<FireballBossComponent>();
	CharacterBody2D enemyBody;


	public override void _Ready()
	{
		(GetParent() as Node2D).GlobalPosition = new Vector2(340, -50);
		// get current enemy count
		var allies = GetTree().GetNodesInGroup("enemy");
		foreach (var ally in allies)
		{
			if (ally is Enemy enemy)
			{
				var allyBossComponent = enemy.GetNode<FireballBossComponent>("FireballBossComponent");
				if (allyBossComponent != null)
				{
					this.allies.Add(allyBossComponent);
				}
			}
		}
		centerPoint = new Vector2(GetViewport().GetVisibleRect().Size.X / 2, GetViewport().GetVisibleRect().Size.Y / 2);
		target = new Vector2(centerPoint.X, centerPoint.Y + radius);
		distanceOnCircomferenceToAngle = 2 * Mathf.Pi * radius / 360;
	}

	public override Vector2 GetDirectionToTarget()
    {
		var owner = Owner as Node2D;
		var directionToTarget = target - owner.GlobalPosition;
		return  directionToTarget.Normalized();
    }

	public override void Move(CharacterBody2D body){
        
		var distanceToTarget = (Owner as Node2D).GlobalPosition.DistanceTo(target);
		if(inPosition == false)
        {
            GetIntoPosition(distanceToTarget);
			visualsNode.Rotation = Mathf.Atan2(target.Y - visualsNode.GlobalPosition.Y, target.X - visualsNode.GlobalPosition.X) - Mathf.Pi / 2;
			body.Velocity = velocity;
			body.MoveAndSlide();
			velocity = body.Velocity;
        }
		else
        {
            RotationLogic(body);
        }
    }

	
    private void GetIntoPosition(float distanceToTarget)
    {
        if (distanceToTarget > 20)
        {
            Direction = GetDirectionToTarget();
            AccelerateInDirection(Direction);
        }
        else
        {
            // direction = vector towards player
            var player = GetTree().GetNodesInGroup("player")[0] as Player;
            Direction = (player.GlobalPosition - (Owner as Node2D).GlobalPosition).Normalized();
            velocity = Vector2.Zero;
            maxRotation = 0f;
            maxSpeed = 0f;
            inPosition = true;
        }
    }

    private void RotationLogic(CharacterBody2D body)
    {
        if (firstTimeInPosition)
        {
            enemyBody = body;
            if (allies.Count == 2)
            {
                var allyPosition = allies[0].GetPositionFromCenter();
                float angle = Mathf.Atan2(allyPosition.Y, allyPosition.X);
                float offsetAngle = angle + DegreesToRadians(120.0f);
                float normalizedAngle = NormalizeAngle(offsetAngle);
                body.GlobalPosition = CalculateCoordinates(normalizedAngle) * radius;
                GD.Print(RadiansToDegrees(Mathf.Atan2(body.GlobalPosition.Y, body.GlobalPosition.X)), "° - ", RadiansToDegrees(angle), "°");
            }
            if (allies.Count == 3)
            {
                var allyPosition = allies[0].GetPositionFromCenter();
                float angle = Mathf.Atan2(allyPosition.Y, allyPosition.X);
                float offsetAngle = angle + DegreesToRadians(240.0f);
                float normalizedAngle = NormalizeAngle(offsetAngle);
                body.GlobalPosition = CalculateCoordinates(normalizedAngle) * radius;
                GD.Print(RadiansToDegrees(Mathf.Atan2(body.GlobalPosition.Y, body.GlobalPosition.X)), "° - ", RadiansToDegrees(angle), "°");
            }
        }
        else
        {
            RotateAroundCenter(body);
        }
        firstTimeInPosition = false;
    }

	private void RotateAroundCenter(CharacterBody2D body)
	{
		var nextPostion = GetCoordinateStepOnCircomference(rotationSpeed);
		body.GlobalPosition = new Vector2(nextPostion.X + centerPoint.X, nextPostion.Y + centerPoint.Y);
		visualsNode.Rotation = Mathf.Atan2(nextPostion.Y, nextPostion.X) - Mathf.Pi / 2;
		
	}

	public Vector2 GetPositionFromCenter()
	{
		var xDistance = enemyBody.GlobalPosition.X - centerPoint.X;
		var yDistance = enemyBody.GlobalPosition.Y - centerPoint.Y;
		return new Vector2(xDistance, yDistance);
	}

	private float GetAngle(float x, float y)
	{
		var angle = Mathf.Atan2(y, x);
		return angle;
	}
	private Vector2 AngleToVector2(float angle)
	{
		return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
	}

	private Vector2 GetCoordinateStepOnCircomference(float distance)
	{
		var position = GetPositionFromCenter();
		var angle = GetAngle(position.X, position.Y);
		var newAngle = angle + distance / distanceOnCircomferenceToAngle;
		return AngleToVector2(newAngle) * radius;
	}

	Vector2 CalculateCoordinates(float angle)
    {
        float x = Mathf.Cos(angle);
        float y = Mathf.Sin(angle);

        return new Vector2(x, y);
    }

	float RadiansToDegrees(float radians)
    {
        return radians * (180.0f / Mathf.Pi);
    }

    float DegreesToRadians(float degrees)
    {
        return degrees * (Mathf.Pi / 180.0f);
    }

    float NormalizeAngle(float angle)
    {
        // Normalize angle to the range [0, 2π)
        angle = angle % (2.0f * Mathf.Pi);

        // Ensure the result is positive
        if (angle < 0.0f)
        {
            angle += 2.0f * Mathf.Pi;
        }

        return angle;
    }
}
