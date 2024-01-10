using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace NPCAttacker.Projectiles.Ranged
{
    public class PiranhaGunProj : BaseAtkProj
    {
        public override int ItemType => ItemID.PiranhaGun;
        public override void AttackEffect()
        {
            for (int num130 = 0; num130 < 3; num130++)
            {
                float num131 = Projectile.velocity.X;
                float num132 = Projectile.velocity.Y;
                num131 += Main.rand.Next(-40, 41) * 0.05f;
                num132 += Main.rand.Next(-40, 41) * 0.05f;
                Projectile.NewProjectile(null, Projectile.Center, new Vector2(num131, num132), ProjectileID.MechanicalPiranha, Projectile.damage, Projectile.knockBack, Main.myPlayer);
            }
        }
    }
}
