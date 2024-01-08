using Terraria;
using Terraria.ModLoader;

namespace NPCAttacker.Buffs
{
    public class CommandBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override bool RightClick(int buffIndex)
        {
            return false;
        }

    }
}