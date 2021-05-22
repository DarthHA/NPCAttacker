using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

namespace NPCAttacker.Projectiles
{
    public static class VanillaItemProjFix
    {
        public static Dictionary<int, int> ItemToProj = new Dictionary<int, int>();
        public static void Load()
        {
            ItemToProj.Add(ItemID.Starfury, ModContent.ProjectileType<StarfuryProj>());
            ItemToProj.Add(ItemID.StarWrath, ModContent.ProjectileType<StarWarthProj>());

            ItemToProj.Add(ItemID.Boomstick, ModContent.ProjectileType<BoomstickProj>());
            ItemToProj.Add(ItemID.ChlorophyteShotbow, ModContent.ProjectileType<ChlorophyteShotbowProj>());
            ItemToProj.Add(ItemID.ClockworkAssaultRifle, ModContent.ProjectileType<ClockworkAssaultRifleProj>());
            ItemToProj.Add(ItemID.DaedalusStormbow, ModContent.ProjectileType<DaedalusStormbowProj>());
            ItemToProj.Add(ItemID.HellwingBow, ModContent.ProjectileType<HellwingProj>());
            ItemToProj.Add(ItemID.OnyxBlaster, ModContent.ProjectileType<OnyxBlasterProj>());
            ItemToProj.Add(ItemID.Shotgun, ModContent.ProjectileType<ShotgunProj>());
            ItemToProj.Add(ItemID.Tsunami, ModContent.ProjectileType<TsunamiProj>());
            ItemToProj.Add(ItemID.Xenopopper, ModContent.ProjectileType<XenopopperProj>());
            ItemToProj.Add(ItemID.TacticalShotgun, ModContent.ProjectileType<TacticalShotgunProj>());

            ItemToProj.Add(ItemID.ApprenticeStaffT3, ModContent.ProjectileType<ApprenticeStaffT3Proj>());
            ItemToProj.Add(ItemID.BlizzardStaff, ModContent.ProjectileType<BlizzardStaffProj>());
            ItemToProj.Add(ItemID.BubbleGun, ModContent.ProjectileType<BubbleGunProj>());
            ItemToProj.Add(ItemID.ClingerStaff, ModContent.ProjectileType<ClingerStaffProj>());
            ItemToProj.Add(ItemID.CrimsonRod, ModContent.ProjectileType<CrimsonRodProj>());
            ItemToProj.Add(ItemID.MagnetSphere, ModContent.ProjectileType<MagnetSphereProj>());
            ItemToProj.Add(ItemID.MeteorStaff, ModContent.ProjectileType<MeteorStaffProj>());
            ItemToProj.Add(ItemID.NimbusRod, ModContent.ProjectileType<NimbusRodProj>());
            ItemToProj.Add(ItemID.PoisonStaff, ModContent.ProjectileType<PoisonStaffProj>());
            ItemToProj.Add(ItemID.VenomStaff, ModContent.ProjectileType<VenomStaffProj>());
            ItemToProj.Add(ItemID.Razorpine, ModContent.ProjectileType<RazorpineProj>());
            ItemToProj.Add(ItemID.SkyFracture, ModContent.ProjectileType<SkyFractureProj>());
            ItemToProj.Add(ItemID.SpiritFlame, ModContent.ProjectileType<SpiritFlameProj>());
            ItemToProj.Add(ItemID.LunarFlareBook, ModContent.ProjectileType<LunarFlareBookProj>());
        }

        public static void UnLoad()
        {
            ItemToProj.Clear();
        }

        public static int TransFormProj(int item)
        {
            if (ItemToProj.ContainsKey(item))
            {
                return ItemToProj[item];
            }
            return -1;
        }

        public static int[] IsUseSpecialProj = ItemID.Sets.Factory.CreateIntSet(0, new int[]
        {
            ItemID.BeesKnees,
            ProjectileID.BeeArrow,

            ItemID.HellwingBow,
            ProjectileID.Hellwing,

            ItemID.MoltenFury,
            ProjectileID.HellfireArrow,

            ItemID.Marrow,
            ProjectileID.BoneArrow,

            ItemID.IceBow,
            ProjectileID.FrostburnArrow,

            ItemID.PulseBow,
            ProjectileID.PulseBolt,

            ItemID.DD2BetsyBow,
            ProjectileID.DD2BetsyArrow,

            ItemID.Uzi,
            ProjectileID.BulletHighVelocity,

            ItemID.SniperRifle,
            ProjectileID.BulletHighVelocity,

            ItemID.NailGun,
            ProjectileID.NailFriendly

        });
    }
}