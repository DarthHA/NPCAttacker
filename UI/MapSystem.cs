using Microsoft.Xna.Framework;
using NPCAttacker.Buffs;
using NPCAttacker.Items;
using NPCAttacker.NPCs;
using NPCAttacker.Projectiles;
using System;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Map;
using Terraria.ModLoader;

namespace NPCAttacker
{
    public class MapSystem : ModMapLayer
    {
        private bool leftMousePress = false;
        private bool rightMousePress = false;
        public static bool TargetSelected = false;
        public static bool StatueSelected = false;


        public override void Draw(ref MapOverlayDrawContext context, ref string text)
        {
            Type type = context.GetType();
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
            FieldInfo fieldInfo = type.GetField("_mapPosition", flags);
            Vector2 _mapPosition = (Vector2)fieldInfo.GetValue(context);   //小地图切片世界坐标

            fieldInfo = type.GetField("_clippingRect", flags);        //小地图切片世界大小，如果是大地图则为null
            Rectangle? _clippingRect = (Rectangle?)fieldInfo.GetValue(context);

            //fieldInfo = type.GetField("_mapScale", flags);
            //float _mapScale = (float)fieldInfo.GetValue(context);

            //fieldInfo = type.GetField("_mapOffset", flags);
            //Vector2 _mapOffset = (Vector2)fieldInfo.GetValue(context);

            //fieldInfo = type.GetField("_drawScale", flags);
            //float _drawScale = (float)fieldInfo.GetValue(context);




            Vector2 MousePos = new(Main.mouseX, Main.mouseY);
            Vector2 WorldPos;

            int mapWidth = Main.maxTilesX * 16;
            int mapHeight = Main.maxTilesY * 16;

            if (_clippingRect != null)
            {
                Vector2 MapCenter = Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
                float MapWorldWidth = (MapCenter.X - _mapPosition.X * 16) * 2;
                float MapWidth = _clippingRect.Value.Width;
                Vector2 RelaPos = MousePos - _clippingRect.Value.TopLeft();
                WorldPos = _mapPosition * 16f + RelaPos * MapWorldWidth / MapWidth;
            }
            else
            {
                Vector2 cursorPosition = MousePos;
                cursorPosition.X -= Main.screenWidth / 2f;
                cursorPosition.Y -= Main.screenHeight / 2f;
                cursorPosition /= 16f;
                cursorPosition *= 16f / Main.mapFullscreenScale;
                WorldPos = Main.mapFullscreenPos + cursorPosition;
                WorldPos *= 16f;

            }
            if (WorldPos.X < 0f)
            {
                WorldPos.X = 0f;
            }
            else if (WorldPos.X > mapWidth)
            {
                WorldPos.X = mapWidth;
            }
            if (WorldPos.Y < 0f)
            {
                WorldPos.Y = 0f;
            }
            else if (WorldPos.Y > mapHeight)
            {
                WorldPos.Y = mapHeight;
            }

            //position = (position - _mapPosition) * _mapScale + _mapOffset;

            if (_clippingRect != null && !_clippingRect.Value.Contains(MousePos.ToPoint()))                //不在地图内操作
            {
                NPCAttacker.CurserInMap = false;
                TargetSelected = false;
                StatueSelected = false;

                foreach (NPC npc in Main.npc)             //小地图绘制血条
                {
                    if (npc.active)
                    {
                        if (npc.IsTownNPC())
                        {
                            if (npc.GetGlobalNPC<ArmedGNPC>().Selected)
                            {
                                Vector2 Pos = ToSmallMapPos(npc.Center, _clippingRect.Value, _mapPosition) + Main.screenPosition;
                                if (_clippingRect.Value.Contains((Pos - Main.screenPosition).ToPoint()))
                                {
                                    float l = npc.life / (float)npc.lifeMax;
                                    Pos += new Vector2(-10, 7);
                                    Color color = Color.Lerp(Color.Red, Color.LawnGreen, l);
                                    Utils.DrawLine(Main.spriteBatch, Pos, Pos + new Vector2(20, 0) * l, color, color, 4);
                                }
                            }
                        }
                    }
                }

                return;
            }
            else
            {
                NPCAttacker.CurserInMap = true;
            }

            if (!UIUtils.CanUseMapDrawing())
            {
                StatueSelected = false;
                TargetSelected = false;
                NPCAttacker.CurserInMap = false;
                return;
            }

            if (Main.mouseLeft)
            {
                if (!leftMousePress)
                {
                    if (Main.LocalPlayer.active && !Main.LocalPlayer.dead && !Main.LocalPlayer.ghost && !Main.LocalPlayer.CCed)
                    {
                        LeftClick(WorldPos, _clippingRect != null);
                    }
                    leftMousePress = true;
                }
            }
            else
            {
                leftMousePress = false;
            }

            if (Main.mouseRight)
            {
                if (!rightMousePress)
                {
                    if (Main.LocalPlayer.active && !Main.LocalPlayer.dead && !Main.LocalPlayer.ghost && !Main.LocalPlayer.CCed)
                    {
                        RightClick(WorldPos, _clippingRect != null);
                    }
                    rightMousePress = true;
                }
            }
            else
            {
                rightMousePress = false;
            }

            UpdateSelectedTarget(WorldPos);
            UpdateSelectedStatue(WorldPos);



            if (_clippingRect == null)   //大地图绘制路线
            {
                foreach (Projectile proj in Main.projectile)
                {
                    if (proj.active)
                    {
                        if (proj.type == ModContent.ProjectileType<WarningLine>())
                        {
                            Vector2 vec1 = Main.npc[(int)proj.ai[1]].Center;
                            Vector2 vec2 = Main.npc[(proj.ModProjectile as WarningLine).EndTarget].Center;
                            vec1 = ToBigMapPos(vec1);
                            vec2 = ToBigMapPos(vec2);
                            Color color = proj.ai[0] == 0 ? Color.LawnGreen : Color.Red;
                            Utils.DrawLine(Main.spriteBatch, vec1 + Main.screenPosition, vec2 + Main.screenPosition, color, color, 1);
                        }
                        if (proj.type == ModContent.ProjectileType<TPLine>())
                        {
                            Vector2 vec1 = (proj.ModProjectile as TPLine).BeginPos;
                            Vector2 vec2 = (proj.ModProjectile as TPLine).EndPos;
                            vec1 = ToBigMapPos(vec1);
                            vec2 = ToBigMapPos(vec2);
                            Color color = Color.Cyan;
                            Utils.DrawLine(Main.spriteBatch, vec1 + Main.screenPosition, vec2 + Main.screenPosition, color, color, 1);
                        }
                    }
                }

                foreach (NPC npc in Main.npc)             //大地图绘制血条
                {
                    if (npc.active)
                    {
                        if (npc.IsTownNPC())
                        {
                            if (npc.GetGlobalNPC<ArmedGNPC>().Selected)
                            {
                                Vector2 Pos = ToBigMapPos(npc.Center) + Main.screenPosition;
                                float l = npc.life / (float)npc.lifeMax;
                                Pos += new Vector2(-10, 15);
                                Color color = Color.Lerp(Color.Red, Color.LawnGreen, l);
                                Utils.DrawLine(Main.spriteBatch, Pos, Pos + new Vector2(20, 0) * l, color, color, 4);
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (NPC npc in Main.npc)             //小地图绘制血条
                {
                    if (npc.active)
                    {
                        if (npc.IsTownNPC())
                        {
                            if (npc.GetGlobalNPC<ArmedGNPC>().Selected)
                            {
                                Vector2 Pos = ToSmallMapPos(npc.Center, _clippingRect.Value, _mapPosition) + Main.screenPosition;
                                if (_clippingRect.Value.Contains((Pos - Main.screenPosition).ToPoint()))
                                {
                                    float l = npc.life / (float)npc.lifeMax;
                                    Pos += new Vector2(-10, 7);
                                    Color color = Color.Lerp(Color.Red, Color.LawnGreen, l);
                                    Utils.DrawLine(Main.spriteBatch, Pos, Pos + new Vector2(20, 0) * l, color, color, 4);
                                }
                            }
                        }
                    }
                }
            }



        }

        public void UpdateSelectedTarget(Vector2 WorldPos)
        {
            foreach (NPC npc in Main.npc)
            {
                if (npc.active)
                {
                    if (npc.CanBeChasedBy() && npc.type != ModContent.NPCType<AssembleNPC>() && npc.type != ModContent.NPCType<AttackTargetNPC>())
                    {
                        if (npc.Distance(WorldPos) < 150f)
                        {
                            TargetSelected = true;
                            return;
                        }
                    }
                }
            }
            TargetSelected = false;
        }

        public void UpdateSelectedStatue(Vector2 WorldPos)
        {
            bool b = false;
            if (UIUtils.ClickTPStatue(WorldPos, ref b))
            {
                StatueSelected = true;
            }
            else
            {
                StatueSelected = false;
            }
        }

        private Vector2 ToBigMapPos(Vector2 WorldPos)
        {
            Vector2 result;

            result = WorldPos /= 16f;
            result -= Main.mapFullscreenPos;
            result *= Main.mapFullscreenScale;
            result += new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
            return result;
        }

        private Vector2 ToSmallMapPos(Vector2 WorldPos, Rectangle _clippingRect, Vector2 _mapPosition)
        {
            Vector2 result;

            Vector2 MapCenter = Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
            float MapWorldWidth = (MapCenter.X - _mapPosition.X * 16) * 2;
            float MapWidth = _clippingRect.Width;

            result = WorldPos - _mapPosition * 16;
            result /= MapWorldWidth / MapWidth;
            result += _clippingRect.TopLeft();
            return result;
        }

        public void LeftClick(Vector2 WorldPos, bool FullMap)
        {
            if (Main.LocalPlayer.HeldItem.type != ModContent.ItemType<AttackerStick>() || !UIUtils.CanUseMapDrawing()) return;

            if (NPCUtils.AnyNPCSelected())    //如果有人被选中
            {
                if (NPCAttacker.ForceAttackKey.Current)   //如果摁住了强制攻击键
                {
                    NPCUtils.ClearWarningLine();
                    int attackSource = NPC.NewNPC(null, (int)WorldPos.X, (int)WorldPos.Y, ModContent.NPCType<AttackTargetNPC>());
                    if (attackSource == -1) goto IL_02;
                    foreach (NPC npc in Main.npc)
                    {
                        if (npc.active)
                        {
                            if (npc.IsTownNPC())
                            {
                                if (npc.GetGlobalNPC<ArmedGNPC>().Selected)
                                {
                                    npc.GetGlobalNPC<ArmedGNPC>().actMode = ArmedGNPC.ActMode.Attack;
                                    npc.GetGlobalNPC<ArmedGNPC>().ActTargetNPC = attackSource;
                                    int protmp = Projectile.NewProjectile(null, npc.Center, Vector2.Zero, ModContent.ProjectileType<WarningLine>(), 0, 0, Main.myPlayer, 1, npc.whoAmI);
                                    (Main.projectile[protmp].ModProjectile as WarningLine).EndTarget = attackSource;
                                }
                            }
                        }
                    }

                }
                else
                {
                    int StickTarget = -1;
                    int StickType = -1;
                    float StickRange = 150;
                    foreach (NPC npc in Main.npc)
                    {
                        if (npc.active)
                        {
                            if (npc.CanBeChasedBy() && npc.type != ModContent.NPCType<AssembleNPC>() && npc.type != ModContent.NPCType<AttackTargetNPC>())
                            {
                                if (npc.Distance(WorldPos) < 150f)
                                {
                                    if (npc.Distance(WorldPos) < StickRange)
                                    {
                                        StickTarget = npc.whoAmI;
                                        StickType = npc.type;
                                        StickRange = npc.Distance(WorldPos);
                                    }
                                }
                            }
                        }
                    }

                    if (StickTarget != -1)   //攻击
                    {
                        NPCUtils.ClearWarningLine();
                        int attackSource = NPC.NewNPC(null, (int)WorldPos.X, (int)WorldPos.Y, ModContent.NPCType<AttackTargetNPC>());
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
                        if (UIUtils.ClickTPStatue(WorldPos, ref kingStatue))
                        {
                            bool TPSuccessly = false;
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
                                            int protmp = Projectile.NewProjectile(null, npc.Center, Vector2.Zero, ModContent.ProjectileType<TPLine>(), 0, 0, Main.myPlayer);
                                            (Main.projectile[protmp].ModProjectile as TPLine).BeginPos = npc.Center;
                                            npc.Center = UIUtils.GetTPStatuePos(WorldPos);
                                            (Main.projectile[protmp].ModProjectile as TPLine).EndPos = npc.Center;
                                            TPSuccessly = true;
                                        }
                                    }
                                }
                            }
                            if (TPSuccessly)
                            {
                                int time = Main.hardMode ? 10 * 60 : 30 * 60;
                                Main.LocalPlayer.AddBuff(ModContent.BuffType<TPCDBuff>(), time);
                            }
                            SoundEngine.PlaySound(new SoundStyle("NPCAttacker/Sounds/TP"));
                            Main.LocalPlayer.SetTalkNPC(-1);
                            return;
                        }
                        else
                        {
                            int assembleSource = NPC.NewNPC(null, (int)WorldPos.X, (int)WorldPos.Y, ModContent.NPCType<AssembleNPC>());
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
            else         //没有选择NPC时
            {
                int findindex = -1;
                float findrange = 100;
                foreach (NPC npc in Main.npc)
                {
                    if (npc.active)
                    {
                        if (npc.IsTownNPC())
                        {
                            if (npc.Distance(WorldPos) < 150)
                            {
                                if (npc.Distance(WorldPos) < findrange)
                                {
                                    findindex = npc.whoAmI;
                                    findrange = npc.Distance(WorldPos);
                                }
                            }
                        }
                    }
                }

                if (findindex != -1)
                {
                    NPCUtils.ClearWarningLine();
                    Main.npc[findindex].GetGlobalNPC<ArmedGNPC>().Selected = true;
                    NPCUtils.SwitchMode(Main.npc[findindex]);
                    if (Main.npc[findindex].GetGlobalNPC<ArmedGNPC>().actMode != ArmedGNPC.ActMode.Default)
                    {
                        int color = Main.npc[findindex].GetGlobalNPC<ArmedGNPC>().actMode == ArmedGNPC.ActMode.Attack ? 1 : 0;
                        int protmp = Projectile.NewProjectile(null, Main.npc[findindex].Center, Vector2.Zero, ModContent.ProjectileType<WarningLine>(), 0, 0, Main.myPlayer, color, Main.npc[findindex].whoAmI);
                        (Main.projectile[protmp].ModProjectile as WarningLine).EndTarget = Main.npc[findindex].GetGlobalNPC<ArmedGNPC>().ActTargetNPC;
                    }
                }
            }

        IL_02:
            SoundEngine.PlaySound(new SoundStyle("NPCAttacker/Sounds/Tab"));
            Main.LocalPlayer.SetTalkNPC(-1);

        }


        public void RightClick(Vector2 WorldPos, bool FullMap)
        {
            if (Main.LocalPlayer.HeldItem.type != ModContent.ItemType<AttackerStick>() || !UIUtils.CanUseMapDrawing()) return;
            UIUtils.RightClick(Main.LocalPlayer);

        }



    }

}