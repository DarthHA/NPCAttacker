using System;
using Terraria;
using Terraria.ID;

namespace NPCAttacker
{
    public static class NPCStats
    {
        //�����õ�ͼNPCʱ�ڼӳ�3.2��ר�ҳ˼�1.5����ʦ1.75������2
        public static void ModifyTotalDamage(NPC npc, ref int damage)
        {
            if (NPCID.Sets.AttackType[npc.type] == 2)       //ħ������
            {
                if (!ArmedGNPC.GetWeapon(npc).IsAir)         //װ������ʱ
                {
                    float MagicModifier;
                    if (ArmedGNPC.GetAltWeapon(npc).IsAir)
                    {
                        MagicModifier = ManaEnoughForPotions(ArmedGNPC.GetWeapon(npc), 0);
                    }
                    else
                    {
                        MagicModifier = ManaEnoughForPotions(ArmedGNPC.GetWeapon(npc), ArmedGNPC.GetAltWeapon(npc).healMana);
                    }
                    if (npc.HasBuff(BuffID.ManaRegeneration))      //ħ������ҩˮ
                    {
                        if (MagicModifier < 1)
                        {
                            MagicModifier = 1;
                        }
                    }
                    damage = (int)(damage * MagicModifier);
                }

                float MagicBonus = 1;
                if (npc.HasBuff(BuffID.MagicPower))      //ħ��ҩˮ
                {
                    MagicBonus += 0.2f;
                }
                damage = (int)(damage * MagicBonus);
            }

            if (NPCID.Sets.AttackType[npc.type] == 1)
            {
                float RangedBonus = 1;
                if (npc.HasBuff(BuffID.AmmoReservation))      //��ҩ����ҩˮ
                {
                    RangedBonus += 0.2f;
                }
                if (npc.HasBuff(BuffID.Archery))      //����ҩˮ
                {
                    RangedBonus += 0.1f;
                }
                damage = (int)(damage * RangedBonus);
            }
            if (npc.HasBuff(117))      //ŭ��ҩˮ
            {
                damage = (int)(damage * 1.1f);
            }
            if (npc.HasBuff(BuffID.WellFed3))       //ʳ��
            {
                damage = (int)(damage * 1.1f);
            }
            else if (npc.HasBuff(BuffID.WellFed2))
            {
                damage = (int)(damage * 1.075f);
            }
            else if (npc.HasBuff(BuffID.WellFed))
            {
                damage = (int)(damage * 1.05f);
            }
            if (npc.HasBuff(BuffID.Tipsy) && NPCID.Sets.AttackType[npc.type] == 3)      //���
            {
                damage = (int)(damage * 1.1f);
            }

            float modifier = (float)Main.ShopHelper.GetShoppingSettings(Main.LocalPlayer, npc).PriceAdjustment;
            if (modifier > 1) modifier = 1;
            damage = (int)(damage / modifier);        //�����ֻ�����˺�

            damage = (int)(damage * NPCAttacker.config.DamageModifier);
        }

        public static void ModifyTotalKB(NPC npc, ref float kb)
        {
            if (npc.HasBuff(BuffID.Titan))       //̩̹ҩˮ
            {
                kb *= 1.5f;
            }
        }

        public static void ModifyShootSpeed(NPC npc, ref float speed)
        {
            if (NPCID.Sets.AttackType[npc.type] == 1)
            {
                float RangedBonus = 1;
                if (npc.HasBuff(BuffID.Archery))      //����ҩˮ
                {
                    RangedBonus += 0.1f;
                }
                speed = (int)(speed * RangedBonus);
            }
        }

        public static void ModifyDef(NPC npc, ref int def)
        {

            int ExtraDef = 0;
            if (npc.HasBuff(BuffID.DryadsWard))
            {
                if (NPC.AnyNPCs(NPCID.Dryad))
                {
                    if (!ArmedGNPC.GetWeapon(Main.npc[NPC.FindFirstNPC(NPCID.Dryad)]).IsAir)
                    {
                        ExtraDef += ArmedGNPC.GetWeapon(Main.npc[NPC.FindFirstNPC(NPCID.Dryad)]).healMana / 25;
                    }
                }
            }


            if (!ArmedGNPC.GetArmor(npc).IsAir)
            {
                ExtraDef += ArmedGNPC.GetArmor(npc).defense;
            }

            if (npc.HasBuff(BuffID.Ironskin))
            {
                ExtraDef += 8;
            }
            if (npc.HasBuff(BuffID.WellFed3))
            {
                ExtraDef += 4;
            }
            else if (npc.HasBuff(BuffID.WellFed2))
            {
                ExtraDef += 3;
            }
            else if (npc.HasBuff(BuffID.WellFed))
            {
                ExtraDef += 2;
            }
            if (npc.HasBuff(BuffID.Tipsy) && NPCID.Sets.AttackType[npc.type] == 3)
            {
                ExtraDef -= 4;
            }
            if (NPC.downedMoonlord)
            {
                def = 30 + ExtraDef * 3;
            }
            else if (NPC.downedPlantBoss)
            {
                def = 20 + ExtraDef * 2;
            }
            else if (Main.hardMode)
            {
                def = 10 + ExtraDef;
            }

            if (Main.masterMode)
            {
                def = (int)(def * 1.2f);
            }
            else if (!Main.expertMode)
            {
                def = (int)(def * 0.6f);
            }
        }

        public static void ModifyDR(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (NPCUtils.BuffNPC())
            {
                if (NPC.downedMoonlord)
                {
                    modifiers.FinalDamage *= 0.25f;
                }
                else if (NPC.downedPlantBoss)
                {
                    modifiers.FinalDamage *= 0.5f;
                }
                else if (Main.hardMode)
                {
                    modifiers.FinalDamage *= 0.66f;
                }
            }

            if (npc.HasBuff(BuffID.Endurance))
            {
                modifiers.FinalDamage *= 0.9f;          //����ҩˮ
            }

            float m = 1 - NPCAttacker.config.DRModifier;
            modifiers.FinalDamage *= m;
        }

        public static void ModifyAttackTime(NPC npc, ref int AttackTime)
        {
            float SpeedBonus = 0;
            float SpeedBousDryad = 0;
            if (npc.HasBuff(BuffID.DryadsWard) && NPC.AnyNPCs(NPCID.Dryad))
            {
                if (!ArmedGNPC.GetWeapon(Main.npc[NPC.FindFirstNPC(NPCID.Dryad)]).IsAir)
                {
                    SpeedBousDryad = ArmedGNPC.GetWeapon(Main.npc[NPC.FindFirstNPC(NPCID.Dryad)]).healMana / 900f;
                    if (SpeedBousDryad > 0.5f) SpeedBousDryad = 0.5f;
                }
            }

            if (npc.HasBuff(BuffID.Battle))           //ս��ҩˮ
            {
                SpeedBonus += 0.1f;
            }
            if (npc.HasBuff(BuffID.Calm))           //��ҩˮ
            {
                SpeedBonus -= 0.1f;
            }
            if (npc.HasBuff(BuffID.WellFed3))       //ʳ��
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
            AttackTime = (int)(AttackTime * (1 - SpeedBousDryad));
            AttackTime = (int)(AttackTime * (1 - SpeedBonus));
        }

        public static int GetModifiedAttackTime(NPC npc)
        {
            int result = NPCID.Sets.AttackTime[npc.type];
            if (!ArmedGNPC.GetWeapon(npc).IsAir && npc.type != NPCID.Nurse && npc.type != NPCID.Dryad)
            {
                result = ArmedGNPC.GetWeapon(npc).useTime;
                if (NPCID.Sets.AttackType[npc.type] == 3) //��������ʹ��ʱ�����ԭ�е�NPC����ʱ��ʱ��ը��ֻ��������
                {
                    if (result > NPCID.Sets.AttackTime[npc.type]) result = NPCID.Sets.AttackTime[npc.type];
                }
            }
            if (npc.type != NPCID.Dryad)
            {
                ModifyAttackTime(npc, ref result);
            }
            return result;
        }

        public static void ModifySpeed(NPC npc, ref float Speed, ref float Acc)
        {
            if (npc.HasBuff(109) && npc.wet)      //����ҩˮ
            {
                Speed *= 1.5f;
                Acc *= 1.5f;
            }

            if (npc.HasBuff(BuffID.Swiftness))      //����ҩˮ
            {
                Speed *= 1.25f;
                Acc *= 1.25f;
            }

            if (npc.HasBuff(BuffID.WellFed3))       //ʳ��
            {
                Speed *= 1.4f;
                Acc *= 1.4f;
            }
            else if (npc.HasBuff(BuffID.WellFed2))
            {
                Speed *= 1.3f;
                Acc *= 1.3f;
            }
            else if (npc.HasBuff(BuffID.WellFed))
            {
                Speed *= 1.2f;
                Acc *= 1.2f;
            }

            Acc *= NPCAttacker.config.SpeedModifier;
            Speed *= NPCAttacker.config.SpeedModifier;
        }
        public static void ModifyCritChance(NPC npc, ref int CritChance)
        {
            if (npc.HasBuff(115))           //��ŭҩˮ
            {
                CritChance += 10;
            }
            if (npc.HasBuff(BuffID.WellFed3))       //ʳ��
            {
                CritChance += 4;
            }
            else if (npc.HasBuff(BuffID.WellFed2))
            {
                CritChance += 3;
            }
            else if (npc.HasBuff(BuffID.WellFed))
            {
                CritChance += 2;
            }
            if (npc.HasBuff(BuffID.Tipsy) && NPCID.Sets.AttackType[npc.type] == 3)
            {
                CritChance += 2;
            }
        }
        public static float ManaEnoughForPotions(Item weapon, int healmana)
        {
            if (weapon.mana == 0) return 1.2f;
            float ManaCanUse = 200 / 7f + healmana / 7f;
            float WeaponUseMana = weapon.mana * (60f / weapon.useTime);
            return Math.Clamp(ManaCanUse / WeaponUseMana, 0.25f, 1.2f);
        }
    }
}