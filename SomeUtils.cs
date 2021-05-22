using Microsoft.Xna.Framework;
using NPCAttacker.Items;
using NPCAttacker.NPCs;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NPCAttacker
{
    public static class SomeUtils
    {
		public static void PumpkinSword(int i, int dmg, float kb)
		{
			int logicCheckScreenHeight = Main.LogicCheckScreenHeight;
			int logicCheckScreenWidth = Main.LogicCheckScreenWidth;
			int num = Main.rand.Next(100, 300);
			int num2 = Main.rand.Next(100, 300);
			if (Main.rand.Next(2) == 0)
			{
				num -= logicCheckScreenWidth / 2 + num;
			}
			else
			{
				num += logicCheckScreenWidth / 2 - num;
			}
			if (Main.rand.Next(2) == 0)
			{
				num2 -= logicCheckScreenHeight / 2 + num2;
			}
			else
			{
				num2 += logicCheckScreenHeight / 2 - num2;
			}
			num += (int)Main.LocalPlayer.position.X;
			num2 += (int)Main.LocalPlayer.position.Y;
			float num3 = 8f;
			Vector2 vector = new Vector2(num, num2);
			float num4 = Main.npc[i].position.X - vector.X;
			float num5 = Main.npc[i].position.Y - vector.Y;
			float num6 = (float)Math.Sqrt(num4 * num4 + num5 * num5);
			num6 = num3 / num6;
			num4 *= num6;
			num5 *= num6;
			int protmp = Projectile.NewProjectile(num, num2, num4, num5, ProjectileID.FlamingJack, dmg, kb, Main.LocalPlayer.whoAmI, i, 0f);
			Main.projectile[protmp].npcProj = true;
			Main.projectile[protmp].usesLocalNPCImmunity = true;
			if (Main.projectile[protmp].localNPCHitCooldown > 10)
			{
				Main.projectile[protmp].localNPCHitCooldown = 10;
			}
		}


		public static void BeeKeeperHit(NPC target,Rectangle HitRectangle,int dmg)
        {
			int BeeCount = Main.rand.Next(1, 4);
			if (Main.LocalPlayer.strongBees && Main.rand.Next(3) == 0)
			{
				BeeCount++;
			}
			for (int i = 0; i < BeeCount; i++)
			{
				float num335 = target.direction * 2 + Main.rand.Next(-35, 36) * 0.02f;
				float num336 = Main.rand.Next(-35, 36) * 0.02f;
				num335 *= 0.2f;
				num336 *= 0.2f;
				int protmp = Projectile.NewProjectile(HitRectangle.X + HitRectangle.Width / 2, HitRectangle.Y + HitRectangle.Height / 2, num335, num336, Main.LocalPlayer.beeType(), Main.LocalPlayer.beeDamage(dmg / 3), Main.LocalPlayer.beeKB(0f), Main.myPlayer, 0f, 0f);
				Main.projectile[protmp].npcProj = true;
				Main.projectile[protmp].usesLocalNPCImmunity = true;
				if (Main.projectile[protmp].localNPCHitCooldown > 10)
				{
					Main.projectile[protmp].localNPCHitCooldown = 10;
				}
			}
			if (Main.rand.NextFloat() < 0.9f)
			{
				target.AddBuff(BuffID.Confused, 120);
			}
		}

        private static Item FindAmmo(Item sItem)
        {
            Item result = new Item();
            int FoundIndex = -1;
            for (int i = 54; i < 58; i++)
            {
                if (Main.LocalPlayer.inventory[i].ammo == sItem.useAmmo && Main.LocalPlayer.inventory[i].stack > 0)
                {
                    if (Main.LocalPlayer.inventory[i].stack >= Math.Min(Main.LocalPlayer.inventory[i].maxStack, 999))
                    {
                        FoundIndex = i;
                        break;
                    }
                }
            }
            if (FoundIndex == -1)
            {
                for (int i = 0; i < 54; i++)
                {
                    if (Main.LocalPlayer.inventory[i].ammo == sItem.useAmmo && Main.LocalPlayer.inventory[i].stack > 0)
                    {
                        if (Main.LocalPlayer.inventory[i].stack >= Math.Min(Main.LocalPlayer.inventory[i].maxStack, 999))
                        {
                            FoundIndex = i;
                            break;
                        }
                    }
                }
            }

            if (FoundIndex != -1) return Main.LocalPlayer.inventory[FoundIndex];
            return result;
        }

        public static bool HasAmmo(Item sItem)
        {
            if (sItem.useAmmo == AmmoID.None) return true;
            return !FindAmmo(sItem).IsAir;
        }

        public static int ItsAmmoShootProj(Item sItem)
        {
            if (sItem.useAmmo == AmmoID.None) return 0;
            Item item = FindAmmo(sItem);
            if (item.IsAir) return 0;
            return item.shoot;
        }


        public static int GetAmmoDmg(Item sItem)
        {
            if (sItem.useAmmo == AmmoID.None) return 0;
            Item item = FindAmmo(sItem);
            if (item.IsAir) return 0;
            return item.damage;
        }


        public static int PickAmmo(Item sItem)
        {
            if (sItem.useAmmo == AmmoID.None) return 0;
            int shoot = 0;
            Item item = FindAmmo(sItem);
            bool canShoot = !item.IsAir;

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
                else if (sItem.type == ItemID.FireworksLauncher)
                {
                    shoot = Main.rand.Next(4) + 167;
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
                return shoot;
            }
            return 0;
        }



        


        public static bool AttackMode()
		{
			return NPC.AnyNPCs(ModContent.NPCType<TargetNPC>()) && !NPC.AnyNPCs(ModContent.NPCType<AssembleNPC>());
		}

		public static bool AssembleMode()
		{
			return !NPC.AnyNPCs(ModContent.NPCType<TargetNPC>()) && NPC.AnyNPCs(ModContent.NPCType<AssembleNPC>());
		}

		public static bool NoMode()
		{
			return !NPC.AnyNPCs(ModContent.NPCType<TargetNPC>()) && !NPC.AnyNPCs(ModContent.NPCType<AssembleNPC>());
		}

		public static bool BuffNPC()
		{
			return Main.LocalPlayer.HeldItem.type == ModContent.ItemType<AttackerStick>();
		}

		public static Item CloneItem(Item target)
		{
			Item result = new Item();
			result.netDefaults(target.netID);
			result = result.CloneWithModdedDataFrom(target);
			result.favorited = target.favorited;
			result.stack = target.stack;
			result.prefix = target.prefix;
			return result;
		}
	}
}