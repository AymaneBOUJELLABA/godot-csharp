using Godot;

public partial class PlayerInputComponent : Node, IMovementInput
{
    public bool CanDash()
    {
        return true;
    }

    public Vector2 GetDirection()
    {
        var directionVector = Input.GetVector(
            MovementComponent.ActionInputMap[MovementComponent.ActionEnum.MoveLeft], // negativeX
            MovementComponent.ActionInputMap[MovementComponent.ActionEnum.MoveRight], // positiveX
            MovementComponent.ActionInputMap[MovementComponent.ActionEnum.MoveUp], // negativeY
            MovementComponent.ActionInputMap[MovementComponent.ActionEnum.MoveDown] //postiveY
        );
        return directionVector;
    }
}
