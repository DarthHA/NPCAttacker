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

            On_Projectile.Update += ProjectileUpdateHook;
            On_Main.DrawProj += DrawProjHook;

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

            On_Projectile.Update -= ProjectileUpdateHook;
            On_Main.DrawProj -= DrawProjHook;

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


        internal void DrawProjHook(On_Main.orig_DrawProj orig, Main self, int i)
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
                    int itemtime = (int)owner.ai[1] - 1;
                    int itemtimeM = NPCStats.GetModifiedAttackTime(owner);
                    if (itemtime > itemtimeM) itemtime = 0;
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

        internal static void ProjectileUpdateHook(On_Projectile.orig_Update orig, Projectile self, int i)
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
                    Main.LocalPlayer.oldVelocity = owner.oldVelocity;
                    Main.LocalPlayer.direction = owner.direction;
                    Main.LocalPlayer.statLife = owner.life;
                    Main.LocalPlayer.statLifeMax2 = owner.lifeMax;
                    if (Main.LocalPlayer.statManaMax2 < 200) Main.LocalPlayer.statManaMax2 = 200;
                    Main.LocalPlayer.statMana = Main.LocalPlayer.statManaMax2;
                    if (result.ChannelTimer > 0)
                    {
                        Main.LocalPlayer.channel = true;
                        switch (owner.GetGlobalNPC<ArmedGNPC>().AlterUseType)
                        {
                            case 0:
                                Main.LocalPlayer.controlUseItem = true;
                                Main.LocalPlayer.controlUseTile = false;
                                Main.LocalPlayer.altFunctionUse = 0;
                                break;
                            case 1:
                                Main.LocalPlayer.controlUseItem = false;
                                Main.LocalPlayer.controlUseTile = true;
                                Main.LocalPlayer.altFunctionUse = 2;
                                break;
                            case 2:
                                Main.LocalPlayer.altFunctionUse = owner.GetGlobalNPC<ArmedGNPC>().NextUseType ? 2 : 0;
                                Main.LocalPlayer.controlUseItem = owner.GetGlobalNPC<ArmedGNPC>().NextUseType;
                                Main.LocalPlayer.controlUseTile = !owner.GetGlobalNPC<ArmedGNPC>().NextUseType;
                                break;
                        }
                    }
                    else
                    {
                        Main.LocalPlayer.channel = false;
                        Main.LocalPlayer.altFunctionUse = 0;
                        Main.LocalPlayer.controlUseItem = false;
                        Main.LocalPlayer.controlUseTile = false;
                    }

                    if (owner.GetGlobalNPC<ArmedGNPC>().NPCTargetForSpecialUse != -1)
                    {
                        NPC target = Main.npc[owner.GetGlobalNPC<ArmedGNPC>().NPCTargetForSpecialUse];
                        Main.mouseX = (int)(target.Center.X - Main.screenPosition.X);
                        Main.mouseY = (int)(target.Center.Y - Main.screenPosition.Y);
                    }
                    int itemtime = (int)owner.ai[1] - 1;
                    int itemtimeM = NPCStats.GetModifiedAttackTime(owner);
                    if (itemtime > itemtimeM) itemtime = 0;
                    Main.LocalPlayer.itemTime = itemtime;
                    Main.LocalPlayer.itemTimeMax = itemtimeM;
                    Main.LocalPlayer.itemAnimation = itemtime;
                    Main.LocalPlayer.itemAnimationMax = itemtimeM;
                    Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem] = ArmedGNPC.GetWeapon(owner);

                    NPCAttacker.SpawnForNPCIndex = result.NPCProjOwner;

                    orig.Invoke(self, i);

                    //一个原版手持弹幕的修正，如果Heldproj了的话强制取消hide
                    if (Main.LocalPlayer.heldProj == i)
                    {
                        self.hide = false;
                    }

                    //消耗生命武器的一个小适配
                    owner.life = Main.LocalPlayer.statLife;
                    owner.lifeMax = Main.LocalPlayer.statLifeMax2;
                    //位移武器的一个适配
                    owner.position = Main.LocalPlayer.position;
                    owner.velocity = Main.LocalPlayer.velocity;
                    owner.oldPosition = Main.LocalPlayer.oldPosition;
                    owner.oldVelocity = Main.LocalPlayer.oldVelocity;
                    owner.spriteDirection = Main.LocalPlayer.direction;

                    NPCAttacker.SpawnForNPCIndex = -1;
                    NPCAttacker.playerDataSaver.CloneTo(Main.LocalPlayer);
                    NPCAttacker.FuckingInvincible = false;

                    if (owner.life <= 0) owner.checkDead();
                    return;
                }
            }

            orig.Invoke(self, i);

        }

        internal static void AIHook(On_NPC.orig_AI_007_TownEntities orig, NPC self)
        {
            if (self.IsTownNPC())
            {
                OverrideAI.AI_007_TownEntities(self);
                return;
            }
            orig.Invoke(self);
        }

        internal static void DrawNPCChatButtonsHook(On_Main.orig_DrawNPCChatButtons orig, int superColor, Color chatColor, int numLines, string focusText, string focusText3)
        {
            NPCAttacker.FocusText1 = focusText;
            NPCAttacker.FocusText3 = focusText3;
            orig.Invoke(superColor, chatColor, numLines, focusText, focusText3);
        }


    }
}
