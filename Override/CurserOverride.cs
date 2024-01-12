using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.UI.Gamepad;

namespace NPCAttacker.Override
{
    public static class CurserOverride
    {
        public static void DrawCursorHook(On_Main.orig_DrawCursor orig, Vector2 bonus, bool smart = false)
        {
            if (Main.gameMenu || NPCAttacker.CurserType == 0)
            {
                orig.Invoke(bonus, smart);
                return;
            }

            if (Main.gameMenu && Main.alreadyGrabbingSunOrMoon)
            {
                return;
            }

            float MapScale = NPCAttacker.CurserInMap ? 0.25f : 1f;

            if (Main.player[Main.myPlayer].dead || Main.player[Main.myPlayer].mouseInterface)
            {
                Main.ClearSmartInteract();
                Main.TileInteractionLX = Main.TileInteractionHX = Main.TileInteractionLY = Main.TileInteractionHY = -1;
            }
            bool flag = UILinkPointNavigator.Available && !PlayerInput.InBuildingMode;
            if (!PlayerInput.SettingsForUI.ShowGamepadCursor)
            {
                if (NPCAttacker.CurserType == 1)
                {
                    Main.spriteBatch.Draw(ModContent.Request<Texture2D>("NPCAttacker/NPCs/AssembleNPC").Value, new Vector2(Main.mouseX, Main.mouseY) + bonus, null, Color.White, 0, ModContent.Request<Texture2D>("NPCAttacker/NPCs/AssembleNPC").Value.Size() / 2f, Main.cursorScale * MapScale, 0f, 0f);
                }
                else if (NPCAttacker.CurserType == 2)
                {
                    Main.spriteBatch.Draw(ModContent.Request<Texture2D>("NPCAttacker/NPCs/AttackTargetNPC").Value, new Vector2(Main.mouseX, Main.mouseY) + bonus, null, Color.White, 0, ModContent.Request<Texture2D>("NPCAttacker/NPCs/AttackTargetNPC").Value.Size() / 2f, Main.cursorScale * MapScale, 0f, 0f);
                }
                else if (NPCAttacker.CurserType == 3)
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
                if (NPCAttacker.CurserType == 1)
                {
                    Main.spriteBatch.Draw(ModContent.Request<Texture2D>("NPCAttacker/NPCs/AssembleNPC").Value, vector2 + bonus, null, Color.White, 0, ModContent.Request<Texture2D>("NPCAttacker/NPCs/AssembleNPC").Value.Size() / 2f, Main.cursorScale * MapScale, 0f, 0f);
                }
                else if (NPCAttacker.CurserType == 2)
                {
                    Main.spriteBatch.Draw(ModContent.Request<Texture2D>("NPCAttacker/NPCs/AttackTargetNPC").Value, vector2 + bonus, null, Color.White, 0, ModContent.Request<Texture2D>("NPCAttacker/NPCs/AssembleNPC").Value.Size() / 2f, Main.cursorScale * MapScale, 0f, 0f);
                }
                else if (NPCAttacker.CurserType == 3)
                {
                    Main.spriteBatch.Draw(ModContent.Request<Texture2D>("NPCAttacker/NPCs/TPNPC").Value, vector2 + bonus, null, Color.White, 0, ModContent.Request<Texture2D>("NPCAttacker/NPCs/TPNPC").Value.Size() / 2f, Main.cursorScale * MapScale, 0f, 0f);
                }
            }
            if (smart && !flag)
            {

                if (NPCAttacker.CurserType == 1)
                {
                    Main.spriteBatch.Draw(ModContent.Request<Texture2D>("NPCAttacker/NPCs/AssembleNPC").Value, vector + bonus, null, Color.White, 0, ModContent.Request<Texture2D>("NPCAttacker/NPCs/AssembleNPC").Value.Size() / 2f, Main.cursorScale * MapScale, 0f, 0f);
                }
                else if (NPCAttacker.CurserType == 2)
                {
                    Main.spriteBatch.Draw(ModContent.Request<Texture2D>("NPCAttacker/NPCs/AttackTargetNPC").Value, vector2 + bonus, null, Color.White, 0, ModContent.Request<Texture2D>("NPCAttacker/NPCs/AttackTargetNPC").Value.Size() / 2f, Main.cursorScale * MapScale, 0f, 0f);
                }
                else if (NPCAttacker.CurserType == 3)
                {
                    Main.spriteBatch.Draw(ModContent.Request<Texture2D>("NPCAttacker/NPCs/TPNPC").Value, vector2 + bonus, null, Color.White, 0, ModContent.Request<Texture2D>("NPCAttacker/NPCs/TPNPC").Value.Size() / 2f, Main.cursorScale * MapScale, 0f, 0f);
                }
                return;
            }
            if (NPCAttacker.CurserType == 1)
            {
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("NPCAttacker/NPCs/AssembleNPC").Value, new Vector2(Main.mouseX, Main.mouseY) + bonus, null, Color.White, 0, ModContent.Request<Texture2D>("NPCAttacker/NPCs/AssembleNPC").Value.Size() / 2f, Main.cursorScale * MapScale, 0f, 0f);
            }
            else if (NPCAttacker.CurserType == 2)
            {
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("NPCAttacker/NPCs/AttackTargetNPC").Value, new Vector2(Main.mouseX, Main.mouseY) + bonus, null, Color.White, 0, ModContent.Request<Texture2D>("NPCAttacker/NPCs/AttackTargetNPC").Value.Size() / 2f, Main.cursorScale * MapScale, 0f, 0f);
            }
            else if (NPCAttacker.CurserType == 3)
            {
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("NPCAttacker/NPCs/TPNPC").Value, new Vector2(Main.mouseX, Main.mouseY) + bonus, null, Color.White, 0, ModContent.Request<Texture2D>("NPCAttacker/NPCs/TPNPC").Value.Size() / 2f, Main.cursorScale * MapScale, 0f, 0f);
            }
        }

        public static Vector2 DrawThickCursorHook(On_Main.orig_DrawThickCursor orig, bool smart = false)
        {
            if (!Main.gameMenu && NPCAttacker.CurserType != 0)
            {
                return new Vector2(2f);
            }
            return orig.Invoke(smart);
        }

    }
}
