﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NPCAttacker.Override;
using NPCAttacker.Projectiles;
using Terraria;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI.Gamepad;

namespace NPCAttacker
{
    public class NPCAttacker : Mod
    {
        public static string FocusText1 = "";
        public static string FocusText3 = "";

        internal static ModKeybind ForceAttackKey;
        internal static ModKeybind DisperseKey;
        internal static ModKeybind AlertKey;
        internal static ModKeybind StopKey;
        internal static ModKeybind F2AKey;
        internal static ModKeybind F2A2Key;
        internal static ModKeybind Team1Key;
        internal static ModKeybind Team2Key;
        internal static ModKeybind Team3Key;
        internal static ModKeybind Team4Key;
        internal static ModKeybind FKey;

        public static int CurserType = 0;
        public static bool CurserInMap = false;

        public static CustomConfig config;

        public static NPCAttacker Instance;

        public NPCAttacker()
        {
            Instance = this;
        }
        public override void Load()
        {
            On_NPC.AI_007_TownEntities += AIHook;
            On_NPC.StrikeNPC_HitInfo_bool_bool += StrikeOverride.StrikeNPCHook;
            On_Main.DrawNPCChatButtons += DrawNPCChatButtonsHook;
            On_Main.DrawCursor += DrawCursorHook;
            On_Main.DrawThickCursor += DrawThickCursorHook;
            On_NPC.Collision_MoveSlopesAndStairFall += CollisionOverride.Collision_MoveSlopesAndStairFall;

            /*
            ForceAttackKey = KeybindLoader.RegisterKeybind(this, "Force Attack/Team Formation", "LeftControl");
            DisperseKey = KeybindLoader.RegisterKeybind(this, "Disperse Units", "X");
            AlertKey = KeybindLoader.RegisterKeybind(this, "Alert Mode", "G");
            StopKey = KeybindLoader.RegisterKeybind(this, "Stop Command", "S");
            F2AKey = KeybindLoader.RegisterKeybind(this, "Select All", "P");
            F2A2Key = KeybindLoader.RegisterKeybind(this, "Select The Same Kind", "T");
            Team1Key = KeybindLoader.RegisterKeybind(this, "Team 1", "D1");
            Team2Key = KeybindLoader.RegisterKeybind(this, "Team 2", "D2");
            Team3Key = KeybindLoader.RegisterKeybind(this, "Team 3", "D3");
            Team4Key = KeybindLoader.RegisterKeybind(this, "Team 4", "D4");
            FKey = KeybindLoader.RegisterKeybind(this, "Follow Unit", "F");
            */

            ForceAttackKey = KeybindLoader.RegisterKeybind(this, "ForceAttack", "LeftControl");
            DisperseKey = KeybindLoader.RegisterKeybind(this, "DisperseUnits", "X");
            AlertKey = KeybindLoader.RegisterKeybind(this, "AlertMode", "G");
            StopKey = KeybindLoader.RegisterKeybind(this, "StopCommand", "S");
            F2AKey = KeybindLoader.RegisterKeybind(this, "SelectAll", "P");
            F2A2Key = KeybindLoader.RegisterKeybind(this, "SelectTheSameKind", "T");
            Team1Key = KeybindLoader.RegisterKeybind(this, "Team1", "D1");
            Team2Key = KeybindLoader.RegisterKeybind(this, "Team2", "D2");
            Team3Key = KeybindLoader.RegisterKeybind(this, "Team3", "D3");
            Team4Key = KeybindLoader.RegisterKeybind(this, "Team4", "D4");
            FKey = KeybindLoader.RegisterKeybind(this, "FollowUnit", "F");


        }


        public override void PostSetupContent()
        {
            VanillaItemProjFix.Load();
            TranslationToPotion.Load();
        }

        public override void Unload()
        {
            VanillaItemProjFix.UnLoad();
            TranslationToPotion.UnLoad();

            ForceAttackKey = null;
            DisperseKey = null;
            AlertKey = null;
            StopKey = null;
            F2AKey = null;
            F2A2Key = null;
            Team1Key = null;
            Team2Key = null;
            Team3Key = null;
            Team4Key = null;
            FKey = null;

            config = null;

            On_NPC.AI_007_TownEntities -= AIHook;
            On_NPC.StrikeNPC_HitInfo_bool_bool -= StrikeOverride.StrikeNPCHook;
            On_Main.DrawNPCChatButtons -= DrawNPCChatButtonsHook;
            On_Main.DrawCursor -= DrawCursorHook;
            On_Main.DrawThickCursor -= DrawThickCursorHook;
            On_NPC.Collision_MoveSlopesAndStairFall -= CollisionOverride.Collision_MoveSlopesAndStairFall;

            Instance = null;

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
            FocusText1 = focusText;
            FocusText3 = focusText3;
            orig.Invoke(superColor, chatColor, numLines, focusText, focusText3);
        }

        public static string UITranslation(string str)
        {
            return Language.GetTextValue("Mods.NPCAttacker.UI." + str);
        }

        public static bool IsChinese()
        {
            return Language.ActiveCulture.LegacyId == (int)GameCulture.CultureName.Chinese;
        }


        public static void DrawCursorHook(On_Main.orig_DrawCursor orig, Vector2 bonus, bool smart = false)
        {
            if (Main.gameMenu || CurserType == 0)
            {
                orig.Invoke(bonus, smart);
                return;
            }

            if (Main.gameMenu && Main.alreadyGrabbingSunOrMoon)
            {
                return;
            }

            float MapScale = CurserInMap ? 0.25f : 1f;

            if (Main.player[Main.myPlayer].dead || Main.player[Main.myPlayer].mouseInterface)
            {
                Main.ClearSmartInteract();
                Main.TileInteractionLX = (Main.TileInteractionHX = (Main.TileInteractionLY = (Main.TileInteractionHY = -1)));
            }
            bool flag = UILinkPointNavigator.Available && !PlayerInput.InBuildingMode;
            if (!PlayerInput.SettingsForUI.ShowGamepadCursor)
            {
                if (CurserType == 1)
                {
                    Main.spriteBatch.Draw(ModContent.Request<Texture2D>("NPCAttacker/NPCs/AssembleNPC").Value, new Vector2(Main.mouseX, Main.mouseY) + bonus, null, Color.White, 0, ModContent.Request<Texture2D>("NPCAttacker/NPCs/AssembleNPC").Value.Size() / 2f, Main.cursorScale * MapScale, 0f, 0f);
                }
                else if (CurserType == 2)
                {
                    Main.spriteBatch.Draw(ModContent.Request<Texture2D>("NPCAttacker/NPCs/AttackTargetNPC").Value, new Vector2(Main.mouseX, Main.mouseY) + bonus, null, Color.White, 0, ModContent.Request<Texture2D>("NPCAttacker/NPCs/AttackTargetNPC").Value.Size() / 2f, Main.cursorScale * MapScale, 0f, 0f);
                }
                else if (CurserType == 3)
                {
                    Main.spriteBatch.Draw(ModContent.Request<Texture2D>("NPCAttacker/NPCs/TPNPC").Value, new Vector2(Main.mouseX, Main.mouseY) + bonus, null, Color.White, 0, ModContent.Request<Texture2D>("NPCAttacker/NPCs/TPNPC").Value.Size() / 2f, Main.cursorScale * MapScale, 0f, 0f);
                }

                return;
            }
            if ((Main.player[Main.myPlayer].dead && !Main.player[Main.myPlayer].ghost && !Main.gameMenu) || PlayerInput.InvisibleGamepadInMenus)
            {
                return;
            }


            Vector2 vector = new(Main.mouseX, Main.mouseY);
            Vector2 vector2 = Vector2.Zero;
            bool flag2 = Main.SmartCursorIsUsed;
            if (flag2)
            {
                PlayerInput.smartSelectPointer.UpdateCenter(Main.ScreenSize.ToVector2() / 2f);
                vector2 = PlayerInput.smartSelectPointer.GetPointerPosition();
                if (Vector2.Distance(vector2, vector) < 1f)
                {
                    flag2 = false;
                }
                else
                {
                    Utils.Swap(ref vector, ref vector2);
                }
            }
            if (flag2)
            {
                if (CurserType == 1)
                {
                    Main.spriteBatch.Draw(ModContent.Request<Texture2D>("NPCAttacker/NPCs/AssembleNPC").Value, vector2 + bonus, null, Color.White, 0, ModContent.Request<Texture2D>("NPCAttacker/NPCs/AssembleNPC").Value.Size() / 2f, Main.cursorScale * MapScale, 0f, 0f);
                }
                else if (CurserType == 2)
                {
                    Main.spriteBatch.Draw(ModContent.Request<Texture2D>("NPCAttacker/NPCs/AttackTargetNPC").Value, vector2 + bonus, null, Color.White, 0, ModContent.Request<Texture2D>("NPCAttacker/NPCs/AssembleNPC").Value.Size() / 2f, Main.cursorScale * MapScale, 0f, 0f);
                }
                else if (CurserType == 3)
                {
                    Main.spriteBatch.Draw(ModContent.Request<Texture2D>("NPCAttacker/NPCs/TPNPC").Value, vector2 + bonus, null, Color.White, 0, ModContent.Request<Texture2D>("NPCAttacker/NPCs/TPNPC").Value.Size() / 2f, Main.cursorScale * MapScale, 0f, 0f);
                }
            }
            if (smart && !flag)
            {

                if (CurserType == 1)
                {
                    Main.spriteBatch.Draw(ModContent.Request<Texture2D>("NPCAttacker/NPCs/AssembleNPC").Value, vector + bonus, null, Color.White, 0, ModContent.Request<Texture2D>("NPCAttacker/NPCs/AssembleNPC").Value.Size() / 2f, Main.cursorScale * MapScale, 0f, 0f);
                }
                else if (CurserType == 2)
                {
                    Main.spriteBatch.Draw(ModContent.Request<Texture2D>("NPCAttacker/NPCs/AttackTargetNPC").Value, vector2 + bonus, null, Color.White, 0, ModContent.Request<Texture2D>("NPCAttacker/NPCs/AttackTargetNPC").Value.Size() / 2f, Main.cursorScale * MapScale, 0f, 0f);
                }
                else if (CurserType == 3)
                {
                    Main.spriteBatch.Draw(ModContent.Request<Texture2D>("NPCAttacker/NPCs/TPNPC").Value, vector2 + bonus, null, Color.White, 0, ModContent.Request<Texture2D>("NPCAttacker/NPCs/TPNPC").Value.Size() / 2f, Main.cursorScale * MapScale, 0f, 0f);
                }
                return;
            }
            if (CurserType == 1)
            {
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("NPCAttacker/NPCs/AssembleNPC").Value, new Vector2(Main.mouseX, Main.mouseY) + bonus, null, Color.White, 0, ModContent.Request<Texture2D>("NPCAttacker/NPCs/AssembleNPC").Value.Size() / 2f, Main.cursorScale * MapScale, 0f, 0f);
            }
            else if (CurserType == 2)
            {
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("NPCAttacker/NPCs/AttackTargetNPC").Value, new Vector2(Main.mouseX, Main.mouseY) + bonus, null, Color.White, 0, ModContent.Request<Texture2D>("NPCAttacker/NPCs/AttackTargetNPC").Value.Size() / 2f, Main.cursorScale * MapScale, 0f, 0f);
            }
            else if (CurserType == 3)
            {
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("NPCAttacker/NPCs/TPNPC").Value, new Vector2(Main.mouseX, Main.mouseY) + bonus, null, Color.White, 0, ModContent.Request<Texture2D>("NPCAttacker/NPCs/TPNPC").Value.Size() / 2f, Main.cursorScale * MapScale, 0f, 0f);
            }
        }

        public static Vector2 DrawThickCursorHook(On_Main.orig_DrawThickCursor orig, bool smart = false)
        {
            if (!Main.gameMenu && CurserType != 0)
            {
                return new Vector2(2f);
            }
            return orig.Invoke(smart);
        }
    }
}