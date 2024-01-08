using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace NPCAttacker.Items
{
    public class AttackerStick : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 0;
            Item.noMelee = true;
            Item.width = 36;
            Item.height = 38;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = 10000;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 10;
            Item.rare = ItemRarityID.Expert;
            Item.UseSound = SoundID.Item1;
            Item.channel = true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (NPCAttacker.CurserInMap) return false;

            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)      //右键
            {
                UIUtils.RightClick(player);
            }
            else
            {
                UIUtils.LeftClickForStick(player, Main.MouseWorld);
            }
            return false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (ItemSlot.ShiftInUse)
            {
                string description = Language.GetTextValue("Mods.NPCAttacker.ItemTooltipExtra");
                tooltips.Add(new TooltipLine(Mod, "tooltip", description));
            }
            else
            {
                string description = Language.GetTextValue("Mods.NPCAttacker.ItemTooltipExtra2");
                tooltips.Add(new TooltipLine(Mod, "tooltip", description));
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.KingStatue, 3)
            .AddIngredient(ItemID.QueenStatue, 3)
            .AddIngredient(ItemID.LovePotion, 10)
            .AddIngredient(ItemID.StinkPotion, 10)
            .AddIngredient(ItemID.CombatBook, 1)
            .AddTile(TileID.DemonAltar)

            .Register();
        }
    }
}