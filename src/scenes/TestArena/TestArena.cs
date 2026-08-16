using System;
using Godot;

public partial class TestArena : Node2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var playerHealth = this.GetNode<HealthComponent>("Player/HealthComponent");
        var enemyHealth = this.GetNode<HealthComponent>("Enemy/HealthComponent");

        playerHealth.HealthValueChanged += OnPlayerHealthChanged;
        playerHealth.HealthReachedZero += OnPlayerDied;

        enemyHealth.HealthValueChanged += OnEnemyHealthChanged;
        enemyHealth.HealthReachedZero += OnEnemyDied;
    }

    private void OnPlayerHealthChanged(int current, int max)
    {
        GD.Print($"Player health: {current}/{max}");
    }

    private void OnPlayerDied()
    {
        GD.Print("Player died!");
    }

    private void OnEnemyHealthChanged(int current, int max)
    {
        GD.Print($"Enemy health: {current}/{max}");
    }

    private void OnEnemyDied()
    {
        GD.Print("Enemy died!");
    }
}
