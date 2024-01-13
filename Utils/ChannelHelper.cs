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
                    if (proj.TryGetGlobalProjectile(out SpecialUseProj modproj))
                    {
                        if (modproj.NPCProjOwner == npc.whoAmI && modproj.ChannelTimer >= 0)
                        {
                            result = true;
                            if (modproj.ChannelTimer > 0)
                            {
                                modproj.ChannelTimer = GetChannelTime(npc);
                            }
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

        public static int GetChannelTime(NPC npc)
        {
            float modifier = 1;
            switch (npc.GetGlobalNPC<ArmedGNPC>().ChannelUseType)
            {
                case 0:
                    modifier = 1.5f;
                    break;
                case 1:
                    modifier = 0.75f;
                    break;
                case 2:
                    modifier = 0.1f;
                    break;
            }
            int result = (int)(NPCStats.GetModifiedAttackTime(npc) * modifier);
            if (result < 2) result = 2;
            return result;
        }
    }
}
