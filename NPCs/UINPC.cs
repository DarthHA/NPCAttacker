using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NPCAttacker.Items;
using NPCAttacker.Projectiles;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NPCAttacker.NPCs
{
    public class UINPC : ModNPC
    {
        public Vector2 EndPos = Vector2.Zero;
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

            Player owner = Main.player[(int)NPC.ai[0]];
            if (!owner.active || owner.dead || owner.ghost)
            {
                NPC.active = false;
                return;
            }
            if (owner.HeldItem.type != ModContent.ItemType<AttackerStick>())
            {
                NPC.active = false;
                return;
            }
            if (owner.CCed)
            {
                NPC.active = false;
                return;
            }
            if (owner.Distance(NPC.Center) > 3000)
            {
                NPC.active = false;
                return;
            }

            if (NPCAttacker.CurserInMap)
            {
                NPC.active = false;
                return;
            }

            if (!owner.channel)
            {
                NPC.active = false;
                NPCUtils.ClearNPCSelect();
                NPCUtils.ClearWarningLine();
                foreach (NPC npc in Main.npc)
                {
                    if (npc.active)
                    {
                        if (npc.IsTownNPC())
                        {
                            if (GetRectangle().Intersects(npc.Hitbox))
                            {
                                npc.GetGlobalNPC<ArmedGNPC>().Selected = true;
                                NPCUtils.SwitchMode(npc);
                                if (npc.GetGlobalNPC<ArmedGNPC>().actMode != ArmedGNPC.ActMode.Default)
                                {
                                    int color = npc.GetGlobalNPC<ArmedGNPC>().actMode == ArmedGNPC.ActMode.Attack ? 1 : 0;
                                    int protmp = Projectile.NewProjectile(null, npc.Center, Vector2.Zero, ModContent.ProjectileType<WarningLine>(), 0, 0, Main.myPlayer, color, npc.whoAmI);
                                    (Main.projectile[protmp].ModProjectile as WarningLine).EndTarget = npc.GetGlobalNPC<ArmedGNPC>().ActTargetNPC;
                                }
                            }
                        }
                    }
                }

                return;
            }
            EndPos = Main.MouseWorld;

            owner.itemAnimation = 2;
            owner.itemTime = 2;
            Vector2 unit = Main.MouseWorld - owner.Center;
            owner.direction = Math.Sign(Main.MouseWorld.X - owner.Center.X);
            int dir = owner.direction;
            owner.itemRotation = (float)Math.Atan2(unit.Y * dir, unit.X * dir);
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
            Utils.DrawRectangle(spriteBatch, GetRectangle().TopLeft(), GetRectangle().BottomRight(), Color.White, Color.White, 2);
            return false;
        }

        private Rectangle GetRectangle()
        {
            if (EndPos == Vector2.Zero)
            {
                return new Rectangle((int)Main.MouseWorld.X, (int)Main.MouseWorld.Y, 0, 0);
            }
            return new Rectangle(
                (int)Math.Min(EndPos.X, NPC.Center.X),
                (int)Math.Min(EndPos.Y, NPC.Center.Y),
                (int)Math.Abs(EndPos.X - NPC.Center.X),
                (int)Math.Abs(EndPos.Y - NPC.Center.Y)
                );
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