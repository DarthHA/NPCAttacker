using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class TacticalShotgunProj : BaseAtkProj
    {
        public override int ItemType => ItemID.TacticalShotgun;
        public override void AttackEffect()
        {
            int ammo = VanillaItemProjFix.AmmoType;
            if (ammo == 0) return;
            for (int num176 = 0; num176 < 6; num176++)
            {
                Vector2 vector2 = Projectile.Center;
                float num177 = Projectile.velocity.X;
                float num178 = Projectile.velocity.Y;
                num177 += Main.rand.Next(-40, 41) * 0.05f;
                num178 += Main.rand.Next(-40, 41) * 0.05f;
                int protmp = Projectile.NewProjectile(null, vector2.X, vector2.Y, num177, num178, ammo, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                Main.projectile[protmp].npcProj = true;
                Main.projectile[protmp].noDropItem = true;


            }
        }

    }

}