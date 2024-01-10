using Microsoft.Xna.Framework;
using NPCAttacker.Systems;
using System;
using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class TrueNightEdgeProj : BaseAtkProj
    {
        public override int ItemType => ItemID.TrueNightsEdge;
        public override void AttackEffect()
        {
            int dir = Math.Sign(Projectile.velocity.X + (Main.rand.Next(2) * 2 - 1) * 0.01f);
            NPC owner = Main.npc[Projectile.GetGlobalProjectile<SpecialUseProj>().NPCProjOwner];

            float adjustedItemScale4 = 1;
            int protmp = Projectile.NewProjectile(null, Projectile.Center, new Vector2(dir, 0f), 972, Projectile.damage, Projectile.knockBack, Main.myPlayer, (float)dir, NPCStats.GetModifiedAttackTime(owner), adjustedItemScale4);
            Main.projectile[protmp].npcProj = true;
            Main.projectile[protmp].noDropItem = true;
            protmp = Projectile.NewProjectile(null, Projectile.Center, Vector2.Normalize(Projectile.velocity) * 18f, 973, Projectile.damage / 2, Projectile.knockBack, Main.myPlayer, (float)dir, 32f, adjustedItemScale4);
            Main.projectile[protmp].npcProj = true;
            Main.projectile[protmp].noDropItem = true;
        }

    }

}