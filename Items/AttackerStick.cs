using Microsoft.Xna.Framework;
using NPCAttacker.NPCs;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace NPCAttacker.Items
{
	public class AttackerStick : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Commander's Baton");
			DisplayName.AddTranslation(GameCulture.Chinese, "指挥官的指挥棒");
			Tooltip.SetDefault(
				"Left click to allow all town NPCs to attack at your mouse position\n" +
				"In hardmode, right click to let them assemble to your mouse position\n" +
				"NPCs will get bonus from your stats and game progress\n" +
				"\'All units! Ready, aim, and fire!\'");
			Tooltip.AddTranslation(GameCulture.Chinese,
				"使用左键会允许所有城镇NPC攻击你的鼠标位置\n" +
				"困难模式后，右键让他们集结到你的鼠标位置\n" +
				"NPC会根据你的属性和游戏时期得到加成\n" +
				"\'所有单位！预备，瞄准，开火！\'");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 0;
			item.noMelee = true;
			item.width = 36;
			item.height = 38;
			item.useTime = 60;
			item.useAnimation = 60;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.value = 10000;
			item.shoot = ProjectileID.WoodenArrowFriendly;
			item.shootSpeed = 10;
			item.rare = ItemRarityID.Expert;
			item.UseSound = SoundID.Item1;
			item.channel = true;
		}

		public override bool AltFunctionUse(Player player)
        {
            if (Main.hardMode)
            {
				return true;
            }
			return false;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
				item.UseSound = null;
			}
            else
            {
				item.UseSound = SoundID.Item1;
			}
			return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.altFunctionUse != 2)
			{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/MookStart"), player.Center);
				if (!NPC.AnyNPCs(ModContent.NPCType<TargetNPC>()))
				{
					NPC.NewNPC((int)Main.MouseWorld.X, (int)Main.MouseWorld.Y, ModContent.NPCType<TargetNPC>(), default, player.whoAmI);
				}
				foreach (NPC npc in Main.npc)
				{
					if (npc.active && npc.type == ModContent.NPCType<AssembleNPC>())
					{
						npc.active = false;
					}
				}
			}
			else
			{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Whistles"), player.Center);
				if(!NPC.AnyNPCs(ModContent.NPCType<AssembleNPC>()))
				{
					NPC.NewNPC((int)Main.MouseWorld.X, (int)Main.MouseWorld.Y, ModContent.NPCType<AssembleNPC>(), default, player.whoAmI);
				}
                else 
				{
					foreach(NPC npc in Main.npc)
                    {
						if(npc.active && npc.type == ModContent.NPCType<AssembleNPC>())
                        {
							npc.active = false;
                        }
                    }
                }
			}
			player.talkNPC = -1;
			return false;
		}

        public override void AddRecipes()
        {
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.KingStatue, 3);
			recipe.AddIngredient(ItemID.QueenStatue, 3);
			recipe.AddIngredient(ItemID.LovePotion, 10);
			recipe.AddIngredient(ItemID.StinkPotion, 10);
			recipe.AddIngredient(ModContent.ItemType<AdvancedAdvancedCombatTechniques>());
			recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }

    }

}