using System;
using Godot;

public partial class HealthComponent : Node, IDamageable, IHealable
{
    [ExportGroup("Health Properties")]
    [Export]
    private int _maxHealth = 100;

    public int CurrentHealth { get; private set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.CurrentHealth = this._maxHealth;
    }

    public void Heal(int healValue)
    {
        //never exceed maxHealth
        this.CurrentHealth = Math.Min(this.CurrentHealth + healValue, this._maxHealth);

        EmitSignal(SignalName.HealthValueChanged, this.CurrentHealth, this._maxHealth);
    }

    public void Damage(int damageValue)
    {
        //never drop below 0
        this.CurrentHealth = Math.Max(this.CurrentHealth - damageValue, 0);
        EmitSignal(SignalName.HealthValueChanged, this.CurrentHealth, this._maxHealth);

        if (this.CurrentHealth == 0)
        {
            EmitSignal(SignalName.HealthReachedZero);
        }
    }

    [Signal]
    public delegate void HealthValueChangedEventHandler(int currentHealth, int maxHealth);

    [Signal]
    public delegate void HealthReachedZeroEventHandler();
}
