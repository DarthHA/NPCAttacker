using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace NPCAttacker.Projectiles
{
    public class AttackerGProj : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public int ProjTarget = -1;
        public int FlaskBuffID = -1;
        public int ExtraInfo = 0;

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
                    if (NPCUtils.BuffNPC())
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

                    if (NPC.AnyNPCs(NPCID.Nurse))
                    {
                        NPC nurse = Main.npc[NPC.FindFirstNPC(NPCID.Nurse)];
                        if (!ArmedGNPC.GetWeapon(nurse).IsAir)
                        {
                            HealLife = ArmedGNPC.GetWeapon(nurse).healLife / 2;
                        }

                        if (!ArmedGNPC.GetAltWeapon(nurse).IsAir)
                        {
                            target.AddBuff(ArmedGNPC.GetAltWeapon(nurse).buffType, ArmedGNPC.GetAltWeapon(nurse).buffTime);
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

            if (NPCUtils.BuffNPC())
            {
                if (projectile.npcProj)
                {
                    if (!projectile.usesIDStaticNPCImmunity)
                    {
                        projectile.usesIDStaticNPCImmunity = true;
                        projectile.idStaticNPCHitCooldown = 10;
                    }
                }
            }

            //排除原版某些弹幕的独一性
            if (projectile.npcProj)
            {
                if (projectile.type == ProjectileID.BloodCloudRaining || projectile.type == ProjectileID.RainCloudRaining)
                {

                    if (++projectile.frameCounter > 8)
                    {
                        projectile.frameCounter = 0;
                        if (++projectile.frame > 5)
                        {
                            projectile.frame = 0;
                        }
                    }
                    projectile.ai[1] += 1f;
                    if (projectile.type == 244 && projectile.ai[1] >= 180f)
                    {
                        projectile.alpha += 5;
                        if (projectile.alpha > 255)
                        {
                            projectile.alpha = 255;
                            projectile.Kill();
                        }
                    }
                    else if (projectile.type == 238 && projectile.ai[1] >= 360f)
                    {
                        projectile.alpha += 5;
                        if (projectile.alpha > 255)
                        {
                            projectile.alpha = 255;
                            projectile.Kill();
                        }
                    }
                    else
                    {
                        projectile.ai[0] += 1f;
                        if (projectile.type == 244)
                        {
                            if (projectile.ai[0] > 10f)
                            {
                                projectile.ai[0] = 0f;
                                if (projectile.owner == Main.myPlayer)
                                {
                                    int num417 = (int)(projectile.position.X + 14f + Main.rand.Next(projectile.width - 28));
                                    int num418 = (int)(projectile.position.Y + projectile.height + 4f);
                                    int protmp = Projectile.NewProjectile(null, num417, num418, 0f, 5f, ProjectileID.BloodRain, projectile.damage, 0f, projectile.owner, 0f, 0f);
                                    Main.projectile[protmp].npcProj = true;
                                }
                            }
                        }
                        else if (projectile.ai[0] > 8f)
                        {
                            projectile.ai[0] = 0f;
                            if (projectile.owner == Main.myPlayer)
                            {
                                int num419 = (int)(projectile.position.X + 14f + Main.rand.Next(projectile.width - 28));
                                int num420 = (int)(projectile.position.Y + projectile.height + 4f);
                                int protmp = Projectile.NewProjectile(null, num419, num420, 0f, 5f, ProjectileID.RainFriendly, projectile.damage, 0f, projectile.owner, 0f, 0f);
                                Main.projectile[protmp].npcProj = true;
                            }
                        }
                    }
                    return false;
                }

                else if (projectile.type == ProjectileID.ClingerStaff)
                {
                    projectile.position.Y = projectile.ai[0];
                    projectile.height = (int)projectile.ai[1];
                    if (projectile.Center.X > Main.player[projectile.owner].Center.X)
                    {
                        projectile.direction = 1;
                    }
                    else
                    {
                        projectile.direction = -1;
                    }
                    projectile.velocity.X = projectile.direction * 1E-06f;
                    float num842 = projectile.width * projectile.height * 0.0045f;
                    int num843 = 0;
                    while (num843 < num842)
                    {
                        int num844 = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.CursedTorch, 0f, 0f, 100, default, 1f);
                        Main.dust[num844].noGravity = true;
                        Dust dust3 = Main.dust[num844];
                        dust3.velocity *= 0.5f;
                        Dust dust101 = Main.dust[num844];
                        dust101.velocity.Y -= 0.5f;
                        Main.dust[num844].scale = 1.4f;
                        Dust dust102 = Main.dust[num844];
                        dust102.position.X += 6f;
                        Dust dust103 = Main.dust[num844];
                        dust103.position.Y -= 2f;
                        num843++;
                    }
                    return false;
                }

                else if (projectile.type == ProjectileID.MagnetSphereBall)
                {
                    if (projectile.ai[0] == 0f)
                    {
                        projectile.ai[0] = projectile.velocity.X;
                        projectile.ai[1] = projectile.velocity.Y;
                    }
                    if (projectile.velocity.X > 0f)
                    {
                        projectile.rotation += (Math.Abs(projectile.velocity.Y) + Math.Abs(projectile.velocity.X)) * 0.001f;
                    }
                    else
                    {
                        projectile.rotation -= (Math.Abs(projectile.velocity.Y) + Math.Abs(projectile.velocity.X)) * 0.001f;
                    }

                    if (++projectile.frameCounter > 6)
                    {
                        projectile.frameCounter = 0;
                        if (++projectile.frame > 4)
                        {
                            projectile.frame = 0;
                        }
                    }
                    if (Math.Sqrt(projectile.velocity.X * projectile.velocity.X + projectile.velocity.Y * projectile.velocity.Y) > 3)
                    {
                        projectile.velocity *= 0.98f;
                    }

                    int[] array = new int[20];
                    int num433 = 0;
                    float num434 = 300f;
                    bool flag14 = false;
                    for (int num435 = 0; num435 < 200; num435++)
                    {
                        if (Main.npc[num435].CanBeChasedBy(projectile, false))
                        {
                            float num436 = Main.npc[num435].position.X + Main.npc[num435].width / 2;
                            float num437 = Main.npc[num435].position.Y + Main.npc[num435].height / 2;
                            float num438 = Math.Abs(projectile.position.X + projectile.width / 2 - num436) + Math.Abs(projectile.position.Y + projectile.height / 2 - num437);
                            if (num438 < num434 && Collision.CanHit(projectile.Center, 1, 1, Main.npc[num435].Center, 1, 1))
                            {
                                if (num433 < 20)
                                {
                                    array[num433] = num435;
                                    num433++;
                                }
                                flag14 = true;
                            }
                        }
                    }
                    if (projectile.timeLeft < 30)
                    {
                        flag14 = false;
                    }
                    if (flag14)
                    {
                        int num439 = Main.rand.Next(num433);
                        num439 = array[num439];
                        float num440 = Main.npc[num439].position.X + Main.npc[num439].width / 2;
                        float num441 = Main.npc[num439].position.Y + Main.npc[num439].height / 2;
                        projectile.localAI[0] += 1f;
                        if (projectile.localAI[0] > 8f)
                        {
                            projectile.localAI[0] = 0f;
                            float num442 = 6f;
                            Vector2 vector32 = new Vector2(projectile.position.X + projectile.width * 0.5f, projectile.position.Y + projectile.height * 0.5f);
                            vector32 += projectile.velocity * 4f;
                            float num443 = num440 - vector32.X;
                            float num444 = num441 - vector32.Y;
                            float num445 = (float)Math.Sqrt(num443 * num443 + num444 * num444);
                            num445 = num442 / num445;
                            num443 *= num445;
                            num444 *= num445;
                            int protmp = Projectile.NewProjectile(null, vector32.X, vector32.Y, num443, num444, ProjectileID.MagnetSphereBolt, projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
                            Main.projectile[protmp].npcProj = true;
                            return false;
                        }
                    }
                    return false;
                }

            }

            return true;
        }


        public override void PostAI(Projectile projectile)
        {
            if (NPCUtils.BuffNPC())
            {
                if (projectile.type == ProjectileID.DryadsWardCircle)
                {
                    float range = 1200f;

                    Player player10 = Main.LocalPlayer;
                    if (player10.active && !player10.dead && projectile.Distance(player10.Center) <= range && player10.FindBuffIndex(BuffID.DryadsWard) == -1)
                    {
                        player10.AddBuff(BuffID.DryadsWard, 180, true, false);
                    }

                    foreach (NPC npc10 in Main.npc)
                    {
                        if (npc10.type != NPCID.TargetDummy && npc10.active && projectile.Distance(npc10.Center) <= range)
                        {
                            if (npc10.townNPC)
                            {
                                npc10.AddBuff(BuffID.DryadsWard, 180, false);
                            }
                        }
                    }
                }
            }

        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (NPCUtils.BuffNPC())
            {
                if (projectile.npcProj)
                {
                    if (Main.rand.Next(100) < projectile.CritChance)
                    {
                        modifiers.SetCrit();
                    }

                    if (FlaskBuffID != 0)
                    {
                        NPCUtils.FlaskToDebuff(target, FlaskBuffID);
                    }
                }
            }
        }


        public override bool PreKill(Projectile projectile, int timeLeft)
        {
            if (projectile.npcProj)
            {
                if (projectile.type == ProjectileID.Xenopopper)
                {
                    SoundEngine.PlaySound(SoundID.Item96, projectile.position);
                    int num267 = Main.rand.Next(5, 9);
                    int num3;
                    for (int num268 = 0; num268 < num267; num268 = num3 + 1)
                    {
                        int num269 = Dust.NewDust(projectile.Center, 0, 0, DustID.Venom, 0f, 0f, 100, default, 1.4f);
                        Dust dust = Main.dust[num269];
                        dust.velocity *= 0.8f;
                        Main.dust[num269].position = Vector2.Lerp(Main.dust[num269].position, projectile.Center, 0.5f);
                        Main.dust[num269].noGravity = true;
                        num3 = num268;
                    }

                    Vector2 value11 = Main.MouseWorld;
                    if (Main.player[projectile.owner].gravDir == -1f)
                    {
                        value11.Y = Main.screenHeight - Main.mouseY + Main.screenPosition.Y;
                    }
                    int target = projectile.GetGlobalProjectile<AttackerGProj>().ProjTarget;
                    if (target != -1)
                    {
                        if (Main.npc[target].active)
                        {
                            value11 = Main.npc[target].Center;
                        }
                    }
                    Vector2 vector13 = Vector2.Normalize(value11 - projectile.Center);
                    vector13 *= projectile.localAI[1];
                    int protmp = Projectile.NewProjectile(null, projectile.Center, vector13, (int)projectile.localAI[0], projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
                    Main.projectile[protmp].npcProj = true;
                    Main.projectile[protmp].noDropItem = true;


                    return false;
                }
                if (projectile.type == ProjectileID.BloodCloudMoving)
                {
                    int protmp = Projectile.NewProjectile(null, projectile.Center, Vector2.Zero, ProjectileID.BloodCloudRaining, projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
                    Main.projectile[protmp].npcProj = true;
                    return false;
                }
                if (projectile.type == ProjectileID.RainCloudMoving)
                {
                    int protmp = Projectile.NewProjectile(null, projectile.Center, Vector2.Zero, ProjectileID.RainCloudRaining, projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
                    Main.projectile[protmp].npcProj = true;
                    return false;
                }
            }
            return true;
        }
    }
}
