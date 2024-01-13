using NPCAttacker.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace NPCAttacker.Systems
{
    public class SpecialUseProj : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (NPCAttacker.SpawnForNPCIndex != -1)
            {
                OldNPCProjOwner = NPCAttacker.SpawnForNPCIndex;
                NPCProjOwner = NPCAttacker.SpawnForNPCIndex;
                projectile.npcProj = true;
                projectile.noDropItem = true;

                if (Main.npc[NPCAttacker.SpawnForNPCIndex].active)
                {
                    NPC owner = Main.npc[NPCAttacker.SpawnForNPCIndex];
                    if (projectile.TryGetGlobalProjectile(out AttackerGProj modproj1))
                    {
                        modproj1.ProjTarget = owner.GetGlobalNPC<ArmedGNPC>().NPCTargetForSpecialUse;       //给弹幕一个目标,和额外数据
                        modproj1.ExtraInfo = Main.LocalPlayer.altFunctionUse;
                    }
                    if (projectile.TryGetGlobalProjectile(out SpecialUseProj modproj2))
                    {
                        modproj2.ChannelTimer = NPCAttacker.SpawnForChannelTime;           //预设蓄力时间
                    }


                    int CritChance = 0;
                    if (!ArmedGNPC.GetWeapon(owner).IsAir)
                    {
                        CritChance += ArmedGNPC.GetWeapon(owner).crit;
                    }
                    NPCStats.ModifyCritChance(owner, ref CritChance);

                    projectile.CritChance += CritChance;
                }
            }

        }

        public int NPCProjOwner = -1;
        public int OldNPCProjOwner = -1;

        public int ChannelTimer = -1;

        public override void PostAI(Projectile projectile)
        {
            if (ChannelTimer > 0) ChannelTimer--;

            UpdateNPCOwnerStatus();
            if (NPCProjOwner == -1 && OldNPCProjOwner != -1)
            {
                projectile.Kill();
            }
        }

        /*
        public override void PostDraw(Projectile projectile, Color lightColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>("NPCAttacker/icon").Value;
            Main.spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Color.White, 0, tex.Size() / 2, 0.5f, SpriteEffects.None, 0);
        }
        */

        public void UpdateNPCOwnerStatus()
        {
            if (NPCProjOwner == -1) return;
            if (!Main.npc[NPCProjOwner].active || !Main.npc[NPCProjOwner].IsTownNPC()) NPCProjOwner = -1;
        }
    }

    public class FuckingInvinciblePlayer : ModPlayer
    {
        public override bool ImmuneTo(PlayerDeathReason damageSource, int cooldownCounter, bool dodgeable)
        {
            return NPCAttacker.FuckingInvincible;
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource)
        {
            if (NPCAttacker.FuckingInvincible)
            {
                playSound = false;
                genDust = false;
                return false;
            }
            return true;
        }
    }
}
