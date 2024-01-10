using Microsoft.Xna.Framework;
using NPCAttacker.Systems;
using System;
using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class TrueEXSwordProj : BaseAtkProj
    {
        public override int ItemType => ItemID.TrueExcalibur;
        public override void AttackEffect()
        {
            int dir = Math.Sign(Projectile.velocity.X + (Main.rand.Next(2) * 2 - 1) * 0.01f);
            NPC owner = Main.npc[Projectile.GetGlobalProjectile<SpecialUseProj>().NPCProjOwner];

            float adjustedItemScale5 = 1;
            int protmp = Projectile.NewProjectile(null, Projectile.Center, new Vector2(dir, 0f), 983, Projectile.damage, Projectile.knockBack, Main.myPlayer, dir, NPCStats.GetModifiedAttackTime(owner), adjustedItemScale5);
            Main.projectile[protmp].npcProj = true;
            Main.projectile[protmp].noDropItem = true;
            protmp = Projectile.NewProjectile(null, Projectile.Center, new Vector2(dir, 0f), 982, 0, Projectile.knockBack, Main.myPlayer, dir, NPCStats.GetModifiedAttackTime(owner), adjustedItemScale5);
            Main.projectile[protmp].npcProj = true;
            Main.projectile[protmp].noDropItem = true;
        }

    }

}