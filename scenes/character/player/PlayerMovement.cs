using System.Numerics;

namespace Character;

public partial class Player : CharacterBody2D
{
	Godot.Vector2 GetMovementVector()
    {
        var xMovement = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");
        var yMovement = Input.GetActionStrength("move_down") - Input.GetActionStrength("move_up");
        return new Godot.Vector2(xMovement, yMovement);
    }

    void Move(double delta)
    {
        var movementVector = GetMovementVector();
        var direction = movementVector.Normalized();
        var targetVelocity = direction * MaxSpeed;

        RotatePlayer(delta, direction);

        if (movementVector.Length() == 0) animationPlayer.Play("RESET");
        else animationPlayer.Play("walk");

        if (movementVector.Y > 0) particles.Emitting = false;
        else particles.Emitting = true;

        var movesign = Mathf.Sign(movementVector.X);
        if (movesign != 0) 
            visuals.Scale = new Godot.Vector2(movesign, 1);

        Velocity = Velocity.Lerp(targetVelocity, (float)(1 - Mathf.Exp(-delta * Acceleration)));

        MoveAndSlide();
    }

    void RotatePlayer(double delta, Godot.Vector2 direction)
    {
        float targetRotation = maxRotation * direction.X;
        currentRotation = (float) Mathf.Lerp(currentRotation, targetRotation, RotationSpeed * delta);
        visuals.Rotation = currentRotation;
    }
}
