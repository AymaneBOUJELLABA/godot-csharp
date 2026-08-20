using System;
using Godot;

public partial class HealthComponent : Node, IDamageable, IHealable
{
    [ExportGroup("Health Properties")]
    [Export]
    private float _maxHealth = 100;

    public float CurrentHealth { get; private set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.CurrentHealth = this._maxHealth;
    }

    public void Heal(float healValue)
    {
        //never exceed maxHealth
        this.CurrentHealth = Math.Min(this.CurrentHealth + healValue, this._maxHealth);

        EmitSignal(SignalName.HealthValueChanged, this.CurrentHealth, this._maxHealth);
    }

    public void Damage(float damageValue)
    {
        //never drop below 0
        this.CurrentHealth = Math.Max(this.CurrentHealth - damageValue, 0);
        EmitSignal(SignalName.HealthValueChanged, this.CurrentHealth, this._maxHealth);

        if (this.CurrentHealth == 0)
        {
            EmitSignal(SignalName.HealthReachedZero);
        }
    }

    public float GetCurrentHealth()
    {
        return this.CurrentHealth;
    }

    public float GetMaxHealth()
    {
        return this._maxHealth;
    }

    [Signal]
    public delegate void HealthValueChangedEventHandler(float currentHealth, float maxHealth);

    [Signal]
    public delegate void HealthReachedZeroEventHandler();
}
