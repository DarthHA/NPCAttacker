using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
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
			Vector2 Pos = Get4ButtonPos(NPCAttacker.FocusText1, NPCAttacker.FocusText3);

			string LocalizedText = TranslationUtils.GetTranslation("Arm");
			Vector2 Size = Get4ButtonSize(LocalizedText);
			TextButton = new UIImageButton(Main.itemTexture[ItemID.BlackDye]);
			TextButton.Left.Set(Pos.X, 0f);
			TextButton.Top.Set(Pos.Y, 0f);
			TextButton.Width.Set(Size.X, 0f);
			TextButton.Height.Set(Size.Y, 0f);
			TextButton.OnClick += Chat_OnClick;

			Append(TextButton);
			base.OnActivate();
        }

        public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			Vector2 Pos = Get4ButtonPos(NPCAttacker.FocusText1, NPCAttacker.FocusText3);
			string LocalizedText = TranslationUtils.GetTranslation("Arm");
			Vector2 Size = Get4ButtonSize(LocalizedText);
			TextButton.Left.Set(Pos.X, 0f);
			TextButton.Top.Set(Pos.Y, 0f);
			TextButton.Width.Set(Size.X, 0f);
			TextButton.Height.Set(Size.Y, 0f);
			Append(TextButton);
			if (MouseHovering() && !HasHover)
            {
				Main.PlaySound(SoundID.MenuTick, -1, -1, 1, 1f, 0f);
				HasHover = true;
            }
			if (!MouseHovering() && HasHover)
			{
				Main.PlaySound(SoundID.MenuTick, -1, -1, 1, 1f, 0f);
				HasHover = false;
			}
		}

        private void Chat_OnClick(UIMouseEvent evt, UIElement listeningElement)
        {
			Main.PlaySound(SoundID.MenuOpen, -1, -1, 1, 1f, 0f);
			Main.playerInventory = true;
			// remove the chat window...
			Main.npcChatText = "";
			// and start an instance of our UIState.
			ArmUI.OpenUI();
			// Note that even though we remove the chat window, Main.LocalPlayer.talkNPC will still be set correctly and we are still technically chatting with the npc.
		}


		public override void Draw(SpriteBatch spriteBatch)
		{
			Vector2 Pos = Get4ButtonPos(NPCAttacker.FocusText1, NPCAttacker.FocusText3);
			string LocalizedText = TranslationUtils.GetTranslation("Arm");
			Vector2 Size = Get4ButtonSize(LocalizedText);
			Vector2 Scale = new Vector2(0.9f);
			if (MouseHovering()) Scale *= 1.1f;
			Vector2 Unit = new Vector2(1f);
			Color chatColor = Color.Red;
			if (Main.LocalPlayer.talkNPC != -1)
			{
				if (Main.npc[Main.LocalPlayer.talkNPC].type == NPCID.Nurse)
				{
					chatColor = CombatText.HealLife;
				}
			}
			ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, LocalizedText, Pos + Size * Unit * 0.5f, chatColor, 0f, Size * 0.5f, Scale * Unit, -1f, 2f);
		}

		public bool MouseHovering()
		{
			Vector2 Pos = Get4ButtonPos(NPCAttacker.FocusText1, NPCAttacker.FocusText3);
			string LocalizedText = TranslationUtils.GetTranslation("Arm");
			Vector2 Size = Get4ButtonSize(LocalizedText);
			if (Main.mouseX > Pos.X && Main.mouseX < Pos.X + Size.X)
            {
				if (Main.mouseY > Pos.Y && Main.mouseY < Pos.Y + Size.Y)
                {
					return true;
                }
            }
			return false;
        }

		public Vector2 Get4ButtonPos(string focusText1, string focusText3)
		{
			Utils.WordwrapString(Main.npcChatText, Main.fontMouseText, 460, 10, out int numLines);
			numLines++;

			int XPos = 180 + (Main.screenWidth - 800) / 2;
			float YPos = 130 + numLines * 30;
			Vector2 baseScale = new Vector2(0.9f);
			Vector2 stringSize = ChatManager.GetStringSize(Main.fontMouseText, focusText1, baseScale);
			Vector2 Unit = new Vector2(1f);
			if (stringSize.X > 260f)
			{
				Unit.X *= 260f / stringSize.X;
			}

			Vector2 CurrentPos = new Vector2(XPos + stringSize.X * Unit.X + 30f, YPos);
			string focusText2 = Language.GetTextValue("LegacyMenu.52");
			stringSize = ChatManager.GetStringSize(Main.fontMouseText, focusText2, baseScale);
			Unit = new Vector2(1f);
			if (stringSize.X > 260f)
			{
				Unit.X *= 260f / stringSize.X;
			}

			if (focusText3 != "")
			{
				CurrentPos = new Vector2(CurrentPos.X + stringSize.X * Unit.X + 30f, YPos);
				stringSize = ChatManager.GetStringSize(Main.fontMouseText, focusText3, baseScale);
				Unit = new Vector2(1f);
				if (stringSize.X > 260f)
				{
					Unit.X *= 260f / stringSize.X;
				}
			}

			CurrentPos = new Vector2(CurrentPos.X + stringSize.X * Unit.X + 30f, YPos);

			return CurrentPos;
		}

		public Vector2 Get4ButtonSize(string focusText4)
		{
			Vector2 baseScale = new Vector2(0.9f);
			return ChatManager.GetStringSize(Main.fontMouseText, focusText4, baseScale);
		}


	}
}
