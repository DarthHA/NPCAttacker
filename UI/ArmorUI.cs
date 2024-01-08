using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.UI;
using Terraria.UI.Chat;

namespace NPCAttacker.UI
{
    // This class represents the UIState for our ExamplePerson Awesomeify chat function. It is similar to the Goblin Tinkerer's Reforge function, except it only gives Awesome and ReallyAwesome prefixes. 
    public class ArmorUI : UIState
    {
        private static VanillaItemSlotWrapper _vanillaItemSlot;
        public static bool Visible = false;
        public override void OnInitialize()
        {
            _vanillaItemSlot = new VanillaItemSlotWrapper(ItemSlot.Context.BankItem, 0.85f)
            {
                Left = { Pixels = 50 },
                Top = { Pixels = 530 },
                ValidItemFunc = new Func<Item, bool>(ValidItem)
            };
            // Here we limit the items that can be placed in the slot. We are fine with placing an empty item in or a non-empty item that can be prefixed. Calling Prefix(-3) is the way to know if the item in question can take a prefix or not.
            Append(_vanillaItemSlot);
        }

        public bool ValidItem(Item item)
        {
            if (item.IsAir) return true;
            if (Main.LocalPlayer.talkNPC == -1) return false;

            return item.defense > 0;

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
                TalkNPC.GetGlobalNPC<ArmedGNPC>().Armor = NPCUtils.CloneItem(_vanillaItemSlot.Item);
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            // This will hide the crafting menu similar to the reforge menu. For best results this UI is placed before "Vanilla: Inventory" to prevent 1 frame of the craft menu showing.
            Main.hidePlayerCraftingMenu = true;

            const int slotX = 50;
            const int slotY = 530;

            if (Main.LocalPlayer.talkNPC == -1) return;

            if (!_vanillaItemSlot.Item.IsAir)
            {
                string message = NPCAttacker.UITranslation("ArmorLoaded");
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Terraria.GameContent.FontAssets.MouseText.Value, message, new Vector2(slotX + 50, slotY), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
            }
            else
            {
                string message1 = NPCAttacker.UITranslation("PlzPutArmor");
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Terraria.GameContent.FontAssets.MouseText.Value, message1, new Vector2(slotX + 50, slotY), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
            }
        }

        public static void CloseUI()
        {
            //if (!Visible) return;
            Visible = false;
            _vanillaItemSlot.Item.TurnToAir();
        }

        public static void OpenUI()
        {
            //if (Visible) return;
            Visible = true;

            if (Main.LocalPlayer.talkNPC != -1)
            {
                NPC TalkNPC = Main.npc[Main.LocalPlayer.talkNPC];
                if (!TalkNPC.GetGlobalNPC<ArmedGNPC>().Armor.IsAir)
                {
                    _vanillaItemSlot.Item = NPCUtils.CloneItem(TalkNPC.GetGlobalNPC<ArmedGNPC>().Armor);
                }
            }
        }
    }
}
