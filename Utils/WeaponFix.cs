


using Microsoft.Xna.Framework;
using NPCAttacker.Projectiles;
using System;
using Terraria;
using Terraria.ID;

namespace NPCAttacker
{

    public static class MeleeWeaponFix
    {
        public static void Bladetongue(NPC npc, Rectangle MeleeHitbox, int dmg, float kb)
        {
            Vector2 vector37 = Vector2.Normalize(new Vector2(npc.direction * 100 + Main.rand.Next(-25, 26), Main.rand.Next(-75, 76)));
            vector37 *= Main.rand.Next(30, 41) * 0.1f;
            Vector2 vector38 = new(MeleeHitbox.X + Main.rand.Next(MeleeHitbox.Width), MeleeHitbox.Y + Main.rand.Next(MeleeHitbox.Height));
            vector38 = (vector38 + npc.Center * 2f) / 3f;
            int protmp = Projectile.NewProjectile(null, vector38, vector37, ProjectileID.IchorSplash, (int)(dmg * 0.7), kb * 0.7f, Main.myPlayer, 0f, 0f);
            Main.projectile[protmp].npcProj = true;
        }
        public static void PumpkinSword(int i, int dmg, float kb)
        {
            int logicCheckScreenHeight = Main.LogicCheckScreenHeight;
            int logicCheckScreenWidth = Main.LogicCheckScreenWidth;
            int num = Main.rand.Next(100, 300);
            int num2 = Main.rand.Next(100, 300);
            if (Main.rand.NextBool(2))
            {
                num -= logicCheckScreenWidth / 2 + num;
            }
            else
            {
                num += logicCheckScreenWidth / 2 - num;
            }
            if (Main.rand.NextBool(2))
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
            Vector2 vector = new(num, num2);
            float num4 = Main.npc[i].position.X - vector.X;
            float num5 = Main.npc[i].position.Y - vector.Y;
            float num6 = (float)Math.Sqrt(num4 * num4 + num5 * num5);
            num6 = num3 / num6;
            num4 *= num6;
            num5 *= num6;
            int protmp = Projectile.NewProjectile(Main.npc[i].GetSource_FromAI(), num, num2, num4, num5, ProjectileID.FlamingJack, dmg, kb, Main.LocalPlayer.whoAmI, i, 0f);
            Main.projectile[protmp].npcProj = true;
        }

        public static void BeeKeeperHit(NPC target, Rectangle HitRectangle, int dmg)
        {
            int BeeCount = Main.rand.Next(1, 4);
            if (Main.LocalPlayer.strongBees && Main.rand.NextBool(3))
            {
                BeeCount++;
            }
            for (int i = 0; i < BeeCount; i++)
            {
                float num335 = target.direction * 2 + Main.rand.Next(-35, 36) * 0.02f;
                float num336 = Main.rand.Next(-35, 36) * 0.02f;
                num335 *= 0.2f;
                num336 *= 0.2f;
                int protmp = Projectile.NewProjectile(target.GetSource_FromAI(), HitRectangle.X + HitRectangle.Width / 2, HitRectangle.Y + HitRectangle.Height / 2, num335, num336, Main.LocalPlayer.beeType(), Main.LocalPlayer.beeDamage(dmg / 3), Main.LocalPlayer.beeKB(0f), Main.myPlayer, 0f, 0f);
                Main.projectile[protmp].npcProj = true;
            }
            if (Main.rand.NextFloat() < 0.9f)
            {
                target.AddBuff(BuffID.Confused, 120);
            }
        }

        public static void TentacleSpike_TrySpiking(NPC Attacker, NPC target, float damage, float knockBack)
        {
            if (target.CanBeChasedBy())
            {
                Vector2 vector = target.Center - Attacker.Center;
                vector = vector.SafeNormalize(Vector2.Zero);
                Vector2 vector2 = target.Hitbox.ClosestPointInRect(Attacker.Center) + vector;
                Vector2 vector3 = (target.Center - vector2) * 0.8f;
                int num = Projectile.NewProjectile(null, vector2, vector3, ProjectileID.TentacleSpike, (int)damage, knockBack, 0, 1f, target.whoAmI);
                Main.projectile[num].StatusNPC(target.whoAmI);
                Main.projectile[num].npcProj = true;
                Main.projectile[num].noDropItem = true;
                //Projectile.KillOldestJavelin(num, 971, target.whoAmI, Player._tentacleSpikesMax5);
            }
        }

        public static bool UseMeleeThrowWeapon(Item item)
        {
            switch (item.type)
            {
                case ItemID.WoodenBoomerang:
                case ItemID.EnchantedBoomerang:
                case ItemID.IceBoomerang:
                case ItemID.FruitcakeChakram:
                case ItemID.ThornChakram:
                case ItemID.BloodyMachete:
                case ItemID.Shroomerang:
                case ItemID.CombatWrench:
                case ItemID.Flamarang:
                case ItemID.BouncingShield:
                case ItemID.PaladinsHammer:
                case ItemID.PossessedHatchet:
                case ItemID.ShadowFlameKnife:
                case ItemID.DayBreak:
                case ItemID.VampireKnives:
                case ItemID.ScourgeoftheCorruptor:
                case ItemID.Bananarang:
                case ItemID.LightDisc:
                    return true;
            }
            return false;
        }
    }

    public static class AmmoFix
    {
        public static Item FindAmmo(NPC npc)
        {
            if (!npc.IsTownNPC()) return new Item();
            if (npc.GetGlobalNPC<ArmedGNPC>().WeaponAlt.IsAir)
            {
                return new Item();
            }
            if (npc.GetGlobalNPC<ArmedGNPC>().Weapon.IsAir)
            {
                return new Item();
            }
            if (npc.GetGlobalNPC<ArmedGNPC>().Weapon.useAmmo == AmmoID.None)
            {
                return new Item();
            }
            if (npc.GetGlobalNPC<ArmedGNPC>().Weapon.useAmmo != npc.GetGlobalNPC<ArmedGNPC>().WeaponAlt.ammo)
            {
                return new Item();
            }

            return npc.GetGlobalNPC<ArmedGNPC>().WeaponAlt;
        }

        public static bool HasAmmo(NPC npc, Item sItem)
        {
            if (sItem.useAmmo == AmmoID.None) return true;
            return !FindAmmo(npc).IsAir;
        }




        public static int GetAmmoDmg(NPC npc, Item sItem)
        {
            if (sItem.useAmmo == AmmoID.None) return 0;
            Item item = FindAmmo(npc);
            if (item.IsAir) return 0;
            return item.damage;
        }

        public static float GetAmmoKB(NPC npc, Item sItem)
        {
            if (sItem.useAmmo == AmmoID.None) return 0;
            Item item = FindAmmo(npc);
            if (item.IsAir) return 0;
            return item.knockBack;
        }


        public static float GetAmmoSpeed(NPC npc, Item sItem)
        {
            if (sItem.useAmmo == AmmoID.None) return 0;
            Item item = FindAmmo(npc);
            if (item.IsAir) return 0;
            return item.shootSpeed;
        }


        public static int PickAmmo(NPC npc, Item sItem)
        {
            int shoot;
            if (sItem.useAmmo == AmmoID.None) return 0;
            Item item = FindAmmo(npc);
            if (item.IsAir) return 0;
            shoot = item.shoot;

            if (sItem.type == ItemID.HellwingBow && shoot == ProjectileID.WoodenArrowFriendly)
            {
                shoot = ProjectileID.Hellwing;
            }
            if (sItem.type == ItemID.ShadowFlameBow)
            {
                shoot = ProjectileID.ShadowFlameArrow;
            }

            if (shoot == ProjectileID.SandBallGun)
            {
                if (item.type == ItemID.EbonsandBlock)
                {
                    shoot = ProjectileID.EbonsandBallGun;
                }
                else if (item.type == ItemID.PearlsandBlock)
                {
                    shoot = ProjectileID.PearlSandBallGun;
                }
                else if (item.type == ItemID.CrimsandBlock)
                {
                    shoot = ProjectileID.CrimsandBallGun;
                }
            }
            if (sItem.type == ItemID.BeesKnees && shoot == ProjectileID.WoodenArrowFriendly)
            {
                shoot = ProjectileID.BeeArrow;
            }
            if (sItem.type == ItemID.ShadowFlameBow)
            {
                shoot = ProjectileID.ShadowFlameArrow;
            }
            if (sItem.type == ItemID.MoltenFury && shoot == ProjectileID.WoodenArrowFriendly)
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

            VanillaItemProjFix.TransFormProjRocket(sItem.type, item.type, ref shoot);

            return shoot;

        }

    }

}