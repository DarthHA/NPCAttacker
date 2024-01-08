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
    public class ArmAltUI : UIState
    {
        private static VanillaItemSlotWrapper _vanillaItemSlot;
        public static bool Visible = false;
        public override void OnInitialize()
        {
            _vanillaItemSlot = new VanillaItemSlotWrapper(ItemSlot.Context.BankItem, 0.85f)
            {
                Left = { Pixels = 50 },
                Top = { Pixels = 450 },
                ValidItemFunc = new Func<Item, bool>(ValidItem)
            };
            // Here we limit the items that can be placed in the slot. We are fine with placing an empty item in or a non-empty item that can be prefixed. Calling Prefix(-3) is the way to know if the item in question can take a prefix or not.
            Append(_vanillaItemSlot);
        }

        public bool ValidItem(Item item)
        {
            if (item.IsAir) return true;
            if (item.channel) return false;
            if (Main.LocalPlayer.talkNPC == -1) return false;
            NPC npc = Main.npc[Main.LocalPlayer.talkNPC];
            if (NPCID.Sets.AttackType[npc.type] == 1)
            {
                if (!ArmedGNPC.GetWeapon(npc).IsAir)
                {
                    if (ArmedGNPC.GetWeapon(npc).useAmmo != AmmoID.None && ArmedGNPC.GetWeapon(npc).useAmmo == item.ammo)
                    {
                        if (item.stack >= 999 || item.stack == item.maxStack)
                        {
                            return true;
                        }
                    }
                }
            }
            else if (NPCID.Sets.AttackType[npc.type] == 2 && npc.type != NPCID.Dryad)
            {
                if (!ArmedGNPC.GetWeapon(npc).IsAir)
                {
                    return (item.healMana > 0) && item.stack >= Math.Min(30, item.maxStack);
                }
            }
            else if (NPCID.Sets.AttackType[npc.type] == 3)
            {
                if (item.buffType > 0 && item.stack >= Math.Min(30, item.maxStack))
                {
                    if (BuffID.Sets.IsAFlaskBuff[item.buffType])
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else if (NPCID.Sets.AttackType[npc.type] == 0 && npc.type != NPCID.Nurse)
            {
                if (!ArmedGNPC.GetWeapon(npc).IsAir)
                {
                    if (item.type == ArmedGNPC.GetWeapon(npc).type)
                    {
                        return false;
                    }
                }
                if (item.ModItem == null)
                {
                    return (item.DamageType == DamageClass.Ranged && item.stack >= Math.Min(99, item.maxStack) && item.consumable) || MeleeWeaponFix.UseMeleeThrowWeapon(item);
                }
                else    //模组非悠悠球非矛非蓄力非阔剑武器
                {
                    if (item.DamageType == DamageClass.Ranged && item.stack >= Math.Min(99, item.maxStack) && item.consumable) return true;
                    else if (item.DamageType == DamageClass.Melee && item.stack >= Math.Min(99, item.maxStack) && item.consumable) return true;
                    else if (!ItemID.Sets.Spears[item.type] && !ItemID.Sets.Yoyo[item.type] && item.DamageType == DamageClass.Melee && item.noMelee && item.noUseGraphic) return true;
                    else return false;
                }
            }
            else if (npc.type == NPCID.Dryad)
            {
                return item.buffType > 0 && item.stack >= Math.Min(30, item.maxStack);
            }
            else if (npc.type == NPCID.Nurse)
            {
                return item.buffType > 0 && item.stack >= Math.Min(30, item.maxStack);
            }
            return false;
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
                TalkNPC.GetGlobalNPC<ArmedGNPC>().WeaponAlt = NPCUtils.CloneItem(_vanillaItemSlot.Item);
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            // This will hide the crafting menu similar to the reforge menu. For best results this UI is placed before "Vanilla: Inventory" to prevent 1 frame of the craft menu showing.
            Main.hidePlayerCraftingMenu = true;

            const int slotX = 50;
            const int slotY = 450;

            if (Main.LocalPlayer.talkNPC == -1) return;
            NPC npc = Main.npc[Main.LocalPlayer.talkNPC];

            if (!_vanillaItemSlot.Item.IsAir)         //副武器栏装了东西
            {
                string message = "";

                if (NPCID.Sets.AttackType[npc.type] == 1)
                {
                    message = NPCAttacker.UITranslation("AmmosLoaded");
                    if (ArmedGNPC.GetWeapon(npc).IsAir)
                    {
                        message = NPCAttacker.UITranslation("NoWeaponUseAmmos");
                    }
                }
                else if (NPCID.Sets.AttackType[npc.type] == 2 && npc.type != NPCID.Dryad)
                {
                    if (!ArmedGNPC.GetWeapon(npc).IsAir)
                    {
                        if (NPCStats.ManaEnoughForPotions(ArmedGNPC.GetWeapon(npc), _vanillaItemSlot.Item.healMana) >= 1)
                        {
                            message = NPCAttacker.UITranslation("ManaEnough");// + NPCStats.ManaEnoughForPotions(ArmedGNPC.GetWeapon(npc), _vanillaItemSlot.Item.healMana).ToString();
                        }
                        else
                        {
                            message = NPCAttacker.UITranslation("ManaNotEnough");// + NPCStats.ManaEnoughForPotions(ArmedGNPC.GetWeapon(npc), _vanillaItemSlot.Item.healMana).ToString(); ;
                        }
                    }
                    else
                    {
                        message = NPCAttacker.UITranslation("NoWeaponUseManaPotions");
                    }
                }
                else if (NPCID.Sets.AttackType[npc.type] == 3)
                {
                    message = NPCAttacker.UITranslation("FlaskLoaded");
                }
                else if (NPCID.Sets.AttackType[npc.type] == 0 && npc.type != NPCID.Nurse)
                {
                    message = NPCAttacker.UITranslation("AltWeaponLoaded");
                }
                else if (npc.type == NPCID.Dryad)
                {
                    message = NPCAttacker.UITranslation("BuffPotionsLoadedDryad");
                }
                else if (npc.type == NPCID.Nurse)
                {
                    message = NPCAttacker.UITranslation("BuffPotionsLoadedNurse");
                }

                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Terraria.GameContent.FontAssets.MouseText.Value, message, new Vector2(slotX + 50, slotY + 20), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
            }
            else
            {
                string message1 = "";
                if (NPCID.Sets.AttackType[npc.type] == 1)
                {
                    if (!ArmedGNPC.GetWeapon(npc).IsAir)
                    {
                        message1 = NPCAttacker.UITranslation("PlzPutAmmos");
                    }
                }
                else if (NPCID.Sets.AttackType[npc.type] == 2 && npc.type != NPCID.Dryad)
                {
                    if (!ArmedGNPC.GetWeapon(npc).IsAir)
                    {
                        message1 = NPCAttacker.UITranslation("PlzPutManaPotions");
                    }
                }
                else if (NPCID.Sets.AttackType[npc.type] == 3)
                {
                    message1 = NPCAttacker.UITranslation("PlzPutFlasks");
                }
                else if (NPCID.Sets.AttackType[npc.type] == 0 && npc.type != NPCID.Nurse)
                {
                    message1 = NPCAttacker.UITranslation("PlzPutAltWeapon");
                }
                else if (npc.type == NPCID.Dryad)
                {
                    message1 = NPCAttacker.UITranslation("PlzPutBuffPotions");
                }
                else if (npc.type == NPCID.Nurse)
                {
                    message1 = NPCAttacker.UITranslation("PlzPutBuffPotions");
                }
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Terraria.GameContent.FontAssets.MouseText.Value, message1, new Vector2(slotX + 50, slotY + 20), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
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
                if (!TalkNPC.GetGlobalNPC<ArmedGNPC>().WeaponAlt.IsAir)
                {
                    _vanillaItemSlot.Item = NPCUtils.CloneItem(TalkNPC.GetGlobalNPC<ArmedGNPC>().WeaponAlt);
                }
            }
        }

        public static bool SlotCanUseRange(NPC npc)
        {
            if (!npc.IsTownNPC()) return false;
            if (NPCID.Sets.AttackType[npc.type] != 1) return false;
            if (!ArmedGNPC.GetWeapon(npc).IsAir)
            {
                if (ArmedGNPC.GetWeapon(npc).useAmmo != AmmoID.None)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool SlotCanUseMagic(NPC npc)
        {
            if (!npc.IsTownNPC()) return false;
            if (NPCID.Sets.AttackType[npc.type] != 2) return false;
            if (!ArmedGNPC.GetWeapon(npc).IsAir)
            {
                if (ArmedGNPC.GetWeapon(npc).mana != 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
