

using NPCAttacker.Items;
using NPCAttacker.UI;
using Terraria;
using Terraria.ModLoader;

namespace NPCAttacker
{
    public class CommanderPlayer : ModPlayer
	{
		public override void UpdateAutopause()
		{
			UpdateUI();
		}
		public override void PostUpdate()
		{
			UpdateUI();
		}

		public void UpdateUI()
        {
			if (player.talkNPC != -1)
			{
				if (!ArmUI.Visible)
				{
					if (NPC.downedMoonlord && Main.LocalPlayer.HeldItem.type == ModContent.ItemType<AttackerStick>())
					{
						UINPCExtraButton.Visible = true;
					}
				}
				else
				{
					UINPCExtraButton.Visible = false;
				}
				if (!Main.playerInventory)
				{
					ArmUI.CloseUI();
				}
			}
			else
			{
				ArmUI.CloseUI();
				UINPCExtraButton.Visible = false;
			}
		}

		public override void CatchFish(Item fishingRod, Item bait, int power, int liquidType, int poolSize, int worldLayer, int questFish, ref int caughtType, ref bool junk)
		{
			if (junk)
			{
				return;
			}
			if (Main.bloodMoon && liquidType == 0 && worldLayer == 1 && power > 30)
			{
				if (!player.HasItem(ModContent.ItemType<AdvancedAdvancedCombatTechniques>()) && !player.HasItem(ModContent.ItemType<AttackerStick>()))
				{
					if (Main.rand.Next(20) == 1)
					{
						caughtType = ModContent.ItemType<AdvancedAdvancedCombatTechniques>();
					}
				}
			}
		}
	}
}