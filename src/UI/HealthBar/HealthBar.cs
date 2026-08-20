using Godot;

public partial class HealthBar : ProgressBar
{
    [Export]
    private HealthComponent _healthComponent;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this._healthComponent ??= GetParent().GetNode<HealthComponent>("../HealthComponent");
        this._healthComponent.HealthValueChanged += this.OnHealthValueChanged;

        // defer the initial read until the whole tree's _Ready() pass is done
        CallDeferred(MethodName.InitializeBar);
    }

    private void InitializeBar()
    {
        this.Value = this._healthComponent.GetCurrentHealth();
        this.MaxValue = this._healthComponent.GetMaxHealth();
        GD.Print($"healthBar min = {this.Value} / max = {this.MaxValue}");
    }

    private void OnHealthValueChanged(float currentHealth, float maxHealth)
    {
        this.Value = currentHealth;
        if (this.MaxValue != maxHealth)
        {
            this.MaxValue = maxHealth;
        }
    }
}
