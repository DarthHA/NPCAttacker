using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class LightBaneProj : BaseAtkProj
    {
        public override int ItemType => ItemID.LightsBane;
        public override void AttackEffect()
        {
            int dmg = Projectile.damage;
            int dir = Math.Sign(Projectile.velocity.X + (Main.rand.Next(2) * 2 - 1) * 0.01f);
            Vector2 vector54 = new Vector2(dir, 4f).SafeNormalize(Vector2.UnitY).RotatedBy((float)Math.PI * 2f * Main.rand.NextFloatDirection() * 0.05f);

            Vector2 SearchCenter = GetTargetPos();
            if (!HasValidTarget())
            {
                SearchCenter = Main.MouseWorld;
            }
            if (SearchCenter.Distance(Projectile.Center) > 50)
            {
                SearchCenter = Projectile.Center + Vector2.Normalize(SearchCenter - Projectile.Center) * 50;
            }


            float ai8 = 1f;
            if (Main.rand.NextBool(4))
            {
                ai8 = 2f;
                dmg *= 2;
            }

            int protmp = Projectile.NewProjectile(null, SearchCenter, vector54 * 0.001f, 974, (int)(dmg * 0.5f), Projectile.knockBack, Main.myPlayer, ai8);
            Main.projectile[protmp].npcProj = true;
            Main.projectile[protmp].noDropItem = true;
            Main.projectile[protmp].usesLocalNPCImmunity = true;
            Main.projectile[protmp].localNPCHitCooldown = 10;

        }

    }

}