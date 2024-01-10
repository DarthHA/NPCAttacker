using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class ChlorophyteShotbowProj : BaseAtkProj
    {
        public override int ItemType => ItemID.ChlorophyteShotbow;
        public override void AttackEffect()
        {
            int ammo = VanillaItemProjFix.AmmoType;
            if (ammo == 0) return;
            int num158 = Main.rand.Next(2, 4);
            if (Main.rand.NextBool(5))
            {
                num158++;
            }
            for (int num159 = 0; num159 < num158; num159++)
            {
                float num160 = Projectile.velocity.X;
                float num161 = Projectile.velocity.Y;
                if (num159 > 0)
                {
                    num160 += Main.rand.Next(-35, 36) * 0.04f;
                    num161 += Main.rand.Next(-35, 36) * 0.04f;
                }
                if (num159 > 1)
                {
                    num160 += Main.rand.Next(-35, 36) * 0.04f;
                    num161 += Main.rand.Next(-35, 36) * 0.04f;
                }
                if (num159 > 2)
                {
                    num160 += Main.rand.Next(-35, 36) * 0.04f;
                    num161 += Main.rand.Next(-35, 36) * 0.04f;
                }
                int protmp = Projectile.NewProjectile(null, Projectile.Center, new Vector2(num160, num161), ammo, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                Main.projectile[protmp].npcProj = true;
                Main.projectile[protmp].noDropItem = true;


            }
        }

    }

}