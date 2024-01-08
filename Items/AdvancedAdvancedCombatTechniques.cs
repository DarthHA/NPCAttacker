using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NPCAttacker.Items
{
    public class AdvancedAdvancedCombatTechniques : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.Deprecated[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.rare = ItemRarityID.Green;
        }

        public override bool CanRightClick()
        {
            if (NPC.downedMoonlord)
            {
                return true;
            }
            return false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {

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
                if (Main.rand.NextBool(10))
                {
                    SummonNPC(player, NPCID.PartyGirl);
                }
                if (Main.rand.NextBool(4))
                {
                    SummonNPC(player, NPCID.TravellingMerchant);
                }
                if (Main.rand.NextBool(4))
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
                NPC.NewNPC(null, (int)player.Center.X, (int)player.Center.Y, type);
            }
        }
    }

}