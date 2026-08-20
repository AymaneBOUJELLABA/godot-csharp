using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class MovementComponent : Node
{
    public enum ActionEnum
    {
        MoveLeft,
        MoveRight,
        MoveUp,
        MoveDown,
        Dash
    }

    private const int AxisXLeft = -1;
    private const int AxisXRight = 1;
    public static readonly Dictionary<ActionEnum, StringName> ActionInputMap =
        new()
        {
            { ActionEnum.MoveLeft, "move_left" },
            { ActionEnum.MoveRight, "move_right" },
            { ActionEnum.MoveUp, "move_forward" },
            { ActionEnum.MoveDown, "move_back" },
            { ActionEnum.Dash, "dash" },
        };

    [ExportGroup("Movement Properties")]
    [Export]
    private float _defaultSpeed = 10f;

    [ExportGroup("Movement Properties")]
    [ExportSubgroup("Dash")]
    [Export]
    private float _dashDuration = 0.5f; //seconds

    [ExportGroup("Movement Properties")]
    [ExportSubgroup("Dash")]
    [Export]
    private float _dashSpeed = 2f; // multiply speed by this value for the dash;
    private CharacterBody2D _parentObject;

    private bool _isDashing = false;
    private float _currentSpeed;

    private bool _isFacingRight = true;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this._currentSpeed = _defaultSpeed;
        this._parentObject = GetParent<CharacterBody2D>();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (this._currentSpeed == 0)
        {
            this._currentSpeed = this._defaultSpeed;
        }

        IMovementInput movementInput = this.GetChildMovementInput();

        var directionVector = movementInput.GetDirection();

        //if body can dash (is player or maybe another AI that has a dash TODO later)
        if (
            movementInput.CanDash()
            && !this._isDashing
            && Input.IsActionJustPressed(ActionInputMap[ActionEnum.Dash])
        )
            this.HandleDashing();

        // only move when direction is > 0
        if (!directionVector.IsZeroApprox())
        {
            //flip left
            if (directionVector.X < 0 && this._isFacingRight)
            {
                this._parentObject.Scale = new Vector2(AxisXLeft, this._parentObject.Scale.Y);
                this._isFacingRight = false;
            }
            //flip right
            if (directionVector.X > 0 && !this._isFacingRight)
            {
                this._parentObject.Scale = new Vector2(AxisXLeft, this._parentObject.Scale.Y);
                this._isFacingRight = true;
            }

            this.Move(directionVector);
        }
    }

    private IMovementInput GetChildMovementInput()
    {
        return this._parentObject.GetChildren().OfType<IMovementInput>().FirstOrDefault();
    }

    private void Move(Vector2 directionVector)
    {
        this._parentObject.Velocity = directionVector * this._currentSpeed;
        this._parentObject.MoveAndSlide();
    }

    private async void HandleDashing()
    {
        //check if the button is pressed
        bool dashPressed = Input.IsActionJustPressed(ActionInputMap[ActionEnum.Dash]);

        //if body is not already dashing and button pressed then we should dash
        if (!this._isDashing && dashPressed)
        {
            //set the flag to true
            this._isDashing = true;

            this._currentSpeed *= this._dashSpeed;

            await ToSignal(
                GetTree().CreateTimer(this._dashDuration),
                SceneTreeTimer.SignalName.Timeout
            );

            this._currentSpeed = this._defaultSpeed;
            this._isDashing = false;
        }
    }
}
