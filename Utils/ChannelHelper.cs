using NPCAttacker.Systems;
using Terraria;

namespace NPCAttacker
{
    public static class ChannelHelper
    {
        public static bool CheckAndContinueChannelProj(NPC npc)
        {
            if (!npc.IsTownNPC()) return false;
            if (ArmedGNPC.GetWeapon(npc).IsAir || !ArmedGNPC.GetWeapon(npc).channel) return false;
            bool result = false;
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active)
                {
                    if (proj.GetGlobalProjectile<SpecialUseProj>().NPCProjOwner == npc.whoAmI && proj.GetGlobalProjectile<SpecialUseProj>().ChannelTimer >= 0)
                    {
                        proj.GetGlobalProjectile<SpecialUseProj>().ChannelTimer = (int)(NPCStats.GetModifiedAttackTime(npc) * 1.5f);
                        result = true;
                    }
                }
            }
            return result;
        }


    }
}
