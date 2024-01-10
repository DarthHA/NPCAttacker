using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class DaedalusStormbowProj : BaseAtkProj
    {
        public override int ItemType => ItemID.DaedalusStormbow;
        public override void AttackEffect()
        {
            if (HasValidTarget())
            {
                int num75 = VanillaItemProjFix.AmmoType;
                if (num75 == 0) return;
                int num90 = 3;
                if (Main.rand.NextBool(3))
                {
                    num90++;
                }
                for (int num91 = 0; num91 < num90; num91++)
                {
                    Vector2 vector2 = new Vector2(Projectile.Center.X + (float)Main.rand.Next(201) * -Math.Sign(Projectile.velocity.X) + (GetTargetPos().X - Projectile.Center.X), Projectile.Center.Y - 600f);
                    vector2.X = (vector2.X * 10f + Projectile.Center.X) / 11f + Main.rand.Next(-100, 101);
                    vector2.Y -= 150 * num91;
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
                    float num92 = num82 + Main.rand.Next(-40, 41) * 0.03f;
                    float speedY = num83 + Main.rand.Next(-40, 41) * 0.03f;
                    num92 *= Main.rand.Next(75, 150) * 0.01f;
                    vector2.X += Main.rand.Next(-50, 51);
                    int protmp = Projectile.NewProjectile(null, vector2.X, vector2.Y, num92, speedY, num75, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                    Main.projectile[protmp].npcProj = true;
                    Main.projectile[protmp].noDropItem = true;


                }
            }
        }

    }

}