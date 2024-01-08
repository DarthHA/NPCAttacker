using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NPCAttacker.Items;
using NPCAttacker.NPCs;
using NPCAttacker.Projectiles;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NPCAttacker
{
    public static class NPCUtils
    {
        public static bool IsTownNPC(this NPC npc)
        {
            return npc.townNPC &&
                !NPCID.Sets.TownCritter[npc.type]
                && !NPCID.Sets.IsTownPet[npc.type]
                && npc.type != NPCID.TravellingMerchant
                && npc.type != NPCID.OldMan
                && npc.type != NPCID.SkeletonMerchant
                && !NPCID.Sets.IsTownSlime[npc.type];
            //return npc.townNPC && !NPCID.Sets.TownCritter[npc.type] && npc.type != NPCID.TravellingMerchant && npc.type != NPCID.OldMan && npc.type != NPCID.SkeletonMerchant;
        }


        public static bool BuffNPC()
        {
            return Main.LocalPlayer.HasItem(ModContent.ItemType<AttackerStick>());
        }

        public static Item CloneItem(Item target)
        {
            Item result = new();
            result.netDefaults(target.netID);
            result = target.Clone()/* tModPorter Note: Removed. Use Clone, ResetPrefix or Refresh */;
            result.favorited = target.favorited;
            result.stack = target.stack;
            result.prefix = target.prefix;
            return result;
        }








        public static float GetDangerDetectRange(NPC npc)
        {
            float result = NPCID.Sets.DangerDetectRange[npc.type];
            if (npc.HasBuff(BuffID.Hunter))             //猎人药水
            {
                result *= 1.25f;
            }
            if (npc.HasBuff(BuffID.NightOwl) && !Main.dayTime)             //夜视
            {
                result *= 1.5f;
            }
            return result;
        }



        public static bool AnyNPCSelected()
        {
            bool result = false;
            foreach (NPC npc in Main.npc)
            {
                if (npc.active)
                {
                    if (npc.IsTownNPC())
                    {
                        if (npc.GetGlobalNPC<ArmedGNPC>().Selected)
                        {
                            result = true;
                        }
                    }
                }
            }
            return result;
        }

        public static void ClearNPCSelect()
        {
            foreach (NPC npc in Main.npc)
            {
                if (npc.active)
                {
                    if (npc.IsTownNPC())
                    {
                        npc.GetGlobalNPC<ArmedGNPC>().Selected = false;
                    }
                }
            }
        }


        public static void SelectTeam(int team)
        {
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

        public static void ClearWarningLine()
        {
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active)
                {
                    if (proj.type == ModContent.ProjectileType<WarningLine>())// || proj.type == ModContent.ProjectileType<TPLine>())
                    {
                        proj.Kill();
                    }
                }
            }
        }

        public static void SwitchMode(NPC npc)
        {
            if (!npc.IsTownNPC()) return;
            if (npc.GetGlobalNPC<ArmedGNPC>().ActTargetNPC == -1)
            {
                npc.GetGlobalNPC<ArmedGNPC>().actMode = ArmedGNPC.ActMode.Default;
            }
            else
            {
                NPC target = Main.npc[npc.GetGlobalNPC<ArmedGNPC>().ActTargetNPC];
                if (!target.active)
                {
                    npc.GetGlobalNPC<ArmedGNPC>().actMode = ArmedGNPC.ActMode.Default;
                    npc.GetGlobalNPC<ArmedGNPC>().ActTargetNPC = -1;
                    return;
                }
                if (target.type == ModContent.NPCType<AssembleNPC>())
                {
                    if (Math.Abs(target.Center.X - npc.Center.X) < 8)
                    {
                        npc.GetGlobalNPC<ArmedGNPC>().actMode = ArmedGNPC.ActMode.Default;
                        npc.GetGlobalNPC<ArmedGNPC>().ActTargetNPC = -1;
                    }
                }
                else if (target.type == ModContent.NPCType<AttackTargetNPC>())
                {
                    int range = Math.Max(100, (int)GetDangerDetectRange(npc));
                    if (NPCID.Sets.AttackType[npc.type] == 3) range = 114514;
                    if (Math.Abs(target.Center.X - npc.Center.X) > range)
                    {
                        npc.GetGlobalNPC<ArmedGNPC>().actMode = ArmedGNPC.ActMode.Move;
                    }
                    else
                    {
                        npc.GetGlobalNPC<ArmedGNPC>().actMode = ArmedGNPC.ActMode.Attack;
                    }
                }
                else
                {
                    npc.GetGlobalNPC<ArmedGNPC>().actMode = ArmedGNPC.ActMode.Default;
                    npc.GetGlobalNPC<ArmedGNPC>().ActTargetNPC = -1;
                }
            }
        }

        public static void DrawNPCHealthBar(NPC npc)
        {
            if (Main.HealthBarDrawSettings == 0)
            {
                return;
            }
            if (npc.life != npc.lifeMax && !npc.dontTakeDamage) return;
            float scale = 1f;
            float num2 = 10f;
            if (Main.HealthBarDrawSettings == 2)
            {
                num2 -= 34f;
            }
            if (Main.HealthBarDrawSettings == 1)
            {
                num2 += Main.NPCAddHeight(npc);
                Main.instance.DrawHealthBar(npc.Center.X, npc.position.Y + npc.height + num2 + npc.gfxOffY, npc.life, npc.lifeMax, Lighting.Brightness((int)((npc.position.X + npc.width / 2) / 16f), (int)((npc.position.Y + npc.height / 2 + npc.gfxOffY) / 16f)), scale, true);
            }
            else if (Main.HealthBarDrawSettings == 2)
            {
                num2 -= Main.NPCAddHeight(npc) / 2f;
                Main.instance.DrawHealthBar(npc.Center.X, npc.position.Y + num2 + npc.gfxOffY, npc.life, npc.lifeMax, Lighting.Brightness((int)((npc.position.X + npc.width / 2) / 16f), (int)((npc.position.Y + npc.height / 2 + npc.gfxOffY) / 16f)), scale, true);
            }
        }

        public static void DrawNPCTeamBar(NPC npc, Color color)
        {
            int team = npc.GetGlobalNPC<ArmedGNPC>().Team;
            if (team == 0) return;
            if (Main.HealthBarDrawSettings == 0)
            {
                return;
            }

            float scale = 1f;
            float num2 = 4f;
            if (Main.HealthBarDrawSettings == 2)
            {
                num2 -= 34f;
            }
            if (Main.HealthBarDrawSettings == 1)
            {
                num2 += Main.NPCAddHeight(npc);
                Vector2 Pos = new(npc.Center.X + 20, npc.position.Y + npc.height + num2 + npc.gfxOffY);
                Utils.DrawBorderString(Main.spriteBatch, team.ToString(), Pos - Main.screenPosition, color, scale);
            }
            else if (Main.HealthBarDrawSettings == 2)
            {

                num2 -= Main.NPCAddHeight(npc) / 2f;
                Vector2 Pos = new(npc.Center.X + 20, npc.position.Y + num2 + npc.gfxOffY);
                Utils.DrawBorderString(Main.spriteBatch, team.ToString(), Pos - Main.screenPosition, color, scale);
            }
        }


        public static void DrawNPCGuard(NPC npc)
        {
            if (!npc.GetGlobalNPC<ArmedGNPC>().AlertMode) return;
            if (Main.HealthBarDrawSettings == 0)
            {
                return;
            }

            float scale = 1f;
            float num2 = 4f;
            if (Main.HealthBarDrawSettings == 2)
            {
                num2 -= 34f;
            }
            if (Main.HealthBarDrawSettings == 1)
            {
                num2 += Main.NPCAddHeight(npc);
                Vector2 Pos = new(npc.Center.X - 45, npc.position.Y + npc.height + num2 + npc.gfxOffY - 5f);
                Texture2D tex = ModContent.Request<Texture2D>("NPCAttacker/NPCs/GuardImage").Value;
                Main.spriteBatch.Draw(tex, Pos - Main.screenPosition, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            }
            else if (Main.HealthBarDrawSettings == 2)
            {

                num2 -= Main.NPCAddHeight(npc) / 2f;
                Vector2 Pos = new(npc.Center.X - 45, npc.position.Y + num2 + npc.gfxOffY - 5f);
                Texture2D tex = ModContent.Request<Texture2D>("NPCAttacker/NPCs/GuardImage").Value;
                Main.spriteBatch.Draw(tex, Pos - Main.screenPosition, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            }
        }
        public static void FlaskToDebuff(NPC target, int ImbueBuffID)
        {
            int meleeEnchant = 0;

            if (ImbueBuffID == 71)
            {
                meleeEnchant = 1;
            }
            else if (ImbueBuffID == 73)
            {
                meleeEnchant = 2;
            }
            else if (ImbueBuffID == 74)
            {
                meleeEnchant = 3;
            }
            else if (ImbueBuffID == 75)
            {
                meleeEnchant = 4;
            }
            else if (ImbueBuffID == 76)
            {
                meleeEnchant = 5;
            }
            else if (ImbueBuffID == 77)
            {
                meleeEnchant = 6;
            }
            else if (ImbueBuffID == 78)
            {
                meleeEnchant = 7;
            }
            else if (ImbueBuffID == 79)
            {
                meleeEnchant = 8;
            }

            if (meleeEnchant == 1)
            {
                target.AddBuff(70, 60 * Main.rand.Next(5, 10), false);
            }
            if (meleeEnchant == 2)
            {
                target.AddBuff(39, 60 * Main.rand.Next(3, 7), false);
            }
            if (meleeEnchant == 3)
            {
                target.AddBuff(24, 60 * Main.rand.Next(3, 7), false);
            }
            if (meleeEnchant == 5)
            {
                target.AddBuff(69, 60 * Main.rand.Next(10, 20), false);
            }
            if (meleeEnchant == 6)
            {
                target.AddBuff(31, 60 * Main.rand.Next(1, 4), false);
            }
            if (meleeEnchant == 7)
            {
                Projectile.NewProjectile(null, target.Center.X, target.Center.Y, target.velocity.X, target.velocity.Y, 289, 0, 0f, Main.myPlayer, 0f, 0f);
            }
            if (meleeEnchant == 8)
            {
                target.AddBuff(20, 60 * Main.rand.Next(5, 10), false);
            }
            if (meleeEnchant == 4)
            {
                target.AddBuff(72, 120, false);
            }
        }
    }
}