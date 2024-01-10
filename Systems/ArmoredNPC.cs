using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NPCAttacker.Override;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace NPCAttacker
{
    public class ArmedGNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        /// <summary>
        /// 主武器
        /// </summary>
        public Item Weapon = new();
        /// <summary>
        /// 副武器
        /// </summary>
        public Item WeaponAlt = new();
        /// <summary>
        /// NPC所用盔甲
        /// </summary>
        public Item Armor = new();
        /// <summary>
        /// 用于特殊用途的索敌NPC
        /// </summary>
        public int NPCTargetForSpecialUse = -1;
        /// <summary>
        /// 是否发动过近战攻击
        /// </summary>
        public bool MeleeAttacked = false;

        public ActMode actMode = ActMode.Default;
        /// <summary>
        /// 用于指示目标的NPC
        /// </summary>
        public int ActTargetNPC = -1;
        /// <summary>
        /// 是否被选中
        /// </summary>
        public bool Selected = false;

        public bool AlertMode = false;

        public int Team = 0;

        public override void SetDefaults(NPC npc)    //投掷型填0，远程型填1，魔法型填2，近战型填3
        {

        }
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.IsTownNPC();
        }

        public override bool PreAI(NPC npc)
        {
            NPCUtils.SwitchMode(npc);
            return true;
        }

        public override void AI(NPC npc)
        {
            if (actMode != ActMode.Default)
            {
                AlertMode = false;
            }
            if (NPCID.Sets.AttackType[npc.type] == 3)
            {
                if (npc.ai[0] != OverrideAI.MeleeAtk)
                {
                    MeleeAttacked = false;
                }
            }
            //装备臭虫剑和臭蛋自杀
            if (!Weapon.IsAir && (Weapon.type == 5129 || Weapon.type == ItemID.RottenEgg))
            {
                npc.StrikeInstantKill();
            }
        }

        public override void DrawTownAttackGun(NPC npc, ref Texture2D item, ref Rectangle itemFrame, ref float scale, ref int horizontalHoldoutOffset)/* tModPorter Note: closeness is now horizontalHoldoutOffset, use 'horizontalHoldoutOffset = Main.DrawPlayerItemPos(1f, itemtype) - originalClosenessValue' to adjust to the change. See docs for how to use hook with an item type. */
        {
            if (Weapon.IsAir) return;
            Main.instance.LoadItem(Weapon.type);
            item = TextureAssets.Item[Weapon.type].Value;
            itemFrame = new Rectangle(0, 0, TextureAssets.Item[Weapon.type].Value.Width, TextureAssets.Item[Weapon.type].Value.Height);
            scale = 1;// Weapon.scale;
            if (Weapon.noUseGraphic)
            {
                itemFrame = new Rectangle(0, 0, 1, 1);
            }
        }


        public override void DrawTownAttackSwing(NPC npc, ref Texture2D item, ref Rectangle itemFrame, ref int itemSize, ref float scale, ref Vector2 offset)
        {
            if (Weapon.IsAir) return;

            Main.instance.LoadItem(Weapon.type);
            item = TextureAssets.Item[Weapon.type].Value;
            itemFrame = new Rectangle(0, 0, TextureAssets.Item[Weapon.type].Value.Width, TextureAssets.Item[Weapon.type].Value.Height);
            scale = 1;// Weapon.scale;
            if (Weapon.noUseGraphic)
            {
                itemFrame = new Rectangle(0, 0, 1, 1);
            }
        }

        public override void TownNPCAttackSwing(NPC npc, ref int itemWidth, ref int itemHeight)
        {
            if (Weapon.IsAir) return;

            itemWidth = Weapon.width;
            itemHeight = Weapon.height;
        }

        public override void TownNPCAttackStrength(NPC npc, ref int damage, ref float knockback)
        {
            if (Weapon.IsAir) return;
            if (npc.type == NPCID.Dryad) return;
            if (npc.type == NPCID.Nurse)
            {
                damage += Weapon.healLife / 8;
                return;
            }
            damage = Weapon.damage;
            knockback = Weapon.knockBack;

            if (AmmoFix.PickAmmo(npc, Weapon) > 0)
            {
                damage += AmmoFix.GetAmmoDmg(npc, Weapon);
                knockback += AmmoFix.GetAmmoKB(npc, Weapon);
            }


            NPCStats.ModifyTotalDamage(npc, ref damage);
            NPCStats.ModifyTotalKB(npc, ref knockback);
        }


        public override void TownNPCAttackProjSpeed(NPC npc, ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            if (NPCUtils.BuffNPC())
            {
                randomOffset = 0;
                gravityCorrection = 0;
            }

            if (npc.type == NPCID.Nurse || npc.type == NPCID.Dryad) return;
            if (Weapon.IsAir)          //无武器时
            {
                NPCStats.ModifyShootSpeed(npc, ref multiplier);
            }
            else
            {
                multiplier = Weapon.shootSpeed;
                if (Weapon.useAmmo > 0)
                {
                    multiplier += AmmoFix.GetAmmoSpeed(npc, Weapon);
                }
                NPCStats.ModifyShootSpeed(npc, ref multiplier);
            }

        }

        public override void TownNPCAttackCooldown(NPC npc, ref int cooldown, ref int randExtraCooldown)
        {
            if (NPCUtils.BuffNPC())
            {
                randExtraCooldown = 1;
                if (npc.type == NPCID.Dryad)
                {
                    cooldown = 1;
                    return;
                }
            }
            if (npc.type != NPCID.Dryad)
            {
                cooldown = NPCStats.GetModifiedAttackTime(npc);
            }
        }

        public override void TownNPCAttackProj(NPC npc, ref int projType, ref int attackDelay)
        {
            if (Weapon.IsAir) return;
            if (npc.type == NPCID.Nurse || npc.type == NPCID.Dryad) return;
            attackDelay = 1;
        }

        public override bool? CanChat(NPC npc)
        {
            if (npc.IsTownNPC())
            {
                if (actMode != ActMode.Default || Selected)
                {
                    return false;
                }
            }
            return null;
        }


        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (npc.IsTownNPC())
            {
                if (Selected)
                {
                    NPCUtils.DrawNPCHealthBar(npc);

                    Color color = Color.White;
                    switch (Team)
                    {
                        case 1:
                            color = Color.Red;
                            break;
                        case 2:
                            color = Color.Yellow;
                            break;
                        case 3:
                            color = Color.Cyan;
                            break;
                        case 4:
                            color = Color.LightGreen;
                            break;
                        default:
                            break;
                    }
                    NPCUtils.DrawNPCTeamBar(npc, color);

                    NPCUtils.DrawNPCGuard(npc);
                }
            }
        }

        public override bool? CanFallThroughPlatforms(NPC npc)
        {
            if (NPCUtils.BuffNPC())
            {
                npc.stairFall = false;
                if (npc.GetGlobalNPC<ArmedGNPC>().actMode == ActMode.Move)
                {
                    if (Main.npc[npc.GetGlobalNPC<ArmedGNPC>().ActTargetNPC].Center.Y > npc.position.Y + npc.height)
                    {
                        return true;
                    }
                }
                return false;
            }
            return null;
        }

        public override void OnKill(NPC npc)
        {
            if (!Weapon.IsAir)
            {
                int itemtmp = Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, Weapon.type, Weapon.stack);
                Main.item[itemtmp] = NPCUtils.CloneItem(Weapon);
                Main.item[itemtmp].position = npc.position;
                Weapon.TurnToAir();
            }
            if (!WeaponAlt.IsAir)
            {
                int itemtmp = Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, WeaponAlt.type, WeaponAlt.stack);
                Main.item[itemtmp] = NPCUtils.CloneItem(WeaponAlt);
                Main.item[itemtmp].position = npc.position;
                WeaponAlt.TurnToAir();
            }
            if (!Armor.IsAir)
            {
                int itemtmp = Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, Armor.type, Armor.stack);
                Main.item[itemtmp] = NPCUtils.CloneItem(Armor);
                Main.item[itemtmp].position = npc.position;
                Armor.TurnToAir();
            }
        }


        public static Item GetWeapon(NPC npc)
        {
            if (npc.IsTownNPC())
            {
                return npc.GetGlobalNPC<ArmedGNPC>().Weapon;
            }
            else
            {
                return new Item();
            }
        }

        public static Item GetAltWeapon(NPC npc)
        {
            if (npc.IsTownNPC())
            {
                return npc.GetGlobalNPC<ArmedGNPC>().WeaponAlt;
            }
            else
            {
                return new Item();
            }
        }

        public static Item GetArmor(NPC npc)
        {
            if (npc.IsTownNPC())
            {
                return npc.GetGlobalNPC<ArmedGNPC>().Armor;
            }
            else
            {
                return new Item();
            }
        }


        public enum ActMode
        {
            Default,
            Attack,
            Move,
        }
    }
}