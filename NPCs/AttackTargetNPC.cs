using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NPCAttacker.NPCs
{
    public class AttackTargetNPC : ModNPC
    {
        public int StickToNPC = -1;
        public int StickToNPCType = -1;
        public override void SetStaticDefaults()
        {
            NPCID.Sets.ImmuneToAllBuffs[NPC.type] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 0;
            NPC.height = 0;
            NPC.damage = 0;
            NPC.lifeMax = 1;
            NPC.dontCountMe = true;
            NPC.dontTakeDamage = true;
            NPC.dontTakeDamageFromHostiles = true;
            NPC.friendly = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0f;
            NPC.lavaImmune = true;
            for (int i = 0; i < NPC.buffImmune.Length; i++)
            {
                NPC.buffImmune[i] = true;
            }
            NPC.aiStyle = -1;
            NPC.chaseable = false;
            NPC.hide = true;
        }

        public override void AI()
        {
            if (NPC.buffTime[0] != 0)
            {
                NPC.buffImmune[NPC.buffType[0]] = true;
                NPC.DelBuff(0);
            }
            if (NPC.timeLeft < 10)
            {
                NPC.timeLeft = 10;
            }

            if (!NPCUtils.BuffNPC())
            {
                NPC.active = false;
                return;
            }

            bool AnyLink = false;
            foreach (NPC npc in Main.npc)
            {
                if (npc.active)
                {
                    if (npc.IsTownNPC())
                    {
                        if (npc.GetGlobalNPC<ArmedGNPC>().ActTargetNPC == NPC.whoAmI)
                        {
                            AnyLink = true;
                        }
                    }
                }
            }

            if (!AnyLink)
            {
                NPC.active = false;
                return;
            }

            if (StickToNPC != -1)
            {
                if (!Main.npc[StickToNPC].active || Main.npc[StickToNPC].type != StickToNPCType)
                {
                    NPC.active = false;
                    return;
                }
                NPC.Center = Main.npc[StickToNPC].Center;
            }



        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
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


        public override bool CheckDead()
        {
            return false;
        }
        public override bool CheckActive()
        {
            return false;
        }
        public override bool CanHitNPC(NPC target)/* tModPorter Suggestion: Return true instead of null */
        {
            return false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            //if (StickToNPC == -1) return false;
            //Texture2D tex = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
            //NPC.ai[1] = (NPC.ai[1] + 1) % 60;
            //float scale = 1f + 0.2f * (float)Math.Sin(NPC.ai[1] / 60 * MathHelper.Pi);
            //spriteBatch.Draw(tex, NPC.Center - Main.screenPosition, null, Color.White, 0, tex.Size() / 2, scale * 0.5f, SpriteEffects.None, 0);
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