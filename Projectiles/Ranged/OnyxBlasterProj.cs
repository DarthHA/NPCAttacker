using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class OnyxBlasterProj : BaseAtkProj
    {
        public override int ItemType => ItemID.OnyxBlaster;
        public override void AttackEffect()
        {
            int ammo = VanillaItemProjFix.AmmoType;
            if (ammo == 0) return;
            Vector2 vector29 = Projectile.velocity;
            float num210 = 0.7853982f;
            int protmp;
            for (int num211 = 0; num211 < 2; num211++)
            {
                protmp = Projectile.NewProjectile(null, Projectile.Center, vector29 + vector29.SafeNormalize(Vector2.Zero).RotatedBy(num210 * (Main.rand.NextFloat() * 0.5f + 0.5f), default) * Main.rand.NextFloatDirection() * 2f, ammo, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                Main.projectile[protmp].npcProj = true;
                Main.projectile[protmp].noDropItem = true;


                protmp = Projectile.NewProjectile(null, Projectile.Center, vector29 + vector29.SafeNormalize(Vector2.Zero).RotatedBy(-num210 * (double)(Main.rand.NextFloat() * 0.5f + 0.5f), default) * Main.rand.NextFloatDirection() * 2f, ammo, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                Main.projectile[protmp].npcProj = true;
                Main.projectile[protmp].noDropItem = true;


            }
            protmp = Projectile.NewProjectile(null, Projectile.Center, vector29.SafeNormalize(Vector2.UnitX * Math.Sign(Projectile.velocity.X)) * (Projectile.velocity.Length() * 1.3f), ProjectileID.BlackBolt, Projectile.damage * 2, Projectile.knockBack, Projectile.owner, 0f, 0f);
            Main.projectile[protmp].npcProj = true;
            Main.projectile[protmp].noDropItem = true;


        }

    }

}