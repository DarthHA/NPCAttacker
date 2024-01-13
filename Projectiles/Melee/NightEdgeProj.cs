using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class NightEdgeProj : BaseAtkProj
    {
        public override int ItemType => ItemID.NightsEdge;
        public override void AttackEffect()
        {
            int dir = Math.Sign(Projectile.velocity.X + (Main.rand.Next(2) * 2 - 1) * 0.01f);
            NPC owner = Main.npc[GetOwner()];
            float num2 = GetTargetPos().X - Projectile.Center.X + (Main.rand.Next(2) * 2 - 1) * 0.01f;
            float num3 = GetTargetPos().Y - Projectile.Center.Y + (Main.rand.Next(2) * 2 - 1) * 0.01f;

            float adjustedItemScale = 1;
            int protmp = Projectile.NewProjectile(null, Projectile.Center, new Vector2(dir, 0f), 972, Projectile.damage, Projectile.knockBack, Main.myPlayer, dir, NPCStats.GetModifiedAttackTime(owner), adjustedItemScale);
            Main.projectile[protmp].npcProj = true;
            Main.projectile[protmp].noDropItem = true;
            protmp = Projectile.NewProjectile(null, Projectile.Center, new Vector2(num2, num3), 972, Projectile.damage, Projectile.knockBack, Main.myPlayer, dir * 0.1f, 30f, adjustedItemScale);
            Main.projectile[protmp].npcProj = true;
            Main.projectile[protmp].noDropItem = true;

        }

    }

}