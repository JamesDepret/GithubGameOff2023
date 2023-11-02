namespace Character;

public partial class Player : CharacterBody2D
{
	Vector2 GetMovementVector()
    {
        var xMovement = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");
        var yMovement = Input.GetActionStrength("move_down") - Input.GetActionStrength("move_up");
        return new Vector2(xMovement, yMovement);
    }

    void Move(double delta)
    {
        var movementVector = GetMovementVector();
        var direction = movementVector.Normalized();
        var targetVelocity = direction * MaxSpeed;

        if (movementVector.Length() == 0) animationPlayer.Play("RESET");
        else animationPlayer.Play("walk");

        var movesign = Mathf.Sign(movementVector.X);
        if (movesign != 0) 
            visuals.Scale = new Vector2(movesign, 1);

        Velocity = Velocity.Lerp(targetVelocity, (float)(1 - Mathf.Exp(-delta * Acceleration)));

        MoveAndSlide();
    }
}
