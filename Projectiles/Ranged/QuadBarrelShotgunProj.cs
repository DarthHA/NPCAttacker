using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace NPCAttacker.Projectiles.Ranged
{
    public class QuadBarrelShotgunProj : BaseAtkProj
    {
        public override int ItemType => ItemID.QuadBarrelShotgun;
        public override void AttackEffect()
        {
            int ammo = VanillaItemProjFix.AmmoType;
            if (ammo == 0) return;
            float num76 = 1.5707964f;
            for (int num77 = 0; num77 < 6; num77++)
            {
                Vector2 vector32 = Projectile.velocity;
                float num78 = vector32.Length();
                vector32 += vector32.SafeNormalize(Vector2.Zero).RotatedBy((double)(num76 * Main.rand.NextFloat()), default) * Main.rand.NextFloatDirection() * 6f;
                vector32 = vector32.SafeNormalize(Vector2.Zero) * num78;
                float num79 = vector32.X;
                float num80 = vector32.Y;
                num79 += Main.rand.Next(-40, 41) * 0.05f;
                num80 += Main.rand.Next(-40, 41) * 0.05f;
                int protmp = Projectile.NewProjectile(null, Projectile.Center.X, Projectile.Center.Y, num79, num80, ammo, Projectile.damage, Projectile.knockBack, Main.myPlayer, 0f, 0f);
                Main.projectile[protmp].npcProj = true;
                Main.projectile[protmp].noDropItem = true;


            }
        }

    }
}
