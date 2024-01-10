

using Microsoft.Xna.Framework;
using NPCAttacker.Buffs;
using NPCAttacker.NPCs;
using NPCAttacker.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static NPCAttacker.ArmedGNPC;

namespace NPCAttacker
{
    public static class UIUtils
    {
        public static bool CanUseMapDrawing()
        {
            return Main.mapStyle == 1 || Main.mapFullscreen;
        }

        public static void LeftClickForStick(Player player, Vector2 ClickPos)
        {
            if (!NPCUtils.AnyNPCSelected())   //选择目标
            {
                NPC.NewNPC(null, (int)ClickPos.X, (int)ClickPos.Y, ModContent.NPCType<UINPC>(), default, player.whoAmI);
            }
            else           //选中目标后
            {
                if (NPCAttacker.ForceAttackKey.Current)   //如果摁住了强制攻击键
                {
                    NPCUtils.ClearWarningLine();

                    int attackSource = NPC.NewNPC(null, (int)ClickPos.X, (int)ClickPos.Y, ModContent.NPCType<AttackTargetNPC>());
                    if (attackSource == -1) goto IL_02;
                    foreach (NPC npc in Main.npc)
                    {
                        if (npc.active)
                        {
                            if (npc.IsTownNPC())
                            {
                                if (npc.GetGlobalNPC<ArmedGNPC>().Selected)
                                {
                                    npc.GetGlobalNPC<ArmedGNPC>().actMode = ActMode.Attack;
                                    npc.GetGlobalNPC<ArmedGNPC>().ActTargetNPC = attackSource;
                                    int protmp = Projectile.NewProjectile(null, npc.Center, Vector2.Zero, ModContent.ProjectileType<WarningLine>(), 0, 0, Main.myPlayer, 1, npc.whoAmI);
                                    (Main.projectile[protmp].ModProjectile as WarningLine).EndTarget = attackSource;
                                }
                            }
                        }
                    }

                }
                else           //普通的选取攻击
                {
                    int StickTarget = -1;
                    int StickType = -1;
                    foreach (NPC npc in Main.npc)
                    {
                        if (npc.active)
                        {
                            if (npc.CanBeChasedBy() && npc.type != ModContent.NPCType<AssembleNPC>() && npc.type != ModContent.NPCType<AttackTargetNPC>())
                            {
                                if (npc.Hitbox.Contains(ClickPos.ToPoint()))
                                {
                                    StickTarget = npc.whoAmI;
                                    StickType = npc.type;
                                    break;
                                }
                            }
                        }
                    }

                    if (StickTarget != -1)   //攻击
                    {
                        NPCUtils.ClearWarningLine();

                        int attackSource = NPC.NewNPC(null, (int)ClickPos.X, (int)ClickPos.Y, ModContent.NPCType<AttackTargetNPC>());
                        if (attackSource == -1) goto IL_02;

                        (Main.npc[attackSource].ModNPC as AttackTargetNPC).StickToNPC = StickTarget;
                        (Main.npc[attackSource].ModNPC as AttackTargetNPC).StickToNPCType = StickType;


                        foreach (NPC npc in Main.npc)
                        {
                            if (npc.active)
                            {
                                if (npc.IsTownNPC())
                                {
                                    if (npc.GetGlobalNPC<ArmedGNPC>().Selected)
                                    {
                                        if (npc.type != NPCID.Nurse)
                                        {
                                            npc.GetGlobalNPC<ArmedGNPC>().actMode = ArmedGNPC.ActMode.Attack;
                                            npc.GetGlobalNPC<ArmedGNPC>().ActTargetNPC = attackSource;
                                            int protmp = Projectile.NewProjectile(null, npc.Center, Vector2.Zero, ModContent.ProjectileType<WarningLine>(), 0, 0, Main.myPlayer, 1, npc.whoAmI);
                                            (Main.projectile[protmp].ModProjectile as WarningLine).EndTarget = attackSource;
                                        }
                                        else
                                        {
                                            int protmp = Projectile.NewProjectile(null, npc.Center, Vector2.Zero, ModContent.ProjectileType<WarningLine>(), 0, 0, Main.myPlayer, 0, npc.whoAmI);
                                            (Main.projectile[protmp].ModProjectile as WarningLine).EndTarget = attackSource;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else          //前往
                    {
                        NPCUtils.ClearWarningLine();
                        bool kingStatue = false;
                        bool TPSuccessly = false;
                        if (ClickTPStatue(ClickPos, ref kingStatue))
                        {
                            foreach (NPC npc in Main.npc)
                            {
                                if (npc.active)
                                {
                                    if (npc.IsTownNPC())
                                    {
                                        if (npc.GetGlobalNPC<ArmedGNPC>().Selected)
                                        {
                                            npc.GetGlobalNPC<ArmedGNPC>().actMode = ActMode.Default;
                                            npc.GetGlobalNPC<ArmedGNPC>().ActTargetNPC = -1;
                                            npc.GetGlobalNPC<ArmedGNPC>().AlertMode = false;
                                            int protmp = Projectile.NewProjectile(null, npc.Center, Vector2.Zero, ModContent.ProjectileType<TPLine>(), 0, 0, Main.myPlayer);
                                            (Main.projectile[protmp].ModProjectile as TPLine).BeginPos = npc.Center;
                                            npc.Center = GetTPStatuePos(ClickPos);
                                            (Main.projectile[protmp].ModProjectile as TPLine).EndPos = npc.Center;
                                            TPSuccessly = true;
                                        }
                                    }
                                }
                            }
                            if (TPSuccessly)
                            {
                                int time = Main.hardMode ? 10 * 60 : 30 * 60;
                                player.AddBuff(ModContent.BuffType<TPCDBuff>(), time);
                            }
                            SoundEngine.PlaySound(new SoundStyle("NPCAttacker/Sounds/TP"));
                            player.SetTalkNPC(-1);
                            return;
                        }
                        else
                        {
                            int assembleSource = NPC.NewNPC(null, (int)ClickPos.X, (int)ClickPos.Y, ModContent.NPCType<AssembleNPC>());
                            if (assembleSource == -1) goto IL_02;
                            foreach (NPC npc in Main.npc)
                            {
                                if (npc.active)
                                {
                                    if (npc.IsTownNPC())
                                    {
                                        if (npc.GetGlobalNPC<ArmedGNPC>().Selected)
                                        {
                                            npc.GetGlobalNPC<ArmedGNPC>().actMode = ArmedGNPC.ActMode.Move;
                                            npc.GetGlobalNPC<ArmedGNPC>().ActTargetNPC = assembleSource;
                                            int protmp = Projectile.NewProjectile(null, npc.Center, Vector2.Zero, ModContent.ProjectileType<WarningLine>(), 0, 0, Main.myPlayer, 0, npc.whoAmI);
                                            (Main.projectile[protmp].ModProjectile as WarningLine).EndTarget = assembleSource;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }


        IL_02:

            SoundEngine.PlaySound(new SoundStyle("NPCAttacker/Sounds/Tab"));
            player.SetTalkNPC(-1);
        }


        public static void RightClick(Player player)
        {
            NPCUtils.ClearNPCSelect();
            SoundEngine.PlaySound(new SoundStyle("NPCAttacker/Sounds/Tab"));
            player.SetTalkNPC(-1);
        }


        public static bool ClickTPStatue(Vector2 MousePos, ref bool IsKingStatue)
        {
            if (Main.LocalPlayer.HasBuff(ModContent.BuffType<TPCDBuff>())) return false;
            int TileX = (int)(MousePos.X / 16f);
            int TileY = (int)(MousePos.Y / 16f);
            if (Main.tile[TileX, TileY] == null) return false;
            if (!Main.tile[TileX, TileY].HasTile) return false;
            if (Main.tile[TileX, TileY].TileType != TileID.Statues) return false;
            int TileFrameX = Main.tile[TileX, TileY].TileFrameX;
            //int TileFrameY = Main.tile[TileX, TileY].TileFrameY;
            if (TileFrameX >= 1476 && TileFrameX <= 1494)//&& TileFrameY >= 0 && TileFrameY <= 36)
            {
                IsKingStatue = false;
                return true;
            }
            else if (TileFrameX >= 1440 && TileFrameX <= 1458) // && TileFrameY >= 162 && TileFrameY <= 198)
            {
                IsKingStatue = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Vector2 GetTPStatuePos(Vector2 MousePos)
        {
            bool king = false;
            if (!ClickTPStatue(MousePos, ref king)) return MousePos;
            int TileX = (int)(MousePos.X / 16f);
            int TileY = (int)(MousePos.Y / 16f);
            int TileFrameX = Main.tile[TileX, TileY].TileFrameX;
            int TileFrameY = Main.tile[TileX, TileY].TileFrameY;
            if (king)
            {
                TileX -= (TileFrameX - 1440) / 18;
            }
            else
            {
                TileX -= (TileFrameX - 1476) / 18;

            }
            if (TileFrameY >= 162)
            {
                TileY -= (TileFrameY - 162) / 18;
            }
            else
            {
                TileY -= (TileFrameY - 0) / 18;
            }
            return new Vector2((TileX + 1) * 16f, (TileY + 1.5f) * 16f);
        }

    }
}
