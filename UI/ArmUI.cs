using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace NPCAttacker.UI
{

    public class ArmUI : UIState
    {
        private static VanillaItemSlotWrapper SlotWeapon;
        private static VanillaItemSlotWrapper SlotAltWeapon;
        private static VanillaItemSlotWrapper SlotArmor;
        private static UIText ButtonAlterUseType;
        private static UIText ButtonChannelType;
        public static bool Visible = false;
        const int slotX = 50;
        const int slotY1 = 270;
        const int slotY2 = 360;
        const int slotY3 = 440;
        const int slotY4 = 500;
        const int slotY5 = 530;
        public override void OnInitialize()
        {
            SlotWeapon = new VanillaItemSlotWrapper(ItemSlot.Context.BankItem, 0.85f)
            {
                Left = { Pixels = slotX },
                Top = { Pixels = slotY1 },
                ValidItemFunc = new Func<Item, bool>(ValidWeapon)
            };
            SlotAltWeapon = new VanillaItemSlotWrapper(ItemSlot.Context.BankItem, 0.85f)
            {
                Left = { Pixels = slotX },
                Top = { Pixels = slotY2 },
                ValidItemFunc = new Func<Item, bool>(ValidAltWeapon)
            };
            SlotArmor = new VanillaItemSlotWrapper(ItemSlot.Context.BankItem, 0.85f)
            {
                Left = { Pixels = slotX },
                Top = { Pixels = slotY3 },
                ValidItemFunc = new Func<Item, bool>(ValidArmor)
            };

            Append(SlotWeapon);
            Append(SlotAltWeapon);
            Append(SlotArmor);

            ButtonAlterUseType = new UIText("")
            {
                Left = { Pixels = slotX },
                Top = { Pixels = slotY4 },
                Width = { Pixels = 30 },
                Height = { Pixels = 30 },
            };

            ButtonChannelType = new UIText("")
            {
                Left = { Pixels = slotX },
                Top = { Pixels = slotY5 },
                Width = { Pixels = 30 },
                Height = { Pixels = 30 },
                
            };
            ButtonAlterUseType.OnLeftClick += ButtonAlterUseType_OnLeftClick;
            ButtonChannelType.OnLeftClick += ButtonChannelType_OnLeftClick;
            ButtonAlterUseType.OnRightClick += ButtonAlterUseType_OnRightClick;
            ButtonChannelType.OnRightClick += ButtonChannelType_OnRightClick;
            Append(ButtonAlterUseType);
            Append(ButtonChannelType);

        }



        public static bool ValidWeapon(Item item)
        {
            if (item.IsAir) return true;
            if (item.type == 5129 || item.type == ItemID.RottenEgg) return true;
            if (item.useStyle == ItemUseStyleID.None) return false;
            if (Main.LocalPlayer.talkNPC == -1) return false;

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
                    //悠悠球
                    if (ItemID.Sets.Yoyo[item.type]) return true;
                    if (ItemID.Sets.Spears[item.type]) return false;
                    //可消耗类多堆叠远程和近战武器
                    if (item.DamageType == DamageClass.Ranged && item.stack >= Math.Min(99, item.maxStack) && item.consumable && item.stack > 1) return true;
                    if ((item.DamageType == DamageClass.Melee || item.DamageType == DamageClass.MeleeNoSpeed) && item.stack >= Math.Min(99, item.maxStack) && item.consumable && item.stack > 1) return true;
                    //非矛挥舞无近战判定武器
                    if ((item.DamageType == DamageClass.Melee || item.DamageType == DamageClass.MeleeNoSpeed) && (item.useStyle == ItemUseStyleID.Swing || item.useStyle == ItemUseStyleID.Shoot) && item.noUseGraphic) return true;
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
                    if (item.shoot == ProjectileID.None && !item.noMelee && !item.noUseGraphic && item.useStyle == ItemUseStyleID.Swing) return true;  //真近战
                    if (item.shoot > ProjectileID.None && item.useStyle == ItemUseStyleID.Swing) return true;//射弹剑
                }
                return false;

            }
            return false;

        }

        public static bool ValidAltWeapon(Item item)
        {
            if (item.IsAir) return true;
            if (item.type == 5129 || item.type == ItemID.RottenEgg) return true;
            if (Main.LocalPlayer.talkNPC == -1) return false;
            if (item.channel) return false;
            NPC npc = Main.npc[Main.LocalPlayer.talkNPC];
            if (NPCID.Sets.AttackType[npc.type] == 1)           //远程只能装备弹药
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
                return false;
            }
            else if (NPCID.Sets.AttackType[npc.type] == 2 && npc.type != NPCID.Dryad)            //法师只能装备魔瓶
            {
                if (!ArmedGNPC.GetWeapon(npc).IsAir)
                {
                    return (item.healMana > 0) && item.stack >= Math.Min(30, item.maxStack) && ArmedGNPC.GetWeapon(npc).mana > 0;
                }
            }
            else if (NPCID.Sets.AttackType[npc.type] == 3)              //近战只能装备药剂
            {
                if (item.buffType > 0 && item.stack >= Math.Min(30, item.maxStack))
                {
                    return BuffID.Sets.IsAFlaskBuff[item.buffType];
                }
                return false;
            }
            else if (NPCID.Sets.AttackType[npc.type] == 0 && npc.type != NPCID.Nurse)          //投掷只能装备副武器
            {
                if (item.shoot <= ProjectileID.None) return false;

                if (!ArmedGNPC.GetWeapon(npc).IsAir)           //不能装备相同武器
                {
                    if (item.type == ArmedGNPC.GetWeapon(npc).type)
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
                    if (ItemID.Sets.Spears[item.type]) return false;
                    //可消耗类多堆叠远程和近战武器
                    if (item.DamageType == DamageClass.Ranged && item.stack >= Math.Min(99, item.maxStack) && item.consumable && item.stack > 1) return true;
                    if ((item.DamageType == DamageClass.Melee || item.DamageType == DamageClass.MeleeNoSpeed) && item.stack >= Math.Min(99, item.maxStack) && item.consumable && item.stack > 1) return true;
                    //非矛挥舞无近战判定武器
                    if ((item.DamageType == DamageClass.Melee || item.DamageType == DamageClass.MeleeNoSpeed) && (item.useStyle == ItemUseStyleID.Swing || item.useStyle == ItemUseStyleID.Shoot) && item.noUseGraphic) return true;
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

        public bool ValidArmor(Item item)
        {
            if (item.IsAir) return true;
            if (item.type == 5129 || item.type == ItemID.RottenEgg) return true;
            if (Main.LocalPlayer.talkNPC == -1) return false;

            return item.defense > 0;

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
                if (TalkNPC.TryGetGlobalNPC(out ArmedGNPC modnpc))
                {
                    modnpc.Weapon = NPCUtils.CloneItem(SlotWeapon.Item);
                    modnpc.WeaponAlt = NPCUtils.CloneItem(SlotAltWeapon.Item);
                    modnpc.Armor = NPCUtils.CloneItem(SlotArmor.Item);

                    string usetype = NPCAttacker.UITranslation("AlterUseType" + modnpc.AlterUseType.ToString());
                    ButtonAlterUseType.SetText(usetype);
                    ButtonAlterUseType.TextColor = ButtonAlterUseType.IsMouseHovering ? Color.Yellow : Color.White;
                    ButtonAlterUseType.Recalculate();

                    string channeltype = NPCAttacker.UITranslation("ChannelType" + modnpc.ChannelUseType.ToString());
                    ButtonChannelType.SetText(channeltype);
                    ButtonChannelType.TextColor = ButtonChannelType.IsMouseHovering ? Color.Yellow : Color.White;
                    ButtonChannelType.Recalculate();
                }

            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            Main.hidePlayerCraftingMenu = true;

            if (Main.LocalPlayer.talkNPC == -1) return;

            NPC talkNPC = Main.npc[Main.LocalPlayer.talkNPC];

            if (!talkNPC.TryGetGlobalNPC(out ArmedGNPC _)) return;

            Color TextColor = Color.White;

            #region 绘制武器栏
            if (!SlotWeapon.Item.IsAir)
            {
                string message = NPCAttacker.UITranslation("RemoveWeapon");
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, message, new Vector2(slotX + 50, slotY1 + 10), TextColor, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
                string ClassInfo = "";
                if (talkNPC.type == NPCID.Dryad)
                {
                    ClassInfo = NPCAttacker.UITranslation("DryadWeaponDescription");
                }
                else if (talkNPC.type == NPCID.Nurse)
                {
                    ClassInfo = NPCAttacker.UITranslation("NurseWeaponDescription");
                }
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, ClassInfo, new Vector2(slotX + 50, slotY1 + 60), TextColor, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
            }
            else
            {
                string message1 = NPCAttacker.UITranslation("PlaceAWeapon");
                //string message2 = NPCAttacker.UITranslation("ComplexNote");
                string ClassInfo = "";
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
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, message1, new Vector2(slotX + 50, slotY1 + 10), TextColor, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, ClassInfo, new Vector2(slotX + 50, slotY1 + 60), TextColor, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
                //ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Terraria.GameContent.FontAssets.MouseText.Value, message2, new Vector2(slotX + 50, slotY + 110), TextColor, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
            }
            #endregion

            #region 绘制副武器栏
            if (!SlotAltWeapon.Item.IsAir)         //副武器栏装了东西
            {
                string message = "";

                if (NPCID.Sets.AttackType[talkNPC.type] == 1)
                {
                    message = NPCAttacker.UITranslation("AmmosLoaded");
                    if (ArmedGNPC.GetWeapon(talkNPC).IsAir)
                    {
                        message = NPCAttacker.UITranslation("NoWeaponUseAmmos");
                    }
                }
                else if (NPCID.Sets.AttackType[talkNPC.type] == 2 && talkNPC.type != NPCID.Dryad)
                {
                    if (!ArmedGNPC.GetWeapon(talkNPC).IsAir)
                    {
                        if (NPCStats.ManaEnoughForPotions(ArmedGNPC.GetWeapon(talkNPC), SlotAltWeapon.Item.healMana) >= 1)
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
                else if (NPCID.Sets.AttackType[talkNPC.type] == 3)
                {
                    message = NPCAttacker.UITranslation("FlaskLoaded");
                }
                else if (NPCID.Sets.AttackType[talkNPC.type] == 0 && talkNPC.type != NPCID.Nurse)
                {
                    message = NPCAttacker.UITranslation("AltWeaponLoaded");
                }
                else if (talkNPC.type == NPCID.Dryad)
                {
                    message = NPCAttacker.UITranslation("BuffPotionsLoadedDryad");
                }
                else if (talkNPC.type == NPCID.Nurse)
                {
                    message = NPCAttacker.UITranslation("BuffPotionsLoadedNurse");
                }

                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, message, new Vector2(slotX + 50, slotY2 + 10), TextColor, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
            }
            else
            {
                string message1 = "";
                if (NPCID.Sets.AttackType[talkNPC.type] == 1)
                {
                    message1 = NPCAttacker.UITranslation("PlzPutAmmos");
                }
                else if (NPCID.Sets.AttackType[talkNPC.type] == 2 && talkNPC.type != NPCID.Dryad)
                {
                    message1 = NPCAttacker.UITranslation("PlzPutManaPotions");
                }
                else if (NPCID.Sets.AttackType[talkNPC.type] == 3)
                {
                    message1 = NPCAttacker.UITranslation("PlzPutFlasks");
                }
                else if (NPCID.Sets.AttackType[talkNPC.type] == 0 && talkNPC.type != NPCID.Nurse)
                {
                    message1 = NPCAttacker.UITranslation("PlzPutAltWeapon");
                }
                else if (talkNPC.type == NPCID.Dryad)
                {
                    message1 = NPCAttacker.UITranslation("PlzPutBuffPotions");
                }
                else if (talkNPC.type == NPCID.Nurse)
                {
                    message1 = NPCAttacker.UITranslation("PlzPutBuffPotions");
                }
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, message1, new Vector2(slotX + 50, slotY2 + 10), TextColor, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
            }
            #endregion

            #region 绘制盔甲栏
            if (!SlotArmor.Item.IsAir)
            {
                string message = NPCAttacker.UITranslation("ArmorLoaded");
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, message, new Vector2(slotX + 50, slotY3 + 10), TextColor, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
            }
            else
            {
                string message1 = NPCAttacker.UITranslation("PlzPutArmor");
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, message1, new Vector2(slotX + 50, slotY3 + 10), TextColor, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
            }
            #endregion

        }

        public static void CloseUI()
        {
            //if (!Visible) return;
            Visible = false;
            SlotWeapon.Item.TurnToAir();
            SlotAltWeapon.Item.TurnToAir();
            SlotArmor.Item.TurnToAir();
            ButtonAlterUseType.SetText("");
            ButtonChannelType.SetText("");
        }

        private void ButtonAlterUseType_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (Main.LocalPlayer.talkNPC == -1) return;
            NPC talkNPC = Main.npc[Main.LocalPlayer.talkNPC];
            if (talkNPC.TryGetGlobalNPC(out ArmedGNPC modnpc))
            {
                modnpc.AlterUseType = (modnpc.AlterUseType + 1) % 3;
            }
        }

        private void ButtonChannelType_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (Main.LocalPlayer.talkNPC == -1) return;
            NPC talkNPC = Main.npc[Main.LocalPlayer.talkNPC];
            if (talkNPC.TryGetGlobalNPC(out ArmedGNPC modnpc))
            {
                modnpc.ChannelUseType = (modnpc.ChannelUseType + 1) % 3;
            }
        }
        private void ButtonAlterUseType_OnRightClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (Main.LocalPlayer.talkNPC == -1) return;
            NPC talkNPC = Main.npc[Main.LocalPlayer.talkNPC];
            if (talkNPC.TryGetGlobalNPC(out ArmedGNPC modnpc))
            {
                modnpc.AlterUseType--;
                if (modnpc.AlterUseType < 0)
                {
                    modnpc.AlterUseType = 2;
                }
            }
        }
        private void ButtonChannelType_OnRightClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (Main.LocalPlayer.talkNPC == -1) return;
            NPC talkNPC = Main.npc[Main.LocalPlayer.talkNPC];
            if (talkNPC.TryGetGlobalNPC(out ArmedGNPC modnpc))
            {
                modnpc.ChannelUseType--;
                if (modnpc.ChannelUseType < 0)
                {
                    modnpc.ChannelUseType = 2;
                }
            }
        }



        public static void OpenUI()
        {
            //if (Visible) return;
            Visible = true;

            ButtonAlterUseType.SetText("");
            ButtonChannelType.SetText("");
            SlotWeapon.Item.TurnToAir();
            SlotAltWeapon.Item.TurnToAir();
            SlotArmor.Item.TurnToAir();

            if (Main.LocalPlayer.talkNPC != -1)
            {
                NPC TalkNPC = Main.npc[Main.LocalPlayer.talkNPC];
                if (TalkNPC.TryGetGlobalNPC(out ArmedGNPC modnpc))
                {
                    if (!modnpc.Weapon.IsAir)
                    {
                        SlotWeapon.Item = NPCUtils.CloneItem(modnpc.Weapon);
                    }
                    if (!modnpc.WeaponAlt.IsAir)
                    {
                        SlotAltWeapon.Item = NPCUtils.CloneItem(modnpc.WeaponAlt);
                    }
                    if (!modnpc.Armor.IsAir)
                    {
                        SlotArmor.Item = NPCUtils.CloneItem(modnpc.Armor);
                    }

                }
                

            }
        }
    }
}
