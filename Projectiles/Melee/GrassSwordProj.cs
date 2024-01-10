using Microsoft.Xna.Framework;
using NPCAttacker.Systems;
using System;
using Terraria;
namespace NPCAttacker.Projectiles
{
    public class GrassSwordProj : BaseAtkProj
    {
        public override int ItemType => 190;
        public override void AttackEffect()
        {
            int dir = Math.Sign(Projectile.velocity.X + (Main.rand.Next(2) * 2 - 1) * 0.01f);
            NPC owner = Main.npc[Projectile.GetGlobalProjectile<SpecialUseProj>().NPCProjOwner];
            Vector2 vector55 = GetTargetPos();
            if (!HasValidTarget())
            {
                vector55 = Main.MouseWorld;
            }
            if (vector55.Distance(Projectile.Center) > 150)
            {
                vector55 = Projectile.Center + Vector2.Normalize(vector55 - Projectile.Center) * 150;
            }
            Vector2 vector56 = Projectile.Center + new Vector2(Main.rand.NextFloatDirection() * 8, 12) * dir;
            Vector2 v6 = vector55 - vector56;
            float num175 = ((float)Math.PI + (float)Math.PI * 2f * Main.rand.NextFloat() * 1.5f) * -dir;
            int num176 = 60;
            float num177 = num175 / num176;
            float num178 = 16f;
            float num179 = v6.Length();
            if (Math.Abs(num177) >= 0.17f)
                num177 *= 0.7f;

            Vector2 vector57 = Vector2.UnitX * num178;
            Vector2 v7 = vector57;
            int num180 = 0;
            while (v7.Length() < num179 && num180 < num176)
            {
                num180++;
                v7 += vector57;
                vector57 = vector57.RotatedBy(num177);
            }

            float num181 = v7.ToRotation();
            Vector2 spinningpoint2 = v6.SafeNormalize(Vector2.UnitY).RotatedBy(0f - num181 - num177) * num178;
            if (num180 == num176)
                spinningpoint2 = new Vector2(dir, 0f) * num178;


            int protmp = Projectile.NewProjectile(null, vector56, spinningpoint2, 976, (int)(Projectile.damage * 0.25f), Projectile.knockBack, Main.myPlayer, num177, num180);
            Main.projectile[protmp].npcProj = true;
            Main.projectile[protmp].noDropItem = true;
            Main.projectile[protmp].usesLocalNPCImmunity = true;
            Main.projectile[protmp].localNPCHitCooldown = 10;

        }


    }

}