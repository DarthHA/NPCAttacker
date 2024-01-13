using Microsoft.Xna.Framework;
using NPCAttacker.Buffs;
using NPCAttacker.Items;
using NPCAttacker.NPCs;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace NPCAttacker.Systems
{
    public class CommandPlayer : ModPlayer
    {
        public int FNPC = -1;
        public int FNPCType = -1;

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Player.HeldItem.type != ModContent.ItemType<AttackerStick>()) return;
            if (NPCAttacker.Team1Key.JustPressed)
            {
                TeamProcess(1);
                SoundEngine.PlaySound(new SoundStyle("NPCAttacker/Sounds/Tab"));
            }
            else if (NPCAttacker.Team2Key.JustPressed)
            {
                TeamProcess(2);
                SoundEngine.PlaySound(new SoundStyle("NPCAttacker/Sounds/Tab"));
            }
            else if (NPCAttacker.Team3Key.JustPressed)
            {
                TeamProcess(3);
                SoundEngine.PlaySound(new SoundStyle("NPCAttacker/Sounds/Tab"));
            }
            else if (NPCAttacker.Team4Key.JustPressed)
            {
                TeamProcess(4);
                SoundEngine.PlaySound(new SoundStyle("NPCAttacker/Sounds/Tab"));
            }
            else if (NPCAttacker.F2AKey.JustPressed)
            {
                Rectangle ScreenSize;

                if (!Main.mapFullscreen)
                {
                    ScreenSize = new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight);
                }
                else
                {
                    Vector2 MapTopLeft = BigMapToWorldPos(Vector2.Zero);
                    Vector2 MapBottomRight = BigMapToWorldPos(new Vector2(Main.screenWidth, Main.screenHeight));
                    ScreenSize = new((int)MapTopLeft.X, (int)MapTopLeft.Y, (int)(MapBottomRight.X - MapTopLeft.X), (int)(MapBottomRight.Y - MapTopLeft.Y));
                }

                bool AllInScreen = true;
                foreach (NPC npc in Main.npc)
                {
                    if (npc.active)
                    {
                        if (npc.IsTownNPC())
                        {
                            if (ScreenSize.Intersects(npc.Hitbox) && !npc.GetGlobalNPC<ArmedGNPC>().Selected)
                            {
                                AllInScreen = false;
                            }
                        }
                    }
                }

                if (AllInScreen)
                {
                    foreach (NPC npc in Main.npc)
                    {
                        if (npc.active)
                        {
                            if (npc.IsTownNPC())
                            {
                                npc.GetGlobalNPC<ArmedGNPC>().Selected = true;
                            }
                        }
                    }
                }
                else
                {
                    foreach (NPC npc in Main.npc)
                    {
                        if (npc.active)
                        {
                            if (npc.IsTownNPC() && ScreenSize.Intersects(npc.Hitbox))
                            {
                                npc.GetGlobalNPC<ArmedGNPC>().Selected = true;
                            }
                        }
                    }
                }


                SoundEngine.PlaySound(new SoundStyle("NPCAttacker/Sounds/Tab"));
            }
            else if (NPCAttacker.F2A2Key.JustPressed)
            {
                SoundEngine.PlaySound(new SoundStyle("NPCAttacker/Sounds/Tab"));
                List<int> AttackType = new();
                foreach (NPC npc in Main.npc)
                {
                    if (npc.active)
                    {
                        if (npc.IsTownNPC())
                        {
                            if (npc.GetGlobalNPC<ArmedGNPC>().Selected && npc.type != NPCID.Nurse && npc.type != NPCID.Dryad)
                            {
                                if (!AttackType.Contains(NPCID.Sets.AttackType[npc.type]))
                                {
                                    AttackType.Add(NPCID.Sets.AttackType[npc.type]);
                                }
                            }
                        }
                    }
                }
                if (AttackType.Count <= 0) return;

                Rectangle ScreenSize;

                if (!Main.mapFullscreen)
                {
                    ScreenSize = new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight);
                }
                else
                {
                    Vector2 MapTopLeft = BigMapToWorldPos(Vector2.Zero);
                    Vector2 MapBottomRight = BigMapToWorldPos(new Vector2(Main.screenWidth, Main.screenHeight));
                    ScreenSize = new((int)MapTopLeft.X, (int)MapTopLeft.Y, (int)(MapBottomRight.X - MapTopLeft.X), (int)(MapBottomRight.Y - MapTopLeft.Y));
                }

                bool AllInScreen = true;
                foreach (NPC npc in Main.npc)
                {
                    if (npc.active)
                    {
                        if (npc.IsTownNPC())
                        {
                            if (AttackType.Contains(NPCID.Sets.AttackType[npc.type]) && npc.type != NPCID.Nurse && npc.type != NPCID.Dryad)
                            {
                                if (ScreenSize.Intersects(npc.Hitbox) && !npc.GetGlobalNPC<ArmedGNPC>().Selected)
                                {
                                    AllInScreen = false;
                                }
                            }
                        }
                    }
                }

                if (AllInScreen)
                {
                    foreach (NPC npc in Main.npc)
                    {
                        if (npc.active)
                        {
                            if (npc.IsTownNPC() && npc.type != NPCID.Nurse && npc.type != NPCID.Dryad)
                            {
                                if (AttackType.Contains(NPCID.Sets.AttackType[npc.type]))
                                {
                                    npc.GetGlobalNPC<ArmedGNPC>().Selected = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (NPC npc in Main.npc)
                    {
                        if (npc.active)
                        {
                            if (npc.IsTownNPC() && ScreenSize.Intersects(npc.Hitbox) && npc.type != NPCID.Nurse && npc.type != NPCID.Dryad)
                            {
                                if (AttackType.Contains(NPCID.Sets.AttackType[npc.type]))
                                {
                                    npc.GetGlobalNPC<ArmedGNPC>().Selected = true;
                                }
                            }
                        }
                    }
                }


                SoundEngine.PlaySound(new SoundStyle("NPCAttacker/Sounds/Tab"));
            }
            else if (NPCAttacker.AlertKey.JustPressed)
            {
                foreach (NPC npc in Main.npc)
                {
                    if (npc.active)
                    {
                        if (npc.IsTownNPC())
                        {
                            if (npc.GetGlobalNPC<ArmedGNPC>().Selected)
                            {
                                npc.GetGlobalNPC<ArmedGNPC>().AlertMode = true;
                                npc.GetGlobalNPC<ArmedGNPC>().actMode = ArmedGNPC.ActMode.Default;
                                npc.GetGlobalNPC<ArmedGNPC>().ActTargetNPC = -1;
                            }
                        }
                    }
                }
                SoundEngine.PlaySound(new SoundStyle("NPCAttacker/Sounds/Command"));
            }
            else if (NPCAttacker.DisperseKey.JustPressed)
            {
                int count = 30;
                foreach (NPC npc in Main.npc)
                {
                    if (npc.active)
                    {
                        if (npc.IsTownNPC())
                        {
                            if (npc.GetGlobalNPC<ArmedGNPC>().Selected)
                            {
                                npc.GetGlobalNPC<ArmedGNPC>().actMode = ArmedGNPC.ActMode.Move;

                                Vector2 SpreadPos;
                                int tryTime = 0;
                                while (true)
                                {
                                    SpreadPos = new(npc.position.X + Main.rand.NextFloat() * 400 - 200, npc.position.Y);

                                    tryTime++;
                                    if (Collision.CanHitLine(npc.position, npc.width, npc.height, SpreadPos, npc.width, npc.height))
                                    {
                                        break;
                                    }
                                    if (tryTime > 10)
                                    {
                                        SpreadPos = npc.Center;
                                        break;
                                    }

                                }
                                int SpreadSource = NPC.NewNPC(null, (int)SpreadPos.X + npc.width / 2, (int)SpreadPos.Y + npc.height / 2, ModContent.NPCType<AssembleNPC>());
                                if (SpreadSource == -1) break;
                                (Main.npc[SpreadSource].ModNPC as AssembleNPC).TimeLeft = 60 * 10;
                                npc.GetGlobalNPC<ArmedGNPC>().ActTargetNPC = SpreadSource;
                                count--;
                                if (count < 0) break;
                            }
                        }
                    }
                }
                SoundEngine.PlaySound(new SoundStyle("NPCAttacker/Sounds/Command"));
            }
            else if (NPCAttacker.StopKey.JustPressed)
            {
                foreach (NPC npc in Main.npc)
                {
                    if (npc.active)
                    {
                        if (npc.IsTownNPC())
                        {
                            if (npc.GetGlobalNPC<ArmedGNPC>().Selected)
                            {
                                npc.GetGlobalNPC<ArmedGNPC>().actMode = ArmedGNPC.ActMode.Default;
                                npc.GetGlobalNPC<ArmedGNPC>().ActTargetNPC = -1;
                                npc.GetGlobalNPC<ArmedGNPC>().AlertMode = false;
                                npc.velocity.X = 0;
                            }
                        }
                    }
                }
                SoundEngine.PlaySound(new SoundStyle("NPCAttacker/Sounds/Command"));
            }
            else if (NPCAttacker.FKey.JustPressed)
            {

                bool Selected = false;

                foreach (NPC npc in Main.npc)
                {
                    if (npc.active)
                    {
                        if (npc.IsTownNPC())
                        {
                            if (npc.GetGlobalNPC<ArmedGNPC>().Selected)
                            {
                                Selected = true;
                                FNPC = npc.whoAmI;
                                FNPCType = npc.type;
                                break;
                            }
                        }
                    }
                }
                if (!Selected)
                {
                    FNPC = -1;
                    FNPCType = -1;
                }

                SoundEngine.PlaySound(new SoundStyle("NPCAttacker/Sounds/Tab"));
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (Player.HeldItem.type != ModContent.ItemType<AttackerStick>())
            {
                NPCUtils.ClearNPCSelect();
            }

            if (Player.HasItem(ModContent.ItemType<AttackerStick>()))
            {
                Player.AddBuff(ModContent.BuffType<CommandBuff>(), 2);
            }
        }

        public override void ModifyScreenPosition()
        {
            if (FNPC != -1)
            {
                if (Main.npc[FNPC].active && Main.npc[FNPC].type == FNPCType)
                {
                    Main.screenPosition = Main.npc[FNPC].Center - new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
                }
                else
                {
                    FNPC = -1;
                    FNPCType = -1;
                }
            }

        }

        public override void ModifyLuck(ref float luck)
        {
            foreach (NPC npc in Main.npc)
            {
                if (npc.active)
                {
                    if (npc.HasBuff(BuffID.Lucky) && npc.Distance(Player.Center) <= 2000)
                    {
                        if (npc.buffTime[npc.FindBuffIndex(BuffID.Lucky)] > 18000)
                        {
                            Main.LocalPlayer.luck += 0.3f;
                        }
                        else if (npc.buffTime[npc.FindBuffIndex(BuffID.Lucky)] > 10800)
                        {
                            Main.LocalPlayer.luck += 0.2f;
                        }
                        else
                        {
                            Main.LocalPlayer.luck += 0.1f;
                        }
                    }
                }
            }
        }

        private void TeamProcess(int team)
        {
            if (NPCAttacker.ForceAttackKey.Current)         //选队
            {
                if (NPCUtils.AnyNPCSelected())
                {
                    bool AllIsThisTeam = true;  //选中的人是否全是队伍里的，如果是，那就把这些人剔除，如果不是，就全入队
                    foreach (NPC npc in Main.npc)
                    {
                        if (npc.active)
                        {
                            if (npc.IsTownNPC())
                            {
                                if (npc.GetGlobalNPC<ArmedGNPC>().Selected && npc.GetGlobalNPC<ArmedGNPC>().Team != team)
                                {
                                    AllIsThisTeam = false;
                                }
                            }
                        }
                    }

                    if (AllIsThisTeam)
                    {
                        foreach (NPC npc in Main.npc)
                        {
                            if (npc.active)
                            {
                                if (npc.IsTownNPC())
                                {
                                    if (npc.GetGlobalNPC<ArmedGNPC>().Selected)
                                    {
                                        npc.GetGlobalNPC<ArmedGNPC>().Team = 0;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (NPC npc in Main.npc)
                        {
                            if (npc.active)
                            {
                                if (npc.IsTownNPC())
                                {
                                    if (npc.GetGlobalNPC<ArmedGNPC>().Selected)
                                    {
                                        npc.GetGlobalNPC<ArmedGNPC>().Team = team;
                                    }
                                }
                            }
                        }
                    }
                }

            }
            else
            {
                NPCUtils.ClearNPCSelect();
                foreach (NPC npc in Main.npc)
                {
                    if (npc.active)
                    {
                        if (npc.IsTownNPC())
                        {
                            if (npc.GetGlobalNPC<ArmedGNPC>().Team == team)
                            {
                                npc.GetGlobalNPC<ArmedGNPC>().Selected = true;
                            }
                        }
                    }
                }
            }
        }


        private Vector2 BigMapToWorldPos(Vector2 MapPos)
        {
            Vector2 cursorPosition = MapPos;
            cursorPosition.X -= Main.screenWidth / 2f;
            cursorPosition.Y -= Main.screenHeight / 2f;
            cursorPosition /= 16f;
            cursorPosition *= 16f / Main.mapFullscreenScale;
            Vector2 WorldPos = Main.mapFullscreenPos + cursorPosition;
            WorldPos *= 16f;
            return WorldPos;
        }
    }
}