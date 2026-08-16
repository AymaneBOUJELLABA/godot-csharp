using System;
using System.Linq;
using Godot;

//TODO add collision damage palyer <> enemy Area2D
//TODO in future trigger Damage on Area2D too (for projectiles or other Area2D and not PhysicsBody2D)
public partial class HitboxComponent : Area2D
{
    [ExportGroup("Hitbox Properties")]
    [Export]
    private int _damage = 5;

    // [Export]
    // private Shape2D _shape;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.BodyEntered += OnBodyEntered;
        // GetNode<CollisionShape2D>("CollisionShape2D").Shape = this._shape;
    }

    private void OnBodyEntered(Node body)
    {
        GD.Print("Body entered");
        // if object is IDamageable damage it directly
        if (body is IDamageable damageableBody)
        {
            GD.Print("Body is damaged");
            damageableBody.Damage(this._damage);
        }
        //otherwise look for the child node that implements the IDamageable
        else
        {
            //in case of recursive and nested Damageable nodes
            // var damageableChild = body.FindChildren("*", "", recursive: true, owned: false)
            //     .OfType<IDamageable>()
            //     .FirstOrDefault();
            GD.Print("Damaging body using IDamageable child");
            var damageableChild = body.GetChildren().OfType<IDamageable>().FirstOrDefault();

            damageableChild?.Damage(this._damage);
        }

        //do nothing if body isn't damageable
    }
}
