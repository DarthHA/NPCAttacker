using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class LunarFlareBookProj : BaseAtkProj
    {
        public override int ItemType => ItemID.LunarFlareBook;
        public override void AttackEffect()
        {
            if (HasValidTarget())
            {
                int num117 = 3;
                for (int num118 = 0; num118 < num117; num118++)
                {
                    Vector2 vector2 = new Vector2(Projectile.Center.X + Main.rand.Next(201) * -(float)Math.Sign(Projectile.velocity.X) + (GetTargetPos().X - Projectile.position.X), Projectile.Center.Y - 600f);
                    vector2.X = (vector2.X + Projectile.Center.X) / 2f + Main.rand.Next(-200, 201);
                    vector2.Y -= 100 * num118;
                    float num82 = GetTargetPos().X - vector2.X;
                    float num83 = GetTargetPos().Y - vector2.Y;
                    float ai2 = num83 + vector2.Y;
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
                    Vector2 vector12 = new Vector2(num82, num83) / 2f;
                    int protmp = Projectile.NewProjectile(null, vector2, vector12, ProjectileID.LunarFlare, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, ai2);
                    Main.projectile[protmp].npcProj = true;
                    Main.projectile[protmp].noDropItem = true;


                }

            }
        }

    }

}