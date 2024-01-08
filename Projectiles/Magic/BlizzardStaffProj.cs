using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class BlizzardStaffProj : BaseAtkProj
    {
        public override void AttackEffect()
        {
            if (HasValidTarget())
            {
                int num111 = 2;
                for (int num112 = 0; num112 < num111; num112++)
                {
                    Vector2 vector2 = new Vector2(Projectile.Center.X + (float)Main.rand.Next(201) * -Math.Sign(Projectile.velocity.X) + (GetTargetPos().X - Projectile.Center.X), Projectile.Center.Y - 600f);
                    vector2.X = (vector2.X + Projectile.Center.X) / 2f + Main.rand.Next(-200, 201);
                    vector2.Y -= 100 * num112;
                    float num82 = GetTargetPos().X - vector2.X;
                    float num83 = GetTargetPos().Y - vector2.Y;
                    if (num83 < 0f)
                    {
                        num83 *= -1f;
                    }
                    if (num83 < 20f)
                    {
                        num83 = 20f;
                    }
                    float num84 = (float)Math.Sqrt(num82 * num82 + num83 * num83);
                    num84 = Projectile.velocity.Length() / num84;
                    num82 *= num84;
                    num83 *= num84;
                    float speedX4 = num82 + Main.rand.Next(-40, 41) * 0.02f;
                    float speedY5 = num83 + Main.rand.Next(-40, 41) * 0.02f;
                    int protmp = Projectile.NewProjectile(null, vector2.X, vector2.Y, speedX4, speedY5, ProjectileID.Blizzard, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, Main.rand.Next(5));
                    Main.projectile[protmp].npcProj = true;
                    Main.projectile[protmp].noDropItem = true;


                }
            }
        }

    }

}