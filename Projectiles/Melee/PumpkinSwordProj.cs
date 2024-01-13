using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace NPCAttacker.Projectiles.Melee
{
    public class PumpkinSwordProj : BaseAtkProj
    {
        public override int ItemType => 1826;
        public override void AttackEffect()
        {
            int dir = Math.Sign(Projectile.velocity.X + (Main.rand.Next(2) * 2 - 1) * 0.01f);
            NPC owner = Main.npc[GetOwner()];
            float adjustedItemScale3 = 1f;
            int protmp = Projectile.NewProjectile(null, Projectile.Center, new Vector2(dir, 0f), 997, Projectile.damage, Projectile.knockBack, Main.myPlayer, dir, NPCStats.GetModifiedAttackTime(owner), adjustedItemScale3);
            Main.projectile[protmp].npcProj = true;
            Main.projectile[protmp].noDropItem = true;
        }
    }
}
