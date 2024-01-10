using NPCAttacker.Systems;
using Terraria;
using Terraria.ID;

namespace NPCAttacker
{
    public static class ChannelHelper
    {
        public static bool CheckAndContinueChannelProj(NPC npc)
        {
            if (!npc.IsTownNPC()) return false;
            if (ArmedGNPC.GetWeapon(npc).IsAir || !ArmedGNPC.GetWeapon(npc).channel) return false;
            //if (npc.GetGlobalNPC<ArmedGNPC>().actMode != ArmedGNPC.ActMode.Attack && !npc.GetGlobalNPC<ArmedGNPC>().AlertMode) return true; //保护机制，防止快速使用蓄力弹幕卡死
            bool result = false;
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active)
                {
                    if (proj.GetGlobalProjectile<SpecialUseProj>().NPCProjOwner == npc.whoAmI && proj.GetGlobalProjectile<SpecialUseProj>().ChannelTimer >= 0)
                    {
                        result = true;
                        if (proj.GetGlobalProjectile<SpecialUseProj>().ChannelTimer > 0)
                        {
                            proj.GetGlobalProjectile<SpecialUseProj>().ChannelTimer = (int)(NPCStats.GetModifiedAttackTime(npc) * (NeedBreakChannel(ArmedGNPC.GetWeapon(npc)) ? 0.75f : 1.5f));
                        }
                    }
                }
            }
            return result;
        }

        public static bool NeedBreakChannel(Item item)
        {
            if (WeaponClassify.UseFlailWeapon(item) || item.type == ItemID.Flamelash || item.type == ItemID.RainbowRod)
            {
                return true;
            }
            return false;

        }
    }
}
