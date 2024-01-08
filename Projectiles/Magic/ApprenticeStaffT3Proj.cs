using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class ApprenticeStaffT3Proj : BaseAtkProj
    {
        public override void AttackEffect()
        {
            Vector2 value5 = Vector2.Normalize(Projectile.velocity) * 40f;
            Vector2 vector2 = Projectile.Center;
            if (Collision.CanHit(vector2, 0, 0, vector2 + value5, 0, 0))
            {
                vector2 += value5;
            }
            Vector2 vector24 = Projectile.velocity;
            vector24 *= 0.8f;
            Vector2 value6 = vector24.SafeNormalize(-Vector2.UnitY);
            float num203 = 0.0174532924f * -Math.Sign(Projectile.velocity.X);
            for (int num204 = 0; num204 <= 2; num204++)
            {
                int protmp = Projectile.NewProjectile(null, vector2, (vector24 + value6 * num204 * 1f).RotatedBy(num204 * num203, default), ProjectileID.ApprenticeStaffT3Shot, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                Main.projectile[protmp].npcProj = true;
                Main.projectile[protmp].noDropItem = true;


            }
        }

    }

}