using Microsoft.Xna.Framework;
using System;
using Terraria;
namespace NPCAttacker.Projectiles
{
    public class CelebrateProj : BaseAtkProj
    {
        public override void AttackEffect()
        {
            for (int num183 = 0; num183 < 2; num183++)
            {
                float num184 = Projectile.velocity.X;
                float num185 = Projectile.velocity.Y;
                num184 += Main.rand.Next(-40, 41) * 0.05f;
                num185 += Main.rand.Next(-40, 41) * 0.05f;
                Vector2 vector63 = Projectile.Center + Vector2.Normalize(new Vector2(num184, num185).RotatedBy((double)(-1.5707964f * Math.Sign(Projectile.velocity.X)), default)) * 6f;
                int protmp = Projectile.NewProjectile(null, vector63.X, vector63.Y, num184, num185, 167 + Main.rand.Next(4), Projectile.damage, Projectile.knockBack, Main.myPlayer, 0f, 1f);
                Main.projectile[protmp].npcProj = true;
                Main.projectile[protmp].noDropItem = true;


            }
        }

    }

}