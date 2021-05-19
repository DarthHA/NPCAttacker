using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace NPCAttacker.Buffs
{
    public class AssembleBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Assemble!");
            DisplayName.AddTranslation(GameCulture.Chinese, "集结！");
            Description.SetDefault("Your NPCs are assembling.");
            Description.AddTranslation(GameCulture.Chinese, "你的NPC正在集结");
            Main.debuff[Type] = false;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            canBeCleared = false;
        }
    }
}