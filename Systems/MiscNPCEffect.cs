using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NPCAttacker.NPCs
{

    public class MiscNPCEffect : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public int RealLife = -1;
        public bool? RealLavaImmune = null;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.IsTownNPC();
        }


        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (npc.HasBuff(BuffID.Regeneration))             //恢复药水
            {
                npc.lifeRegen += 2 * 2;
            }
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            NPCStats.ModifyDR(npc, ref modifiers);
        }



        public override void ModifyHitNPC(NPC npc, NPC target, ref NPC.HitModifiers modifiers)
        {

            NPCStats.ModifyDR(target, ref modifiers);

            if (NPCUtils.BuffNPC())
            {
                if (NPCID.Sets.AttackType[target.type] == 3)
                {
                    modifiers.FinalDamage *= 0.5f;
                }
            }
        }
        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (NPCUtils.BuffNPC())
            {
                int ExtraDef = 0;

                NPCStats.ModifyDef(npc, ref ExtraDef);
                modifiers.SourceDamage.Flat -= ExtraDef;
            }
        }

        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (npc.HasBuff(BuffID.Invisibility))             //隐身药水
            {
                return false;
            }
            return true;
        }
        public override void PostAI(NPC npc)
        {
            if (npc.HasBuff(BuffID.Lifeforce))           //生命力药水
            {
                npc.lifeMax = RealLife + 100;
            }
            else
            {
                if (RealLife == -1)
                {
                    RealLife = npc.lifeMax;
                }
                npc.lifeMax = RealLife;
            }

            if (npc.HasBuff(BuffID.ObsidianSkin))           //黑曜石皮肤药水
            {
                npc.lavaImmune = true;
                npc.onFire = false;
                npc.onFire2 = false;
                npc.onFire3 = false;
                for (int i = 0; i < npc.buffTime.Length; i++)
                {
                    switch (npc.buffType[i])
                    {
                        case BuffID.OnFire:
                        case BuffID.OnFire3:
                        case BuffID.Burning:
                            npc.buffTime[i] = 0;
                            break;
                    }
                }
            }
            else
            {
                if (RealLavaImmune == null)
                {
                    RealLavaImmune = npc.lavaImmune;
                }
                npc.lavaImmune = RealLavaImmune.Value;
            }

            if (npc.HasBuff(BuffID.Featherfall))        //羽落药水
            {
                if (npc.velocity.Y > 5)
                {
                    npc.velocity.Y = 5;
                }
            }
            if (npc.HasBuff(4))    //鱼鳃药水
            {
                if (npc.breath < 200)
                {
                    npc.breath = 200;
                }
            }

            if (npc.HasBuff(BuffID.Shine))       //光芒药水
            {
                Lighting.AddLight((int)npc.Center.X / 16, (int)npc.Center.Y / 16, 0.8f, 0.95f, 1f);
            }

            if (npc.HasBuff(BuffID.Inferno))   //狱火药水
            {
                Lighting.AddLight((int)(npc.Center.X / 16f), (int)(npc.Center.Y / 16f), 0.65f, 0.4f, 0.1f);
                int num3 = 24;
                float num4 = 200f;
                bool flag = Main.rand.NextBool(60);
                int damage = 10;

                for (int k = 0; k < 200; k++)
                {
                    NPC target = Main.npc[k];
                    if (target.active && !target.friendly && target.damage > 0 && !target.dontTakeDamage && !target.buffImmune[num3] && Vector2.Distance(npc.Center, target.Center) <= num4)
                    {
                        if (target.FindBuffIndex(num3) == -1)
                        {
                            target.AddBuff(num3, 120, false);
                        }
                        if (flag)
                        {
                            target.SimpleStrikeNPC(damage, 0);
                        }
                    }
                }

            }



        }


        public override void OnHitNPC(NPC npc, NPC target, NPC.HitInfo hit)
        {
            if (target.HasBuff(BuffID.Thorns) && target.IsTownNPC())
            {
                npc.SimpleStrikeNPC(hit.Damage, -Math.Sign(target.Center.X - npc.Center.X), false, hit.Knockback);
            }
        }

    }
}
