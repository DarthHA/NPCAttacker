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
        public int NPCTargetForSpecialUse = -1;
        public bool MeleeAttacked = false;

        public override void AI(NPC npc)
        {
            if (!SomeUtils.BuffNPC()) return;
            if (npc.townNPC && NPCID.Sets.AttackType[npc.type] == 3)
            {
                if (npc.ai[0] != NPCOverrideAI.MeleeAtk)
                {
                    MeleeAttacked = false;
                }
            }
        }

        public override void DrawTownAttackGun(NPC npc, ref float scale, ref int item, ref int closeness)
        {
            if (!SomeUtils.BuffNPC()) return;
            if (Weapon.IsAir) return;
            item = Weapon.type;
        }

        public override void DrawTownAttackSwing(NPC npc, ref Texture2D item, ref int itemSize, ref float scale, ref Vector2 offset)
        {
            if (!SomeUtils.BuffNPC()) return;
            if (Weapon.IsAir) return;
            item = Main.itemTexture[Weapon.type];
            scale = Weapon.scale;
        }

        public override void TownNPCAttackSwing(NPC npc, ref int itemWidth, ref int itemHeight)
        {
            if (!SomeUtils.BuffNPC()) return;
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
                    int dmg = Main.LocalPlayer.GetWeaponDamage(Weapon);
                    float Kb = Main.LocalPlayer.GetWeaponKnockback(Weapon, Weapon.knockBack);
                    Vector2 Pos = npc.Center;
                    Vector2 Speed;
                    if (SomeUtils.AttackMode())
                    {
                        Speed = Vector2.Normalize(Main.MouseWorld - npc.Center) * Weapon.shootSpeed;
                    }
                    else
                    {
                        Speed = new Vector2(1, 0) * npc.direction * Weapon.shootSpeed;
                    }
                    if (Weapon.modItem != null)
                    {
                        ShouldShoot = Weapon.modItem.Shoot(Main.LocalPlayer, ref Pos, ref Speed.X, ref Speed.Y, ref shoot, ref dmg, ref Kb);
                    }
                    if (ShouldShoot)
                    {
                        int protmp = Projectile.NewProjectile(Pos, Speed, shoot, dmg, Kb, Main.myPlayer);
                        Main.projectile[protmp].npcProj = true;
                        Main.projectile[protmp].noDropItem = true;
                        Main.projectile[protmp].usesLocalNPCImmunity = true;
                        if (Main.projectile[protmp].localNPCHitCooldown > 10)
                        {
                            Main.projectile[protmp].localNPCHitCooldown = 10;
                        }
                    }
                    if (Weapon.UseSound != null)
                    {
                        Main.PlaySound(Weapon.UseSound, npc.Center);
                    }
                }
            }
        }

        public override void TownNPCAttackStrength(NPC npc, ref int damage, ref float knockback)
        {
            if (!SomeUtils.BuffNPC()) return;
            if (Weapon.IsAir) return;
            if (npc.type == NPCID.Nurse) return;
            damage = Main.LocalPlayer.GetWeaponDamage(Weapon);
            knockback = Main.LocalPlayer.GetWeaponKnockback(Weapon, knockback);
            if (Weapon.ranged && Weapon.useAmmo > 0)
            {
                damage += SomeUtils.GetAmmoDmg(Weapon);
            }
        }


        public override void TownNPCAttackProjSpeed(NPC npc, ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            if (!SomeUtils.BuffNPC()) return;
            if (Weapon.IsAir) return;
            if (npc.type == NPCID.Nurse) return;
            switch (NPCID.Sets.AttackType[npc.type])
            {
                case 0:
                    float k = Weapon.shootSpeed / multiplier;
                    multiplier = Weapon.shootSpeed * Main.LocalPlayer.thrownVelocity;
                    gravityCorrection *= k;
                    randomOffset /= 3;
                    break;
                case 1:
                    multiplier = Weapon.shootSpeed;
                    break;
                case 2:
                    multiplier = Weapon.shootSpeed;
                    randomOffset /= 2;
                    break;
                default:
                    break;
            }

        }

        public override void TownNPCAttackCooldown(NPC npc, ref int cooldown, ref int randExtraCooldown)
        {
            if (!SomeUtils.BuffNPC()) return;
            if (Weapon.IsAir) return;
            if (npc.type == NPCID.Nurse) return;
            cooldown = Weapon.useTime;
            randExtraCooldown = 1;
        }

        public override void TownNPCAttackProj(NPC npc, ref int projType, ref int attackDelay)
        {
            if (!SomeUtils.BuffNPC()) return;
            if (Weapon.IsAir) return;
            if (npc.type == NPCID.Nurse) return;
            attackDelay = 1;

            if (NPCID.Sets.AttackType[npc.type] == 0 || NPCID.Sets.AttackType[npc.type] == 2)
            {
                int shoot = Weapon.shoot;
                if (npc.localAI[3] == 0)
                {
                    if (Weapon.modItem != null)
                    {
                        int dmg = Main.LocalPlayer.GetWeaponDamage(Weapon);
                        Vector2 Pos = npc.Center;
                        Vector2 Speed;
                        if (NPCTargetForSpecialUse != -1)
                        {
                            Speed = Vector2.Normalize(Main.npc[NPCTargetForSpecialUse].Center - npc.Center) * Weapon.shootSpeed;
                        }
                        else
                        {
                            Speed = new Vector2(npc.direction, 0) * Weapon.shootSpeed;
                        }
                        float Kb = Weapon.knockBack;
                        if (Weapon.modItem.Shoot(Main.LocalPlayer, ref Pos, ref Speed.X, ref Speed.Y, ref shoot, ref dmg, ref Kb))
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
                    }
                }
                else
                {
                    projType = 0;
                }
                
            }
            else if (NPCID.Sets.AttackType[npc.type] == 1)                  //大头射手
            {
                if (!SomeUtils.HasAmmo(Weapon))
                {
                    projType = 0;
                    return;
                }
                if (npc.localAI[3] == 0)
                {
                    if (Weapon.modItem != null)
                    {
                        int shoot;
                        if (SomeUtils.ItsAmmoShootProj(Weapon) > 0)
                        {
                            shoot = SomeUtils.PickAmmo(Weapon);
                        }
                        else
                        {
                            shoot = Weapon.shoot;
                        }

                        int dmg = Main.LocalPlayer.GetWeaponDamage(Weapon);
                        Vector2 Pos = npc.Center;
                        Vector2 Speed;
                        if (NPCTargetForSpecialUse != -1)
                        {
                            Speed = Vector2.Normalize(Main.npc[NPCTargetForSpecialUse].Center - npc.Center) * Weapon.shootSpeed;
                        }
                        else
                        {
                            Speed = new Vector2(npc.direction, 0) * Weapon.shootSpeed;
                        }
                        float Kb = Weapon.knockBack;
                        if (Weapon.modItem.Shoot(Main.LocalPlayer, ref Pos, ref Speed.X, ref Speed.Y, ref shoot, ref dmg, ref Kb))
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
                        if (SomeUtils.ItsAmmoShootProj(Weapon) > 0)
                        {
                            bool ForceChange = false;
                            if (Weapon.useAmmo == AmmoID.Bullet || Weapon.useAmmo == AmmoID.Arrow || Weapon.useAmmo == AmmoID.Dart || Weapon.useAmmo == AmmoID.NailFriendly)
                            {
                                if (SomeUtils.IsUseSpecialProj[Weapon.type] != 0)
                                {
                                    ForceChange = true;
                                }
                            }
                            if (ForceChange)
                            {
                                projType = SomeUtils.IsUseSpecialProj[Weapon.type];
                            }
                            else
                            {
                                projType = SomeUtils.PickAmmo(Weapon);
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

                    }
                }
                else
                {
                    projType = 0;
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



        public static Item GetWeapon(NPC npc)
        {
            return npc.GetGlobalNPC<ArmedGNPC>().Weapon;
        }
    }
}