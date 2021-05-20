using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NPCAttacker.NPCs
{
    public class ArmedGNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public Item Weapon = new Item();
        public bool MeleeAttacked = false;

        public override void AI(NPC npc)
        {
            if (!NPCAttacker.BuffNPC()) return;
            if (npc.townNPC && NPCID.Sets.AttackType[npc.type] == 3)
            {
                if (npc.ai[0] != NPCAttacker.MeleeAtk)
                {
                    MeleeAttacked = false;
                }
            }
        }

        public override void DrawTownAttackGun(NPC npc, ref float scale, ref int item, ref int closeness)
        {
            if (!NPCAttacker.BuffNPC()) return;
            if (Weapon.IsAir) return;
            if (Weapon.ranged)
            {
                item = Weapon.type;
            }
        }

        public override void DrawTownAttackSwing(NPC npc, ref Texture2D item, ref int itemSize, ref float scale, ref Vector2 offset)
        {
            if (!NPCAttacker.BuffNPC()) return;
            if (Weapon.IsAir) return;
            if (Weapon.noMelee || Weapon.noUseGraphic || Weapon.useStyle != ItemUseStyleID.SwingThrow) return;
            if (Weapon.melee)
            {
                item = Main.itemTexture[Weapon.type];
                scale = Weapon.scale;
            }
        }

        public override void TownNPCAttackSwing(NPC npc, ref int itemWidth, ref int itemHeight)
        {
            if (!NPCAttacker.BuffNPC()) return;
            if (Weapon.IsAir) return;
            if (Weapon.noMelee || Weapon.noUseGraphic || Weapon.useStyle != ItemUseStyleID.SwingThrow) return;
            if (Weapon.melee)
            {
                itemWidth = Weapon.width;
                itemHeight = Weapon.height;
                if (Weapon.shoot != ProjectileID.None) 
                {
                    if (!MeleeAttacked)
                    {
                        MeleeAttacked = true;
                        Vector2 ShootVel;
                        if (NPCAttacker.AttackMode())
                        {
                            ShootVel = Vector2.Normalize(Main.MouseWorld - npc.Center);
                        }
                        else
                        {
                            ShootVel = new Vector2(1, 0) * npc.direction;
                        }
                        int protmp = Projectile.NewProjectile(npc.Center, ShootVel * Weapon.shootSpeed, Weapon.shoot, (int)(Weapon.damage * Main.LocalPlayer.meleeDamage), Weapon.knockBack, Main.myPlayer);
                        Main.projectile[protmp].npcProj = true;
                        Main.projectile[protmp].noDropItem = true;
                        if (Weapon.UseSound != null)
                        {
                            Main.PlaySound(Weapon.UseSound, npc.Center);
                        }
                    }
                }
            }
        }

        public override void TownNPCAttackStrength(NPC npc, ref int damage, ref float knockback)
        {
            if (!NPCAttacker.BuffNPC()) return;
            if (Weapon.IsAir) return;

            if (NPCID.Sets.AttackType[npc.type] == 0 && Weapon.thrown)
            {
                damage = Weapon.damage;
            }
            else if (NPCID.Sets.AttackType[npc.type] == 1 && Weapon.ranged)
            {
                damage = Weapon.damage;
            }
            else if (NPCID.Sets.AttackType[npc.type] == 2 && Weapon.magic)
            {
                damage = Weapon.damage;
            }
            else if (NPCID.Sets.AttackType[npc.type] == 3 && Weapon.melee)
            {
                if (Weapon.useStyle == ItemUseStyleID.SwingThrow)
                {
                    damage = Weapon.damage;
                }
            }
        }


        public override void TownNPCAttackProjSpeed(NPC npc, ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            if (!NPCAttacker.BuffNPC()) return;
            if (Weapon.IsAir) return;
            if (NPCID.Sets.AttackType[npc.type] == 0 && Weapon.thrown)
            {
                float k = Weapon.shootSpeed / multiplier;
                multiplier = Weapon.shootSpeed;
                gravityCorrection *= k;
                randomOffset /= 3;
            }
            else if (NPCID.Sets.AttackType[npc.type] == 1 && Weapon.ranged)
            {
                multiplier = Weapon.shootSpeed;
            }
            else if (NPCID.Sets.AttackType[npc.type] == 2 && Weapon.magic)
            {
                multiplier = Weapon.shootSpeed;
                randomOffset /= 2;
            }
        }

        public override void TownNPCAttackCooldown(NPC npc, ref int cooldown, ref int randExtraCooldown)
        {
            if (!NPCAttacker.BuffNPC()) return;
            if (Weapon.IsAir) return;
            if (NPCID.Sets.AttackType[npc.type] == 0 && Weapon.thrown)
            {
                cooldown = Weapon.useAnimation;
                randExtraCooldown = 1;
            }
            else if (NPCID.Sets.AttackType[npc.type] == 1 && Weapon.ranged)
            {
                cooldown = Weapon.useAnimation;
                randExtraCooldown = 1;
            }
            else if (NPCID.Sets.AttackType[npc.type] == 2 && Weapon.magic)
            {
                cooldown = Weapon.useAnimation;
                randExtraCooldown = 1;
            }
            else if (NPCID.Sets.AttackType[npc.type] == 3 && Weapon.melee)
            {
                if (Weapon.useStyle == ItemUseStyleID.SwingThrow && !Weapon.noMelee && !Weapon.noUseGraphic)
                {
                    cooldown = Weapon.useAnimation;
                    randExtraCooldown = 1;
                }
            }
        }

        public override void TownNPCAttackProj(NPC npc, ref int projType, ref int attackDelay)
        {
            if (!NPCAttacker.BuffNPC()) return;
            if (Weapon.IsAir) return;
            if (NPCID.Sets.AttackType[npc.type] == 0 && Weapon.thrown)
            {
                if (Weapon.shoot != ProjectileID.None)
                {
                    attackDelay = 1;
                    projType = Weapon.shoot;
                }
            }
            else if (NPCID.Sets.AttackType[npc.type] == 1 && Weapon.ranged)
            {
                if (Weapon.shoot != ProjectileID.None)
                {
                    if (Weapon.useAmmo == AmmoID.Dart || Weapon.useAmmo == AmmoID.Bullet)
                    {
                        if (Weapon.shoot == ProjectileID.PurificationPowder || Weapon.shoot == ProjectileID.Xenopopper)
                        {
                            int shoot = PickAmmo(Weapon);
                            if (shoot > 0)
                            {
                                attackDelay = 1;
                                projType = shoot;
                            }
                        }
                        else
                        {
                            attackDelay = 1;
                            projType = Weapon.shoot;
                        }
                    }
                    else
                    {
                        attackDelay = 1;
                        projType = Weapon.shoot;
                    }
                }
                else
                {
                    if (Weapon.useAmmo > 0)
                    {
                        int shoot = PickAmmo(Weapon);
                        if (shoot > 0)
                        {
                            attackDelay = 1;
                            projType = shoot;
                        }
                    }
                }

                
            }
            else if (NPCID.Sets.AttackType[npc.type] == 2 && Weapon.magic)
            {
                if (Weapon.shoot != ProjectileID.None)
                {
                    attackDelay = 1;
                    projType = Weapon.shoot;
                }
            }
        }

        public override void NPCLoot(NPC npc)
        {
            if (!Weapon.IsAir)
            {
                int itemtmp = Item.NewItem(npc.Hitbox, Weapon.type, Weapon.stack);
                Main.item[itemtmp] = Main.item[itemtmp].CloneWithModdedDataFrom(Weapon);
                Main.item[itemtmp].position = npc.Center;
                Weapon.TurnToAir();
            }
        }



        public int PickAmmo(Item sItem)
        {
            int shoot = 0;
            bool canShoot = false;
            Item item = new Item();
            bool Found = false;
            for (int i = 54; i < 58; i++)
            {
                if (Main.LocalPlayer.inventory[i].ammo == sItem.useAmmo && Main.LocalPlayer.inventory[i].stack > 0)
                {
                    item = Main.LocalPlayer.inventory[i];
                    canShoot = true;
                    Found = true;
                    break;
                }
            }
            if (!Found)
            {
                for (int j = 0; j < 54; j++)
                {
                    if (Main.LocalPlayer.inventory[j].ammo == sItem.useAmmo && Main.LocalPlayer.inventory[j].stack > 0)
                    {
                        item = Main.LocalPlayer.inventory[j];
                        canShoot = true;
                        break;
                    }
                }
            }

            if (canShoot)
            {
                if (sItem.type == ItemID.SnowmanCannon)
                {
                    shoot = 338 + item.type - 771;
                    if (shoot > 341)
                    {
                        shoot = 341;
                    }
                }
                else if (sItem.useAmmo == AmmoID.Rocket)
                {
                    shoot += item.shoot;
                }
                else if (sItem.useAmmo == 780)
                {
                    shoot += item.shoot;
                }
                else if (item.shoot > ProjectileID.None)
                {
                    shoot = item.shoot;
                }
                if (sItem.type == ItemID.HellwingBow && shoot == 1)
                {
                    shoot = 485;
                }
                if (sItem.type == ItemID.ShadowFlameBow)
                {
                    shoot = 495;
                }
                if (sItem.type == ItemID.BoneGlove && shoot == 21)
                {
                    shoot = 532;
                }
                
                if (shoot == 42)
                {
                    if (item.type == ItemID.EbonsandBlock)
                    {
                        shoot = 65;
                    }
                    else if (item.type == ItemID.PearlsandBlock)
                    {
                        shoot = 68;
                    }
                    else if (item.type == ItemID.CrimsandBlock)
                    {
                        shoot = 354;
                    }
                }
                if (sItem.type == ItemID.BeesKnees && shoot == 1)
                {
                    shoot = 469;
                }
                if (sItem.type == ItemID.ShadowFlameBow)
                {
                    shoot = ProjectileID.ShadowFlameArrow;
                }
                if (sItem.type == ItemID.MoltenFury && shoot == 1)
                {
                    shoot = ProjectileID.FlamingArrow;
                }
                if (sItem.type == ItemID.PulseBow)
                {
                    shoot = ProjectileID.PulseBolt;
                }
                if (sItem.type == ItemID.IceBow)
                {
                    shoot = ProjectileID.FrostburnArrow;
                }
                if (sItem.type == ItemID.Marrow)
                {
                    shoot = ProjectileID.BoneArrow;
                }
                if (sItem.type == ItemID.DD2BetsyBow)
                {
                    shoot = ProjectileID.DD2BetsyArrow;
                }
                if (sItem.type == ItemID.Uzi || sItem.type == ItemID.SniperRifle)
                {
                    shoot = ProjectileID.BulletHighVelocity;
                }
                if (sItem.type == ItemID.Xenopopper)
                {
                    shoot = ProjectileID.MoonlordBullet;
                }
                return shoot;
            }
            return 0;
        }
    }
}