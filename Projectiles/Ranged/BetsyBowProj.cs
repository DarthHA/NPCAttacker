using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace NPCAttacker.Projectiles.Ranged
{
    public class BetsyBowProj : BaseAtkProj
    {
        public override void AttackEffect()
        {
            Vector2 vector52 = Projectile.velocity;
            vector52 *= 0.8f;
            Vector2 vector53 = vector52.SafeNormalize(-Vector2.UnitY);
            float num170 = 0.017453292f * (float)-(float)Math.Sign(Projectile.velocity.X);
            for (float num171 = -2.5f; num171 < 3f; num171 += 1f)
            {
                int protmp = Projectile.NewProjectile(null, Projectile.Center, (vector52 + vector53 * num171 * 0.5f).RotatedBy((double)(num171 * num170), default), ProjectileID.DD2BetsyArrow, Projectile.damage, Projectile.knockBack, Main.myPlayer, 0f, 0f);
                Main.projectile[protmp].npcProj = true;
                Main.projectile[protmp].noDropItem = true;
            }
        }
    }
}
