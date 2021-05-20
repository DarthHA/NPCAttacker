using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NPCAttacker.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.UI;
using Terraria.UI.Chat;

namespace NPCAttacker.UI
{
    // This class represents the UIState for our ExamplePerson Awesomeify chat function. It is similar to the Goblin Tinkerer's Reforge function, except it only gives Awesome and ReallyAwesome prefixes. 
    public class ArmUI : UIState
	{
		private static VanillaItemSlotWrapper _vanillaItemSlot;
		public static bool Visible = false;
		public override void OnInitialize()
		{
			_vanillaItemSlot = new VanillaItemSlotWrapper(ItemSlot.Context.BankItem, 0.85f)
			{
				Left = { Pixels = 50 },
				Top = { Pixels = 270 },
				ValidItemFunc = new System.Func<Item, bool>(ValidItem)
			};
			// Here we limit the items that can be placed in the slot. We are fine with placing an empty item in or a non-empty item that can be prefixed. Calling Prefix(-3) is the way to know if the item in question can take a prefix or not.
			Append(_vanillaItemSlot);
		}

		public bool ValidItem(Item item)
        {
			if (item.IsAir) return true;
			if (item.damage == 0 || item.accessory || item.channel) return false;
			if (Main.LocalPlayer.talkNPC == -1) return true;
			NPC npc = Main.npc[Main.LocalPlayer.talkNPC];
			if (NPCID.Sets.AttackType[npc.type] == 0)
			{
				return item.thrown;
			}
			else if (NPCID.Sets.AttackType[npc.type] == 1)
            {
				return item.ranged;
            }
			else if (NPCID.Sets.AttackType[npc.type] == 2)
            {
				return item.magic;
            }
			else if (NPCID.Sets.AttackType[npc.type] == 3)
            {
				return item.melee && !item.noMelee && !item.noUseGraphic;
            }
            else
            {
				return false;
            }
        }
		public override void OnDeactivate() 
		{

		}

		// Update is called on a UIState while it is the active state of the UserInterface.
		// We use Update to handle automatically closing our UI when the player is no longer talking to our Example Person NPC.
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			if (Main.LocalPlayer.talkNPC == -1)
			{
				CloseUI();
			}
			else
			{
				NPC TalkNPC = Main.npc[Main.LocalPlayer.talkNPC];
				TalkNPC.GetGlobalNPC<ArmedGNPC>().Weapon = NPCAttacker.CloneItem(_vanillaItemSlot.Item);
			}
		}

		protected override void DrawSelf(SpriteBatch spriteBatch) 
		{
			base.DrawSelf(spriteBatch);

			// This will hide the crafting menu similar to the reforge menu. For best results this UI is placed before "Vanilla: Inventory" to prevent 1 frame of the craft menu showing.
			Main.HidePlayerCraftingMenu = true;

			const int slotX = 50;
			const int slotY = 270;

			if (Main.mouseX > slotX && Main.mouseX < slotX + _vanillaItemSlot.Width.GetValue(0f))
			{
				if (Main.mouseY > slotY && Main.mouseX < slotY + _vanillaItemSlot.Height.GetValue(0f))
				{
					Main.hoverItemName = TranslationUtils.GetTranslation("ArmUIHoverText");
				}
			}

			if (!_vanillaItemSlot.Item.IsAir)
			{
				string message = TranslationUtils.GetTranslation("ArmUIarmered");
				ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, message, new Vector2(slotX + 50, slotY), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
			}
			else
			{
				string message1 = TranslationUtils.GetTranslation("ArmUIunarmered1");
				string message2 = TranslationUtils.GetTranslation("ArmUIunarmered2");
				ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, message1, new Vector2(slotX + 50, slotY), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
				ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, message2, new Vector2(slotX + 50, slotY + 50), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
			}
		}

		public static void CloseUI()
        {
			if (!Visible) return;
			Visible = false;
			_vanillaItemSlot.Item.TurnToAir();
		}

		public static void OpenUI()
		{
			if (Visible) return;
			Visible = true;

			if (Main.LocalPlayer.talkNPC != -1)
			{
				NPC TalkNPC = Main.npc[Main.LocalPlayer.talkNPC];
				if (!TalkNPC.GetGlobalNPC<ArmedGNPC>().Weapon.IsAir)
				{
					_vanillaItemSlot.Item = NPCAttacker.CloneItem(TalkNPC.GetGlobalNPC<ArmedGNPC>().Weapon);
				}
			}
		}
	}
}
