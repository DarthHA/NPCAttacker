using Microsoft.Xna.Framework;
using NPCAttacker.Override;
using Terraria;
using Terraria.ModLoader;

namespace NPCAttacker.Systems
{
    public class HookSystem : ModSystem
    {

        public override void Load()
        {
            On_NPC.AI_007_TownEntities += AIHook;
            On_NPC.StrikeNPC_HitInfo_bool_bool += StrikeOverride.StrikeNPCHook;
            On_Main.DrawNPCChatButtons += DrawNPCChatButtonsHook;
            On_Main.DrawCursor += CurserOverride.DrawCursorHook;
            On_Main.DrawThickCursor += CurserOverride.DrawThickCursorHook;
            On_NPC.Collision_MoveSlopesAndStairFall += CollisionOverride.Collision_MoveSlopesAndStairFall;

            On_Projectile.Update += On_Projectile_Update;
            On_NPC.UpdateNPC_Inner += On_NPC_UpdateNPC_Inner;
            On_Main.DrawProj += On_Main_DrawProj;

            On_Player.ChooseAmmo += UseNPCAmmo;
            On_Player.CheckMana_int_bool_bool += PreventManaCost1;
            On_Player.CheckMana_Item_int_bool_bool += PreventManaCost2;
        }

        public override void Unload()
        {
            On_NPC.AI_007_TownEntities -= AIHook;
            On_NPC.StrikeNPC_HitInfo_bool_bool -= StrikeOverride.StrikeNPCHook;
            On_Main.DrawNPCChatButtons -= DrawNPCChatButtonsHook;
            On_Main.DrawCursor -= CurserOverride.DrawCursorHook;
            On_Main.DrawThickCursor -= CurserOverride.DrawThickCursorHook;
            On_NPC.Collision_MoveSlopesAndStairFall -= CollisionOverride.Collision_MoveSlopesAndStairFall;

            On_Projectile.Update -= On_Projectile_Update;
            On_Main.DrawProj -= On_Main_DrawProj;
            On_NPC.UpdateNPC_Inner -= On_NPC_UpdateNPC_Inner;

            On_Player.ChooseAmmo -= UseNPCAmmo;
            On_Player.CheckMana_int_bool_bool -= PreventManaCost1;
            On_Player.CheckMana_Item_int_bool_bool -= PreventManaCost2;
        }

        internal static bool PreventManaCost2(On_Player.orig_CheckMana_Item_int_bool_bool orig, Player self, Item item, int amount, bool pay, bool blockQuickMana)
        {
            if (NPCAttacker.SpawnForNPCIndex != -1)
            {
                NPC owner = Main.npc[NPCAttacker.SpawnForNPCIndex];
                if (owner.active)
                {
                    return true;
                }
            }
            return orig.Invoke(self, item, amount, pay, blockQuickMana);
        }

        internal static bool PreventManaCost1(On_Player.orig_CheckMana_int_bool_bool orig, Player self, int amount, bool pay, bool blockQuickMana)
        {
            if (NPCAttacker.SpawnForNPCIndex != -1)
            {
                NPC owner = Main.npc[NPCAttacker.SpawnForNPCIndex];
                if (owner.active)
                {
                    return true;
                }
            }
            return orig.Invoke(self, amount, pay, blockQuickMana);
        }

        internal static Item UseNPCAmmo(On_Player.orig_ChooseAmmo orig, Player self, Item weapon)
        {
            if (NPCAttacker.SpawnForNPCIndex != -1)
            {
                NPC owner = Main.npc[NPCAttacker.SpawnForNPCIndex];
                if (owner.active)
                {
                    return AmmoFix.FindAmmo(owner);
                }
            }
            return orig.Invoke(self, weapon);
        }

        private void On_NPC_UpdateNPC_Inner(On_NPC.orig_UpdateNPC_Inner orig, NPC self, int i)
        {
            if (!self.active)
            {
                orig.Invoke(self, i);
                return;
            }
            if (self.IsTownNPC())
            {
                NPCAttacker.SpawnForNPCIndex = self.whoAmI;

                if (!ArmedGNPC.GetWeapon(self).IsAir && ArmedGNPC.GetWeapon(self).channel)
                {
                    NPCAttacker.SpawnForChannelTime = (int)(NPCStats.GetModifiedAttackTime(self) * (ChannelHelper.NeedBreakChannel(ArmedGNPC.GetWeapon(self)) ? 0.75f : 1.5f));
                }
            }
            orig.Invoke(self, i);
            NPCAttacker.SpawnForNPCIndex = -1;
            NPCAttacker.SpawnForChannelTime = -1;
        }


        internal void On_Main_DrawProj(On_Main.orig_DrawProj orig, Main self, int i)
        {
            Projectile proj = Main.projectile[i];
            if (!proj.active)
            {
                orig.Invoke(self, i);
                return;
            }

            if (proj.TryGetGlobalProjectile(out SpecialUseProj result))
            {
                //Main.NewText("Draw:" + Lang.GetProjectileName(proj.type) + " " + result.NPCProjOwner);
                result.UpdateNPCOwnerStatus();
                if (result.NPCProjOwner != -1 && Main.LocalPlayer.active)
                {
                    //Main.NewText(proj.Center - Main.LocalPlayer.Center);

                    NPC owner = Main.npc[result.NPCProjOwner];
                    NPCAttacker.playerDataSaver.CloneFrom(Main.LocalPlayer);
                    Main.LocalPlayer.position = owner.position;
                    Main.LocalPlayer.oldPosition = owner.position - owner.velocity;
                    Main.LocalPlayer.velocity = owner.velocity;
                    Main.LocalPlayer.direction = owner.direction;
                    if (result.ChannelTimer > 0)
                    {
                        Main.LocalPlayer.channel = true;
                    }

                    if (owner.GetGlobalNPC<ArmedGNPC>().NPCTargetForSpecialUse != -1)
                    {
                        NPC target = Main.npc[owner.GetGlobalNPC<ArmedGNPC>().NPCTargetForSpecialUse];
                        Main.mouseX = (int)(target.Center.X - Main.screenPosition.X);
                        Main.mouseY = (int)(target.Center.Y - Main.screenPosition.Y);
                    }
                    int itemtime = (int)owner.ai[1];
                    int itemtimeM = NPCStats.GetModifiedAttackTime(Main.npc[result.NPCProjOwner]);
                    Main.LocalPlayer.itemTime = itemtime;
                    Main.LocalPlayer.itemTimeMax = itemtimeM;
                    Main.LocalPlayer.itemAnimation = itemtime;
                    Main.LocalPlayer.itemAnimationMax = itemtimeM;
                    Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem] = ArmedGNPC.GetWeapon(Main.npc[result.NPCProjOwner]);
                    orig.Invoke(self, i);
                    NPCAttacker.playerDataSaver.CloneTo(Main.LocalPlayer);
                    return;
                }
            }
            orig.Invoke(self, i);
        }

        internal static void On_Projectile_Update(On_Projectile.orig_Update orig, Projectile self, int i)
        {
            if (!self.active)
            {
                orig.Invoke(self, i);
                return;
            }
            if (self.TryGetGlobalProjectile(out SpecialUseProj result))
            {
                //Main.NewText("Logic:" + Lang.GetProjectileName(self.type) + " " + result.NPCProjOwner);
                result.UpdateNPCOwnerStatus();
                if (result.NPCProjOwner != -1 && Main.LocalPlayer.active)
                {
                    NPC owner = Main.npc[result.NPCProjOwner];
                    NPCAttacker.playerDataSaver.CloneFrom(Main.LocalPlayer);
                    NPCAttacker.FuckingInvincible = true;
                    Main.LocalPlayer.position = owner.position;
                    Main.LocalPlayer.oldPosition = owner.position - owner.velocity;
                    Main.LocalPlayer.velocity = owner.velocity;
                    Main.LocalPlayer.direction = owner.direction;
                    if (result.ChannelTimer > 0)
                    {
                        Main.LocalPlayer.channel = true;
                        Main.LocalPlayer.controlUseItem = true;
                    }
                    if (owner.GetGlobalNPC<ArmedGNPC>().NPCTargetForSpecialUse != -1)
                    {
                        NPC target = Main.npc[owner.GetGlobalNPC<ArmedGNPC>().NPCTargetForSpecialUse];
                        Main.mouseX = (int)(target.Center.X - Main.screenPosition.X);
                        Main.mouseY = (int)(target.Center.Y - Main.screenPosition.Y);
                    }
                    int itemtime = (int)owner.ai[1];
                    int itemtimeM = NPCStats.GetModifiedAttackTime(Main.npc[result.NPCProjOwner]);
                    Main.LocalPlayer.itemTime = itemtime;
                    Main.LocalPlayer.itemTimeMax = itemtimeM;
                    Main.LocalPlayer.itemAnimation = itemtime;
                    Main.LocalPlayer.itemAnimationMax = itemtimeM;
                    Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem] = ArmedGNPC.GetWeapon(Main.npc[result.NPCProjOwner]);

                    NPCAttacker.SpawnForNPCIndex = result.NPCProjOwner;

                    orig.Invoke(self, i);

                    //一个原版手持弹幕的修正，如果Heldproj了的话强制取消hide
                    if (Main.LocalPlayer.heldProj == i)
                    {
                        self.hide = false;
                    }
                    NPCAttacker.SpawnForNPCIndex = -1;
                    NPCAttacker.playerDataSaver.CloneTo(Main.LocalPlayer);
                    NPCAttacker.FuckingInvincible = false;
                    return;
                }
            }

            orig.Invoke(self, i);

        }


        public static void AIHook(On_NPC.orig_AI_007_TownEntities orig, NPC self)
        {
            if (self.IsTownNPC())
            {
                OverrideAI.AI_007_TownEntities(self);
                return;
            }
            orig.Invoke(self);
        }


        public static void DrawNPCChatButtonsHook(On_Main.orig_DrawNPCChatButtons orig, int superColor, Color chatColor, int numLines, string focusText, string focusText3)
        {
            NPCAttacker.FocusText1 = focusText;
            NPCAttacker.FocusText3 = focusText3;
            orig.Invoke(superColor, chatColor, numLines, focusText, focusText3);
        }


    }
}
