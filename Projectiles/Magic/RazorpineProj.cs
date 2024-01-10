using System;
using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class RazorpineProj : BaseAtkProj
    {
        public override int ItemType => ItemID.Razorpine;
        public override void AttackEffect()
        {
            int num106 = 2 + Main.rand.Next(3);
            for (int num107 = 0; num107 < num106; num107++)
            {
                float num108 = Projectile.velocity.X;
                float num109 = Projectile.velocity.Y;
                float num110 = 0.025f * num107;
                num108 += Main.rand.Next(-35, 36) * num110;
                num109 += Main.rand.Next(-35, 36) * num110;
                float num84 = (float)Math.Sqrt(num108 * num108 + num109 * num109);
                num84 = Projectile.velocity.Length() / num84;
                num108 *= num84;
                num109 *= num84;
                float x = Projectile.Center.X + Projectile.velocity.X * (num106 - num107) * 1.75f;
                float y = Projectile.Center.Y + Projectile.velocity.Y * (num106 - num107) * 1.75f;
                int protmp = Projectile.NewProjectile(null, x, y, num108, num109, ProjectileID.PineNeedleFriendly, Projectile.damage, Projectile.knockBack, Projectile.owner, Main.rand.Next(0, 10 * (num107 + 1)), 0f);
                Main.projectile[protmp].npcProj = true;
                Main.projectile[protmp].noDropItem = true;


            }
        }

    }

}