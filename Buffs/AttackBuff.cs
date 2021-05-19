using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace NPCAttacker.Buffs
{
    public class AttackBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Attack!");
            DisplayName.AddTranslation(GameCulture.Chinese, "进攻！");
            Description.SetDefault("Your NPCs is attacking.");
            Description.AddTranslation(GameCulture.Chinese, "你的NPC正在进攻。");
            Main.debuff[Type] = false;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            canBeCleared = false;
        }
    }
}