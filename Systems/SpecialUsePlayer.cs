using Microsoft.Xna.Framework;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace NPCAttacker.Systems
{
    /*
    public class SpecialUsePlayer : ModPlayer
    {
        public bool StatsChangedToPw = false;

        PlayerStats _playerStats;

        public bool CheckUseTime()
        {
            if (_playerStats.itemAnimation < 0)
            {
                _playerStats.itemAnimation = 0;
            }
            if (_playerStats.itemTime < 0)
            {
                _playerStats.itemTime = 0;
            }
            if (_playerStats.itemAnimation == 0 && _playerStats.reuseDelay > 0)
            {
                _playerStats.itemAnimation = _playerStats.reuseDelay;
                _playerStats.itemTime = _playerStats.reuseDelay;
                _playerStats.reuseDelay = 0;
            }
            if (_playerStats.itemAnimation == 0)
            {
                _playerStats.itemAnimation = (_playerStats.itemAnimationMax = CombinedHooks.TotalAnimationTime((float)this.item.useAnimation, this.player, this.item));
                _playerStats.reuseDelay = (int)((float)this.item.reuseDelay / CombinedHooks.TotalUseSpeedMultiplier(this.player, this.item));
                this.ItemUsesThisAnimation = 0;
            }
            bool flag = _playerStats.itemAnimation > 0 && _playerStats.itemTime == 0;
            if (this.item.shootsEveryUse)
            {
                flag = (_playerStats.itemAnimation == _playerStats.itemAnimationMax && _playerStats.itemAnimation > 0);
            }
            if (_playerStats.itemTime == 0 && _playerStats.itemAnimation > 0)
            {
                _playerStats.itemTime = (_playerStats.itemTimeMax = CombinedHooks.TotalUseTime((float)this.item.useTime, this.player, this.item));
                this.ItemUsesThisAnimation++;
            }
            return (this.item.useLimitPerAnimation == null || this.ItemUsesThisAnimation < this.item.useLimitPerAnimation.Value) && flag;
        }

        public void Shoot()
        {
            PlayerStats.ChangeWithCheck(this.player, this.PlayerStats);
            player.GetModPlayer<PWPlayer>().ShootingWithPW = true;
            player.GetModPlayer<PWPlayer>().imbuedPw = this;
            Shoot_Inner();
            player.GetModPlayer<PWPlayer>().ShootingWithPW = false;
            player.GetModPlayer<PWPlayer>().imbuedPw = null;
            PlayerStats.CreateWithCheck(this.player, ref this.PlayerStats);
            PlayerStats.ChangePlayerTo(this.player, PWSystem.GlobalStatsPool);
            player.PW().StatsChangedToPw = false;
        }

        public static void Shoot_Inner(Player player,Item item,int damage)
        {
            MethodInfo ItemCheck_Shoot = typeof(Player).GetMethod("ItemCheck_Shoot", BindingFlags.Instance | BindingFlags.NonPublic);
            object[] parameters = new object[]
            {
                player.whoAmI,
                item,
                damage
            };
            ItemCheck_Shoot.Invoke(player, parameters);
        }
    }

    public class PlayerStats
    {
        // Token: 0x06000038 RID: 56 RVA: 0x00003850 File Offset: 0x00001A50
        public PlayerStats(Item heldItem, Vector2 center)
        {
            HeldItem = heldItem;
            Center = center;
            oldPosition = center;
            MouseWorld = Vector2.Zero;
            heldProj = -1;
            channel = false;
            direction = (itemAnimation = (itemAnimationMax = (itemTime = (itemTimeMax = (reuseDelay = 0)))));
            meleeNPCHitCooldown = new int[Main.maxNPCs];
        }

        // Token: 0x06000039 RID: 57 RVA: 0x000038D5 File Offset: 0x00001AD5
        public static void CreateWithCheck(Player player, ref PlayerStats defStats)
        {
            if (!player.GetModPlayer<SpecialUsePlayer>().StatsChangedToPw)
            {
                return;
            }
            CreateFromPlayer(player, ref defStats);
            defStats.oldPosition = player.position;
        }

        // Token: 0x0600003A RID: 58 RVA: 0x000038F9 File Offset: 0x00001AF9
        public static void ChangeWithCheck(Player player, PlayerStats stats)
        {
            if (player.GetModPlayer<SpecialUsePlayer>().StatsChangedToPw)
            {
                return;
            }
            ChangePlayerTo(player, stats);
            player.oldPosition = player.position;
            player.GetModPlayer<SpecialUsePlayer>().StatsChangedToPw = true;
        }

        // Token: 0x0600003B RID: 59 RVA: 0x00003928 File Offset: 0x00001B28
        public static void CreateFromPlayer(Player player, ref PlayerStats defStats)
        {
            defStats = new PlayerStats(player.inventory[player.selectedItem], player.Center)
            {
                oldPosition = player.oldPosition,
                MouseWorld = Main.MouseWorld,
                heldProj = player.heldProj,
                channel = player.channel,
                direction = player.direction,
                itemAnimation = player.itemAnimation,
                itemAnimationMax = player.itemAnimationMax,
                itemTime = player.itemTime,
                itemTimeMax = player.itemTimeMax,
                reuseDelay = player.reuseDelay,
                meleeNPCHitCooldown = player.meleeNPCHitCooldown
            };
        }

        // Token: 0x0600003C RID: 60 RVA: 0x000039D4 File Offset: 0x00001BD4
        public static void ChangePlayerTo(Player player, PlayerStats stats)
        {
            player.inventory[player.selectedItem] = stats.HeldItem;
            player.Center = stats.Center;
            player.oldPosition = stats.oldPosition;
            Main.mouseX = (int)(stats.MouseWorld.X - Main.screenPosition.X);
            Main.mouseY = (int)(stats.MouseWorld.Y - Main.screenPosition.Y);
            player.heldProj = stats.heldProj;
            player.channel = stats.channel;
            player.direction = stats.direction;
            player.itemAnimation = stats.itemAnimation;
            player.itemAnimationMax = stats.itemAnimationMax;
            player.itemTime = stats.itemTime;
            player.itemTimeMax = stats.itemTimeMax;
            player.reuseDelay = stats.reuseDelay;
            player.meleeNPCHitCooldown = stats.meleeNPCHitCooldown;
        }

        public Item HeldItem;

        public Vector2 Center;

        public Vector2 oldPosition;

        public Vector2 MouseWorld;

        public int heldProj;

        public bool channel;

        public int direction;

        public int itemAnimation;

        public int itemAnimationMax;

        public int itemTime;

        public int itemTimeMax;

        public int reuseDelay;

        public int[] meleeNPCHitCooldown;
    }
    */
}
