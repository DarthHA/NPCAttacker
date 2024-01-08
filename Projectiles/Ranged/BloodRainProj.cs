using System;
using System.Numerics;
using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class BloodRainProj : BaseAtkProj
    {
        public override void AttackEffect()
        {

            int num18 = Main.rand.Next(1, 3);
            if (Main.rand.NextBool(3))
            {
                num18++;
            }
            for (int n = 0; n < num18; n++)
            {
                Vector2 vector = new(Projectile.Center.X + Main.rand.Next(61) * -Math.Sign(Projectile.velocity.X) + (GetTargetPos().X - Projectile.position.X), Projectile.Center.Y - 600f);
                vector.X = (vector.X * 10f + Projectile.Center.X) / 11f + Main.rand.Next(-30, 31);
                vector.Y -= 150f * Main.rand.NextFloat();
                float num6 = GetTargetPos().X - vector.X;
                float num7 = GetTargetPos().Y - vector.Y;
                if (num7 < 0f)
                {
                    num7 *= -1f;
                }
                if (num7 < 20f)
                {
                    num7 = 20f;
                }
                float num8 = (float)Math.Sqrt(num6 * num6 + num7 * num7);
                num8 = Projectile.velocity.Length() / num8;
                num6 *= num8;
                num7 *= num8;
                float num19 = num6 + Main.rand.Next(-20, 21) * 0.03f;
                float speedY2 = num7 + Main.rand.Next(-40, 41) * 0.03f;
                num19 *= Main.rand.Next(55, 80) * 0.01f;
                vector.X += Main.rand.Next(-50, 51);
                int protmp = Projectile.NewProjectile(null, vector.X, vector.Y, num19, speedY2, ProjectileID.BloodArrow, Projectile.damage, Projectile.knockBack, Main.myPlayer, 0f, 0f);
                Main.projectile[protmp].npcProj = true;
                Main.projectile[protmp].noDropItem = true;
            }
        }
    }

}