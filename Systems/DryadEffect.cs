using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NPCAttacker.NPCs
{
    public class DryadEffect : GlobalNPC
    {
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (!NPCUtils.BuffNPC()) return;
            if (npc.HasBuff(BuffID.DryadsWardDebuff))
            {
                int baseDmg = 8;
                if (NPC.AnyNPCs(NPCID.Dryad))
                {
                    if (!ArmedGNPC.GetWeapon(Main.npc[NPC.FindFirstNPC(NPCID.Dryad)]).IsAir)
                    {
                        baseDmg += ArmedGNPC.GetWeapon(Main.npc[NPC.FindFirstNPC(NPCID.Dryad)]).healMana / 5;
                    }
                }
                float Multiplier = 0f;
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
                    Multiplier *= 2;
                }
                if (Main.masterMode)
                {
                    Multiplier *= 2;
                }
                int FinalDmg = (int)(baseDmg * (Multiplier + 1));
                FinalDmg = (int)(FinalDmg * NPCAttacker.config.DamageModifier);
                npc.lifeRegen -= 2 * FinalDmg;
                if (damage < FinalDmg)
                {
                    damage = FinalDmg / 3;
                }
            }

            if (npc.HasBuff(BuffID.DryadsWard) && NPC.AnyNPCs(NPCID.Dryad))
            {
                if (!ArmedGNPC.GetWeapon(Main.npc[NPC.FindFirstNPC(NPCID.Dryad)]).IsAir)
                {
                    float LifeRegenBonus = ArmedGNPC.GetWeapon(Main.npc[NPC.FindFirstNPC(NPCID.Dryad)]).healMana / 50f;
                    if (LifeRegenBonus < 1) LifeRegenBonus = 1;
                    if (LifeRegenBonus >= 3)
                    {
                        if (npc.lifeRegen < 0)
                        {
                            npc.lifeRegen = 0;
                        }
                    }
                    npc.lifeRegen += (int)(LifeRegenBonus * 2);
                }

                if (!ArmedGNPC.GetAltWeapon(Main.npc[NPC.FindFirstNPC(NPCID.Dryad)]).IsAir)
                {
                    npc.AddBuff(ArmedGNPC.GetAltWeapon(Main.npc[NPC.FindFirstNPC(NPCID.Dryad)]).buffType, 60);
                }
            }
        }



    }

}