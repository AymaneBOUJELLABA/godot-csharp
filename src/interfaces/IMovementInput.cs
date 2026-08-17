using Godot;

public interface IMovementInput
{
    public Vector2 GetDirection();

    public bool CanDash();
}
