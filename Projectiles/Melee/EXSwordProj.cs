using Microsoft.Xna.Framework;
using NPCAttacker.Systems;
using System;
using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class EXSwordProj : BaseAtkProj
    {
        public override int ItemType => ItemID.Excalibur;
        public override void AttackEffect()
        {
            int dir = Math.Sign(Projectile.velocity.X + (Main.rand.Next(2) * 2 - 1) * 0.01f);
            NPC owner = Main.npc[Projectile.GetGlobalProjectile<SpecialUseProj>().NPCProjOwner];
            float adjustedItemScale2 = 1;
            int protmp = Projectile.NewProjectile(null, Projectile.Center, new Vector2(dir, 0f), 982, Projectile.damage, Projectile.knockBack, Main.myPlayer, dir, NPCStats.GetModifiedAttackTime(owner), adjustedItemScale2);
            Main.projectile[protmp].npcProj = true;
            Main.projectile[protmp].noDropItem = true;
        }

    }

}