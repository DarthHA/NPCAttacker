using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace NPCAttacker
{
    public class StrikeOverride
    {
        public static int StrikeNPCHook(On_NPC.orig_StrikeNPC_HitInfo_bool_bool orig, NPC Npc, NPC.HitInfo hit, bool fromNet = false, bool noPlayerInteraction = false)
        {
            if (!Npc.IsTownNPC())
            {
                return orig.Invoke(Npc, hit, fromNet, noPlayerInteraction);
            }
            bool flag = Main.netMode == NetmodeID.SinglePlayer;
            flag &= !noPlayerInteraction;
            if (!Npc.active || Npc.life <= 0)
            {
                return 0;
            }
            double num = hit.Damage;
            bool crit = hit.Crit;
            int hitDirection = hit.HitDirection;
            if (hit.InstantKill)
            {
                num = (Npc.realLife > 0) ? Main.npc[Npc.realLife].life : Npc.life;
            }
            if (!hit.HideCombatText && !hit.InstantKill && Npc.lifeMax > 1 && !Npc.HideStrikeDamage)
            {
                if (Npc.friendly)
                {
                    Color color = crit ? CombatText.DamagedFriendlyCrit : CombatText.DamagedFriendly;
                    CombatText.NewText(new Rectangle((int)Npc.position.X, (int)Npc.position.Y, Npc.width, Npc.height), color, (int)num, crit, false);
                }
                else
                {
                    Color color2 = crit ? CombatText.DamagedHostileCrit : CombatText.DamagedHostile;
                    if (fromNet)
                    {
                        color2 = (crit ? CombatText.OthersDamagedHostileCrit : CombatText.OthersDamagedHostile);
                    }
                    CombatText.NewText(new Rectangle((int)Npc.position.X, (int)Npc.position.Y, Npc.width, Npc.height), color2, (int)num, crit, false);
                }
            }
            if (num >= 1.0)
            {
                if (flag)
                {
                    Npc.PlayerInteraction(Main.myPlayer);
                }
                Npc.justHit = true;

                if (!NPCUtils.BuffNPC())
                {
                    if (Npc.townNPC)
                    {
                        if (Npc.aiStyle == 7 && (Npc.ai[0] == 3f || Npc.ai[0] == 4f || Npc.ai[0] == 16f || Npc.ai[0] == 17f))
                        {
                            NPC nPC = Main.npc[(int)Npc.ai[2]];
                            if (nPC.active)
                            {
                                nPC.ai[0] = 1f;
                                nPC.ai[1] = 300 + Main.rand.Next(300);
                                nPC.ai[2] = 0f;
                                nPC.localAI[3] = 0f;
                                nPC.direction = hitDirection;
                                nPC.netUpdate = true;
                            }
                        }
                        Npc.ai[0] = 1f;
                        Npc.ai[1] = 300 + Main.rand.Next(300);
                        Npc.ai[2] = 0f;
                        Npc.localAI[3] = 0f;
                        Npc.direction = hitDirection;
                        Npc.netUpdate = true;
                    }
                }

                if (Npc.IsTownNPC())
                {
                    if (Npc.GetGlobalNPC<ArmedGNPC>().actMode == ArmedGNPC.ActMode.Attack)
                    {
                        if (NPCID.Sets.AttackType[Npc.type] == 3)
                        {
                            hit.Knockback = 0;
                        }
                    }

                    if (Npc.HasBuff(BuffID.Invisibility))   //????
                    {
                        hit.Knockback = 0;
                    }

                    if (NPC.downedMoonlord)
                    {
                        hit.Knockback *= 0.25f;
                    }
                    else if (Main.hardMode)
                    {
                        hit.Knockback *= 0.5f;
                    }
                }

                if (!Npc.immortal)
                {
                    if (Npc.realLife >= 0)
                    {
                        Main.npc[Npc.realLife].life -= (int)num;
                        Npc.life = Main.npc[Npc.realLife].life;
                        Npc.lifeMax = Main.npc[Npc.realLife].lifeMax;
                    }
                    else
                    {
                        Npc.life -= (int)num;
                    }
                }
                if (hit.Knockback > 0f)
                {
                    float num3 = hit.Knockback;
                    if (num3 > 8f)
                    {
                        float num4 = num3 - 8f;
                        num4 *= 0.9f;
                        num3 = 8f + num4;
                    }
                    if (num3 > 10f)
                    {
                        float num5 = num3 - 10f;
                        num5 *= 0.8f;
                        num3 = 10f + num5;
                    }
                    if (num3 > 12f)
                    {
                        float num6 = num3 - 12f;
                        num6 *= 0.7f;
                        num3 = 12f + num6;
                    }
                    if (num3 > 14f)
                    {
                        float num7 = num3 - 14f;
                        num7 *= 0.6f;
                        num3 = 14f + num7;
                    }
                    if (num3 > 16f)
                    {
                        num3 = 16f;
                    }
                    if (crit)
                    {
                        num3 *= 1.4f;
                    }
                    int num8 = (int)num * 10;
                    if (Main.expertMode)
                    {
                        num8 = (int)num * 15;
                    }
                    if (num8 > Npc.lifeMax)
                    {
                        if (hitDirection < 0 && Npc.velocity.X > 0f - num3)
                        {
                            if (Npc.velocity.X > 0f)
                            {
                                Npc.velocity.X = Npc.velocity.X - num3;
                            }
                            Npc.velocity.X = Npc.velocity.X - num3;
                            if (Npc.velocity.X < 0f - num3)
                            {
                                Npc.velocity.X = 0f - num3;
                            }
                        }
                        else if (hitDirection > 0 && Npc.velocity.X < num3)
                        {
                            if (Npc.velocity.X < 0f)
                            {
                                Npc.velocity.X = Npc.velocity.X + num3;
                            }
                            Npc.velocity.X = Npc.velocity.X + num3;
                            if (Npc.velocity.X > num3)
                            {
                                Npc.velocity.X = num3;
                            }
                        }
                        if (Npc.type == 185)
                        {
                            num3 *= 1.5f;
                        }
                        num3 = (Npc.noGravity ? (num3 * -0.5f) : (num3 * -0.75f));
                        if (Npc.velocity.Y > num3)
                        {
                            Npc.velocity.Y = Npc.velocity.Y + num3;
                            if (Npc.velocity.Y < num3)
                            {
                                Npc.velocity.Y = num3;
                            }
                        }
                    }
                    else
                    {
                        if (!Npc.noGravity)
                        {
                            Npc.velocity.Y = (0f - num3) * 0.75f * Npc.knockBackResist;
                        }
                        else
                        {
                            Npc.velocity.Y = (0f - num3) * 0.5f * Npc.knockBackResist;
                        }
                        Npc.velocity.X = num3 * hitDirection * Npc.knockBackResist;
                    }
                }

                Npc.HitEffect(hit);

                if (Npc.HitSound != null)
                {
                    SoundEngine.PlaySound(Npc.HitSound, new Vector2?(Npc.position));
                }
                if (Npc.realLife >= 0)
                {
                    Main.npc[Npc.realLife].checkDead();
                }
                else
                {
                    Npc.checkDead();
                }
                return (int)num;
            }
            return 0;
        }
    }
}