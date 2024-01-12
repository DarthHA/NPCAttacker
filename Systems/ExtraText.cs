using Microsoft.Xna.Framework;
using NPCAttacker.UI;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace NPCAttacker.Systems
{
    public class ExtraText : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ArmUI.Visible)
            {
                if (Main.LocalPlayer.talkNPC != -1)
                {
                    bool Valid = false;
                    NPC npc = Main.npc[Main.LocalPlayer.talkNPC];
                    if (ArmUI.ValidWeapon(item)) Valid = true;
                    if (Valid)
                    {
                        string description = string.Format(Language.GetTextValue("Mods.NPCAttacker.UI.CanUseNormal"), npc.GivenOrTypeName);
                        if (npc.type == NPCID.Dryad || npc.type == NPCID.Nurse)
                        {
                            description = string.Format(Language.GetTextValue("Mods.NPCAttacker.UI.CanUsePotion"), npc.GivenOrTypeName);
                        }
                        if (item.type == ItemID.RottenEgg || item.type == 5129)
                        {
                            description = string.Format(Language.GetTextValue("Mods.NPCAttacker.UI.CanUseKill"), npc.GivenOrTypeName);
                        }
                        TooltipLine tooltip = new (Mod, "TooltipCanUse", description);
                        tooltip.OverrideColor = Color.Cyan;
                        tooltips.Add(tooltip);
                    }
                }
            }
            if (ArmUI.Visible)
            {
                string description = TranslationToPotion.GetLocalizedExtraText(item);
                tooltips.Add(new TooltipLine(Mod, "TooltipPotionEffect", description));
            }
        }
        
    }
}
