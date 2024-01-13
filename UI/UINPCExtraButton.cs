using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;

namespace NPCAttacker.UI
{
    public class UINPCExtraButton : UIState
    {
        public UIText TextButton;
        private bool HasHover = false;
        public static bool Visible = false;
        public override void OnActivate()
        {
            TextButton = new UIText("");
            TextButton.Left.Set(0, 0f);
            TextButton.Top.Set(0, 0f);
            TextButton.OnLeftClick += Chat_OnClick;

            Append(TextButton);
            base.OnActivate();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Main.LocalPlayer.talkNPC == -1) return;
            NPC npc = Main.npc[Main.LocalPlayer.talkNPC];
            if (!npc.IsTownNPC()) return;
            Vector2 Pos = Get5ButtonPos(NPCAttacker.FocusText1, NPCAttacker.FocusText3);
            string LocalizedText = Main.npcChatText == "" ? "" : NPCAttacker.UITranslation("Arm");
            float multiplier = TextButton.IsMouseHovering ? 1.25f : 1f;
            TextButton.Left.Set(Pos.X + (TextButton.IsMouseHovering ? 0 : 4), 0f);
            TextButton.Top.Set(Pos.Y + (TextButton.IsMouseHovering ? 0 : 4), 0f);
            TextButton.SetText(LocalizedText,0.9f * multiplier, false);
            if (npc.type == NPCID.Nurse || npc.type == NPCID.Dryad)
            {
                TextButton.TextColor = CombatText.HealLife;
            }
            else
            {
                TextButton.TextColor = Color.Red;
            }
            TextButton.TextColor *= Main.mouseTextColor / 255f;
            TextButton.Recalculate();
            if (TextButton.IsMouseHovering && !HasHover)
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                HasHover = true;
            }
            if (!TextButton.IsMouseHovering && HasHover)
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                HasHover = false;
            }
        }

        private void Chat_OnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            SoundEngine.PlaySound(SoundID.MenuOpen);
            Main.playerInventory = true;

            Main.npcChatText = "";

            if (Main.LocalPlayer.talkNPC != -1)
            {
                if (NPCID.Sets.AttackType[Main.npc[Main.LocalPlayer.talkNPC].type] >= 0 && NPCID.Sets.AttackType[Main.npc[Main.LocalPlayer.talkNPC].type] <= 3)
                {
                    ArmUI.OpenUI();
                }
            }
        }

        public Vector2 Get5ButtonPos(string focusText1, string focusText3)
        {
            DynamicSpriteFont font = FontAssets.MouseText.Value;
            Utils.WordwrapString(Main.npcChatText, font, 460, 10, out int numLines);
            numLines++;

            float PosY = 130 + numLines * 30;
            int PosX = 180 + (Main.screenWidth - 800) / 2;

            Vector2 CurrentPos;
            Vector2 baseScale = new(0.9f);
            Vector2 UnitX = new(1f);

            Vector2 stringSize = ChatManager.GetStringSize(font, focusText1, baseScale, -1f);
            if (stringSize.X > 260f)
            {
                UnitX.X *= 260f / stringSize.X;
            }
            CurrentPos = new(PosX + stringSize.X * UnitX.X + 30f, PosY);

            string text2 = Lang.inter[52].Value;
            stringSize = ChatManager.GetStringSize(font, text2, baseScale, -1f);
            UnitX = new(1f);
            CurrentPos = new(CurrentPos.X + stringSize.X * UnitX.X + 30f, PosY);

            if (!string.IsNullOrWhiteSpace(focusText3))
            {
                stringSize = ChatManager.GetStringSize(font, focusText3, baseScale, -1f);
                CurrentPos = new(CurrentPos.X + stringSize.X * UnitX.X + 30f, PosY);
            }

            if (Main.player[Main.myPlayer].currentShoppingSettings.HappinessReport != "")
            {
                string text4 = Language.GetTextValue("UI.NPCCheckHappiness");
                stringSize = ChatManager.GetStringSize(font, text4, baseScale, -1f);
                CurrentPos = new(CurrentPos.X + stringSize.X * UnitX.X + 30f, PosY);
            }

            //CurrentPos = new (CurrentPos.X + stringSize.X * UnitX.X + 30f, PosY);
            return CurrentPos;
        }

    }
}
