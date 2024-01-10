using Microsoft.Xna.Framework;
using Terraria;

namespace NPCAttacker.Systems
{

    public class PlayerDataSaver
    {
        public bool HasValue = false;
        public int Life;
        public int LifeMax;
        public int Mana;
        public int ManaMax;
        public int itemTime;
        public int itemTimeMax;
        public int itemAnimation;
        public int itemAnimationMax;
        public int reuseDelay;
        public Vector2 Position;
        public Vector2 OldPosition;
        public Vector2 Velocity;
        public Vector2 OldVelocity;
        public Item HeldItem;
        public Vector2 MouseWorld;
        public int heldProj;
        public bool channel;
        public bool ControlUseItem;
        public int direction;


        public void CloneFrom(Player player)
        {
            HasValue = true;
            Life = player.statLife;
            LifeMax = player.statLifeMax2;
            Mana = player.statMana;
            ManaMax = player.statManaMax2;
            itemTime = player.itemTime;
            itemTimeMax = player.itemTimeMax;
            itemAnimation = player.itemAnimation;
            itemAnimationMax = player.itemAnimationMax;
            reuseDelay = player.reuseDelay;
            Position = player.position;
            OldPosition = player.oldPosition;
            Velocity = player.velocity;
            OldVelocity = player.oldVelocity;
            HeldItem = player.HeldItem;
            MouseWorld = Main.MouseWorld;
            heldProj = player.heldProj;
            channel = player.channel;
            ControlUseItem = player.controlUseItem;
            direction = player.direction;
        }

        public void CloneTo(Player player)
        {
            if (HasValue)
            {
                HasValue = false;
                player.statLife = Life;
                player.statLifeMax2 = LifeMax;
                player.statMana = Mana;
                player.statManaMax2 = ManaMax;
                player.itemTime = itemTime;
                player.itemTimeMax = itemTimeMax;
                player.itemAnimation = itemAnimation;
                player.itemAnimationMax = itemAnimationMax;
                player.reuseDelay = reuseDelay;
                player.position = Position;
                player.oldPosition = OldPosition;
                player.velocity = Velocity;
                player.oldVelocity = OldVelocity;
                Main.mouseX = (int)(MouseWorld.X - Main.screenPosition.X);
                Main.mouseY = (int)(MouseWorld.Y - Main.screenPosition.Y);
                player.heldProj = heldProj;
                player.inventory[player.selectedItem] = HeldItem;
                player.channel = channel;
                player.controlUseItem = ControlUseItem;
                player.direction = direction;
            }
        }

        public void CloneTo(PlayerDataSaver other)
        {
            if (HasValue)
            {
                other.HasValue = HasValue;
                other.Life = Life;
                other.LifeMax = LifeMax;
                other.Mana = Mana;
                other.ManaMax = ManaMax;
                other.itemTime = itemTime;
                other.itemTimeMax = itemTimeMax;
                other.itemAnimation = itemAnimation;
                other.itemAnimationMax = itemAnimationMax;
                other.reuseDelay = reuseDelay;
                other.Position = Position;
                other.OldPosition = OldPosition;
                other.Velocity = Velocity;
                other.OldVelocity = OldVelocity;
                other.MouseWorld = MouseWorld;
                other.heldProj = heldProj;
                other.HeldItem = HeldItem;
                other.channel = channel;
                other.ControlUseItem = ControlUseItem;
                other.direction = direction;
            }
        }
    }

}
