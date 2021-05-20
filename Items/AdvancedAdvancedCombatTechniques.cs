using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace NPCAttacker.Items
{
    public class AdvancedAdvancedCombatTechniques : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Advanced Advanced Combat Techniques");
			DisplayName.AddTranslation(GameCulture.Chinese, "先进先进作战手册");
			Tooltip.SetDefault(
				"Contains offensive and defensive fighting techniques\n" +
				"Increases the defense and strength of all villagers...in a different way\n" +
				"Can be obtained by fishing during a Blood Moon\n" +
				"\'This time, we no longer flinch!\'");
			Tooltip.AddTranslation(GameCulture.Chinese,
				"含有攻防战斗技能\n" +
				"以另一种方式增强所有村民的防御力和力量\n" +
				"血月钓鱼有几率获得\n" +
				"“这次，我们不再退缩！”");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 30;
			item.rare = ItemRarityID.Green;
		}

        public override bool CanRightClick()
        {
			if (NPC.downedMoonlord)
            {
				return true;
            }
			return false;
        }
        public override void RightClick(Player player)
        {
			if (NPC.downedMoonlord)
			{
				SummonNPC(player, NPCID.Guide);
				SummonNPC(player, NPCID.Merchant);
				SummonNPC(player, NPCID.Nurse);
				SummonNPC(player, NPCID.Demolitionist);
				SummonNPC(player, NPCID.DyeTrader);
				SummonNPC(player, NPCID.Angler);
				SummonNPC(player, NPCID.Painter);
				SummonNPC(player, NPCID.ArmsDealer);
				if (Main.rand.Next(10) == 1)
				{
					SummonNPC(player, NPCID.PartyGirl);
				}
				if (Main.rand.Next(4) == 1)
				{
					SummonNPC(player, NPCID.TravellingMerchant);
				}
				if (Main.rand.Next(4) == 1)
				{
					SummonNPC(player, NPCID.SkeletonMerchant);
				}
				SummonNPC(player, NPCID.Stylist);
                if (NPC.downedBoss1)
                {
					SummonNPC(player, NPCID.Dryad);
                }
                if (NPC.downedBoss2)
                {
					SummonNPC(player, NPCID.DD2Bartender);
                }
                if (NPC.downedGoblins)
                {
					SummonNPC(player, NPCID.GoblinTinkerer);
                }
                if (NPC.downedQueenBee)
                {
					SummonNPC(player, NPCID.WitchDoctor);
                }
                if (NPC.downedBoss3)
                {
					SummonNPC(player, NPCID.Clothier);
					SummonNPC(player, NPCID.Mechanic);
                }

                if (Main.hardMode)
                {
					SummonNPC(player, NPCID.Wizard);
					SummonNPC(player, NPCID.TaxCollector);
					SummonNPC(player, NPCID.Truffle);

                    if (NPC.downedPirates)
                    {
						SummonNPC(player, NPCID.Pirate);
                    }
                    if (NPC.downedMechBossAny)
                    {
						SummonNPC(player, NPCID.Steampunker);
                    }
                    if (NPC.downedPlantBoss)
                    {
						SummonNPC(player, NPCID.Cyborg);
                    }
                    if (Main.xMas && NPC.downedFrost)
                    {
						SummonNPC(player, NPCID.SantaClaus);
                    }
                }
			}
		}

		private void SummonNPC(Player player, int type)
        {
            if (NPC.AnyNPCs(type))
            {
				Main.npc[NPC.FindFirstNPC(type)].Center = player.Center;
            }
            else
            {
				NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, type);
            }
        }
    }

}