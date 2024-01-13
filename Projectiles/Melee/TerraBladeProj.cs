using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace NPCAttacker.Projectiles.Melee
{
    public class TerraBladeProj : BaseAtkProj
    {
        public override int ItemType => ItemID.TerraBlade;
        public override void AttackEffect()
        {
            int dir = Math.Sign(Projectile.velocity.X + (Main.rand.Next(2) * 2 - 1) * 0.01f);
            NPC owner = Main.npc[GetOwner()];

            float adjustedItemScale4 = 1;
            int protmp = Projectile.NewProjectile(null, Projectile.Center, new Vector2(dir, 0f), 984, Projectile.damage, Projectile.knockBack, Main.myPlayer, dir, NPCStats.GetModifiedAttackTime(owner), adjustedItemScale4);
            Main.projectile[protmp].npcProj = true;
            Main.projectile[protmp].noDropItem = true;
            protmp = Projectile.NewProjectile(null, Projectile.Center, Vector2.Normalize(Projectile.velocity) * 50f, 985, Projectile.damage, Projectile.knockBack, Main.myPlayer, dir, 18f, adjustedItemScale4);
            Main.projectile[protmp].npcProj = true;
            Main.projectile[protmp].noDropItem = true;
        }
    }
}
