using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;

namespace NPCAttacker.UI
{
    public class UINPCExtraButton : UIState
    {
        public UIImageButton TextButton;
        private bool HasHover = false;
        public static bool Visible = false;
        public override void OnActivate()
        {
            Vector2 Pos = Get5ButtonPos(NPCAttacker.FocusText1, NPCAttacker.FocusText3);

            string LocalizedText = NPCAttacker.UITranslation("Arm");
            Vector2 Size = Get5ButtonSize(LocalizedText);
            Main.instance.LoadItem(ItemID.BlackDye);
            TextButton = new UIImageButton(Terraria.GameContent.TextureAssets.Item[ItemID.BlackDye]);
            TextButton.Left.Set(Pos.X, 0f);
            TextButton.Top.Set(Pos.Y, 0f);
            TextButton.Width.Set(Size.X, 0f);
            TextButton.Height.Set(Size.Y, 0f);
            TextButton.OnLeftClick += Chat_OnClick;

            Append(TextButton);
            base.OnActivate();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Vector2 Pos = Get5ButtonPos(NPCAttacker.FocusText1, NPCAttacker.FocusText3);
            string LocalizedText = NPCAttacker.UITranslation("Arm");
            Vector2 Size = Get5ButtonSize(LocalizedText);
            TextButton.Left.Set(Pos.X, 0f);
            TextButton.Top.Set(Pos.Y, 0f);
            TextButton.Width.Set(Size.X, 0f);
            TextButton.Height.Set(Size.Y, 0f);
            Append(TextButton);
            if (MouseHovering() && !HasHover)
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                HasHover = true;
            }
            if (!MouseHovering() && HasHover)
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                HasHover = false;
            }
        }

        private void Chat_OnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            SoundEngine.PlaySound(SoundID.MenuOpen);
            Main.playerInventory = true;
            // remove the chat window...
            Main.npcChatText = "";
            // and start an instance of our UIState.
            ArmUI.OpenUI();
            ArmorUI.OpenUI();
            if (Main.LocalPlayer.talkNPC != -1)
            {
                if (NPCID.Sets.AttackType[Main.npc[Main.LocalPlayer.talkNPC].type] >= 0 && NPCID.Sets.AttackType[Main.npc[Main.LocalPlayer.talkNPC].type] <= 3)
                {
                    ArmAltUI.OpenUI();
                }
            }
            // Note that even though we remove the chat window, Main.LocalPlayer.talkNPC will still be set correctly and we are still technically chatting with the npc.
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 Pos = Get5ButtonPos(NPCAttacker.FocusText1, NPCAttacker.FocusText3);
            string LocalizedText = NPCAttacker.UITranslation("Arm");
            Vector2 Size = Get5ButtonSize(LocalizedText);
            Vector2 Scale = new(0.9f);
            if (MouseHovering()) Scale *= 1.1f;
            Vector2 Unit = new(1f);
            Color chatColor = Color.Red;
            if (Main.LocalPlayer.talkNPC != -1)
            {
                if (Main.npc[Main.LocalPlayer.talkNPC].type == NPCID.Nurse || Main.npc[Main.LocalPlayer.talkNPC].type == NPCID.Dryad)
                {
                    chatColor = CombatText.HealLife;
                }
            }
            ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Terraria.GameContent.FontAssets.MouseText.Value, LocalizedText, Pos + Size * Unit * 0.5f, chatColor, 0f, Size * 0.5f, Scale * Unit, -1f, 2f);
        }

        public bool MouseHovering()
        {
            Vector2 Pos = Get5ButtonPos(NPCAttacker.FocusText1, NPCAttacker.FocusText3);
            string LocalizedText = NPCAttacker.UITranslation("Arm");
            Vector2 Size = Get5ButtonSize(LocalizedText);
            if (Main.mouseX > Pos.X && Main.mouseX < Pos.X + Size.X)
            {
                if (Main.mouseY > Pos.Y && Main.mouseY < Pos.Y + Size.Y)
                {
                    return true;
                }
            }
            return false;
        }

        public Vector2 Get5ButtonPos(string focusText1, string focusText3)
        {
            DynamicSpriteFont font = Terraria.GameContent.FontAssets.MouseText.Value;
            Terraria.Utils.WordwrapString(Main.npcChatText, font, 460, 10, out int numLines);
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

        public Vector2 Get5ButtonSize(string focusText4)
        {
            Vector2 baseScale = new(0.9f);
            return ChatManager.GetStringSize(Terraria.GameContent.FontAssets.MouseText.Value, focusText4, baseScale);
        }


    }
}
