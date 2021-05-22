using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NPCAttacker.NPCs
{
    public class AttackerGNPC : GlobalNPC
    {
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (npc.HasBuff(BuffID.DryadsWardDebuff) && SomeUtils.BuffNPC())
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
                if (SomeUtils.BuffNPC())
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
                if (SomeUtils.BuffNPC())
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
                if (SomeUtils.BuffNPC())
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
                if (SomeUtils.AssembleMode() || SomeUtils.AttackMode())
                {
                    return false;
                }
            }
            return null;
        }

    }


    
}