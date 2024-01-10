

using Microsoft.Xna.Framework;
using NPCAttacker.Systems;
using System;
using Terraria;
using Terraria.ID;

namespace NPCAttacker.Projectiles.Ranged
{
    public class TwilightBowProj : BaseAtkProj
    {
        public override int ItemType => 4953;
        public override void AttackEffect()
        {
            if (Main.rand.Next(12) < 11) return;
            int ammo = VanillaItemProjFix.AmmoType;
            if (ammo == 0) return;

            for (int i = 0; i < 5; i++)
            {
                int Dmg = Projectile.damage;
                int shoot = ammo;
                if (ammo == ProjectileID.WoodenArrowFriendly) shoot = 932;

                Vector2 ShootDist = Vector2.Normalize(Projectile.velocity) * 40;
                bool Canhit = Collision.CanHit(Projectile.Center, 0, 0, Projectile.Center + ShootDist, 0, 0);

                float r = i - 2;
                Vector2 ShootDist2 = ShootDist.RotatedBy(r * MathHelper.Pi / 10f);
                if (!Canhit)
                    ShootDist2 -= ShootDist;

                Vector2 origin = Projectile.Center + ShootDist2;
                Vector2 ShootVel = Projectile.velocity;
                if (r == 0)
                {
                    shoot = 932;
                    Dmg *= 2;
                }

                if (shoot == 932)
                {
                    float ai3 = Main.LocalPlayer.miscCounterNormalized * 12f % 1f;
                    ShootVel *= 2;
                    Projectile.NewProjectile(null, origin, ShootVel, shoot, Dmg, Projectile.knockBack, Main.myPlayer, 0f, ai3);
                }
                else
                {
                    int num65 = Projectile.NewProjectile(null, origin, ShootVel, shoot, Dmg, Projectile.knockBack, Main.myPlayer);
                    Main.projectile[num65].noDropItem = true;
                }
            }
        }
    }
}
