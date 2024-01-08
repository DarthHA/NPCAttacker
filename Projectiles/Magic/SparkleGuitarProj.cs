using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace NPCAttacker.Projectiles.Magic
{
    public class SparkleGuitarProj : BaseAtkProj
    {
        public override void AttackEffect()
        {
            Vector2 vector36 = GetTargetPos();
            Vector2 vector37 = vector36 - Projectile.Center;
            Vector2 vector38 = Main.rand.NextVector2CircularEdge(1f, 1f);
            if (vector38.Y > 0f)
            {
                vector38 *= -1f;
            }
            if (Math.Abs(vector38.Y) < 0.5f)
            {
                vector38.Y = (0f - Main.rand.NextFloat()) * 0.5f - 0.5f;
            }
            vector38 *= vector37.Length() * 2f;
            int protmp = Projectile.NewProjectile(null, Projectile.Center, vector38, ProjectileID.SparkleGuitar, Projectile.damage, Projectile.knockBack, Main.myPlayer, vector36.X, vector36.Y);
            Main.projectile[protmp].npcProj = true;
            Main.projectile[protmp].noDropItem = true;


        }

    }
}
