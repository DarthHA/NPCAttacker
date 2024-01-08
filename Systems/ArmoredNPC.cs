using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Core.Platforms;
using NPCAttacker.Override;
using NPCAttacker.Projectiles;
using Terraria;
using Terraria.Audio;
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
        }

        public override void DrawTownAttackGun(NPC npc, ref Texture2D item, ref Rectangle itemFrame, ref float scale, ref int horizontalHoldoutOffset)/* tModPorter Note: closeness is now horizontalHoldoutOffset, use 'horizontalHoldoutOffset = Main.DrawPlayerItemPos(1f, itemtype) - originalClosenessValue' to adjust to the change. See docs for how to use hook with an item type. */
        {
            if (Weapon.IsAir) return;
            Main.instance.LoadItem(Weapon.type);
            item = TextureAssets.Item[Weapon.type].Value;
            scale = Weapon.scale;
        }


        public override void DrawTownAttackSwing(NPC npc, ref Texture2D item, ref Rectangle itemFrame, ref int itemSize, ref float scale, ref Vector2 offset)
        {
            if (Weapon.IsAir) return;
            Main.instance.LoadItem(Weapon.type);
            item = TextureAssets.Item[Weapon.type].Value;
            itemFrame = new Rectangle(0, 0, TextureAssets.Item[Weapon.type].Value.Width, TextureAssets.Item[Weapon.type].Value.Height);
            scale = 1;// Weapon.scale;
        }

        public override void TownNPCAttackSwing(NPC npc, ref int itemWidth, ref int itemHeight)
        {
            if (Weapon.IsAir) return;

            itemWidth = Weapon.width;
            itemHeight = Weapon.height;
            if (Weapon.shoot != ProjectileID.None)
            {
                if (!MeleeAttacked)
                {
                    MeleeAttacked = true;

                    bool ShouldShoot = true;
                    int shoot = Weapon.shoot;
                    int dmg = Weapon.damage;
                    float shootSpeed = Weapon.shootSpeed;
                    float Kb = Weapon.knockBack;

                    NPCStats.ModifyTotalDamage(npc, ref dmg);
                    NPCStats.ModifyShootSpeed(npc, ref shootSpeed);
                    NPCStats.ModifyTotalKB(npc, ref Kb);

                    Vector2 Pos = npc.Center;
                    Vector2 Speed;

                    if (actMode == ActMode.Attack)
                    {
                        Speed = Vector2.Normalize(Main.npc[ActTargetNPC].Center - npc.Center) * shootSpeed;
                    }
                    else
                    {
                        Speed = new Vector2(1, 0) * npc.direction * shootSpeed;
                    }

                    if (Weapon.ModItem != null)
                    {
                        Weapon.ModItem.ModifyShootStats(Main.LocalPlayer, ref Pos, ref Speed, ref shoot, ref dmg, ref Kb);
                        ShouldShoot = Weapon.ModItem.Shoot(Main.LocalPlayer, null, Pos, Speed, shoot, dmg, Kb);
                    }
                    if (ShouldShoot)
                    {

                        if (VanillaItemProjFix.TransFormProj(npc, Weapon.type) != -1)
                        {
                            shoot = VanillaItemProjFix.TransFormProj(npc, Weapon.type);
                        }

                        int protmp = Projectile.NewProjectile(null, Pos, Speed, shoot, dmg, Kb, Main.myPlayer);
                        Main.projectile[protmp].npcProj = true;
                        Main.projectile[protmp].noDropItem = true;

                        int CritChance = GetWeapon(npc).crit;
                        NPCStats.ModifyCritChance(npc, ref CritChance);

                        Main.projectile[protmp].CritChance += CritChance;

                    }
                    if (Weapon.UseSound != null)
                    {
                        SoundEngine.PlaySound(Weapon.UseSound, npc.Center);
                    }
                }
            }
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

            if (Weapon.useAmmo > 0)
            {
                damage += AmmoFix.GetAmmoDmg(npc, Weapon);
                knockback += AmmoFix.GetAmmoKB(npc, Weapon);
            }

            NPCStats.ModifyTotalDamage(npc, ref damage);
            NPCStats.ModifyTotalKB(npc, ref knockback);
        }


        public override void TownNPCAttackProjSpeed(NPC npc, ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            if (Weapon.IsAir) return;
            if (npc.type == NPCID.Nurse || npc.type == NPCID.Dryad) return;
            switch (NPCID.Sets.AttackType[npc.type])
            {
                case 0:
                    float k = multiplier;
                    multiplier = Weapon.shootSpeed;
                    NPCStats.ModifyShootSpeed(npc, ref multiplier);
                    gravityCorrection *= multiplier / k;
                    randomOffset = 0;
                    break;
                case 1:
                    multiplier = Weapon.shootSpeed;
                    if (Weapon.useAmmo > 0)
                    {
                        multiplier += AmmoFix.GetAmmoSpeed(npc, Weapon);
                    }
                    break;
                case 2:
                    multiplier = Weapon.shootSpeed;
                    randomOffset = 0;
                    break;
                default:
                    break;
            }

        }

        public override void TownNPCAttackCooldown(NPC npc, ref int cooldown, ref int randExtraCooldown)
        {
            if (npc.type == NPCID.Dryad && NPCUtils.BuffNPC())
            {
                cooldown = 1;
                randExtraCooldown = 1;
                return;
            }

            if (!Weapon.IsAir && npc.type != NPCID.Nurse)
            {
                cooldown = Weapon.useTime;
                randExtraCooldown = 1;
            }
            float SpeedBonus = 0;
            float SpeedBousDryad = 0;
            if (npc.HasBuff(BuffID.DryadsWard) && NPC.AnyNPCs(NPCID.Dryad))
            {
                if (!GetWeapon(Main.npc[NPC.FindFirstNPC(NPCID.Dryad)]).IsAir)
                {
                    SpeedBousDryad += GetWeapon(Main.npc[NPC.FindFirstNPC(NPCID.Dryad)]).healMana / 900f;
                    if (SpeedBousDryad > 0.5f) SpeedBousDryad = 0.5f;
                }
            }


            if (npc.HasBuff(BuffID.Battle))           //战争药水
            {
                SpeedBonus += 0.1f;
            }
            if (npc.HasBuff(BuffID.Calm))           //镇定药水
            {
                SpeedBonus -= 0.1f;
            }
            if (npc.HasBuff(BuffID.WellFed3))       //食物
            {
                SpeedBonus += 0.1f;
            }
            else if (npc.HasBuff(BuffID.WellFed2))
            {
                SpeedBonus += 0.075f;
            }
            else if (npc.HasBuff(BuffID.WellFed))
            {
                SpeedBonus += 0.05f;
            }
            if (npc.HasBuff(BuffID.Tipsy) && NPCID.Sets.AttackType[npc.type] == 3)
            {
                SpeedBonus += 0.1f;
            }
            cooldown = (int)(cooldown * (1 - SpeedBousDryad));
            cooldown = (int)(cooldown * (1 - SpeedBonus));
        }

        public override void TownNPCAttackProj(NPC npc, ref int projType, ref int attackDelay)
        {
            if (Weapon.IsAir) return;
            if (npc.type == NPCID.Nurse || npc.type == NPCID.Dryad) return;
            attackDelay = 1;

            
            if (NPCID.Sets.AttackType[npc.type] == 0 || NPCID.Sets.AttackType[npc.type] == 2)
            {
                int shoot = Weapon.shoot;

                if (npc.localAI[3] == 0)
                {
                    if (Weapon.ModItem != null)
                    {
                        int dmg = Weapon.damage;
                        float Kb = Weapon.knockBack;
                        float shootSpeed = Weapon.shootSpeed;

                        NPCStats.ModifyShootSpeed(npc, ref shootSpeed);
                        NPCStats.ModifyTotalDamage(npc, ref dmg);
                        NPCStats.ModifyTotalKB(npc, ref Kb);

                        Vector2 Pos = new(npc.Center.X + npc.spriteDirection * 16, npc.Center.Y - 2f);
                        Vector2 Speed;
                        if (NPCTargetForSpecialUse != -1)
                        {
                            if (Main.npc[NPCTargetForSpecialUse].active)
                            {
                                Speed = Vector2.Normalize(Main.npc[NPCTargetForSpecialUse].Center - npc.Center) * shootSpeed;
                            }
                            else
                            {
                                Speed = new Vector2(npc.direction, 0) * shootSpeed;
                            }
                        }
                        else
                        {
                            Speed = new Vector2(npc.direction, 0) * shootSpeed;
                        }


                        Weapon.ModItem.ModifyShootStats(Main.LocalPlayer, ref Pos, ref Speed, ref shoot, ref dmg, ref Kb);
                        if (Weapon.ModItem.Shoot(Main.LocalPlayer, null, Pos, Speed, shoot, dmg, Kb))
                        {
                            projType = shoot;
                        }
                        else
                        {
                            projType = 0;
                        }
                    }
                    else
                    {
                        projType = shoot;

                        if (VanillaItemProjFix.TransFormProj(npc, Weapon.type) != -1)
                        {
                            projType = VanillaItemProjFix.TransFormProj(npc, Weapon.type);
                        }

                    }
                }
                else
                {
                    projType = 0;
                }

            }
            else if (NPCID.Sets.AttackType[npc.type] == 1)                  //大头射手
            {
                if (!AmmoFix.HasAmmo(npc, Weapon))
                {
                    projType = 0;
                    return;
                }
                if (npc.localAI[3] == 0)
                {
                    if (Weapon.ModItem != null)
                    {
                        int shoot;
                        if (AmmoFix.PickAmmo(npc, Weapon) > 0)
                        {
                            shoot = AmmoFix.PickAmmo(npc, Weapon);
                        }
                        else
                        {
                            shoot = Weapon.shoot;
                        }

                        int dmg = Weapon.damage;
                        float Kb = Weapon.knockBack;
                        float shootSpeed = Weapon.shootSpeed;

                        if (Weapon.useAmmo > 0)
                        {
                            dmg += AmmoFix.GetAmmoDmg(npc, Weapon);
                            Kb += AmmoFix.GetAmmoKB(npc, Weapon);
                            shootSpeed += AmmoFix.GetAmmoSpeed(npc, Weapon);
                        }
                        NPCStats.ModifyTotalDamage(npc, ref dmg);
                        NPCStats.ModifyShootSpeed(npc, ref shootSpeed);
                        NPCStats.ModifyTotalKB(npc, ref Kb);

                        Vector2 Speed;
                        Vector2 Pos = new(npc.Center.X + npc.spriteDirection * 16, npc.Center.Y - 2f);
                        if (NPCTargetForSpecialUse != -1)
                        {
                            if (Main.npc[NPCTargetForSpecialUse].active)
                            {
                                Speed = Vector2.Normalize(Main.npc[NPCTargetForSpecialUse].Center - npc.Center) * shootSpeed;
                            }
                            else
                            {
                                Speed = new Vector2(npc.direction, 0) * shootSpeed;
                            }
                        }
                        else
                        {
                            Speed = new Vector2(npc.direction, 0) * shootSpeed;
                        }

                        Weapon.ModItem.ModifyShootStats(Main.LocalPlayer, ref Pos, ref Speed, ref shoot, ref dmg, ref Kb);
                        if (Weapon.ModItem.Shoot(Main.LocalPlayer, null, Pos, Speed, shoot, dmg, Kb))
                        {
                            projType = shoot;
                        }
                        else
                        {
                            projType = 0;
                        }
                    }
                    else
                    {
                        if (AmmoFix.PickAmmo(npc, Weapon) > 0)
                        {
                            bool ForceChange = false;
                            if (Weapon.useAmmo == AmmoID.Bullet || Weapon.useAmmo == AmmoID.Arrow || Weapon.useAmmo == AmmoID.Dart || Weapon.useAmmo == AmmoID.NailFriendly)
                            {
                                if (VanillaItemProjFix.IsUseSpecialProj[Weapon.type] != 0)
                                {
                                    ForceChange = true;
                                }
                            }
                            if (ForceChange)
                            {
                                projType = VanillaItemProjFix.IsUseSpecialProj[Weapon.type];
                            }
                            else
                            {
                                projType = AmmoFix.PickAmmo(npc, Weapon);
                            }

                        }
                        else
                        {
                            projType = Weapon.shoot;
                            if (Weapon.type == ItemID.Toxikarp)
                            {
                                projType = ProjectileID.ToxicBubble;
                            }
                            if (Weapon.type == ItemID.Harpoon)
                            {
                                projType = ProjectileID.Harpoon;
                            }
                        }

                        if (VanillaItemProjFix.TransFormProj(npc, Weapon.type) != -1)
                        {
                            projType = VanillaItemProjFix.TransFormProj(npc, Weapon.type);
                        }

                    }
                }
                else
                {
                    projType = 0;
                }

            }

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