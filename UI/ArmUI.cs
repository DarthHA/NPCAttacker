using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
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
                ValidItemFunc = new Func<Item, bool>(ValidItem)
            };
            // Here we limit the items that can be placed in the slot. We are fine with placing an empty item in or a non-empty item that can be prefixed. Calling Prefix(-3) is the way to know if the item in question can take a prefix or not.
            Append(_vanillaItemSlot);
        }

        public bool ValidItem(Item item)
        {
            if (item.IsAir) return true;
            if (item.accessory || item.channel) return false;
            if (Main.LocalPlayer.talkNPC == -1) return true;
            NPC npc = Main.npc[Main.LocalPlayer.talkNPC];
            if (NPCID.Sets.AttackType[npc.type] == 0)
            {
                if (npc.type == NPCID.Nurse)
                {
                    return (item.healLife > 0) && item.stack >= Math.Min(30, item.maxStack);
                }
                if (item.ModItem == null)
                {
                    if (!ArmedGNPC.GetAltWeapon(npc).IsAir)
                    {
                        if (item.type == ArmedGNPC.GetAltWeapon(npc).type)
                        {
                            return false;
                        }
                    }
                    return (item.DamageType == DamageClass.Ranged && item.stack >= Math.Min(99, item.maxStack) && item.consumable) || MeleeWeaponFix.UseMeleeThrowWeapon(item);
                }
                else    //模组非悠悠球非矛非蓄力非阔剑武器
                {
                    if (item.DamageType == DamageClass.Ranged && item.stack >= Math.Min(99, item.maxStack) && item.consumable) return true;
                    else if (item.DamageType == DamageClass.Melee && item.stack >= Math.Min(99, item.maxStack) && item.consumable) return true;
                    else if (!ItemID.Sets.Spears[item.type] && !ItemID.Sets.Yoyo[item.type] && item.DamageType == DamageClass.Melee && item.useStyle == ItemUseStyleID.Swing) return true;
                    else return false;
                }
            }
            else if (NPCID.Sets.AttackType[npc.type] == 1)
            {
                if (item.ammo != AmmoID.None || item.maxStack > 1 || item.DamageType != DamageClass.Ranged) return false;
                if (item.type == ItemID.Clentaminator) return false;
                if (ArmedGNPC.GetAltWeapon(npc).IsAir)
                {
                    return true;
                }
                else
                {
                    return ArmedGNPC.GetAltWeapon(npc).ammo == item.useAmmo;
                }
            }
            else if (NPCID.Sets.AttackType[npc.type] == 2)
            {
                if (npc.type == NPCID.Dryad)
                {
                    return (item.healMana > 0) && item.stack >= Math.Min(30, item.maxStack);
                }
                return item.DamageType == DamageClass.Magic && item.useAmmo == AmmoID.None;
            }
            else if (NPCID.Sets.AttackType[npc.type] == 3)
            {
                //return item.DamageType == DamageClass.Melee && !item.noMelee && !item.noUseGraphic;
                return item.DamageType == DamageClass.Melee && !item.noUseGraphic && item.useStyle == ItemUseStyleID.Swing;
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
                TalkNPC.GetGlobalNPC<ArmedGNPC>().Weapon = NPCUtils.CloneItem(_vanillaItemSlot.Item);
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            // This will hide the crafting menu similar to the reforge menu. For best results this UI is placed before "Vanilla: Inventory" to prevent 1 frame of the craft menu showing.
            Main.hidePlayerCraftingMenu = true;

            const int slotX = 50;
            const int slotY = 270;

            if (!_vanillaItemSlot.Item.IsAir)
            {
                string message = NPCAttacker.UITranslation("RemoveWeapon");
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Terraria.GameContent.FontAssets.MouseText.Value, message, new Vector2(slotX + 50, slotY + 50), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);

                if (Main.LocalPlayer.talkNPC != -1)
                {
                    string ClassInfo = "";
                    NPC talkNPC = Main.npc[Main.LocalPlayer.talkNPC];
                    if (talkNPC.type == NPCID.Dryad)
                    {
                        ClassInfo = NPCAttacker.UITranslation("DryadWeaponDescription");
                    }
                    else if (talkNPC.type == NPCID.Nurse)
                    {
                        ClassInfo = NPCAttacker.UITranslation("NurseWeaponDescription");
                    }
                    ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Terraria.GameContent.FontAssets.MouseText.Value, ClassInfo, new Vector2(slotX + 50, slotY + 100), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
                }

            }
            else
            {
                string message1 = NPCAttacker.UITranslation("PlaceAWeapon");
                string message2 = NPCAttacker.UITranslation("ComplexNote");
                string ClassInfo = "";
                if (Main.LocalPlayer.talkNPC != -1)
                {
                    NPC talkNPC = Main.npc[Main.LocalPlayer.talkNPC];
                    switch (NPCID.Sets.AttackType[talkNPC.type])
                    {
                        case 0:
                            if (talkNPC.type == NPCID.Nurse)
                            {
                                ClassInfo = NPCAttacker.UITranslation("NurseWeapon");
                            }
                            else
                            {
                                ClassInfo = NPCAttacker.UITranslation("ThrowerWeapon");
                            }
                            break;
                        case 1:
                            ClassInfo = NPCAttacker.UITranslation("RangerWeapon");
                            break;
                        case 2:
                            if (talkNPC.type == NPCID.Dryad)
                            {
                                ClassInfo = NPCAttacker.UITranslation("DryadWeapon");
                            }
                            else
                            {
                                ClassInfo = NPCAttacker.UITranslation("MagicWeapon");
                            }
                            break;
                        case 3:
                            ClassInfo = NPCAttacker.UITranslation("MeleeWeapon");
                            break;
                    }
                }
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Terraria.GameContent.FontAssets.MouseText.Value, message1, new Vector2(slotX + 50, slotY + 50), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Terraria.GameContent.FontAssets.MouseText.Value, ClassInfo, new Vector2(slotX + 50, slotY + 100), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Terraria.GameContent.FontAssets.MouseText.Value, message2, new Vector2(slotX + 50, slotY + 150), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
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
                if (!TalkNPC.GetGlobalNPC<ArmedGNPC>().Weapon.IsAir)
                {
                    _vanillaItemSlot.Item = NPCUtils.CloneItem(TalkNPC.GetGlobalNPC<ArmedGNPC>().Weapon);
                }
            }
        }
    }
}
