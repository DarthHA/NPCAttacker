using Microsoft.Xna.Framework;
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
            OldNPCProjOwner = PlayerDataSaver.SpawnForNPCIndex;
            NPCProjOwner = PlayerDataSaver.SpawnForNPCIndex;
            projectile.npcProj = true;
            projectile.noDropItem = true;
        }

        public int NPCProjOwner = -1;
        public int OldNPCProjOwner = -1;

        public override bool PreAI(Projectile projectile)
        {
            UpdateNPCOwnerStatus();
            if (NPCProjOwner != -1 && Main.LocalPlayer.active)
            {
                PlayerDataSaver.CloneFrom(Main.LocalPlayer);
                PlayerDataSaver.FuckingInvincible = true;
                Main.player[projectile.owner].Center = Main.npc[NPCProjOwner].Center;
                Main.player[projectile.owner].velocity = Main.npc[NPCProjOwner].velocity;
                Main.player[projectile.owner].direction = Main.npc[NPCProjOwner].direction;
            }
            return true;
        }

        public override void PostAI(Projectile projectile)
        {
            if (Main.LocalPlayer.active)
            {
                PlayerDataSaver.CloneTo(Main.LocalPlayer);
                PlayerDataSaver.FuckingInvincible = false;
            }
            UpdateNPCOwnerStatus();
            if (NPCProjOwner == -1 && OldNPCProjOwner != -1)
            {
                projectile.Kill();
            }
        }

        public void UpdateNPCOwnerStatus()
        {
            if (NPCProjOwner == -1) return;
            if (!Main.npc[NPCProjOwner].active || !Main.npc[NPCProjOwner].IsTownNPC()) NPCProjOwner = -1;
        }
    }
    
    public static class PlayerDataSaver
    {
        public static bool HasValue = false;
        public static int Life;
        public static int LifeMax;
        public static int itemTime;
        public static int itemTimeMax;
        public static int itemAnimation;
        public static int itemAnimationMax;
        public static int reuseDelay;
        public static Vector2 Position;
        public static Vector2 Velocity;
        public static Vector2 oldPosition;
        public static Item HeldItem;
        public static Vector2 MouseWorld;
        public static int heldProj;
        public static bool channel;
        public static int direction;

        public static bool FuckingInvincible = false;
        public static int SpawnForNPCIndex = -1;

        public static void CloneFrom(Player player)
        {
            HasValue = true;
            Life = player.statLife;
            LifeMax = player.statLifeMax2;
            itemTime = player.itemTime;
            itemTimeMax = player.itemTimeMax;
            itemAnimation = player.itemAnimation;
            itemAnimationMax = player.itemAnimationMax;
            reuseDelay = player.reuseDelay;
            Position = player.position;
            Velocity = player.velocity;
            oldPosition = player.oldPosition;
            HeldItem = player.HeldItem;
            MouseWorld = Main.MouseWorld;
            heldProj = player.heldProj;
            channel = player.channel;
            direction = player.direction;
        }

        public static void CloneTo(Player player)
        {
            if (HasValue)
            {
                HasValue = false;
                player.statLife = Life;
                player.statLifeMax2 = LifeMax;
                player.itemTime = itemTime;
                player.itemTimeMax = itemTimeMax;
                player.itemAnimation = itemAnimation;
                player.itemAnimationMax = itemAnimationMax;
                player.reuseDelay = reuseDelay;
                player.position = Position;
                player.velocity = Velocity;
                player.oldPosition = oldPosition;
                Main.mouseX = (int)(MouseWorld.X - Main.screenPosition.X);
                Main.mouseY = (int)(MouseWorld.Y - Main.screenPosition.Y);
                player.heldProj = heldProj;
                player.channel = channel;
                player.direction = direction;
            }
        }
    }

    public class FuckingInvinciblePlayer : ModPlayer
    {
        public override bool ImmuneTo(PlayerDeathReason damageSource, int cooldownCounter, bool dodgeable)
        {
            return PlayerDataSaver.FuckingInvincible;
        }
    }
}
