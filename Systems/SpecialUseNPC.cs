using Terraria;
using Terraria.ModLoader;

namespace NPCAttacker.Systems
{
    public class SpecialUseNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override bool PreAI(NPC npc)
        {
            if (npc.IsTownNPC())
            {
                PlayerDataSaver.SpawnForNPCIndex = npc.whoAmI;
            }
            return true;
        }

        public override void PostAI(NPC npc)
        {
            PlayerDataSaver.SpawnForNPCIndex = -1;
        }
    }
}
