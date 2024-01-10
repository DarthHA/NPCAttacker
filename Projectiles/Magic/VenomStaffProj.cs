using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class VenomStaffProj : BaseAtkProj
    {
        public override int ItemType => ItemID.VenomStaff;
        public override void AttackEffect()
        {
            int num134 = 4;
            if (Main.rand.NextBool(3))
            {
                num134++;
            }
            if (Main.rand.NextBool(4))
            {
                num134++;
            }
            if (Main.rand.NextBool(5))
            {
                num134++;
            }
            for (int num135 = 0; num135 < num134; num135++)
            {
                float num136 = Projectile.velocity.X;
                float num137 = Projectile.velocity.Y;
                float num138 = 0.05f * num135;
                num136 += Main.rand.Next(-35, 36) * num138;
                num137 += Main.rand.Next(-35, 36) * num138;
                float num84 = (float)Math.Sqrt(num136 * num136 + num137 * num137);
                num84 = Projectile.velocity.Length() / num84;
                num136 *= num84;
                num137 *= num84;
                int protmp = Projectile.NewProjectile(null, Projectile.Center, new Vector2(num136, num137), ProjectileID.VenomFang, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                Main.projectile[protmp].npcProj = true;
                Main.projectile[protmp].noDropItem = true;


            }
        }

    }

}