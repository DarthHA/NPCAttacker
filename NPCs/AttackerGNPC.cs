using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NPCAttacker.NPCs
{
    public class AttackerGNPC : GlobalNPC
    {
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (npc.HasBuff(BuffID.DryadsWardDebuff) && NPCAttacker.BuffNPC())
            {
                int baseDmg = 4;
                float Multiplier = 1f;
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                if (NPC.downedBoss1)
                {
                    Multiplier += 0.1f;
                }
                if (NPC.downedBoss2)
                {
                    Multiplier += 0.1f;
                }
                if (NPC.downedBoss3)
                {
                    Multiplier += 0.1f;
                }
                if (NPC.downedQueenBee)
                {
                    Multiplier += 0.1f;
                }
                if (Main.hardMode)
                {
                    Multiplier += 0.4f;
                }
                if (NPC.downedMechBoss1)
                {
                    Multiplier += 0.15f;
                }
                if (NPC.downedMechBoss2)
                {
                    Multiplier += 0.15f;
                }
                if (NPC.downedMechBoss3)
                {
                    Multiplier += 0.15f;
                }
                if (NPC.downedPlantBoss)
                {
                    Multiplier += 0.15f;
                }
                if (NPC.downedGolemBoss)
                {
                    Multiplier += 0.15f;
                }
                if (NPC.downedAncientCultist)
                {
                    Multiplier += 0.15f;
                }
                if (Main.expertMode)
                {
                    Multiplier *= Main.expertNPCDamage;
                }
                int FinalDmg = (int)(baseDmg * (Multiplier * Main.LocalPlayer.magicDamage - 1));
                npc.lifeRegen -= 2 * FinalDmg;
                int FinalFinalDmg = (int)(baseDmg * Multiplier * Main.LocalPlayer.magicDamage);
                if (damage < FinalFinalDmg)
                {
                    damage = FinalFinalDmg / 3;
                }
            }
        }


        public override void ModifyHitNPC(NPC npc, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if (target.townNPC || target.type == NPCID.SkeletonMerchant)
            {
                if (NPCAttacker.BuffNPC())
                {
                    damage = (int)(damage * (1 - Main.LocalPlayer.endurance));

                    if (NPC.downedMoonlord)
                    {
                        damage = (int)(damage * 0.5f);
                    }
                    else if (NPC.downedPlantBoss)
                    {
                        damage = (int)(damage * 0.66f);
                    }
                    else if (Main.hardMode)
                    {
                        damage = (int)(damage * 0.75f);
                    }
                    
                }
            }
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (npc.townNPC || npc.type == NPCID.SkeletonMerchant)
            {
                if (NPCAttacker.BuffNPC())
                {
                    damage = (int)(damage * (1 - Main.LocalPlayer.endurance));

                    if (NPC.downedMoonlord)
                    {
                        damage = (int)(damage * 0.5f);
                    }
                    else if (NPC.downedPlantBoss)
                    {
                        damage = (int)(damage * 0.66f);
                    }
                    else if (Main.hardMode)
                    {
                        damage = (int)(damage * 0.75f);
                    }
                }
            }
        }

        public override bool StrikeNPC(NPC npc, ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            if (npc.townNPC || npc.type == NPCID.SkeletonMerchant)
            {
                if (NPCAttacker.BuffNPC())
                {
                    if (npc.HasBuff(BuffID.DryadsWard))
                    {
                        int ExtraDef = 0;
                        if (NPC.downedMoonlord)
                        {
                            ExtraDef = 10;
                        }
                        else if (NPC.downedPlantBoss)
                        {
                            ExtraDef = 5;
                        }
                        else if (Main.hardMode)
                        {
                            ExtraDef = 3;
                        }
                        if (!Main.expertMode)
                        {
                            ExtraDef = (int)(ExtraDef * 0.6f);
                        }
                        damage -= ExtraDef;
                        if (damage < 1.0)
                        {
                            damage = 1.0;
                        }
                    }
                }
            }
            return true;
        }


        public override bool? CanChat(NPC npc)
        {
            if (npc.townNPC || npc.type == NPCID.SkeletonMerchant)
            {
                if (NPCAttacker.AssembleMode() || NPCAttacker.AttackMode())
                {
                    return false;
                }
            }
            return null;
        }

    }


    public class AttackerGProj : GlobalProjectile
    {
        public override bool PreAI(Projectile projectile)
        {
            if (projectile.type == ProjectileID.NurseSyringeHeal)
            {
                if (projectile.localAI[1] == 0f)
                {
                    projectile.localAI[1] = projectile.velocity.Length();
                }

                if (!Main.npc[(int)projectile.ai[0]].active || !Main.npc[(int)projectile.ai[0]].townNPC)
                {
                    projectile.Kill();
                    return false;
                }
                NPC target = Main.npc[(int)projectile.ai[0]];
                Vector2 ShootVel = target.Center - projectile.Center;
                if (ShootVel.Length() < projectile.localAI[1] || projectile.Hitbox.Intersects(target.Hitbox))
                {
                    projectile.Kill();
                    int DamageTaken = target.lifeMax - target.life;
                    int HealLife = 20;
                    if (NPCAttacker.BuffNPC())
                    {
                        if (NPC.downedMoonlord)
                        {
                            HealLife = 40;
                        }
                        else if (NPC.downedPlantBoss)
                        {
                            HealLife = 30;
                        }
                        else if (Main.hardMode)
                        {
                            HealLife = 25;
                        }

                        if (NPC.downedPlantBoss)
                        {
                            for (int i = 0; i < target.buffTime.Length; i++)
                            {
                                if (Main.debuff[target.buffType[i]] && target.buffTime[i] > 0)
                                {
                                    target.buffTime[i] = 0;
                                }
                            }
                        }
                    }
                    if (DamageTaken > HealLife)
                    {
                        DamageTaken = HealLife;
                    }
                    if (DamageTaken > 0)
                    {
                        target.life += DamageTaken;
                        target.HealEffect(DamageTaken, true);
                    }

                    return false;
                }
                ShootVel.Normalize();
                ShootVel *= projectile.localAI[1];
                if (ShootVel.Y < projectile.velocity.Y)
                {
                    ShootVel.Y = projectile.velocity.Y;
                }
                ShootVel.Y += 1f;
                projectile.velocity = Vector2.Lerp(projectile.velocity, ShootVel, 0.04f);
                projectile.rotation += projectile.velocity.X * 0.05f;
                return false;
            }
            return true;
        }


        public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (projectile.npcProj)
            {
                if (NPCAttacker.BuffNPC())
                {
                    if (projectile.melee)
                    {
                        crit = Main.rand.Next(100) <= Main.LocalPlayer.meleeCrit;
                    }
                    else if (projectile.ranged)
                    {
                        crit = Main.rand.Next(100) <= Main.LocalPlayer.rangedCrit;
                    }
                    else if (projectile.magic)
                    {
                        crit = Main.rand.Next(100) <= Main.LocalPlayer.magicCrit;
                    }
                    else if (projectile.thrown)
                    {
                        crit = Main.rand.Next(100) <= Main.LocalPlayer.magicCrit;
                    }
                }
            }
        }
    }
}