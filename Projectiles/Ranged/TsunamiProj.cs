using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class TsunamiProj : BaseAtkProj
    {
        public override int ItemType => ItemID.Tsunami;
        public override void AttackEffect()
        {
            int ammo = VanillaItemProjFix.AmmoType;
            if (ammo == 0) return;
            float num121 = 0.314159274f;
            int num122 = 5;
            Vector2 vector15 = Projectile.velocity;
            vector15.Normalize();
            vector15 *= 40f;
            bool flag13 = Collision.CanHit(Projectile.Center, 0, 0, Projectile.Center + vector15, 0, 0);
            for (int num123 = 0; num123 < num122; num123++)
            {
                float num124 = num123 - (num122 - 1f) / 2f;
                Vector2 vector16 = vector15.RotatedBy(num121 * num124, default);
                if (!flag13)
                {
                    vector16 -= vector15;
                }
                int protmp = Projectile.NewProjectile(null, Projectile.Center + vector16, Projectile.velocity, ammo, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                Main.projectile[protmp].npcProj = true;
                Main.projectile[protmp].noDropItem = true;


            }
        }

    }

}