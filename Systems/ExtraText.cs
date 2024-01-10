using NPCAttacker.UI;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace NPCAttacker.Systems
{
    public class ExtraText : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ArmAltUI.Visible)
            {
                string description = TranslationToPotion.GetLocalizedExtraText(item);
                tooltips.Add(new TooltipLine(Mod, "tooltip", description));
            }
        }
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.buffType > 0;
        }
    }
}
