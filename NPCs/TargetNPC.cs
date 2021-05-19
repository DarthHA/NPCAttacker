using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System;
using NPCAttacker.Items;
using NPCAttacker.Buffs;

namespace NPCAttacker.NPCs
{
    public class TargetNPC : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("");
        }

        public override void SetDefaults()
        {
            npc.width = 0;
            npc.height = 0;
            npc.damage = 0;
            npc.lifeMax = 1;
            npc.dontCountMe = true;
            npc.dontTakeDamage = true;
            npc.dontTakeDamageFromHostiles = true;
            npc.friendly = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.knockBackResist = 0f;
            npc.lavaImmune = true;
            for(int i = 0; i < npc.buffImmune.Length; i++)
            {
                npc.buffImmune[i] = true;
            }
            npc.aiStyle = -1;
            npc.chaseable = false;
            npc.hide = true;
        }

        public override void AI()
        {
            if (npc.buffTime[0] != 0)
            {
                npc.buffImmune[npc.buffType[0]] = true;
                npc.DelBuff(0);
            }
            if (npc.timeLeft < 10)
            {
                npc.timeLeft = 10;
            }
            Player owner = Main.player[(int)npc.ai[0]];
            if (!owner.active || owner.dead || owner.ghost)
            {
                npc.active = false;
                return;
            }
            if (owner.HeldItem.type != ModContent.ItemType<AttackerStick>())
            {
                npc.active = false;
                return;
            }
            if (!owner.channel || owner.CCed)
            {
                npc.active = false;
                return;
            }

            npc.Center = Main.MouseWorld;
            owner.itemAnimation = 2;
            owner.itemTime = 2;
            Vector2 unit = Main.MouseWorld - owner.Center;
            owner.direction = Math.Sign(Main.MouseWorld.X - owner.Center.X);
            int dir = owner.direction;
            owner.itemRotation = (float)Math.Atan2(unit.Y * dir, unit.X * dir);
            owner.AddBuff(ModContent.BuffType<AttackBuff>(), 2);

        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false;
        }
        public override bool PreNPCLoot()
        {
            return false;
        }
        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return false;
        }
        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return false;
        }
        public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            return false;
        }

        public override bool CheckDead()
        {
            return false;
        }
        public override bool CheckActive()
        {
            return false;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D tex = Main.npcTexture[npc.type];
            npc.ai[1] = (npc.ai[1] + 1) % 60;
            float scale = 1f + 0.2f * (float)Math.Sin(npc.ai[1] / 60 * MathHelper.Pi);
            spriteBatch.Draw(tex, npc.Center - Main.screenPosition, null, Color.White, 0, tex.Size() / 2, scale, SpriteEffects.None, 0);
            return false;
        }
        public override void DrawBehind(int index)
        {
            Main.instance.DrawCacheNPCsOverPlayers.Add(index);
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }
    }
}