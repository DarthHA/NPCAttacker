using Terraria;
using Terraria.ModLoader;

namespace NPCAttacker.Buffs
{
    public class TPCDBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            Terraria.ID.BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

    }
}