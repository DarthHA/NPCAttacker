using NPCAttacker.Projectiles.Magic;
using NPCAttacker.Projectiles.Melee;
using NPCAttacker.Projectiles.Ranged;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NPCAttacker.Projectiles
{
    public static class VanillaItemProjFix
    {
        public static Dictionary<int, int> ItemToProj = new();

        public static int AmmoType = 0;
        public static void Load()
        {

            ItemToProj.Add(ItemID.Zenith, ModContent.ProjectileType<ZenithProj>());
            ItemToProj.Add(ItemID.TerraBlade, ModContent.ProjectileType<TerraBladeProj>());
            ItemToProj.Add(ItemID.TrueExcalibur, ModContent.ProjectileType<TrueEXSwordProj>());
            ItemToProj.Add(ItemID.TrueNightsEdge, ModContent.ProjectileType<TrueNightEdgeProj>());
            ItemToProj.Add(ItemID.Excalibur, ModContent.ProjectileType<EXSwordProj>());
            ItemToProj.Add(ItemID.NightsEdge, ModContent.ProjectileType<NightEdgeProj>());
            ItemToProj.Add(ItemID.LightsBane, ModContent.ProjectileType<LightBaneProj>());
            ItemToProj.Add(190, ModContent.ProjectileType<GrassSwordProj>());
            ItemToProj.Add(1826, ModContent.ProjectileType<PumpkinSwordProj>());
            ItemToProj.Add(ItemID.Starfury, ModContent.ProjectileType<StarfuryProj>());
            ItemToProj.Add(ItemID.StarWrath, ModContent.ProjectileType<StarWarthProj>());
            ItemToProj.Add(ItemID.VampireKnives, ModContent.ProjectileType<VampireKnifeProj>());

            ItemToProj.Add(ItemID.DD2BetsyBow, ModContent.ProjectileType<BetsyBowProj>());
            ItemToProj.Add(ItemID.BloodRainBow, ModContent.ProjectileType<BloodRainProj>());
            ItemToProj.Add(ItemID.Boomstick, ModContent.ProjectileType<BoomstickProj>());
            ItemToProj.Add(ItemID.ChlorophyteShotbow, ModContent.ProjectileType<ChlorophyteShotbowProj>());
            ItemToProj.Add(ItemID.ClockworkAssaultRifle, ModContent.ProjectileType<ClockworkAssaultRifleProj>());
            ItemToProj.Add(ItemID.DaedalusStormbow, ModContent.ProjectileType<DaedalusStormbowProj>());
            ItemToProj.Add(ItemID.HellwingBow, ModContent.ProjectileType<HellwingProj>());
            ItemToProj.Add(ItemID.OnyxBlaster, ModContent.ProjectileType<OnyxBlasterProj>());
            ItemToProj.Add(ItemID.PewMaticHorn, ModContent.ProjectileType<PewMaticHornProj>());
            ItemToProj.Add(ItemID.QuadBarrelShotgun, ModContent.ProjectileType<QuadBarrelShotgunProj>());
            ItemToProj.Add(ItemID.Shotgun, ModContent.ProjectileType<ShotgunProj>());
            ItemToProj.Add(ItemID.Tsunami, ModContent.ProjectileType<TsunamiProj>());
            ItemToProj.Add(ItemID.Xenopopper, ModContent.ProjectileType<XenopopperProj>());
            ItemToProj.Add(ItemID.TacticalShotgun, ModContent.ProjectileType<TacticalShotgunProj>());
            ItemToProj.Add(ItemID.Toxikarp, ProjectileID.ToxicBubble);
            ItemToProj.Add(ItemID.Harpoon, ProjectileID.Harpoon);
            ItemToProj.Add(1910, ModContent.ProjectileType<BlueFireProj>());  //精灵熔炉
            ItemToProj.Add(ItemID.PiranhaGun, ModContent.ProjectileType<PiranhaGunProj>());
            ItemToProj.Add(4953, ModContent.ProjectileType<TwilightBowProj>());        //日暮

            ItemToProj.Add(ItemID.ApprenticeStaffT3, ModContent.ProjectileType<ApprenticeStaffT3Proj>());
            ItemToProj.Add(ItemID.BlizzardStaff, ModContent.ProjectileType<BlizzardStaffProj>());
            ItemToProj.Add(ItemID.BubbleGun, ModContent.ProjectileType<BubbleGunProj>());
            ItemToProj.Add(ItemID.ClingerStaff, ModContent.ProjectileType<ClingerStaffProj>());
            ItemToProj.Add(ItemID.CrimsonRod, ModContent.ProjectileType<CrimsonRodProj>());
            ItemToProj.Add(ItemID.MagnetSphere, ModContent.ProjectileType<MagnetSphereProj>());
            ItemToProj.Add(ItemID.MeteorStaff, ModContent.ProjectileType<MeteorStaffProj>());
            ItemToProj.Add(ItemID.NimbusRod, ModContent.ProjectileType<NimbusRodProj>());
            ItemToProj.Add(ItemID.PoisonStaff, ModContent.ProjectileType<PoisonStaffProj>());
            ItemToProj.Add(ItemID.FairyQueenMagicItem, ModContent.ProjectileType<QueenLightProj>());
            ItemToProj.Add(ItemID.PrincessWeapon, ModContent.ProjectileType<PrincessWeaponProj>());
            ItemToProj.Add(ItemID.VenomStaff, ModContent.ProjectileType<VenomStaffProj>());
            ItemToProj.Add(ItemID.Razorpine, ModContent.ProjectileType<RazorpineProj>());
            ItemToProj.Add(ItemID.SharpTears, ModContent.ProjectileType<SharpTearProj>());
            ItemToProj.Add(ItemID.SkyFracture, ModContent.ProjectileType<SkyFractureProj>());
            ItemToProj.Add(ItemID.SparkleGuitar, ModContent.ProjectileType<SparkleGuitarProj>());
            ItemToProj.Add(ItemID.SpiritFlame, ModContent.ProjectileType<SpiritFlameProj>());
            ItemToProj.Add(ItemID.LunarFlareBook, ModContent.ProjectileType<LunarFlareBookProj>());

            /*
            ItemToProj = new();
            foreach (Type type in AssemblyManager.GetLoadableTypes(NPCAttacker.Instance.Code))
            {
                if (type.IsSubclassOf(typeof(BaseAtkProj)) && !type.IsAbstract && type != typeof(BaseAtkProj))
                {
                    BaseAtkProj instance = (BaseAtkProj)FormatterServices.GetUninitializedObject(type);
                    if (instance.ItemType != 0)
                    {
                        ItemToProj.Add(instance.ItemType, instance.Type);
                    }
                }
            }
            */

        }

        public static void UnLoad()
        {
            ItemToProj.Clear();
        }

        public static int TransFormProj(NPC npc, int item)
        {
            if (ItemToProj.ContainsKey(item))
            {
                AmmoType = AmmoFix.FindAmmo(npc).shoot;
                return ItemToProj[item];
            }
            return -1;
        }

        public static void TransFormProjRocket(int WeaponType, int AmmoType, ref int shoot)
        {
            switch (WeaponType)
            {
                case ItemID.RocketLauncher:
                    switch (AmmoType)
                    {
                        case ItemID.RocketI:
                        case ItemID.RocketII:
                        case ItemID.RocketIII:
                        case ItemID.RocketIV:
                            shoot = ProjectileID.RocketI + (AmmoType - ItemID.RocketI) * 3;
                            break;
                        case ItemID.ClusterRocketI:
                            shoot = ProjectileID.ClusterRocketI;
                            break;
                        case ItemID.ClusterRocketII:
                            shoot = ProjectileID.ClusterRocketII;
                            break;
                        case ItemID.WetRocket:
                            shoot = ProjectileID.WetRocket;
                            break;
                        case ItemID.LavaRocket:
                            shoot = ProjectileID.LavaRocket;
                            break;
                        case ItemID.HoneyRocket:
                            shoot = ProjectileID.HoneyRocket;
                            break;
                        case ItemID.DryRocket:
                            shoot = ProjectileID.DryRocket;
                            break;
                        case ItemID.MiniNukeI:
                            shoot = ProjectileID.MiniNukeRocketI;
                            break;
                        case ItemID.MiniNukeII:
                            shoot = ProjectileID.MiniNukeRocketII;
                            break;
                    }
                    break;
                case ItemID.ProximityMineLauncher:
                    switch (AmmoType)
                    {
                        case ItemID.RocketI:
                        case ItemID.RocketII:
                        case ItemID.RocketIII:
                        case ItemID.RocketIV:
                            shoot = ProjectileID.ProximityMineI + (AmmoType - ItemID.RocketI) * 3;
                            break;
                        case ItemID.ClusterRocketI:
                            shoot = ProjectileID.ClusterMineI;
                            break;
                        case ItemID.ClusterRocketII:
                            shoot = ProjectileID.ClusterMineII;
                            break;
                        case ItemID.WetRocket:
                            shoot = ProjectileID.WetMine;
                            break;
                        case ItemID.LavaRocket:
                            shoot = ProjectileID.LavaMine;
                            break;
                        case ItemID.HoneyRocket:
                            shoot = ProjectileID.HoneyMine;
                            break;
                        case ItemID.DryRocket:
                            shoot = ProjectileID.DryMine;
                            break;
                        case ItemID.MiniNukeI:
                            shoot = ProjectileID.MiniNukeMineI;
                            break;
                        case ItemID.MiniNukeII:
                            shoot = ProjectileID.MiniNukeMineII;
                            break;
                    }
                    break;
                case ItemID.GrenadeLauncher:
                    switch (AmmoType)
                    {
                        case ItemID.RocketI:
                        case ItemID.RocketII:
                        case ItemID.RocketIII:
                        case ItemID.RocketIV:
                            shoot = ProjectileID.GrenadeI + (AmmoType - ItemID.RocketI) * 3;
                            break;
                        case ItemID.ClusterRocketI:
                            shoot = ProjectileID.ClusterGrenadeI;
                            break;
                        case ItemID.ClusterRocketII:
                            shoot = ProjectileID.ClusterGrenadeII;
                            break;
                        case ItemID.WetRocket:
                            shoot = ProjectileID.WetGrenade;
                            break;
                        case ItemID.LavaRocket:
                            shoot = ProjectileID.LavaGrenade;
                            break;
                        case ItemID.HoneyRocket:
                            shoot = ProjectileID.HoneyGrenade;
                            break;
                        case ItemID.DryRocket:
                            shoot = ProjectileID.DryGrenade;
                            break;
                        case ItemID.MiniNukeI:
                            shoot = ProjectileID.MiniNukeGrenadeI;
                            break;
                        case ItemID.MiniNukeII:
                            shoot = ProjectileID.MiniNukeGrenadeII;
                            break;
                    }
                    break;
                case ItemID.SnowmanCannon:
                    switch (AmmoType)
                    {
                        case ItemID.RocketI:
                        case ItemID.RocketII:
                        case ItemID.RocketIII:
                        case ItemID.RocketIV:
                            shoot = ProjectileID.RocketSnowmanI + (AmmoType - ItemID.RocketI);
                            break;
                        case ItemID.ClusterRocketI:
                            shoot = ProjectileID.ClusterSnowmanRocketI;
                            break;
                        case ItemID.ClusterRocketII:
                            shoot = ProjectileID.ClusterSnowmanRocketII;
                            break;
                        case ItemID.WetRocket:
                            shoot = ProjectileID.WetSnowmanRocket;
                            break;
                        case ItemID.LavaRocket:
                            shoot = ProjectileID.LavaSnowmanRocket;
                            break;
                        case ItemID.HoneyRocket:
                            shoot = ProjectileID.HoneySnowmanRocket;
                            break;
                        case ItemID.DryRocket:
                            shoot = ProjectileID.DrySnowmanRocket;
                            break;
                        case ItemID.MiniNukeI:
                            shoot = ProjectileID.MiniNukeSnowmanRocketI;
                            break;
                        case ItemID.MiniNukeII:
                            shoot = ProjectileID.MiniNukeSnowmanRocketII;
                            break;
                    }
                    break;
                case ItemID.ElectrosphereLauncher:
                    shoot = ModContent.ProjectileType<ElectrosphereProj>();
                    break;
                case 3546:      //喜庆弹射器
                    shoot = ModContent.ProjectileType<CelebrateProj>();
                    break;
            }
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