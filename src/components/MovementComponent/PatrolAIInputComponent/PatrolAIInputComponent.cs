using Godot;

public partial class PatrolAIInputComponent : Node, IMovementInput
{
    [Export]
    private RayCast2D _rayCast2d;

    private int _xDirection; // -1 left +1 right;

    public override void _Ready()
    {
        base._Ready();
        this._xDirection = 1; // default to right
    }

    public Vector2 GetDirection()
    {
        if (this._rayCast2d.IsColliding())
        {
            this._xDirection *= -1;
        }

        return new Vector2(this._xDirection, 0);
    }

    public bool CanDash()
    {
        return false;
    }
}
