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
            Append(_vanillaItemSlot);
        }

        public bool ValidItem(Item item)
        {
            if (item.IsAir) return true;
            if (item.useStyle == ItemUseStyleID.None) return false;
            if (Main.LocalPlayer.talkNPC == -1) return true;
            NPC npc = Main.npc[Main.LocalPlayer.talkNPC];
            if (NPCID.Sets.AttackType[npc.type] == 0)               //投掷类武器
            {
                if (npc.type == NPCID.Nurse)
                {
                    return (item.healLife > 0) && item.stack >= Math.Min(30, item.maxStack);
                }
                if (item.shoot <= ProjectileID.None || item.damage == 0) return false;

                if (!ArmedGNPC.GetAltWeapon(npc).IsAir)             //不能重复装备武器
                {
                    if (item.type == ArmedGNPC.GetAltWeapon(npc).type)
                    {
                        return false;
                    }
                }
                if (item.ModItem == null)          //原版武器
                {
                    //可消耗类多堆叠远程武器
                    if (item.DamageType == DamageClass.Ranged && item.stack >= Math.Min(99, item.maxStack) && item.consumable && item.stack > 1) return true;
                    if (WeaponClassify.UseMeleeThrowWeapon(item)) return true;
                    if (WeaponClassify.UseFlailWeapon(item)) return true;
                    if (WeaponClassify.UseFlailWeapon2(item)) return true;
                }
                else
                {
                    //可消耗类多堆叠远程和近战武器
                    if (item.DamageType == DamageClass.Ranged && item.stack >= Math.Min(99, item.maxStack) && item.consumable && item.stack > 1) return true;
                    if (item.DamageType == DamageClass.Melee && item.stack >= Math.Min(99, item.maxStack) && item.consumable && item.stack > 1) return true;
                    //悠悠球
                    if (ItemID.Sets.Yoyo[item.type]) return true;
                    //非矛挥舞无近战判定武器
                    if (item.DamageType == DamageClass.Melee && !ItemID.Sets.Spears[item.type] && item.useStyle == ItemUseStyleID.Swing && item.noMelee) return true;
                }
                return false;
            }
            else if (NPCID.Sets.AttackType[npc.type] == 1)                //远程武器
            {
                if (item.useAmmo == AmmoID.Coin) return true;        //钱币枪豁免
                if (item.type == ItemID.Clentaminator) return false;
                if (item.shoot <= ProjectileID.None) return false;
                if (item.DamageType == DamageClass.Ranged && item.maxStack == 1 && !item.consumable)            //不可堆叠不可消耗的远程武器
                {
                    if (item.useAmmo == AmmoID.None)             //不需要弹药
                    {
                        if (item.damage > 0) return true;            //无弹药需要保证自己有威力
                    }
                    else      //需要弹药
                    {
                        if (ArmedGNPC.GetAltWeapon(npc).IsAir) return true;           //无弹药装填
                        else if (ArmedGNPC.GetAltWeapon(npc).ammo == item.useAmmo) return true;       //弹药合理装填
                    }
                }

                return false;
            }
            else if (NPCID.Sets.AttackType[npc.type] == 2)         //法师武器
            {
                if (npc.type == NPCID.Dryad)
                {
                    return (item.healMana > 0) && item.stack >= Math.Min(30, item.maxStack);
                }
                if (item.shoot <= ProjectileID.None || item.damage == 0) return false;
                return item.DamageType == DamageClass.Magic && item.useAmmo == AmmoID.None && item.type != ItemID.SoulDrain;
            }
            else if (NPCID.Sets.AttackType[npc.type] == 3)              //近战
            {
                if (item.damage == 0) return false;
                if (WeaponClassify.UseSpearWeapon(item)) return true;//矛类
                if (WeaponClassify.UseWhipWeapon(item)) return true;//鞭类
                if (WeaponClassify.UseShortSword(item)) return true;//短剑类
                if (item.DamageType == DamageClass.MeleeNoSpeed || item.DamageType == DamageClass.Melee)
                {
                    if (item.shoot == ProjectileID.None && !item.noMelee && !item.noUseGraphic) return true;  //真近战
                    if (item.shoot > ProjectileID.None && item.useStyle == ItemUseStyleID.Swing) return true;//射弹剑
                    if (item.shoot > ProjectileID.None && (!item.noMelee || !item.noUseGraphic)) return true;//射弹剑2
                }
                return false;

            }
            return false;

        }
        public override void OnDeactivate()
        {

        }


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
