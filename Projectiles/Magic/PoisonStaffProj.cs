using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class PoisonStaffProj : BaseAtkProj
    {
        public override void AttackEffect()
        {
            int num139 = 3;
            if (Main.rand.NextBool(3))
            {
                num139++;
            }
            for (int num140 = 0; num140 < num139; num140++)
            {
                float num141 = Projectile.velocity.X;
                float num142 = Projectile.velocity.X;
                float num143 = 0.05f * num140;
                num141 += Main.rand.Next(-35, 36) * num143;
                num142 += Main.rand.Next(-35, 36) * num143;
                float num84 = (float)Math.Sqrt(num141 * num141 + num142 * num142);
                num84 = Projectile.velocity.Length() / num84;
                num141 *= num84;
                num142 *= num84;
                int protmp = Projectile.NewProjectile(null, Projectile.Center, new Vector2(num141, num142), ProjectileID.PoisonFang, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                Main.projectile[protmp].npcProj = true;
                Main.projectile[protmp].noDropItem = true;


            }
        }

    }

}