using Terraria;

namespace NPCAttacker.Projectiles.Ranged
{

    public class BlueFireProj : BaseAtkProj
    {
        public override int ItemType => 1910;
        public override void AttackEffect()
        {
            Projectile.NewProjectile(null, Projectile.Center, Projectile.velocity, 85, Projectile.damage, Projectile.knockBack, Main.myPlayer, 1f);
        }
    }
}
