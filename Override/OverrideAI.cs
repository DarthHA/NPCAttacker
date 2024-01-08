using Microsoft.Xna.Framework;
using NPCAttacker.Projectiles;
using System;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;

namespace NPCAttacker.Override
{
    public static class OverrideAI
    {
        public const float Default = 0f;
        public const float Walking = 1f;
        public const float Stop = 2f;
        public const float Talking1 = 3f;
        public const float Talking2 = 4f;
        public const float Sit = 5f;
        public const float TalkingPartyToPlayer = 6f;
        public const float TalkingToPlayer = 7f;
        public const float WalkingForBattle = 8f;            //不确定
        public const float Interacting = 9f;
        public const float ThrowerAtk = 10f;
        public const float PriateSpecial = 11f;
        public const float RangerAtk = 12f;
        public const float NurseHeal = 13f;
        public const float MagicAtk = 14f;
        public const float MeleeAtk = 15f;
        public const float Talking3 = 16f;
        public const float Talking4 = 17f;
        public const float TalkingBartenderToPlayer = 18f;
        /*
        ai[0] 主行为
        0 闲置 和玩家谈话
        1 行走
        2 减速闲置
        3 发起谈话
        4 应答谈话
        5 坐下
        6 派对女孩以及派对期间其他NPC与玩家互动
        7 与玩家互动
        8 发现敌人临战
        9 又是减速闲置
        10 投掷攻击
        11 与海盗相关
        12 远程攻击
        13 护士治疗其他NPC
        14 魔法攻击
        15 近战攻击
        16 发起谈话2
        17 应答谈话2
        18 酒保与玩家互动
        25 微光相关
        ai[1] 主行为计时器
        */

        public static void AI_007_TownEntities(NPC Npc)
        {
            NPC.ShimmeredTownNPCs[Npc.type] = Npc.IsShimmerVariant;
            if (Npc.type == NPCID.TaxCollector && Npc.GivenName == "Andrew")
            {
                Npc.defDefense = 200;
            }

            int SitDownChance = 300;
            if (Npc.type == NPCID.TownDog || Npc.type == NPCID.TownBunny || NPCID.Sets.IsTownSlime[Npc.type])
            {
                SitDownChance = 0;
            }

            #region 该回家了
            bool RestingTime = Main.raining;
            if (!Main.dayTime)
            {
                RestingTime = true;
            }
            if (Main.eclipse)
            {
                RestingTime = true;
            }
            if (Main.slimeRain)
            {
                RestingTime = true;
            }
            #endregion

            if (Npc.GetGlobalNPC<ArmedGNPC>().actMode != ArmedGNPC.ActMode.Default || Npc.GetGlobalNPC<ArmedGNPC>().Selected || Npc.GetGlobalNPC<ArmedGNPC>().Team != 0 || Npc.GetGlobalNPC<ArmedGNPC>().AlertMode)
            {
                RestingTime = false;        //不会在临战时跑掉
            }

            #region 树妖强化
            float damageMult = 1f;
            if (Main.masterMode)
            {
                Npc.defense = Npc.dryadWard ? (Npc.defDefense + 14) : Npc.defDefense;
            }
            else if (Main.expertMode)
            {
                Npc.defense = Npc.dryadWard ? (Npc.defDefense + 10) : Npc.defDefense;
            }
            else
            {
                Npc.defense = Npc.dryadWard ? (Npc.defDefense + 6) : Npc.defDefense;
            }
            #endregion

            #region 攻防强化
            if (Npc.isLikeATownNPC)
            {
                if (NPC.combatBookWasUsed)
                {
                    damageMult += 0.2f;
                    Npc.defense += 6;
                }
                if (NPC.combatBookVolumeTwoWasUsed)
                {
                    damageMult += 0.2f;
                    Npc.defense += 6;
                }

                if (ArmedGNPC.GetWeapon(Npc).IsAir)      //无武器时应用时期伤害加成
                {
                    if (NPC.downedBoss1)
                    {
                        damageMult += 0.1f;
                    }
                    if (NPC.downedBoss2)
                    {
                        damageMult += 0.1f;
                    }
                    if (NPC.downedBoss3)
                    {
                        damageMult += 0.1f;
                    }
                    if (NPC.downedQueenBee)
                    {
                        damageMult += 0.1f;
                    }
                    if (Main.hardMode)
                    {
                        damageMult += 0.4f;
                    }
                    if (NPC.downedQueenSlime)
                    {
                        damageMult += 0.15f;
                    }
                    if (NPC.downedMechBoss1)
                    {
                        damageMult += 0.15f;
                    }
                    if (NPC.downedMechBoss2)
                    {
                        damageMult += 0.15f;
                    }
                    if (NPC.downedMechBoss3)
                    {
                        damageMult += 0.15f;
                    }
                    if (NPC.downedPlantBoss)
                    {
                        damageMult += 0.15f;
                    }
                    if (NPC.downedEmpressOfLight)
                    {
                        damageMult += 0.15f;
                    }
                    if (NPC.downedGolemBoss)
                    {
                        damageMult += 0.15f;
                    }
                    if (NPC.downedAncientCultist)
                    {
                        damageMult += 0.15f;
                    }
                }

                if (NPC.downedBoss1)
                {
                    Npc.defense += 3;
                }
                if (NPC.downedBoss2)
                {
                    Npc.defense += 3;
                }
                if (NPC.downedBoss3)
                {
                    Npc.defense += 3;
                }
                if (NPC.downedQueenBee)
                {
                    Npc.defense += 3;
                }
                if (Main.hardMode)
                {
                    Npc.defense += 12;
                }
                if (NPC.downedQueenSlime)
                {
                    Npc.defense += 6;
                }
                if (NPC.downedMechBoss1)
                {
                    Npc.defense += 6;
                }
                if (NPC.downedMechBoss2)
                {
                    Npc.defense += 6;
                }
                if (NPC.downedMechBoss3)
                {
                    Npc.defense += 6;
                }
                if (NPC.downedPlantBoss)
                {
                    Npc.defense += 8;
                }
                if (NPC.downedEmpressOfLight)
                {
                    Npc.defense += 8;
                }
                if (NPC.downedGolemBoss)
                {
                    Npc.defense += 8;
                }
                if (NPC.downedAncientCultist)
                {
                    Npc.defense += 8;
                }

                NPCLoader.BuffTownNPC(ref damageMult, ref Npc.defense);
            }
            #endregion

            #region 自杀的圣诞老人
            if (Npc.type == NPCID.SantaClaus && Main.netMode != NetmodeID.MultiplayerClient && !Main.xMas)
            {
                Npc.SimpleStrikeNPC(9999, 0, false, 0, null, false, 0, true);
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, Npc.whoAmI, 9999f, 0f, 0f, 0, 0, 0);
                }
            }
            #endregion

            #region 企鹅
            if ((Npc.type == NPCID.Penguin || Npc.type == NPCID.PenguinBlack) && Npc.localAI[0] == 0f)
            {
                Npc.localAI[0] = Main.rand.Next(1, 5);
            }
            #endregion

            #region 电工妹回旋镖检测
            if (Npc.type == NPCID.Mechanic)
            {
                int num3 = NPC.lazyNPCOwnedProjectileSearchArray[Npc.whoAmI];
                bool flag2 = false;
                if (Main.projectile.IndexInRange(num3))
                {
                    Projectile projectile = Main.projectile[num3];
                    if (projectile.active && projectile.type == 582 && projectile.ai[1] == Npc.whoAmI)
                    {
                        flag2 = true;
                    }
                }
                Npc.localAI[0] = flag2.ToInt();
            }
            #endregion

            #region 飞鸭
            if ((Npc.type == NPCID.Duck || Npc.type == NPCID.DuckWhite || Npc.type == NPCID.Seagull || Npc.type == NPCID.Grebe) && Main.netMode != NetmodeID.MultiplayerClient && (Npc.velocity.Y > 4f || Npc.velocity.Y < -4f || Npc.wet))
            {
                int num4 = Npc.direction;
                Npc.Transform(Npc.type + 1);
                Npc.TargetClosest(true);
                Npc.direction = num4;
                Npc.netUpdate = true;
                return;
            }
            #endregion

            #region 拯救
            if (Npc.type <= NPCID.Stylist)
            {
                if (Npc.type <= NPCID.Wizard)
                {
                    if (Npc.type != NPCID.GoblinTinkerer)
                    {
                        if (Npc.type == NPCID.Wizard)
                        {
                            NPC.savedWizard = true;
                        }
                    }
                    else
                    {
                        NPC.savedGoblin = true;
                    }
                }
                else if (Npc.type != NPCID.Mechanic)
                {
                    if (Npc.type == NPCID.Stylist)
                    {
                        NPC.savedStylist = true;
                    }
                }
                else
                {
                    NPC.savedMech = true;
                }
            }
            else if (Npc.type <= NPCID.TaxCollector)
            {
                if (Npc.type != NPCID.Angler)
                {
                    if (Npc.type == NPCID.TaxCollector)
                    {
                        NPC.savedTaxCollector = true;
                    }
                }
                else
                {
                    NPC.savedAngler = true;
                }
            }
            else if (Npc.type != NPCID.DD2Bartender)
            {
                if (Npc.type == NPCID.Golfer)
                {
                    NPC.savedGolfer = true;
                }
            }
            else
            {
                NPC.savedBartender = true;
            }
            #endregion


            #region 微光相关
            Npc.dontTakeDamage = false;
            if (Npc.ai[0] == 25f)
            {
                Npc.dontTakeDamage = true;
                if (Npc.ai[1] == 0f)
                {
                    Npc.velocity.X = 0f;
                }
                Npc.shimmerWet = false;
                Npc.wet = false;
                Npc.lavaWet = false;
                Npc.honeyWet = false;
                if (Npc.ai[1] == 0f && Main.netMode == NetmodeID.MultiplayerClient)
                {
                    return;
                }
                if (Npc.ai[1] == 0f && Npc.ai[2] < 1f)
                {
                    Npc.AI_007_TownEntities_Shimmer_TeleportToLandingSpot();
                }
                if (Npc.ai[2] > 0f)
                {
                    Npc.ai[2] -= 1f;
                    if (Npc.ai[2] <= 0f)
                    {
                        Npc.ai[1] = 1f;
                    }
                    return;
                }
                Npc.ai[1] += 1f;
                if (Npc.ai[1] >= 30f)
                {
                    if (!Collision.WetCollision(Npc.position, Npc.width, Npc.height))
                    {
                        Npc.shimmerTransparency = MathHelper.Clamp(Npc.shimmerTransparency - 0.016666668f, 0f, 1f);
                    }
                    else
                    {
                        Npc.ai[1] = 30f;
                    }
                    Npc.velocity = new Vector2(0f, -4f * Npc.shimmerTransparency);
                }
                Rectangle hitbox = Npc.Hitbox;
                hitbox.Y += 20;
                hitbox.Height -= 20;
                float num5 = Main.rand.NextFloatDirection();
                Lighting.AddLight(Npc.Center, Main.hslToRgb((float)Main.timeForVisualEffects / 360f % 1f, 0.6f, 0.65f, byte.MaxValue).ToVector3() * Utils.Remap(Npc.ai[1], 30f, 90f, 0f, 0.7f, true));
                if (Main.rand.NextFloat() > Utils.Remap(Npc.ai[1], 30f, 60f, 1f, 0.5f, true))
                {
                    Dust.NewDustPerfect(Main.rand.NextVector2FromRectangle(hitbox) + Main.rand.NextVector2Circular(8f, 0f) + new Vector2(0f, 4f), 309, new Vector2?(new Vector2(0f, -2f).RotatedBy((double)(num5 * 6.2831855f * 0.11f), default)), 0, default, 1.7f - Math.Abs(num5) * 1.3f);
                }
                if (Npc.ai[1] > 60f && Main.rand.NextBool(15))
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 vector = Main.rand.NextVector2FromRectangle(Npc.Hitbox);
                        ParticleOrchestrator.RequestParticleSpawn(true, ParticleOrchestraType.ShimmerBlock, new ParticleOrchestraSettings
                        {
                            PositionInWorld = vector,
                            MovementVector = Npc.DirectionTo(vector).RotatedBy((double)(1.4137167f * (Main.rand.Next(2) * 2 - 1)), default) * Main.rand.NextFloat()
                        }, null);
                    }
                }
                Npc.TargetClosest(true);
                NPCAimedTarget targetData = Npc.GetTargetData(true);
                if (Npc.ai[1] >= 75f && Npc.shimmerTransparency <= 0f && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Npc.ai[0] = 0f;
                    Npc.ai[1] = 0f;
                    Npc.ai[2] = 0f;
                    Npc.ai[3] = 0f;
                    Math.Sign(targetData.Center.X - Npc.Center.X);
                    Npc.velocity = new Vector2(0f, -4f);
                    Npc.localAI[0] = 0f;
                    Npc.localAI[1] = 0f;
                    Npc.localAI[2] = 0f;
                    Npc.localAI[3] = 0f;
                    Npc.netUpdate = true;
                    Npc.townNpcVariationIndex = (Npc.townNpcVariationIndex != 1) ? 1 : 0;
                    NetMessage.SendData(MessageID.UniqueTownNPCInfoSyncRequest, -1, -1, null, Npc.whoAmI, 0f, 0f, 0f, 0, 0, 0);
                    Npc.Teleport(Npc.position, 12, 0);
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ShimmerTownNPC, new ParticleOrchestraSettings
                    {
                        PositionInWorld = Npc.Center
                    });
                }
                return;

            }
            #endregion
            else
            {
                #region 找家
                if (Npc.type >= NPCID.None && NPCID.Sets.TownCritter[Npc.type] && Npc.target == 255)
                {
                    Npc.TargetClosest(true);
                    if (Npc.position.X < Main.player[Npc.target].position.X)
                    {
                        Npc.direction = 1;
                        Npc.spriteDirection = Npc.direction;
                    }
                    if (Npc.position.X > Main.player[Npc.target].position.X)
                    {
                        Npc.direction = -1;
                        Npc.spriteDirection = Npc.direction;
                    }
                    if (Npc.homeTileX == -1)
                    {
                        Npc.UpdateHomeTileState(Npc.homeless, (int)((Npc.position.X + Npc.width / 2) / 16f), Npc.homeTileY);
                    }
                }
                else if (Npc.homeTileX == -1 && Npc.homeTileY == -1 && Npc.velocity.Y == 0f && !Npc.shimmering)
                {
                    Npc.UpdateHomeTileState(Npc.homeless, (int)Npc.Center.X / 16, (int)(Npc.position.Y + Npc.height + 4f) / 16);
                }
                #endregion

                bool IsTalking = false;
                int TileCenterX = (int)(Npc.position.X + Npc.width / 2) / 16;
                int TileCenterY = (int)(Npc.position.Y + Npc.height + 1f) / 16;
                Npc.AI_007_FindGoodRestingSpot(TileCenterX, TileCenterY, out int floorX, out int floorY);

                if (Npc.type == NPCID.TaxCollector)
                {
                    NPC.taxCollector = true;
                }
                Npc.directionY = -1;
                if (Npc.direction == 0)
                {
                    Npc.direction = 1;
                }

                #region 调整NPC与玩家对话相关
                if (Npc.ai[0] != 24f)
                {
                    for (int j = 0; j < 255; j++)
                    {
                        if (Main.player[j].active && Main.player[j].talkNPC == Npc.whoAmI)
                        {
                            IsTalking = true;
                            if (Npc.ai[0] != 0f)
                            {
                                Npc.netUpdate = true;
                            }
                            Npc.ai[0] = 0f;
                            Npc.ai[1] = 300f;
                            Npc.localAI[3] = 100f;
                            if (Main.player[j].position.X + Main.player[j].width / 2 < Npc.position.X + Npc.width / 2)
                            {
                                Npc.direction = -1;
                            }
                            else
                            {
                                Npc.direction = 1;
                            }
                        }
                    }
                }
                #endregion

                #region 地牢老人
                if (Npc.ai[3] == 1f)
                {
                    Npc.life = -1;
                    Npc.HitEffect(0, 10.0, null);
                    Npc.active = false;
                    Npc.netUpdate = true;
                    if (Npc.type == NPCID.OldMan)
                    {
                        SoundEngine.PlaySound(SoundID.NPCDeath15, Npc.position);
                    }
                    return;
                }
                if (Npc.type == NPCID.OldMan && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Npc.UpdateHomeTileState(false, Main.dungeonX, Main.dungeonY);
                    if (NPC.downedBoss3)
                    {
                        Npc.ai[3] = 1f;
                        Npc.netUpdate = true;
                    }
                }
                #endregion

                #region 游商
                if (Npc.type == NPCID.TravellingMerchant)
                {
                    Npc.homeless = true;
                    if (!Main.dayTime)
                    {
                        if (!Npc.shimmering)
                        {
                            Npc.UpdateHomeTileState(Npc.homeless, (int)(Npc.Center.X / 16f), (int)(Npc.position.Y + Npc.height + 2f) / 16);
                        }
                        if (!IsTalking && Npc.ai[0] == 0f)
                        {
                            Npc.ai[0] = 1f;
                            Npc.ai[1] = 200f;
                        }
                        RestingTime = false;
                    }
                }
                #endregion

                #region 防止渔夫淹死让它往世界中心跑
                if (Npc.type == NPCID.Angler && Npc.homeless && Npc.wet)
                {
                    if (Npc.Center.X / 16f < 380f || Npc.Center.X / 16f > Main.maxTilesX - 380)
                    {
                        Npc.UpdateHomeTileState(Npc.homeless, Main.spawnTileX, Main.spawnTileY);
                        Npc.ai[0] = 1f;
                        Npc.ai[1] = 200f;
                    }
                    if (Npc.position.X / 16f < 300f)
                    {
                        Npc.direction = 1;
                    }
                    else if (Npc.position.X / 16f > Main.maxTilesX - 300)
                    {
                        Npc.direction = -1;
                    }
                }
                #endregion

                #region NPC位于未加载区块中时不行动
                if (!WorldGen.InWorld(TileCenterX, TileCenterY, 0) || (Main.netMode == NetmodeID.MultiplayerClient && !Main.sectionManager.TileLoaded(TileCenterX, TileCenterY)))
                {
                    return;
                }
                #endregion

                #region NPC瞬移回家
                if (!Npc.homeless && Main.netMode != NetmodeID.MultiplayerClient && Npc.townNPC && (RestingTime || (Npc.type == NPCID.OldMan && Main.tileDungeon[Main.tile[TileCenterX, TileCenterY].TileType])) && !Npc.AI_007_TownEntities_IsInAGoodRestingSpot(TileCenterX, TileCenterY, floorX, floorY))
                {
                    if (Npc.GetGlobalNPC<ArmedGNPC>().actMode == ArmedGNPC.ActMode.Default && !Npc.GetGlobalNPC<ArmedGNPC>().Selected && Npc.GetGlobalNPC<ArmedGNPC>().Team == 0 && !Npc.GetGlobalNPC<ArmedGNPC>().AlertMode)        //不会在临战时跑掉
                    {
                        bool ReturnHome = true;
                        int TryTime = 0;
                        while (TryTime < 2 && ReturnHome)
                        {
                            Rectangle rectangle;
                            rectangle = new((int)(Npc.position.X + Npc.width / 2 - NPC.sWidth / 2 - NPC.safeRangeX), (int)(Npc.position.Y + Npc.height / 2 - NPC.sHeight / 2 - NPC.safeRangeY), NPC.sWidth + NPC.safeRangeX * 2, NPC.sHeight + NPC.safeRangeY * 2);
                            if (TryTime == 1)
                            {
                                rectangle = new(floorX * 16 + 8 - NPC.sWidth / 2 - NPC.safeRangeX, floorY * 16 + 8 - NPC.sHeight / 2 - NPC.safeRangeY, NPC.sWidth + NPC.safeRangeX * 2, NPC.sHeight + NPC.safeRangeY * 2);
                            }
                            for (int l = 0; l < 255; l++)
                            {
                                if (Main.player[l].active && new Rectangle((int)Main.player[l].position.X, (int)Main.player[l].position.Y, Main.player[l].width, Main.player[l].height).Intersects(rectangle))
                                {
                                    ReturnHome = false;
                                    break;
                                }
                            }
                            TryTime++;
                        }
                        if (ReturnHome)
                        {
                            Npc.AI_007_TownEntities_TeleportToHome(floorX, floorY);
                        }
                    }
                }
                #endregion

                bool IsRat = Npc.type == NPCID.Mouse || Npc.type == NPCID.GoldMouse || Npc.type == NPCID.Rat;
                bool IsTurtle = Npc.type == NPCID.Turtle || Npc.type == NPCID.TurtleJungle || Npc.type == NPCID.SeaTurtle;
                bool IsFrog = Npc.type == NPCID.Frog || Npc.type == NPCID.GoldFrog || Npc.type == 687;
                bool IsSlimePet = NPCID.Sets.IsTownSlime[Npc.type];
                bool IsPet = NPCID.Sets.IsTownPet[Npc.type];
                bool IsWaterAnimal = IsTurtle || IsFrog;
                bool IsWaterAnimal2 = IsTurtle || IsFrog;
                float DangerDetectRange = 200f;

                if (NPCID.Sets.DangerDetectRange[Npc.type] != -1)        //猎人药水
                {
                    DangerDetectRange = NPCID.Sets.DangerDetectRange[Npc.type];
                }

                bool HasTarget = false;
                bool HasTarget2 = false;
                float ShootLeft = -1f;
                float ShootRight = -1f;
                int ShootDir = 0;
                int TargetLeft = -1;
                int TargetRight = -1;

                if (!IsTurtle && Main.netMode != NetmodeID.MultiplayerClient && !IsTalking)
                {
                    if (Npc.GetGlobalNPC<ArmedGNPC>().actMode == ArmedGNPC.ActMode.Attack)
                    {
                        NPC target = Main.npc[Npc.GetGlobalNPC<ArmedGNPC>().ActTargetNPC];

                        HasTarget = true;
                        float ShootX = target.Center.X - Npc.Center.X;
                        if (ShootX < 0f && (ShootLeft == -1f || ShootX > ShootLeft))
                        {
                            ShootLeft = ShootX;
                        }
                        if (ShootX > 0f && (ShootRight == -1f || ShootX < ShootRight))
                        {
                            ShootRight = ShootX;
                        }
                        TargetLeft = target.whoAmI;
                        TargetRight = target.whoAmI;
                    }
                    else if (Npc.GetGlobalNPC<ArmedGNPC>().actMode == ArmedGNPC.ActMode.Move)
                    {
                        //一无所获
                        HasTarget = false;
                    }
                    else
                    {
                        for (int m = 0; m < 200; m++)
                        {
                            if (Main.npc[m].active && !Main.npc[m].friendly && Main.npc[m].damage > 0 && Main.npc[m].Distance(Npc.Center) < DangerDetectRange && (Npc.type != NPCID.SkeletonMerchant || !NPCID.Sets.Skeletons[Main.npc[m].type]) && (Main.npc[m].noTileCollide || Collision.CanHit(Npc.Center, 0, 0, Main.npc[m].Center, 0, 0)) && NPCLoader.CanHitNPC(Main.npc[m], Npc))
                            {
                                bool CanBeChasedBy = Main.npc[m].CanBeChasedBy(Npc, false);
                                HasTarget = true;
                                float DistanceX = Main.npc[m].Center.X - Npc.Center.X;
                                if (Npc.type == NPCID.ExplosiveBunny)
                                {
                                    if (DistanceX < 0f && (ShootLeft == -1f || DistanceX > ShootLeft))
                                    {
                                        ShootRight = DistanceX;
                                        TargetRight = m;
                                    }
                                    if (DistanceX > 0f && (ShootRight == -1f || DistanceX < ShootRight))
                                    {
                                        ShootLeft = DistanceX;
                                        TargetLeft = m;
                                    }
                                }
                                else
                                {
                                    if (DistanceX < 0f && (ShootLeft == -1f || DistanceX > ShootLeft))
                                    {
                                        ShootLeft = DistanceX;
                                        if (CanBeChasedBy)
                                        {
                                            TargetLeft = m;
                                        }
                                    }
                                    if (DistanceX > 0f && (ShootRight == -1f || DistanceX < ShootRight))
                                    {
                                        ShootRight = DistanceX;
                                        if (CanBeChasedBy)
                                        {
                                            TargetRight = m;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (HasTarget)        //发现敌人
                    {
                        ShootDir = (ShootLeft == -1f) ? 1 : ((ShootRight != -1f) ? (ShootRight < 0f - ShootLeft).ToDirectionInt() : -1);
                        float NearestDist = 0f;
                        if (ShootLeft != -1f)
                        {
                            NearestDist = 0f - ShootLeft;
                        }
                        if (NearestDist == 0f || (ShootRight < NearestDist && ShootRight > 0f))
                        {
                            NearestDist = ShootRight;
                        }
                        if (Npc.ai[0] == 8f)
                        {
                            if (Npc.direction == -ShootDir)
                            {
                                Npc.ai[0] = 1f;
                                Npc.ai[1] = 300 + Main.rand.Next(300);
                                Npc.ai[2] = 0f;
                                Npc.localAI[3] = 0f;
                                Npc.netUpdate = true;
                            }
                        }
                        else if (Npc.ai[0] != 10f && Npc.ai[0] != 12f && Npc.ai[0] != 13f && Npc.ai[0] != 14f && Npc.ai[0] != 15f)   //不在攻击状态时
                        {
                            if (NPCID.Sets.PrettySafe[Npc.type] != -1 && NPCID.Sets.PrettySafe[Npc.type] < NearestDist)
                            {
                                if (Npc.GetGlobalNPC<ArmedGNPC>().actMode == ArmedGNPC.ActMode.Attack)
                                {
                                    HasTarget = true;
                                    HasTarget2 = true;
                                }
                                else if (Npc.GetGlobalNPC<ArmedGNPC>().actMode == ArmedGNPC.ActMode.Move)
                                {
                                    HasTarget = false;
                                    HasTarget2 = false;
                                }
                                else
                                {
                                    HasTarget = false;
                                    HasTarget2 = NPCID.Sets.AttackType[Npc.type] > -1;
                                }

                            }
                            else if (Npc.ai[0] != 1f)
                            {
                                int tileX = (int)((Npc.position.X + Npc.width / 2 + 15 * Npc.direction) / 16f);
                                int tileY = (int)((Npc.position.Y + Npc.height - 16f) / 16f);
                                bool currentlyDrowning = Npc.wet && !IsWaterAnimal;
                                Npc.AI_007_TownEntities_GetWalkPrediction(TileCenterX, floorX, IsWaterAnimal, currentlyDrowning, tileX, tileY, out bool flag34, out bool avoidFalling);
                                if (!avoidFalling)
                                {
                                    if (Npc.ai[0] == 3f || Npc.ai[0] == 4f || Npc.ai[0] == 16f || Npc.ai[0] == 17f)
                                    {
                                        NPC nPC = Main.npc[(int)Npc.ai[2]];
                                        if (nPC.active)
                                        {
                                            nPC.ai[0] = 1f;
                                            nPC.ai[1] = 120 + Main.rand.Next(120);
                                            nPC.ai[2] = 0f;
                                            nPC.localAI[3] = 0f;
                                            nPC.direction = -ShootDir;
                                            nPC.netUpdate = true;
                                        }
                                    }
                                    Npc.ai[0] = 1f;
                                    Npc.ai[1] = 120 + Main.rand.Next(120);
                                    Npc.ai[2] = 0f;
                                    Npc.localAI[3] = 0f;
                                    Npc.direction = -ShootDir;
                                    Npc.netUpdate = true;
                                }
                            }
                            else if (Npc.ai[0] == 1f)
                            {
                                if (Npc.GetGlobalNPC<ArmedGNPC>().actMode == ArmedGNPC.ActMode.Attack)
                                {
                                    if (Npc.direction != ShootDir)
                                    {
                                        Npc.direction = ShootDir;
                                        Npc.netUpdate = true;
                                    }
                                }
                                else if (Npc.GetGlobalNPC<ArmedGNPC>().actMode == ArmedGNPC.ActMode.Move)
                                {
                                    Npc.netUpdate = true;
                                }
                                else
                                {
                                    if (Npc.direction != -ShootDir)
                                    {
                                        Npc.direction = -ShootDir;
                                        Npc.netUpdate = true;
                                    }
                                }
                            }
                        }
                    }
                }
                if (Npc.ai[0] == 0f)         //游走状态
                {
                    if (Npc.localAI[3] > 0f)
                    {
                        Npc.localAI[3] -= 1f;
                    }
                    int num16 = 120;
                    if (Npc.type == NPCID.TownDog)
                    {
                        num16 = 60;
                    }
                    if ((IsFrog || IsSlimePet) && Npc.wet)             //青蛙相关
                    {
                        Npc.ai[0] = 1f;
                        Npc.ai[1] = 200 + Main.rand.Next(500, 700);
                        Npc.ai[2] = 0f;
                        Npc.localAI[3] = 0f;
                        Npc.netUpdate = true;
                    }
                    else if (RestingTime && !IsTalking && !NPCID.Sets.TownCritter[Npc.type])     //准备回家
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            if (Npc.GetGlobalNPC<ArmedGNPC>().actMode == ArmedGNPC.ActMode.Move)
                            {
                                Npc.ai[0] = 1f;
                                Npc.ai[1] = 120;
                                Npc.ai[2] = 0f;
                            }

                            if (TileCenterX == floorX && TileCenterY == floorY)    //走到家里减速
                            {
                                if (Npc.velocity.X != 0f)
                                {
                                    Npc.netUpdate = true;
                                }
                                if (Npc.velocity.X > 0.1f)
                                {
                                    Npc.velocity.X -= 0.1f;
                                }
                                else if (Npc.velocity.X < -0.1f)
                                {
                                    Npc.velocity.X += 0.1f;
                                }
                                else
                                {
                                    Npc.velocity.X = 0f;
                                    Npc.AI_007_TryForcingSitting(floorX, floorY);
                                }
                                if (NPCID.Sets.IsTownPet[Npc.type])
                                {
                                    Npc.AI_007_AttemptToPlayIdleAnimationsForPets(num16 * 4);
                                }
                            }
                            else     //走回家行动
                            {
                                if (TileCenterX > floorX)
                                {
                                    Npc.direction = -1;
                                }
                                else
                                {
                                    Npc.direction = 1;
                                }
                                Npc.ai[0] = 1f;
                                Npc.ai[1] = 200 + Main.rand.Next(200);
                                Npc.ai[2] = 0f;
                                Npc.localAI[3] = 0f;
                                Npc.netUpdate = true;
                            }
                        }
                    }
                    else
                    {
                        if (IsRat)
                        {
                            Npc.velocity.X *= 0.5f;
                        }
                        if (Npc.velocity.X > 0.1f)
                        {
                            Npc.velocity.X -= 0.1f;
                        }
                        else if (Npc.velocity.X < -0.1f)
                        {
                            Npc.velocity.X += 0.1f;
                        }
                        else
                        {
                            Npc.velocity.X = 0f;
                        }

                        #region 寻路行为
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            if (!IsTalking && NPCID.Sets.IsTownPet[Npc.type] && Npc.ai[1] >= 100f && Npc.ai[1] <= 150f)
                            {
                                Npc.AI_007_AttemptToPlayIdleAnimationsForPets(num16);
                            }


                            if (Npc.GetGlobalNPC<ArmedGNPC>().actMode == ArmedGNPC.ActMode.Move)
                            {
                                Npc.ai[0] = Walking;
                                Npc.ai[1] = 120;
                                Npc.ai[2] = 0f;
                            }

                            if (Npc.ai[1] > 0f)
                            {
                                Npc.ai[1] -= 1f;
                                if (Npc.ai[0] == Talking1 || Npc.ai[0] == Talking2 || Npc.ai[0] == Sit || Npc.ai[0] == Talking3 || Npc.ai[0] == Talking4 || Npc.ai[0] == TalkingToPlayer || Npc.ai[0] == TalkingPartyToPlayer || Npc.ai[0] == TalkingBartenderToPlayer)
                                {
                                    if (Npc.GetGlobalNPC<ArmedGNPC>().actMode != ArmedGNPC.ActMode.Default || Npc.GetGlobalNPC<ArmedGNPC>().AlertMode)
                                    {
                                        if (Npc.ai[1] > 1f)
                                        {
                                            Npc.ai[1] = 1f;
                                        }
                                    }
                                }
                            }

                            bool flag16 = true;
                            int tileX2 = (int)((Npc.position.X + Npc.width / 2 + 15 * Npc.direction) / 16f);
                            int tileY2 = (int)((Npc.position.Y + Npc.height - 16f) / 16f);
                            bool currentlyDrowning2 = Npc.wet && !IsWaterAnimal;
                            Npc.AI_007_TownEntities_GetWalkPrediction(TileCenterX, floorX, IsWaterAnimal, currentlyDrowning2, tileX2, tileY2, out bool flag34, out bool avoidFalling2);
                            if (Npc.wet && !IsWaterAnimal)
                            {
                                bool currentlyDrowning3 = Collision.DrownCollision(Npc.position, Npc.width, Npc.height, 1f, true);
                                if (Npc.AI_007_TownEntities_CheckIfWillDrown(currentlyDrowning3))
                                {
                                    Npc.ai[0] = 1f;
                                    Npc.ai[1] = 200 + Main.rand.Next(300);
                                    Npc.ai[2] = 0f;
                                    if (NPCID.Sets.TownCritter[Npc.type])
                                    {
                                        Npc.ai[1] += Main.rand.Next(200, 400);
                                    }
                                    Npc.localAI[3] = 0f;
                                    Npc.netUpdate = true;
                                }
                            }
                            if (avoidFalling2)
                            {
                                flag16 = false;
                            }
                            if (Npc.ai[1] <= 0f)
                            {
                                if (flag16 && !avoidFalling2)
                                {
                                    Npc.ai[0] = 1f;
                                    Npc.ai[1] = 200 + Main.rand.Next(300);
                                    Npc.ai[2] = 0f;
                                    if (NPCID.Sets.TownCritter[Npc.type])
                                    {
                                        Npc.ai[1] += Main.rand.Next(200, 400);
                                    }
                                    Npc.localAI[3] = 0f;
                                    Npc.netUpdate = true;
                                }
                                else
                                {
                                    Npc.direction *= -1;
                                    Npc.ai[1] = 60 + Main.rand.Next(120);
                                    Npc.netUpdate = true;
                                }
                            }
                        }
                        #endregion
                    }
                    if (Main.netMode != NetmodeID.MultiplayerClient && (!RestingTime || Npc.AI_007_TownEntities_IsInAGoodRestingSpot(TileCenterX, TileCenterY, floorX, floorY)))
                    {
                        if (TileCenterX < floorX - 25 || TileCenterX > floorX + 25)    //游走闲逛
                        {
                            if (Npc.localAI[3] == 0f)
                            {
                                if (TileCenterX < floorX - 50 && Npc.direction == -1)
                                {
                                    Npc.direction = 1;
                                    Npc.netUpdate = true;
                                }
                                else if (TileCenterX > floorX + 50 && Npc.direction == 1)
                                {
                                    Npc.direction = -1;
                                    Npc.netUpdate = true;
                                }
                            }
                        }
                        else if (Main.rand.NextBool(80) && Npc.localAI[3] == 0f)
                        {
                            Npc.localAI[3] = 200f;
                            Npc.direction *= -1;
                            Npc.netUpdate = true;
                        }
                    }
                }
                else if (Npc.ai[0] == 1f)
                {      //到家后
                    if (Main.netMode != NetmodeID.MultiplayerClient && RestingTime && Npc.AI_007_TownEntities_IsInAGoodRestingSpot(TileCenterX, TileCenterY, floorX, floorY) && !NPCID.Sets.TownCritter[Npc.type])
                    {
                        Npc.ai[0] = 0f;
                        Npc.ai[1] = 200 + Main.rand.Next(200);
                        Npc.localAI[3] = 60f;
                        Npc.netUpdate = true;
                    }
                    else
                    {
                        bool IsDrown = !IsWaterAnimal && Collision.DrownCollision(Npc.position, Npc.width, Npc.height, 1f, true);
                        if (!IsDrown)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient && !Npc.homeless && !Main.tileDungeon[Main.tile[TileCenterX, TileCenterY].TileType] && (TileCenterX < floorX - 35 || TileCenterX > floorX + 35))
                            {
                                if (Npc.position.X < floorX * 16 && Npc.direction == -1)             //如果移动方向远离家则更快的结束行走
                                {
                                    Npc.ai[1] -= 5f;
                                }
                                else if (Npc.position.X > floorX * 16 && Npc.direction == 1)
                                {
                                    Npc.ai[1] -= 5f;
                                }
                            }
                            Npc.ai[1] -= 1f;
                        }

                        if (Npc.GetGlobalNPC<ArmedGNPC>().AlertMode)
                        {
                            if (Npc.ai[1] > 0)
                            {
                                Npc.ai[1] = 0;
                            }
                        }

                        if (Npc.ai[1] <= 0f)            //结束行走
                        {
                            if (Npc.GetGlobalNPC<ArmedGNPC>().actMode != ArmedGNPC.ActMode.Move)
                            {
                                Npc.ai[0] = Default;
                                Npc.ai[1] = 300 + Main.rand.Next(300);
                                Npc.ai[2] = 0f;
                                if (NPCID.Sets.TownCritter[Npc.type])  //动物闲置时间更短
                                {
                                    Npc.ai[1] -= Main.rand.Next(100);
                                }
                                else
                                {
                                    Npc.ai[1] += Main.rand.Next(900);//NPC闲置时间更短
                                }
                                Npc.localAI[3] = 60f;
                                Npc.netUpdate = true;
                            }
                            else
                            {
                                Npc.netUpdate = true;
                            }
                        }

                        #region 门相关，不细究了
                        if (Npc.closeDoor && ((Npc.position.X + Npc.width / 2) / 16f > Npc.doorX + 2 || (Npc.position.X + Npc.width / 2) / 16f < Npc.doorX - 2))
                        {
                            Tile tileSafely = Framing.GetTileSafely(Npc.doorX, Npc.doorY);
                            if (TileLoader.CloseDoorID(tileSafely) >= 0)
                            {
                                if (WorldGen.CloseDoor(Npc.doorX, Npc.doorY, false))
                                {
                                    Npc.closeDoor = false;
                                    NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 1, Npc.doorX, Npc.doorY, Npc.direction, 0, 0, 0);
                                }
                                if ((Npc.position.X + Npc.width / 2) / 16f > Npc.doorX + 4 || (Npc.position.X + Npc.width / 2) / 16f < Npc.doorX - 4 || (Npc.position.Y + Npc.height / 2) / 16f > Npc.doorY + 4 || (Npc.position.Y + Npc.height / 2) / 16f < Npc.doorY - 4)
                                {
                                    Npc.closeDoor = false;
                                }
                            }
                            else if (tileSafely.TileType == 389)
                            {
                                if (WorldGen.ShiftTallGate(Npc.doorX, Npc.doorY, true, false))
                                {
                                    Npc.closeDoor = false;
                                    NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 5, Npc.doorX, Npc.doorY, 0f, 0, 0, 0);
                                }
                                if ((Npc.position.X + Npc.width / 2) / 16f > Npc.doorX + 4 || (Npc.position.X + Npc.width / 2) / 16f < Npc.doorX - 4 || (Npc.position.Y + Npc.height / 2) / 16f > Npc.doorY + 4 || (Npc.position.Y + Npc.height / 2) / 16f < Npc.doorY - 4)
                                {
                                    Npc.closeDoor = false;
                                }
                            }
                            else
                            {
                                Npc.closeDoor = false;
                            }
                        }
                        #endregion


                        float Speed = 1f;
                        float Acc = 0.07f;
                        if (Npc.type == NPCID.ExplosiveBunny && HasTarget)
                        {
                            Speed = 1.5f;
                            Acc = 0.1f;
                        }
                        else if (Npc.type == NPCID.Squirrel || Npc.type == NPCID.SquirrelGold || Npc.type == NPCID.SquirrelRed || (Npc.type >= NPCID.GemSquirrelAmethyst && Npc.type <= NPCID.GemSquirrelAmber))
                        {
                            Speed = 1.5f;
                        }
                        else if (IsTurtle)
                        {
                            if (Npc.wet)
                            {
                                Acc = 1f;
                                Speed = 2f;
                            }
                            else
                            {
                                Acc = 0.07f;
                                Speed = 0.5f;
                            }
                        }
                        if (Npc.type == NPCID.SeaTurtle)
                        {
                            if (Npc.wet)
                            {
                                Acc = 1f;
                                Speed = 2.5f;
                            }
                            else
                            {
                                Acc = 0.07f;
                                Speed = 0.2f;
                            }
                        }
                        if (IsRat)
                        {
                            Speed = 2f;
                            Acc = 1f;
                        }
                        if (Npc.friendly && (HasTarget || IsDrown))    //想要逃走时
                        {
                            Speed = 1.5f;
                            float num19 = 1f - Npc.life / (float)Npc.lifeMax;
                            Speed += num19 * 0.9f;
                            Acc = 0.1f;
                        }

                        if (Npc.GetGlobalNPC<ArmedGNPC>().actMode == ArmedGNPC.ActMode.Attack)
                        {
                            if (NPCID.Sets.AttackType[Npc.type] == 3)
                            {
                                float baseAcc = 0;
                                if (NPC.AnyNPCs(NPCID.Dryad) && Npc.HasBuff(BuffID.DryadsWard))
                                {
                                    if (!ArmedGNPC.GetWeapon(Main.npc[NPC.FindFirstNPC(NPCID.Dryad)]).IsAir)
                                    {
                                        baseAcc = ArmedGNPC.GetWeapon(Main.npc[NPC.FindFirstNPC(NPCID.Dryad)]).healMana / 600f;
                                    }
                                }
                                Speed += baseAcc;
                                Acc += baseAcc;
                                if (NPC.downedMoonlord)
                                {
                                    Speed *= 2.5f;
                                    Acc *= 2.5f;
                                }
                                else if (Main.hardMode)
                                {
                                    Speed *= 2;
                                    Acc *= 2;
                                }
                                else
                                {
                                    Speed *= 1.5f;
                                    Acc *= 1.5f;
                                }

                            }
                            else              // if (NPCID.Sets.AttackType[npc.type] != 0 || npc.type == NPCID.Nurse)
                            {
                                Speed /= 100;           //远程集火不走A
                                Acc /= 100;
                            }
                        }
                        else if (Npc.GetGlobalNPC<ArmedGNPC>().actMode == ArmedGNPC.ActMode.Move)
                        {
                            if (NPC.downedMoonlord)
                            {
                                Speed *= 2.5f;
                                Acc *= 2.5f;
                            }
                            else
                            {
                                Acc *= 2;
                                Speed *= 2;
                            }
                            Npc.direction = Math.Sign(Main.npc[Npc.GetGlobalNPC<ArmedGNPC>().ActTargetNPC].Center.X - Npc.Center.X);
                        }

                        NPCStats.ModifySpeed(Npc, ref Speed, ref Acc);


                        if (IsSlimePet && Npc.wet)
                        {
                            Speed = 2f;
                            Acc = 0.2f;
                        }
                        if (IsFrog && Npc.wet)
                        {
                            if (Math.Abs(Npc.velocity.X) < 0.05f && Math.Abs(Npc.velocity.Y) < 0.05f)
                            {
                                Npc.velocity.X += Speed * 10f * Npc.direction;
                            }
                            else
                            {
                                Npc.velocity.X *= 0.9f;
                            }
                        }
                        else if (Npc.velocity.X < 0f - Speed || Npc.velocity.X > Speed)
                        {
                            if (Npc.velocity.Y == 0f)
                            {
                                Npc.velocity *= 0.8f;
                            }
                        }
                        else if (Npc.velocity.X < Speed && Npc.direction == 1)
                        {
                            Npc.velocity.X += Acc;
                            if (Npc.velocity.X > Speed)
                            {
                                Npc.velocity.X = Speed;
                            }
                        }
                        else if (Npc.velocity.X > 0f - Speed && Npc.direction == -1)
                        {
                            Npc.velocity.X -= Acc;
                            if (Npc.velocity.X > Speed)
                            {
                                Npc.velocity.X = Speed;
                            }
                        }


                        bool UnderHome = true;
                        if (Npc.homeTileY * 16 - 32 > Npc.position.Y)     //在家上面时
                        {
                            UnderHome = false;
                        }
                        if (NPCUtils.BuffNPC())
                        {
                            UnderHome = false;
                            if (Npc.GetGlobalNPC<ArmedGNPC>().actMode == ArmedGNPC.ActMode.Move)
                            {
                                if (Main.npc[Npc.GetGlobalNPC<ArmedGNPC>().ActTargetNPC].Center.Y > Npc.position.Y + Npc.height)
                                {
                                    UnderHome = true;
                                }
                            }
                        }
                        UnderHome = false;
                        if (!UnderHome && Npc.velocity.Y == 0f)
                        {
                            Collision.StepDown(ref Npc.position, ref Npc.velocity, Npc.width, Npc.height, ref Npc.stepSpeed, ref Npc.gfxOffY, 1, false);
                        }
                        if (Npc.velocity.Y >= 0f)
                        {
                            Collision.StepUp(ref Npc.position, ref Npc.velocity, Npc.width, Npc.height, ref Npc.stepSpeed, ref Npc.gfxOffY, 1, UnderHome, 1);
                        }

                        /*
                        if (Npc.wet && !IsWaterAnimal && Npc.townNPC && Npc.AI_007_TownEntities_CheckIfWillDrown(IsDrown))       //水上漂药水
                        {
                            int num22 = (int)((Npc.Center.X + 15 * Npc.direction) / 16f);
                            int num23 = (int)((Npc.position.Y + Npc.height - 16f) / 16f);
                            int num25 = 0;
                            int num26 = 0;
                            while (num26 <= 10 && Framing.GetTileSafely(num22 - Npc.direction, num23 - num26).LiquidAmount != 0)
                            {
                                num25++;
                                num26++;
                            }
                            float num27 = 0.3f;
                            float num28 = (float)Math.Sqrt((double)((num25 * 16 + 16) * 2f * num27));
                            if (num28 > 26f)
                            {
                                num28 = 26f;
                            }
                            Npc.velocity.Y = 0f - num28;
                            Npc.collideY = true;
                        }
                        */

                        #region 判断障碍
                        if (Npc.velocity.Y == 0f)
                        {
                            int num20 = (int)((Npc.position.X + Npc.width / 2 + 15 * Npc.direction) / 16f);
                            int num21 = (int)((Npc.position.Y + Npc.height - 16f) / 16f);
                            int num22 = 180;
                            Npc.AI_007_TownEntities_GetWalkPrediction(TileCenterX, floorX, IsWaterAnimal, IsDrown, num20, num21, out bool keepwalking3, out bool avoidFalling3);
                            bool flag19 = false;
                            bool flag20 = false;
                            if (Npc.wet && !IsWaterAnimal && Npc.townNPC && (flag20 = Npc.AI_007_TownEntities_CheckIfWillDrown(IsDrown)) && Npc.localAI[3] <= 0f)
                            {
                                avoidFalling3 = true;
                                Npc.localAI[3] = num22;
                                int num23 = 0;
                                int n = 0;
                                while (n <= 10 && Framing.GetTileSafely(num20 - Npc.direction, num21 - n).LiquidAmount != 0)
                                {
                                    num23++;
                                    n++;
                                }
                                float num24 = 0.3f;
                                float num25 = (float)Math.Sqrt((double)((num23 * 16 + 16) * 2f * num24));
                                if (num25 > 26f)
                                {
                                    num25 = 26f;
                                }
                                Npc.velocity.Y = 0f - num25;
                                Npc.localAI[3] = Npc.position.X;
                                flag19 = true;
                            }
                            if (avoidFalling3 && !flag19)
                            {
                                int num26 = (int)((Npc.position.X + Npc.width / 2) / 16f);
                                int num27 = 0;
                                for (int num28 = -1; num28 <= 1; num28++)
                                {
                                    Tile tileSafely2 = Framing.GetTileSafely(num26 + num28, num21 + 1);
                                    if (tileSafely2.HasUnactuatedTile && Main.tileSolid[tileSafely2.TileType])
                                    {
                                        num27++;
                                    }
                                }
                                if (num27 <= 2)
                                {
                                    if (Npc.velocity.X != 0f)
                                    {
                                        Npc.netUpdate = true;
                                    }
                                    avoidFalling3 = keepwalking3 = false;
                                    Npc.ai[0] = 0f;
                                    Npc.ai[1] = 50 + Main.rand.Next(50);
                                    Npc.ai[2] = 0f;
                                    Npc.localAI[3] = 40f;
                                }
                            }
                            if (Npc.position.X == Npc.localAI[3] && !flag19)
                            {
                                Npc.direction *= -1;
                                Npc.netUpdate = true;
                                Npc.localAI[3] = num22;
                            }
                            if (IsDrown && !flag19)
                            {
                                if (Npc.localAI[3] > num22)
                                {
                                    Npc.localAI[3] = num22;
                                }
                                if (Npc.localAI[3] > 0f)
                                {
                                    Npc.localAI[3] -= 1f;
                                }
                            }
                            else
                            {
                                Npc.localAI[3] = -1f;
                            }
                            Tile tileSafely3 = Framing.GetTileSafely(num20, num21);
                            Tile tileSafely4 = Framing.GetTileSafely(num20, num21 - 1);
                            Tile tileSafely5 = Framing.GetTileSafely(num20, num21 - 2);
                            bool flag21 = Npc.height / 16 < 3;
                            //开门
                            if ((Npc.townNPC || NPCID.Sets.AllowDoorInteraction[Npc.type]) && tileSafely5.HasUnactuatedTile && (TileLoader.OpenDoorID(tileSafely5) >= 0 || tileSafely5.TileType == 388) && (Main.rand.NextBool(10) || RestingTime || Npc.GetGlobalNPC<ArmedGNPC>().actMode != ArmedGNPC.ActMode.Default))
                            {
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    if (WorldGen.OpenDoor(num20, num21 - 2, Npc.direction))
                                    {
                                        Npc.closeDoor = true;
                                        Npc.doorX = num20;
                                        Npc.doorY = num21 - 2;
                                        NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 0, num20, num21 - 2, Npc.direction, 0, 0, 0);
                                        Npc.netUpdate = true;
                                        Npc.ai[1] += 80f;
                                    }
                                    else if (WorldGen.OpenDoor(num20, num21 - 2, -Npc.direction))
                                    {
                                        Npc.closeDoor = true;
                                        Npc.doorX = num20;
                                        Npc.doorY = num21 - 2;
                                        NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 0, num20, num21 - 2, (float)-(float)Npc.direction, 0, 0, 0);
                                        Npc.netUpdate = true;
                                        Npc.ai[1] += 80f;
                                    }
                                    else if (WorldGen.ShiftTallGate(num20, num21 - 2, false, false))
                                    {
                                        Npc.closeDoor = true;
                                        Npc.doorX = num20;
                                        Npc.doorY = num21 - 2;
                                        NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 4, num20, num21 - 2, 0f, 0, 0, 0);
                                        Npc.netUpdate = true;
                                        Npc.ai[1] += 80f;
                                    }
                                    else
                                    {
                                        Npc.direction *= -1;
                                        Npc.netUpdate = true;
                                    }
                                }
                            }
                            else
                            {
                                if ((Npc.velocity.X < 0f && Npc.direction == -1) || (Npc.velocity.X > 0f && Npc.direction == 1))
                                {
                                    bool flag22 = false;
                                    bool EnemySpotted2 = false;
                                    if (tileSafely5.HasUnactuatedTile && Main.tileSolid[tileSafely5.TileType] && !Main.tileSolidTop[tileSafely5.TileType] && (!flag21 || (tileSafely4.HasUnactuatedTile && Main.tileSolid[tileSafely4.TileType] && !Main.tileSolidTop[tileSafely4.TileType])))
                                    {
                                        if (!Collision.SolidTilesVersatile(num20 - Npc.direction * 2, num20 - Npc.direction, num21 - 5, num21 - 1) && !Collision.SolidTiles(num20, num20, num21 - 5, num21 - 3))
                                        {
                                            Npc.velocity.Y = -6f;
                                            Npc.netUpdate = true;
                                        }
                                        else if (IsRat)
                                        {
                                            if (WorldGen.SolidTile((int)(Npc.Center.X / 16f) + Npc.direction, (int)(Npc.Center.Y / 16f), false))
                                            {
                                                Npc.direction *= -1;
                                                Npc.velocity.X *= 0f;
                                                Npc.netUpdate = true;
                                            }
                                        }
                                        else if (HasTarget)
                                        {
                                            EnemySpotted2 = true;
                                            flag22 = true;
                                        }
                                        else if (!flag20)
                                        {
                                            flag22 = true;
                                        }
                                    }
                                    else if (tileSafely4.HasUnactuatedTile && Main.tileSolid[tileSafely4.TileType] && !Main.tileSolidTop[tileSafely4.TileType])
                                    {
                                        if (!Collision.SolidTilesVersatile(num20 - Npc.direction * 2, num20 - Npc.direction, num21 - 4, num21 - 1) && !Collision.SolidTiles(num20, num20, num21 - 4, num21 - 2))
                                        {
                                            Npc.velocity.Y = -5f;
                                            Npc.netUpdate = true;
                                        }
                                        else if (HasTarget)
                                        {
                                            EnemySpotted2 = true;
                                            flag22 = true;
                                        }
                                        else
                                        {
                                            flag22 = true;
                                        }
                                    }
                                    else if (Npc.position.Y + Npc.height - num21 * 16 > 20f && tileSafely3.HasUnactuatedTile && Main.tileSolid[tileSafely3.TileType] && !tileSafely3.TopSlope())
                                    {
                                        if (!Collision.SolidTilesVersatile(num20 - Npc.direction * 2, num20, num21 - 3, num21 - 1))
                                        {
                                            Npc.velocity.Y = -4.4f;
                                            Npc.netUpdate = true;
                                        }
                                        else if (HasTarget)
                                        {
                                            EnemySpotted2 = true;
                                            flag22 = true;
                                        }
                                        else
                                        {
                                            flag22 = true;
                                        }
                                    }
                                    else if (avoidFalling3)
                                    {
                                        if (!flag20)
                                        {
                                            flag22 = true;
                                        }
                                        if (HasTarget)
                                        {
                                            EnemySpotted2 = true;
                                        }
                                    }
                                    else if (IsSlimePet && !Collision.SolidTilesVersatile(num20 - Npc.direction * 2, num20 - Npc.direction, num21 - 2, num21 - 1))
                                    {
                                        Npc.velocity.Y = -5f;
                                        Npc.netUpdate = true;
                                    }
                                    if (EnemySpotted2)
                                    {
                                        keepwalking3 = false;
                                        Npc.velocity.X = 0f;
                                        Npc.ai[0] = 8f;
                                        Npc.ai[1] = 240f;
                                        Npc.netUpdate = true;
                                    }
                                    if (flag22)
                                    {
                                        Npc.direction *= -1;
                                        Npc.velocity.X *= -1f;
                                        Npc.netUpdate = true;
                                    }
                                    if (keepwalking3)
                                    {
                                        Npc.ai[1] = 90f;
                                        Npc.netUpdate = true;
                                    }
                                    if (Npc.velocity.Y < 0f)
                                    {
                                        Npc.localAI[3] = Npc.position.X;
                                    }
                                }
                                if (Npc.velocity.Y < 0f && Npc.wet)
                                {
                                    Npc.velocity.Y *= 1.2f;
                                }
                                if (Npc.velocity.Y < 0f && NPCID.Sets.TownCritter[Npc.type] && !IsRat)
                                {
                                    Npc.velocity.Y *= 1.2f;
                                }
                            }
                        }
                        else if (IsSlimePet && !Npc.wet)
                        {
                            int num29 = (int)(Npc.Center.X / 16f);
                            int num30 = (int)((Npc.position.Y + Npc.height - 16f) / 16f);
                            int num31 = 0;
                            for (int num32 = -1; num32 <= 1; num32++)
                            {
                                for (int num33 = 1; num33 <= 6; num33++)
                                {
                                    Tile tileSafely6 = Framing.GetTileSafely(num29 + num32, num30 + num33);
                                    if (tileSafely6.LiquidAmount > 0 || (tileSafely6.HasUnactuatedTile && Main.tileSolid[tileSafely6.TileType]))
                                    {
                                        num31++;
                                    }
                                }
                            }
                            if (num31 <= 2)
                            {
                                if (Npc.velocity.X != 0f)
                                {
                                    Npc.netUpdate = true;
                                }
                                Npc.velocity.X *= 0.2f;
                                Npc.ai[0] = 0f;
                                Npc.ai[1] = 50 + Main.rand.Next(50);
                                Npc.ai[2] = 0f;
                                Npc.localAI[3] = 40f;
                            }
                        }
                        #endregion
                    }
                }
                else if (Npc.ai[0] == 2f || Npc.ai[0] == 11f)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Npc.localAI[3] -= 1f;
                        if (Main.rand.NextBool(60) && Npc.localAI[3] == 0f)
                        {
                            Npc.localAI[3] = 60f;
                            Npc.direction *= -1;
                            Npc.netUpdate = true;
                        }
                    }
                    Npc.ai[1] -= 1f;
                    Npc.velocity.X *= 0.8f;
                    if (Npc.ai[1] <= 0f)
                    {
                        Npc.localAI[3] = 40f;
                        Npc.ai[0] = 0f;
                        Npc.ai[1] = 60 + Main.rand.Next(60);
                        Npc.netUpdate = true;
                    }
                }
                else if (Npc.ai[0] == 3f || Npc.ai[0] == 4f || Npc.ai[0] == 5f || Npc.ai[0] == 8f || Npc.ai[0] == 9f || Npc.ai[0] == 16f || Npc.ai[0] == 17f || Npc.ai[0] == 20f || Npc.ai[0] == 21f || Npc.ai[0] == 22f || Npc.ai[0] == 23f)
                {

                    if (Npc.GetGlobalNPC<ArmedGNPC>().actMode == ArmedGNPC.ActMode.Move)
                    {
                        Npc.ai[0] = 1f;
                        Npc.ai[1] = 180f;
                    }
                    else if (Npc.GetGlobalNPC<ArmedGNPC>().actMode == ArmedGNPC.ActMode.Attack)
                    {
                        Npc.ai[0] = 1f;
                        Npc.ai[1] = 180f;
                    }
                    else if (Npc.GetGlobalNPC<ArmedGNPC>().AlertMode)
                    {
                        if (Npc.ai[1] > 1)
                        {
                            Npc.ai[1] = 1;
                        }
                    }

                    Npc.velocity.X *= 0.8f;
                    Npc.ai[1] -= 1f;
                    if (Npc.ai[0] == 8f && Npc.ai[1] < 60f && HasTarget)
                    {
                        Npc.ai[1] = 180f;
                        Npc.netUpdate = true;
                    }
                    if (Npc.ai[0] == 5f)            //NPC坐下
                    {
                        Point coords = (Npc.Bottom + Vector2.UnitY * -2f).ToTileCoordinates();
                        Tile tile = Main.tile[coords.X, coords.Y];
                        if (!TileID.Sets.CanBeSatOnForNPCs[tile.TileType])
                        {
                            Npc.ai[1] = 0f;
                        }
                        else
                        {
                            Main.sittingManager.AddNPC(Npc.whoAmI, coords);
                        }
                    }
                    if (Npc.ai[1] <= 0f)
                    {
                        Npc.ai[0] = 0f;
                        Npc.ai[1] = 60 + Main.rand.Next(60);
                        Npc.ai[2] = 0f;
                        Npc.localAI[3] = 30 + Main.rand.Next(60);
                        Npc.netUpdate = true;
                    }
                }
                else if (Npc.ai[0] == 6f || Npc.ai[0] == 7f || Npc.ai[0] == 18f || Npc.ai[0] == 19f)
                {
                    if (Npc.ai[0] == 18f && (Npc.localAI[3] < 1f || Npc.localAI[3] > 2f))
                    {
                        Npc.localAI[3] = 2f;
                    }
                    Npc.velocity.X *= 0.8f;
                    Npc.ai[1] -= 1f;
                    int TalkPlayer = (int)Npc.ai[2];
                    if (TalkPlayer < 0 || TalkPlayer > 255 || !Main.player[TalkPlayer].CanBeTalkedTo || Main.player[TalkPlayer].Distance(Npc.Center) > 200f || !Collision.CanHitLine(Npc.Top, 0, 0, Main.player[TalkPlayer].Top, 0, 0))
                    {
                        Npc.ai[1] = 0f;           //谈话行为
                    }
                    if (Npc.ai[1] > 0f)        //谈话面向玩家
                    {
                        int TalkDir = (Npc.Center.X < Main.player[TalkPlayer].Center.X) ? 1 : -1;
                        if (TalkDir != Npc.direction)
                        {
                            Npc.netUpdate = true;
                        }
                        Npc.direction = TalkDir;
                    }
                    else
                    {
                        Npc.ai[0] = 0f;
                        Npc.ai[1] = 60 + Main.rand.Next(60);
                        Npc.ai[2] = 0f;
                        Npc.localAI[3] = 30 + Main.rand.Next(60);
                        Npc.netUpdate = true;
                    }
                }
                else if (Npc.ai[0] == 10f)      //投掷攻击
                {
                    int ProjType = 0;
                    int Damage = 0;
                    float knockBack = 0f;
                    float ShootSpeed = 0f;
                    int attackDelay = 0;
                    int AttackCooldown = 0;
                    int RandomAttackCooldown = 0;
                    float gravityCorrection = 0f;
                    float DetectRange = NPCUtils.GetDangerDetectRange(Npc);
                    float randomOffset = 0f;

                    int Target = -1;
                    if (ShootDir == 1 && Npc.spriteDirection == 1 && TargetRight != -1)
                    {
                        Target = TargetRight;
                    }
                    if (ShootDir == -1 && Npc.spriteDirection == -1 && TargetLeft != -1)
                    {
                        Target = TargetLeft;
                    }
                    Npc.GetGlobalNPC<ArmedGNPC>().NPCTargetForSpecialUse = Target;

                    if (NPCStats.GetModifiedAttackTime(Npc) == Npc.ai[1])
                    {
                        Npc.frameCounter = 0.0;
                        Npc.localAI[3] = 0f;
                    }

                    if (Npc.type == NPCID.Demolitionist)
                    {
                        ProjType = 30;
                        ShootSpeed = 6f;
                        Damage = 20;
                        attackDelay = 10;
                        AttackCooldown = 180;
                        RandomAttackCooldown = 120;
                        gravityCorrection = 16f;
                        knockBack = 7f;
                    }
                    else if (Npc.type == NPCID.BestiaryGirl)
                    {
                        ProjType = 880;
                        ShootSpeed = 24f;
                        Damage = 15;
                        attackDelay = 1;
                        gravityCorrection = 0f;
                        knockBack = 7f;
                        AttackCooldown = 15;
                        RandomAttackCooldown = 10;
                        if (Npc.ShouldBestiaryGirlBeLycantrope())
                        {
                            ProjType = 929;
                            Damage = (int)(Damage * 1.5f);
                        }
                    }
                    else if (Npc.type == NPCID.DD2Bartender)
                    {
                        ProjType = 669;
                        ShootSpeed = 6f;
                        Damage = 24;
                        attackDelay = 10;
                        AttackCooldown = 120;
                        RandomAttackCooldown = 60;
                        gravityCorrection = 16f;
                        knockBack = 9f;
                    }
                    else if (Npc.type == NPCID.Golfer)
                    {
                        ProjType = 721;
                        ShootSpeed = 8f;
                        Damage = 15;
                        attackDelay = 5;
                        AttackCooldown = 20;
                        RandomAttackCooldown = 10;
                        gravityCorrection = 16f;
                        knockBack = 9f;
                    }
                    else if (Npc.type == NPCID.PartyGirl)
                    {
                        ProjType = 588;
                        ShootSpeed = 6f;
                        Damage = 30;
                        attackDelay = 10;
                        AttackCooldown = 60;
                        RandomAttackCooldown = 120;
                        gravityCorrection = 16f;
                        knockBack = 6f;
                    }
                    else if (Npc.type == NPCID.Merchant)
                    {
                        ProjType = 48;
                        ShootSpeed = 9f;
                        Damage = 12;
                        attackDelay = 10;
                        AttackCooldown = 60;
                        RandomAttackCooldown = 60;
                        gravityCorrection = 16f;
                        knockBack = 1.5f;
                    }
                    else if (Npc.type == NPCID.Angler)
                    {
                        ProjType = 520;
                        ShootSpeed = 12f;
                        Damage = 10;
                        attackDelay = 10;
                        AttackCooldown = 0;
                        RandomAttackCooldown = 1;
                        gravityCorrection = 16f;
                        knockBack = 3f;
                    }
                    else if (Npc.type == NPCID.SkeletonMerchant)
                    {
                        ProjType = 21;
                        ShootSpeed = 14f;
                        Damage = 14;
                        attackDelay = 10;
                        AttackCooldown = 0;
                        RandomAttackCooldown = 1;
                        gravityCorrection = 16f;
                        knockBack = 3f;
                    }
                    else if (Npc.type == NPCID.GoblinTinkerer)
                    {
                        ProjType = 24;
                        ShootSpeed = 5f;
                        Damage = 15;
                        attackDelay = 10;
                        AttackCooldown = 60;
                        RandomAttackCooldown = 60;
                        gravityCorrection = 16f;
                        knockBack = 1f;
                    }
                    else if (Npc.type == NPCID.Mechanic)
                    {
                        ProjType = 582;
                        ShootSpeed = 10f;
                        Damage = 11;
                        attackDelay = 1;
                        AttackCooldown = 30;
                        RandomAttackCooldown = 30;
                        knockBack = 3.5f;
                    }
                    else if (Npc.type == NPCID.Nurse)
                    {
                        ProjType = 583;
                        ShootSpeed = 8f;
                        Damage = 8;
                        attackDelay = 1;
                        AttackCooldown = 15;
                        RandomAttackCooldown = 10;
                        knockBack = 2f;
                        gravityCorrection = 10f;
                    }
                    else if (Npc.type == NPCID.SantaClaus)
                    {
                        ProjType = 589;
                        ShootSpeed = 7f;
                        Damage = 22;
                        attackDelay = 1;
                        AttackCooldown = 10;
                        RandomAttackCooldown = 1;
                        knockBack = 2f;
                        gravityCorrection = 10f;
                    }
                    NPCLoader.TownNPCAttackStrength(Npc, ref Damage, ref knockBack);
                    NPCLoader.TownNPCAttackCooldown(Npc, ref AttackCooldown, ref RandomAttackCooldown);
                    NPCLoader.TownNPCAttackProj(Npc, ref ProjType, ref attackDelay);
                    NPCLoader.TownNPCAttackProjSpeed(Npc, ref ShootSpeed, ref gravityCorrection, ref randomOffset);
                    if (Main.expertMode)
                    {
                        Damage = (int)(Damage * Main.GameModeInfo.TownNPCDamageMultiplier);
                    }
                    Damage = (int)(Damage * damageMult);
                    Npc.velocity.X *= 0.8f;
                    Npc.ai[1] -= 1f;
                    Npc.localAI[3] += 1f;
                    if (Npc.localAI[3] == attackDelay && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 ShootVel = -Vector2.UnitY;
                        if (ShootDir == 1 && Npc.spriteDirection == 1 && TargetRight != -1)
                        {
                            ShootVel = Npc.DirectionTo(Main.npc[TargetRight].Center + new Vector2(0f, (0f - gravityCorrection) * MathHelper.Clamp(Npc.Distance(Main.npc[TargetRight].Center) / DetectRange, 0f, 1f)));
                        }
                        if (ShootDir == -1 && Npc.spriteDirection == -1 && TargetLeft != -1)
                        {
                            ShootVel = Npc.DirectionTo(Main.npc[TargetLeft].Center + new Vector2(0f, (0f - gravityCorrection) * MathHelper.Clamp(Npc.Distance(Main.npc[TargetLeft].Center) / DetectRange, 0f, 1f)));
                        }
                        if (ShootVel.HasNaNs() || Math.Sign(ShootVel.X) != Npc.spriteDirection)
                        {
                            ShootVel = new(Npc.spriteDirection, -1f);
                        }
                        ShootVel *= ShootSpeed;
                        ShootVel += Utils.RandomVector2(Main.rand, 0f - randomOffset, randomOffset);
                        int num44 = (Npc.type == NPCID.Mechanic) ? Projectile.NewProjectile(Npc.GetSpawnSource_ForProjectile(), Npc.Center.X + Npc.spriteDirection * 16, Npc.Center.Y - 2f, ShootVel.X, ShootVel.Y, ProjType, Damage, knockBack, Main.myPlayer, 0f, Npc.whoAmI, Npc.townNpcVariationIndex) : ((Npc.type != NPCID.SantaClaus) ? Projectile.NewProjectile(Npc.GetSpawnSource_ForProjectile(), Npc.Center.X + Npc.spriteDirection * 16, Npc.Center.Y - 2f, ShootVel.X, ShootVel.Y, ProjType, Damage, knockBack, Main.myPlayer, 0f, 0f, 0f) : Projectile.NewProjectile(Npc.GetSpawnSource_ForProjectile(), Npc.Center.X + Npc.spriteDirection * 16, Npc.Center.Y - 2f, ShootVel.X, ShootVel.Y, ProjType, Damage, knockBack, Main.myPlayer, 0f, Main.rand.Next(5), 0f));
                        Main.projectile[num44].npcProj = true;
                        Main.projectile[num44].noDropItem = true;
                        Main.projectile[num44].GetGlobalProjectile<AttackerGProj>().ProjTarget = Target;


                        if (!ArmedGNPC.GetWeapon(Npc).IsAir && Npc.type != NPCID.Nurse)
                        {
                            int CritChance = ArmedGNPC.GetWeapon(Npc).crit;
                            NPCStats.ModifyCritChance(Npc, ref CritChance);

                            Main.projectile[num44].CritChance += CritChance;

                            if (ArmedGNPC.GetWeapon(Npc).UseSound != null)
                            {
                                SoundEngine.PlaySound(ArmedGNPC.GetWeapon(Npc).UseSound, Npc.Center);
                            }
                        }

                        if (Npc.type == NPCID.Golfer && ArmedGNPC.GetWeapon(Npc).IsAir)
                        {
                            Main.projectile[num44].timeLeft = 480;
                        }

                        if (Main.rand.NextBool(5))
                        {
                            if (!ArmedGNPC.GetAltWeapon(Npc).IsAir && Npc.type != NPCID.Nurse)          //投手副武器
                            {
                                Item AltWeapon = ArmedGNPC.GetAltWeapon(Npc);
                                int shoot = AltWeapon.shoot;
                                int DamageAlt = AltWeapon.damage;
                                float KnockBackAlt = AltWeapon.knockBack;
                                Vector2 SpeedAlt = Vector2.Normalize(ShootVel) * AltWeapon.shootSpeed;
                                Vector2 PosAlt = new(Npc.Center.X + Npc.spriteDirection * 16, Npc.Center.Y - 2f);

                                if (AltWeapon.ModItem != null)
                                {
                                    AltWeapon.ModItem.ModifyShootStats(Main.LocalPlayer, ref PosAlt, ref SpeedAlt, ref shoot, ref DamageAlt, ref KnockBackAlt);
                                    if (AltWeapon.ModItem.Shoot(Main.LocalPlayer, null, PosAlt, SpeedAlt, shoot, DamageAlt, KnockBackAlt))
                                    {
                                        ProjType = shoot;
                                    }
                                    else
                                    {
                                        ProjType = 0;
                                    }
                                }
                                else
                                {
                                    ProjType = shoot;

                                    if (VanillaItemProjFix.TransFormProj(Npc, AltWeapon.type) != -1)
                                    {
                                        ProjType = VanillaItemProjFix.TransFormProj(Npc, AltWeapon.type);
                                    }
                                }
                                if (ProjType != 0)
                                {
                                    int protmp = Projectile.NewProjectile(Npc.GetSpawnSource_ForProjectile(), PosAlt, SpeedAlt, ProjType, DamageAlt, KnockBackAlt, Main.myPlayer, 0f, 0f);
                                    Main.projectile[protmp].npcProj = true;
                                    Main.projectile[protmp].noDropItem = true;
                                    Main.projectile[protmp].GetGlobalProjectile<AttackerGProj>().ProjTarget = Target;
                                }
                            }

                        }
                    }



                    if (Npc.ai[1] <= 0f)
                    {
                        Npc.ai[0] = (Npc.localAI[2] == 8f && HasTarget) ? 8 : 0;
                        Npc.ai[1] = AttackCooldown + Main.rand.Next(RandomAttackCooldown);
                        Npc.ai[2] = 0f;
                        Npc.localAI[1] = Npc.localAI[3] = AttackCooldown / 2 + Main.rand.Next(RandomAttackCooldown);
                        Npc.netUpdate = true;
                    }
                }
                else if (Npc.ai[0] == 12f)    //远程攻击
                {
                    int ProjType = 0;
                    int Damage = 0;
                    float ShootSpeed = 0f;
                    int attackDelay = 0;
                    int AttackCooldown = 0;
                    int RandomAttackCooldown = 0;
                    float knockBack = 0f;
                    float gravityCorrection = 0f;
                    bool inBetweenShots = false;
                    float randomOffset = 0f;
                    if (NPCStats.GetModifiedAttackTime(Npc) == Npc.ai[1])
                    {
                        Npc.frameCounter = 0.0;
                        Npc.localAI[3] = 0f;
                    }
                    int Target = -1;
                    if (ShootDir == 1 && Npc.spriteDirection == 1)
                    {
                        Target = TargetRight;
                    }
                    if (ShootDir == -1 && Npc.spriteDirection == -1)
                    {
                        Target = TargetLeft;
                    }
                    Npc.GetGlobalNPC<ArmedGNPC>().NPCTargetForSpecialUse = Target;

                    if (Npc.type == NPCID.ArmsDealer)
                    {
                        ProjType = 14;
                        ShootSpeed = 13f;
                        Damage = 24;
                        AttackCooldown = 14;
                        RandomAttackCooldown = 4;
                        knockBack = 3f;
                        attackDelay = 1;
                        randomOffset = 0.5f;
                        if (NPCStats.GetModifiedAttackTime(Npc) == Npc.ai[1])
                        {
                            Npc.frameCounter = 0.0;
                            Npc.localAI[3] = 0f;
                        }
                        if (Main.hardMode)
                        {
                            Damage = 15;
                            if (Npc.localAI[3] > attackDelay)
                            {
                                attackDelay = 10;
                                inBetweenShots = true;
                            }
                            if (Npc.localAI[3] > attackDelay)
                            {
                                attackDelay = 20;
                                inBetweenShots = true;
                            }
                            if (Npc.localAI[3] > attackDelay)
                            {
                                attackDelay = 30;
                                inBetweenShots = true;
                            }
                        }
                    }
                    else if (Npc.type == NPCID.Painter)
                    {
                        ProjType = 587;
                        ShootSpeed = 10f;
                        Damage = 8;
                        AttackCooldown = 10;
                        RandomAttackCooldown = 1;
                        knockBack = 1.75f;
                        attackDelay = 1;
                        randomOffset = 0.5f;
                        if (Npc.localAI[3] > attackDelay)
                        {
                            attackDelay = 12;
                            inBetweenShots = true;
                        }
                        if (Npc.localAI[3] > attackDelay)
                        {
                            attackDelay = 24;
                            inBetweenShots = true;
                        }
                        if (Main.hardMode)
                        {
                            Damage += 2;
                        }
                    }
                    else if (Npc.type == NPCID.TravellingMerchant)
                    {
                        ProjType = 14;
                        ShootSpeed = 13f;
                        Damage = 24;
                        AttackCooldown = 12;
                        RandomAttackCooldown = 5;
                        knockBack = 2f;
                        attackDelay = 1;
                        randomOffset = 0.2f;
                        if (Main.hardMode)
                        {
                            Damage = 30;
                            ProjType = 357;
                        }
                    }
                    else if (Npc.type == NPCID.Guide)
                    {
                        ShootSpeed = 10f;
                        Damage = 8;
                        attackDelay = 1;
                        if (Main.hardMode)
                        {
                            ProjType = 2;
                            AttackCooldown = 15;
                            RandomAttackCooldown = 10;
                            Damage += 6;
                        }
                        else
                        {
                            ProjType = 1;
                            AttackCooldown = 30;
                            RandomAttackCooldown = 20;
                        }
                        knockBack = 2.75f;
                        gravityCorrection = 4f;
                        randomOffset = 0.7f;
                    }
                    else if (Npc.type == NPCID.WitchDoctor)
                    {
                        ProjType = 267;
                        ShootSpeed = 14f;
                        Damage = 20;
                        attackDelay = 1;
                        AttackCooldown = 10;
                        RandomAttackCooldown = 1;
                        knockBack = 3f;
                        gravityCorrection = 6f;
                        randomOffset = 0.4f;
                    }
                    else if (Npc.type == NPCID.Steampunker)
                    {
                        ProjType = 242;
                        ShootSpeed = 13f;
                        Damage = (!Main.hardMode) ? 11 : 15;
                        AttackCooldown = 10;
                        RandomAttackCooldown = 1;
                        knockBack = 2f;
                        attackDelay = 1;
                        if (Npc.localAI[3] > attackDelay)
                        {
                            attackDelay = 8;
                            inBetweenShots = true;
                        }
                        if (Npc.localAI[3] > attackDelay)
                        {
                            attackDelay = 16;
                            inBetweenShots = true;
                        }
                        randomOffset = 0.3f;
                    }
                    else if (Npc.type == NPCID.Pirate)
                    {
                        ProjType = 14;
                        ShootSpeed = 14f;
                        Damage = 24;
                        AttackCooldown = 10;
                        RandomAttackCooldown = 1;
                        knockBack = 2f;
                        attackDelay = 1;
                        randomOffset = 0.7f;
                        if (Npc.localAI[3] > attackDelay)
                        {
                            attackDelay = 16;
                            inBetweenShots = true;
                        }
                        if (Npc.localAI[3] > attackDelay)
                        {
                            attackDelay = 24;
                            inBetweenShots = true;
                        }
                        if (Npc.localAI[3] > attackDelay)
                        {
                            attackDelay = 32;
                            inBetweenShots = true;
                        }
                        if (Npc.localAI[3] > attackDelay)
                        {
                            attackDelay = 40;
                            inBetweenShots = true;
                        }
                        if (Npc.localAI[3] > attackDelay)
                        {
                            attackDelay = 48;
                            inBetweenShots = true;
                        }
                        if (Npc.localAI[3] == 0f && Target != -1 && Npc.Distance(Main.npc[Target].Center) < NPCID.Sets.PrettySafe[Npc.type])
                        {
                            randomOffset = 0.1f;
                            ProjType = 162;
                            Damage = 50;
                            knockBack = 10f;
                            ShootSpeed = 24f;
                        }
                    }
                    else if (Npc.type == NPCID.Cyborg)
                    {
                        ProjType = Utils.SelectRandom<int>(Main.rand, new int[]
                        {
                            134,
                            133,
                            135
                        });
                        attackDelay = 1;
                        switch (ProjType)
                        {
                            case 133:
                                ShootSpeed = 10f;
                                Damage = 25;
                                AttackCooldown = 10;
                                RandomAttackCooldown = 1;
                                knockBack = 6f;
                                randomOffset = 0.2f;
                                break;
                            case 134:
                                ShootSpeed = 13f;
                                Damage = 20;
                                AttackCooldown = 20;
                                RandomAttackCooldown = 10;
                                knockBack = 4f;
                                randomOffset = 0.1f;
                                break;
                            case 135:
                                ShootSpeed = 12f;
                                Damage = 30;
                                AttackCooldown = 30;
                                RandomAttackCooldown = 10;
                                knockBack = 7f;
                                randomOffset = 0.2f;
                                break;
                        }
                    }
                    NPCLoader.TownNPCAttackStrength(Npc, ref Damage, ref knockBack);
                    NPCLoader.TownNPCAttackCooldown(Npc, ref AttackCooldown, ref RandomAttackCooldown);
                    NPCLoader.TownNPCAttackProj(Npc, ref ProjType, ref attackDelay);
                    NPCLoader.TownNPCAttackProjSpeed(Npc, ref ShootSpeed, ref gravityCorrection, ref randomOffset);
                    NPCLoader.TownNPCAttackShoot(Npc, ref inBetweenShots);
                    if (Main.expertMode)
                    {
                        Damage = (int)(Damage * Main.GameModeInfo.TownNPCDamageMultiplier);
                    }
                    Damage = (int)(Damage * damageMult);
                    Npc.velocity.X *= 0.8f;
                    Npc.ai[1] -= 1f;
                    Npc.localAI[3] += 1f;
                    if (Npc.localAI[3] == attackDelay && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 ShootVel = Vector2.Zero;
                        if (Target != -1)
                        {
                            ShootVel = Npc.DirectionTo(Main.npc[Target].Center + new Vector2(0f, -gravityCorrection));
                        }
                        if (ShootVel.HasNaNs() || Math.Sign(ShootVel.X) != Npc.spriteDirection)
                        {
                            ShootVel = new(Npc.spriteDirection, 0f);
                        }
                        ShootVel *= ShootSpeed;
                        ShootVel += Utils.RandomVector2(Main.rand, 0f - randomOffset, randomOffset);
                        int num53 = (Npc.type != NPCID.Painter) ? Projectile.NewProjectile(Npc.GetSpawnSource_ForProjectile(), Npc.Center.X + Npc.spriteDirection * 16, Npc.Center.Y - 2f, ShootVel.X, ShootVel.Y, ProjType, Damage, knockBack, Main.myPlayer, 0f, 0f, 0f) : Projectile.NewProjectile(Npc.GetSpawnSource_ForProjectile(), Npc.Center.X + Npc.spriteDirection * 16, Npc.Center.Y - 2f, ShootVel.X, ShootVel.Y, ProjType, Damage, knockBack, Main.myPlayer, 0f, Main.rand.Next(12) / 6f, 0f);
                        Main.projectile[num53].npcProj = true;
                        Main.projectile[num53].noDropItem = true;
                        Main.projectile[num53].GetGlobalProjectile<AttackerGProj>().ProjTarget = Target;

                        if (!ArmedGNPC.GetWeapon(Npc).IsAir)
                        {
                            int CritChance = ArmedGNPC.GetWeapon(Npc).crit;
                            NPCStats.ModifyCritChance(Npc, ref CritChance);

                            Main.projectile[num53].CritChance += CritChance;

                            if (ArmedGNPC.GetWeapon(Npc).UseSound != null)
                            {
                                SoundEngine.PlaySound(ArmedGNPC.GetWeapon(Npc).UseSound, Npc.Center);
                            }
                        }
                    }



                    if (Npc.localAI[3] == attackDelay && inBetweenShots && Target != -1)
                    {
                        Vector2 vector2 = Npc.DirectionTo(Main.npc[Target].Center);
                        if (vector2.Y <= 0.5f && vector2.Y >= -0.5f)
                        {
                            Npc.ai[2] = vector2.Y;
                        }
                    }
                    if (Npc.ai[1] <= 0f)
                    {
                        Npc.ai[0] = (Npc.localAI[2] == 8f && HasTarget) ? 8 : 0;
                        Npc.ai[1] = AttackCooldown + Main.rand.Next(RandomAttackCooldown);
                        Npc.ai[2] = 0f;
                        Npc.localAI[1] = Npc.localAI[3] = AttackCooldown / 2 + Main.rand.Next(RandomAttackCooldown);
                        Npc.netUpdate = true;
                    }
                }
                else if (Npc.ai[0] == 13f)             //护士治疗
                {
                    Npc.velocity.X *= 0.8f;
                    if (NPCStats.GetModifiedAttackTime(Npc) == Npc.ai[1])
                    {
                        Npc.frameCounter = 0.0;
                    }
                    Npc.ai[1] -= 1f;
                    Npc.localAI[3] += 1f;
                    if (Npc.localAI[3] == 1f && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 vec3 = Npc.DirectionTo(Main.npc[(int)Npc.ai[2]].Center + new Vector2(0f, -20f));
                        if (vec3.HasNaNs() || Math.Sign(vec3.X) == -Npc.spriteDirection)
                        {
                            vec3 = new(Npc.spriteDirection, -1f);
                        }
                        vec3 *= 8f;
                        int num54 = Projectile.NewProjectile(Npc.GetSpawnSource_ForProjectile(), Npc.Center.X + Npc.spriteDirection * 16, Npc.Center.Y - 2f, vec3.X, vec3.Y, 584, 0, 0f, Main.myPlayer, Npc.ai[2], 0f, 0f);
                        Main.projectile[num54].npcProj = true;
                        Main.projectile[num54].noDropItem = true;
                    }
                    if (Npc.ai[1] <= 0f)
                    {
                        Npc.ai[0] = 0f;
                        Npc.ai[1] = 10 + Main.rand.Next(10);
                        Npc.ai[2] = 0f;
                        Npc.localAI[3] = 5 + Main.rand.Next(10);
                        Npc.netUpdate = true;
                    }
                }
                else if (Npc.ai[0] == 14f)       //魔法攻击
                {
                    int ProjType = 0;
                    int Damage = 0;
                    float ShootSpeed = 0f;
                    int attackDelay = 0;
                    int AttackCooldown = 0;
                    int RandomAttackCooldown = 0;
                    float knockBack = 0f;
                    float gravityCorrection = 0f;
                    float DetectRange = NPCUtils.GetDangerDetectRange(Npc);
                    float auraLightMultiplier = 1f;
                    float randomOffset = 0f;
                    if (NPCStats.GetModifiedAttackTime(Npc) == Npc.ai[1])
                    {
                        Npc.frameCounter = 0.0;
                        Npc.localAI[3] = 0f;
                    }
                    int Target = -1;
                    if (ShootDir == 1 && Npc.spriteDirection == 1)
                    {
                        Target = TargetRight;
                    }
                    if (ShootDir == -1 && Npc.spriteDirection == -1)
                    {
                        Target = TargetLeft;
                    }
                    Npc.GetGlobalNPC<ArmedGNPC>().NPCTargetForSpecialUse = Target;

                    if (Npc.type == NPCID.Clothier)
                    {
                        ProjType = 585;
                        ShootSpeed = 10f;
                        Damage = 16;
                        attackDelay = 30;
                        AttackCooldown = 20;
                        RandomAttackCooldown = 15;
                        knockBack = 2f;
                        randomOffset = 1f;
                    }
                    else if (Npc.type == NPCID.Wizard)
                    {
                        ProjType = 15;
                        ShootSpeed = 6f;
                        Damage = 18;
                        attackDelay = 15;
                        AttackCooldown = 15;
                        RandomAttackCooldown = 5;
                        knockBack = 3f;
                        gravityCorrection = 20f;
                    }
                    else if (Npc.type == NPCID.Truffle)
                    {
                        ProjType = 590;
                        Damage = 40;
                        attackDelay = 15;
                        AttackCooldown = 10;
                        RandomAttackCooldown = 1;
                        knockBack = 3f;
                        while (Npc.localAI[3] > attackDelay)
                        {
                            attackDelay += 15;
                        }
                    }
                    else if (Npc.type == NPCID.Princess)
                    {
                        ProjType = 950;
                        Damage = (!Main.hardMode) ? 15 : 20;
                        attackDelay = 15;
                        AttackCooldown = 0;
                        RandomAttackCooldown = 0;
                        knockBack = 3f;
                        while (Npc.localAI[3] > attackDelay)
                        {
                            attackDelay += 10;
                        }
                    }
                    else if (Npc.type == NPCID.Dryad)
                    {
                        ProjType = 586;
                        attackDelay = 24;
                        AttackCooldown = 10;
                        RandomAttackCooldown = 1;
                        knockBack = 3f;
                    }
                    NPCLoader.TownNPCAttackStrength(Npc, ref Damage, ref knockBack);
                    NPCLoader.TownNPCAttackCooldown(Npc, ref AttackCooldown, ref RandomAttackCooldown);
                    NPCLoader.TownNPCAttackProj(Npc, ref ProjType, ref attackDelay);
                    NPCLoader.TownNPCAttackProjSpeed(Npc, ref ShootSpeed, ref gravityCorrection, ref randomOffset);
                    NPCLoader.TownNPCAttackMagic(Npc, ref auraLightMultiplier);
                    if (Main.expertMode)
                    {
                        Damage = (int)(Damage * Main.GameModeInfo.TownNPCDamageMultiplier);
                    }
                    Damage = (int)(Damage * damageMult);
                    Npc.velocity.X *= 0.8f;
                    Npc.ai[1] -= 1f;
                    Npc.localAI[3] += 1f;
                    if (Npc.localAI[3] == attackDelay && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 ShootVel = Vector2.Zero;
                        if (Target != -1)
                        {
                            ShootVel = Npc.DirectionTo(Main.npc[Target].Center + new Vector2(0f, (0f - gravityCorrection) * MathHelper.Clamp(Npc.Distance(Main.npc[Target].Center) / DetectRange, 0f, 1f)));
                        }
                        if (ShootVel.HasNaNs() || Math.Sign(ShootVel.X) != Npc.spriteDirection)
                        {
                            ShootVel = new(Npc.spriteDirection, 0f);
                        }
                        ShootVel *= ShootSpeed;
                        ShootVel += Utils.RandomVector2(Main.rand, 0f - randomOffset, randomOffset);

                        if (ArmedGNPC.GetWeapon(Npc).IsAir)
                        {

                            if (Npc.type == NPCID.Wizard)
                            {
                                int num65 = Utils.SelectRandom<int>(Main.rand, new int[]
                                {
                                1,
                                1,
                                1,
                                1,
                                2,
                                2,
                                3
                                });
                                for (int num66 = 0; num66 < num65; num66++)
                                {
                                    Vector2 RandomOffset2 = Utils.RandomVector2(Main.rand, -3.4f, 3.4f);
                                    int num67 = Projectile.NewProjectile(Npc.GetSpawnSource_ForProjectile(), Npc.Center.X + Npc.spriteDirection * 16, Npc.Center.Y - 2f, ShootVel.X + RandomOffset2.X, ShootVel.Y + RandomOffset2.Y, ProjType, Damage, knockBack, Main.myPlayer, 0f, 0f, Npc.townNpcVariationIndex);
                                    Main.projectile[num67].npcProj = true;
                                    Main.projectile[num67].noDropItem = true;

                                    int CritChance = 0;
                                    NPCStats.ModifyCritChance(Npc, ref CritChance);
                                    Main.projectile[num67].CritChance += CritChance;
                                }
                            }
                            else if (Npc.type == NPCID.Truffle)
                            {
                                if (Target != -1)
                                {
                                    Vector2 ShootPos = Main.npc[Target].position - Main.npc[Target].Size * 2f + Main.npc[Target].Size * Utils.RandomVector2(Main.rand, 0f, 1f) * 5f;
                                    int ProjAmount = 10;
                                    while (ProjAmount > 0 && WorldGen.SolidTile(Framing.GetTileSafely((int)ShootPos.X / 16, (int)ShootPos.Y / 16)))
                                    {
                                        ProjAmount--;
                                        ShootPos = Main.npc[Target].position - Main.npc[Target].Size * 2f + Main.npc[Target].Size * Utils.RandomVector2(Main.rand, 0f, 1f) * 5f;
                                    }
                                    int num69 = Projectile.NewProjectile(Npc.GetSpawnSource_ForProjectile(), ShootPos.X, ShootPos.Y, 0f, 0f, ProjType, Damage, knockBack, Main.myPlayer, 0f, 0f, Npc.townNpcVariationIndex);
                                    Main.projectile[num69].npcProj = true;
                                    Main.projectile[num69].noDropItem = true;
                                    int CritChance = 0;
                                    NPCStats.ModifyCritChance(Npc, ref CritChance);
                                    Main.projectile[num69].CritChance += CritChance;
                                }
                            }
                            else if (Npc.type == NPCID.Princess)
                            {
                                if (Target != -1)
                                {
                                    Vector2 ShootPos = Main.npc[Target].position + Main.npc[Target].Size * Utils.RandomVector2(Main.rand, 0f, 1f) * 1f;
                                    int ProjAmount = 5;
                                    while (ProjAmount > 0 && WorldGen.SolidTile(Framing.GetTileSafely((int)ShootPos.X / 16, (int)ShootPos.Y / 16)))
                                    {
                                        ProjAmount--;
                                        ShootPos = Main.npc[Target].position + Main.npc[Target].Size * Utils.RandomVector2(Main.rand, 0f, 1f) * 1f;
                                    }
                                    int num71 = Projectile.NewProjectile(Npc.GetSpawnSource_ForProjectile(), ShootPos.X, ShootPos.Y, 0f, 0f, ProjType, Damage, knockBack, Main.myPlayer, 0f, 0f, Npc.townNpcVariationIndex);
                                    Main.projectile[num71].npcProj = true;
                                    Main.projectile[num71].noDropItem = true;
                                    int CritChance = 0;
                                    NPCStats.ModifyCritChance(Npc, ref CritChance);
                                    Main.projectile[num71].CritChance += CritChance;
                                }
                            }
                            else if (Npc.type == NPCID.Dryad)
                            {
                                int num72 = Projectile.NewProjectile(Npc.GetSpawnSource_ForProjectile(), Npc.Center.X + Npc.spriteDirection * 16, Npc.Center.Y - 2f, ShootVel.X, ShootVel.Y, ProjType, Damage, knockBack, Main.myPlayer, 0f, Npc.whoAmI, Npc.townNpcVariationIndex);
                                Main.projectile[num72].npcProj = true;
                                Main.projectile[num72].noDropItem = true;
                                int CritChance = 0;
                                NPCStats.ModifyCritChance(Npc, ref CritChance);
                                Main.projectile[num72].CritChance += CritChance;
                            }
                            else
                            {
                                int num73 = Projectile.NewProjectile(Npc.GetSpawnSource_ForProjectile(), Npc.Center.X + Npc.spriteDirection * 16, Npc.Center.Y - 2f, ShootVel.X, ShootVel.Y, ProjType, Damage, knockBack, Main.myPlayer, 0f, 0f, 0f);
                                Main.projectile[num73].npcProj = true;
                                Main.projectile[num73].noDropItem = true;
                                int CritChance = 0;
                                NPCStats.ModifyCritChance(Npc, ref CritChance);
                                Main.projectile[num73].CritChance += CritChance;
                            }
                        }
                        else
                        {
                            if (Npc.type != NPCID.Dryad)
                            {
                                int protmp = Projectile.NewProjectile(Npc.GetSpawnSource_ForProjectile(), Npc.Center.X + Npc.spriteDirection * 16, Npc.Center.Y - 2f, ShootVel.X, ShootVel.Y, ProjType, Damage, knockBack, Main.myPlayer, 0f, 0f);
                                Main.projectile[protmp].npcProj = true;
                                Main.projectile[protmp].noDropItem = true;
                                Main.projectile[protmp].GetGlobalProjectile<AttackerGProj>().ProjTarget = Target;
                                int CritChance = ArmedGNPC.GetWeapon(Npc).crit;
                                NPCStats.ModifyCritChance(Npc, ref CritChance);
                                Main.projectile[protmp].CritChance += CritChance;

                                if (ArmedGNPC.GetWeapon(Npc).UseSound != null)
                                {
                                    SoundEngine.PlaySound(ArmedGNPC.GetWeapon(Npc).UseSound, Npc.Center);
                                }
                            }
                            else
                            {
                                int protmp = Projectile.NewProjectile(Npc.GetSpawnSource_ForProjectile(), Npc.Center.X + Npc.spriteDirection * 16, Npc.Center.Y - 2f, ShootVel.X, ShootVel.Y, ProjType, Damage, knockBack, Main.myPlayer, 0f, Npc.whoAmI);
                                Main.projectile[protmp].npcProj = true;
                                Main.projectile[protmp].noDropItem = true;
                            }
                        }


                    }
                    if (auraLightMultiplier > 0f)
                    {
                        Vector3 vector6 = Npc.GetMagicAuraColor().ToVector3() * auraLightMultiplier;
                        Lighting.AddLight(Npc.Center, vector6.X, vector6.Y, vector6.Z);
                    }
                    if (Npc.ai[1] <= 0f)
                    {
                        Npc.ai[0] = (Npc.localAI[2] == 8f && HasTarget) ? 8 : 0;
                        Npc.ai[1] = AttackCooldown + Main.rand.Next(RandomAttackCooldown);
                        Npc.ai[2] = 0f;
                        Npc.localAI[1] = Npc.localAI[3] = AttackCooldown / 2 + Main.rand.Next(RandomAttackCooldown);
                        Npc.netUpdate = true;
                    }
                }
                else if (Npc.ai[0] == 15f)
                {
                    int AttackCooldown = 0;
                    int RandomAttackCooldown = 0;
                    if (NPCStats.GetModifiedAttackTime(Npc) == Npc.ai[1])
                    {
                        Npc.frameCounter = 0.0;
                        Npc.localAI[3] = 0f;
                    }
                    int Damage = 0;
                    float knockBack = 0f;
                    int itemWidth = 0;
                    int itemHeight = 0;

                    /*         意义不明
                    if (ShootDir == 1)
                    {
                        int num131 = Npc.spriteDirection;
                    }
                    if (ShootDir == -1)
                    {
                        int num132 = Npc.spriteDirection;
                    }
                    */

                    if (Npc.type == NPCID.DyeTrader)
                    {
                        Damage = 11;
                        itemHeight = itemWidth = 32;
                        AttackCooldown = 12;
                        RandomAttackCooldown = 6;
                        knockBack = 4.25f;
                    }
                    else if (Npc.type == NPCID.TaxCollector)
                    {
                        Damage = 9;
                        itemHeight = itemWidth = 28;
                        AttackCooldown = 9;
                        RandomAttackCooldown = 3;
                        knockBack = 3.5f;
                        if (Npc.GivenName == "Andrew")
                        {
                            Damage *= 2;
                            knockBack *= 2f;
                        }
                    }
                    else if (Npc.type == NPCID.Stylist)
                    {
                        Damage = 10;
                        itemHeight = itemWidth = 32;
                        AttackCooldown = 15;
                        RandomAttackCooldown = 8;
                        knockBack = 5f;
                    }
                    else if (NPCID.Sets.IsTownPet[Npc.type])
                    {
                        Damage = 10;
                        itemHeight = itemWidth = 32;
                        AttackCooldown = 15;
                        RandomAttackCooldown = 8;
                        knockBack = 3f;
                    }
                    NPCLoader.TownNPCAttackStrength(Npc, ref Damage, ref knockBack);
                    NPCLoader.TownNPCAttackCooldown(Npc, ref AttackCooldown, ref RandomAttackCooldown);
                    NPCLoader.TownNPCAttackSwing(Npc, ref itemWidth, ref itemHeight);
                    if (Main.expertMode)
                    {
                        Damage = (int)(Damage * Main.GameModeInfo.TownNPCDamageMultiplier);
                    }
                    Damage = (int)(Damage * damageMult);
                    Npc.velocity.X *= 0.8f;
                    Npc.ai[1] -= 1f;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Tuple<Vector2, float> swingStats = Npc.GetSwingStats(NPCStats.GetModifiedAttackTime(Npc) * 2, (int)Npc.ai[1], Npc.spriteDirection, itemWidth, itemHeight);
                        Rectangle MeleeHitbox = new((int)swingStats.Item1.X, (int)swingStats.Item1.Y, itemWidth, itemHeight);
                        if (Npc.spriteDirection == -1)
                        {
                            MeleeHitbox.X -= itemWidth;
                        }
                        MeleeHitbox.Y -= itemHeight;
                        Npc.TweakSwingStats(NPCStats.GetModifiedAttackTime(Npc) * 2, (int)Npc.ai[1], Npc.spriteDirection, ref MeleeHitbox);

                        if (!ArmedGNPC.GetWeapon(Npc).IsAir)
                        {
                            foreach (NPC target in Main.npc)
                            {
                                if (target.active && target.immune[Npc.whoAmI] == 0 && !target.dontTakeDamage && !target.friendly && MeleeHitbox.Intersects(target.Hitbox) && (target.noTileCollide || Collision.CanHit(Npc.position, Npc.width, Npc.height, target.position, target.width, target.height)))
                                {
                                    int CritChance = ArmedGNPC.GetWeapon(Npc).crit;

                                    NPCStats.ModifyCritChance(Npc, ref CritChance);

                                    bool crit = Main.rand.Next(100) <= CritChance;

                                    NPC.HitModifiers modifiers = target.GetIncomingStrikeModifiers(DamageClass.Melee, Npc.spriteDirection, false);

                                    if (ArmedGNPC.GetWeapon(Npc).ModItem != null)
                                    {
                                        ArmedGNPC.GetWeapon(Npc).ModItem.ModifyHitNPC(Main.LocalPlayer, target, ref modifiers);
                                    }
                                    NPC.HitInfo strike = modifiers.ToHitInfo(Damage, crit, knockBack, true, Main.LocalPlayer.luck);
                                    int DmgDone = target.StrikeNPC(strike, false, true);
                                    //target.StrikeNPCNoInteraction(Damage, knockBack, Npc.spriteDirection, crit, false, false);
                                    if (Main.netMode != NetmodeID.SinglePlayer)
                                    {
                                        NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, itemHeight, Damage, knockBack, Npc.spriteDirection, 0, 0, 0);
                                    }
                                    target.netUpdate = true;
                                    target.immune[Npc.whoAmI] = (int)Npc.ai[1] + 2;

                                    if (ArmedGNPC.GetWeapon(Npc).ModItem != null)
                                    {
                                        ArmedGNPC.GetWeapon(Npc).ModItem.OnHitNPC(Main.LocalPlayer, target, strike, DmgDone);
                                    }
                                    else
                                    {
                                        switch (ArmedGNPC.GetWeapon(Npc).type)
                                        {
                                            //给近战命中武器加钩子
                                            case ItemID.TheHorsemansBlade:
                                                MeleeWeaponFix.PumpkinSword(target.whoAmI, Damage, knockBack);
                                                break;
                                            case ItemID.BeeKeeper:
                                                MeleeWeaponFix.BeeKeeperHit(target, MeleeHitbox, Damage);
                                                break;
                                            case ItemID.Bladetongue:
                                                MeleeWeaponFix.Bladetongue(Npc, MeleeHitbox, Damage, knockBack);
                                                break;
                                            case ItemID.TentacleSpike:
                                                MeleeWeaponFix.TentacleSpike_TrySpiking(Npc, target, Damage, knockBack);
                                                break;
                                            case ItemID.BatBat:
                                                if (Npc.life < Npc.lifeMax)
                                                {
                                                    Npc.life++;
                                                    Npc.HealEffect(1);
                                                }
                                                break;
                                            case ItemID.BladeofGrass:
                                                if (Main.rand.NextBool(4))
                                                {
                                                    target.AddBuff(BuffID.Poisoned, 7 * 60);
                                                }
                                                break;
                                            case 121:   //熔岩剑
                                                if (Main.rand.NextBool(2))
                                                {
                                                    target.AddBuff(BuffID.OnFire, 3 * 60);
                                                }
                                                break;
                                            case ItemID.DD2SquireDemonSword:
                                                if (Main.rand.NextBool(4))
                                                {
                                                    target.AddBuff(BuffID.OnFire3, 5 * 60);
                                                }
                                                break;
                                        }
                                    }

                                    if (!ArmedGNPC.GetAltWeapon(Npc).IsAir)
                                    {
                                        NPCUtils.FlaskToDebuff(target, ArmedGNPC.GetAltWeapon(Npc).buffType);
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (NPC target in Main.npc)
                            {
                                if (target.active && target.immune[Main.myPlayer] == 0 && !target.dontTakeDamage && !target.friendly && target.damage > 0 && MeleeHitbox.Intersects(target.Hitbox) && (target.noTileCollide || Collision.CanHit(Npc.position, Npc.width, Npc.height, target.position, target.width, target.height)))
                                {
                                    int CritChance = 4;
                                    if (Npc.HasBuff(115))           //暴怒药水
                                    {
                                        CritChance += 10;
                                    }
                                    bool crit = Main.rand.Next(100) <= CritChance;

                                    target.SimpleStrikeNPC(Damage, Npc.spriteDirection, crit, knockBack, null, false, 0, true);
                                    if (Main.netMode != NetmodeID.SinglePlayer)
                                    {
                                        NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, target.whoAmI, Damage, knockBack, Npc.spriteDirection, 0, 0, 0);
                                    }
                                    target.netUpdate = true;
                                    target.immune[Main.myPlayer] = (int)Npc.ai[1] + 2;

                                    if (!ArmedGNPC.GetAltWeapon(Npc).IsAir)
                                    {
                                        NPCUtils.FlaskToDebuff(target, ArmedGNPC.GetAltWeapon(Npc).buffType);
                                    }
                                }
                            }
                        }

                    }
                    if (Npc.ai[1] <= 0f)   //攻击完了后
                    {
                        bool StopAtk = false;
                        if (HasTarget)
                        {
                            if (!Collision.CanHit(Npc.Center, 0, 0, Npc.Center + Vector2.UnitX * -ShootDir * 32f, 0, 0) || Npc.localAI[2] == 8f)
                            {
                                StopAtk = true;
                            }
                            if (StopAtk)
                            {
                                int AttackTime = NPCStats.GetModifiedAttackTime(Npc);
                                int AttackTarget = (ShootDir == 1) ? TargetRight : TargetLeft;
                                int AlterAttackTarget = (ShootDir == 1) ? TargetLeft : TargetRight;
                                if (AttackTarget != -1 && !Collision.CanHit(Npc.Center, 0, 0, Main.npc[AttackTarget].Center, 0, 0))
                                {
                                    AttackTarget = (AlterAttackTarget == -1 || !Collision.CanHit(Npc.Center, 0, 0, Main.npc[AlterAttackTarget].Center, 0, 0)) ? -1 : AlterAttackTarget;
                                }
                                if (AttackTarget != -1)
                                {

                                    if (Npc.GetGlobalNPC<ArmedGNPC>().actMode == ArmedGNPC.ActMode.Attack)
                                    {
                                        bool HasEnemyOnRoad = false;
                                        Rectangle MeleeHitbox;
                                        if (Npc.direction <= 0)
                                        {
                                            MeleeHitbox = new Rectangle((int)Npc.position.X - 30, (int)Npc.position.Y, Npc.width + 30, Npc.height);
                                        }
                                        else
                                        {
                                            MeleeHitbox = new Rectangle((int)Npc.position.X, (int)Npc.position.Y, Npc.width + 30, Npc.height);
                                        }
                                        foreach (NPC target in Main.npc)
                                        {
                                            if (target.active && !target.dontTakeDamage && (target.damage > 0 || target.lifeMax > 5) && !target.friendly && MeleeHitbox.Intersects(target.Hitbox) && (target.noTileCollide || Collision.CanHit(Npc.position, Npc.width, Npc.height, target.position, target.width, target.height)))
                                            {
                                                HasEnemyOnRoad = true;
                                                break;
                                            }
                                        }
                                        if (HasEnemyOnRoad || Math.Abs(Main.npc[AttackTarget].Center.X - Npc.Center.X) < 30)
                                        {
                                            Npc.ai[0] = MeleeAtk;
                                            Npc.ai[1] = NPCStats.GetModifiedAttackTime(Npc);
                                            Npc.ai[2] = 0f;
                                            Npc.localAI[3] = 0f;
                                            Npc.direction = (Npc.position.X < Main.npc[AttackTarget].position.X) ? 1 : -1;
                                            Npc.netUpdate = true;
                                        }
                                        else
                                        {
                                            StopAtk = false;
                                        }
                                    }
                                    else
                                    {
                                        Npc.ai[0] = 15f;         //再次近战
                                        Npc.ai[1] = AttackTime;
                                        Npc.ai[2] = 0f;
                                        Npc.localAI[3] = 0f;
                                        Npc.direction = (Npc.position.X < Main.npc[AttackTarget].position.X) ? 1 : -1;
                                        Npc.netUpdate = true;
                                    }
                                }
                                else
                                {
                                    StopAtk = false;
                                }
                            }
                        }
                        if (!StopAtk)
                        {
                            Npc.ai[0] = (Npc.localAI[2] == 8f && HasTarget) ? 8 : 0;
                            Npc.ai[1] = AttackCooldown + Main.rand.Next(RandomAttackCooldown);
                            Npc.ai[2] = 0f;
                            Npc.localAI[1] = Npc.localAI[3] = AttackCooldown / 2 + Main.rand.Next(RandomAttackCooldown);
                            Npc.netUpdate = true;
                        }
                    }
                }
                else if (Npc.ai[0] == 24f)             //???
                {
                    Npc.velocity.X *= 0.8f;
                    Npc.ai[1] -= 1f;
                    Npc.localAI[3] += 1f;
                    Npc.direction = 1;
                    Npc.spriteDirection = 1;
                    Vector3 vector7 = Npc.GetMagicAuraColor().ToVector3();
                    Lighting.AddLight(Npc.Center, vector7.X, vector7.Y, vector7.Z);
                    if (Npc.ai[1] <= 0f)
                    {
                        Npc.ai[0] = 0f;
                        Npc.ai[1] = 480f;
                        Npc.ai[2] = 0f;
                        Npc.localAI[1] = 480f;
                        Npc.netUpdate = true;
                    }
                }
                if (IsSlimePet && Npc.wet)
                {
                    int num84 = (int)(Npc.Center.X / 16f);
                    int num85 = 5;
                    if (Npc.collideX || (num84 < num85 && Npc.direction == -1) || (num84 > Main.maxTilesX - num85 && Npc.direction == 1))
                    {
                        Npc.direction *= -1;
                        Npc.velocity.X *= -0.25f;
                        Npc.netUpdate = true;
                    }
                    Npc.velocity.Y *= 0.9f;
                    Npc.velocity.Y -= 0.5f;
                    if (Npc.velocity.Y < -15f)
                    {
                        Npc.velocity.Y = -15f;
                    }
                }

                #region 水生动物相关
                if (IsWaterAnimal2 && Npc.wet)
                {
                    if (IsFrog)
                    {
                        Npc.ai[1] = 50f;
                    }
                    int num86 = (int)(Npc.Center.X / 16f);
                    int num87 = 5;
                    if (Npc.collideX || (num86 < num87 && Npc.direction == -1) || (num86 > Main.maxTilesX - num87 && Npc.direction == 1))
                    {
                        Npc.direction *= -1;
                        Npc.velocity.X *= -0.25f;
                        Npc.netUpdate = true;
                    }
                    if (Collision.GetWaterLine(Npc.Center.ToTileCoordinates(), out float waterLineHeight))
                    {
                        float num88 = Npc.Center.Y + 1f;
                        if (Npc.Center.Y > waterLineHeight)
                        {
                            Npc.velocity.Y -= 0.8f;
                            if (Npc.velocity.Y < -4f)
                            {
                                Npc.velocity.Y = -4f;
                            }
                            if (num88 + Npc.velocity.Y < waterLineHeight)
                            {
                                Npc.velocity.Y = waterLineHeight - num88;
                            }
                        }
                        else
                        {
                            Npc.velocity.Y = MathHelper.Min(Npc.velocity.Y, waterLineHeight - num88);
                        }
                    }
                    else
                    {
                        Npc.velocity.Y -= 0.2f;
                    }
                }
                #endregion

                if (Main.netMode == NetmodeID.MultiplayerClient || !Npc.isLikeATownNPC || IsTalking)       //说话时停止战斗AI
                {
                    return;
                }

                bool ActNoDanger = Npc.ai[0] < Stop && !HasTarget && !Npc.wet && !Npc.GetGlobalNPC<ArmedGNPC>().AlertMode;
                bool ReadyToFight = (Npc.ai[0] < 2f || Npc.ai[0] == 8f) && (HasTarget || HasTarget2);
                if (Npc.localAI[1] > 0f)
                {
                    Npc.localAI[1] -= 1f;
                }
                if (Npc.localAI[1] > 0f)
                {
                    ReadyToFight = false;
                }
                if (ReadyToFight && Npc.type == NPCID.Mechanic && Npc.localAI[0] == 1f)
                {
                    ReadyToFight = false;
                }
                if (ReadyToFight && Npc.type == NPCID.Dryad)
                {
                    ReadyToFight = false;
                    for (int num89 = 0; num89 < 200; num89++)
                    {
                        NPC nPC3 = Main.npc[num89];
                        if (nPC3.active && nPC3.townNPC && Npc.Distance(nPC3.Center) <= 1200f) //&& nPC3.FindBuffIndex(165) == -1)
                        {
                            ReadyToFight = true;
                            break;
                        }
                    }
                }
                if (Npc.CanTalk && ActNoDanger && Npc.ai[0] == 0f && Npc.velocity.Y == 0f && Main.rand.NextBool(300))          //npc之间互相谈话
                {
                    int TalkTime = 420;
                    TalkTime = (!Main.rand.NextBool(2)) ? (TalkTime * Main.rand.Next(1, 3)) : (TalkTime * Main.rand.Next(1, 4));
                    int TalkRange = 100;
                    int TalkMinRange = 20;
                    for (int NPCToTalk = 0; NPCToTalk < 200; NPCToTalk++)
                    {
                        NPC nPC4 = Main.npc[NPCToTalk];
                        bool CannotTalk = (nPC4.ai[0] == 1f && nPC4.closeDoor) || (nPC4.ai[0] == 1f && nPC4.ai[1] > 200f) || nPC4.ai[0] > 1f || nPC4.wet;
                        if (nPC4 != Npc && nPC4.active && nPC4.CanBeTalkedTo && !CannotTalk && nPC4.Distance(Npc.Center) < TalkRange && nPC4.Distance(Npc.Center) > TalkMinRange && Collision.CanHit(Npc.Center, 0, 0, nPC4.Center, 0, 0))
                        {
                            int num94 = (Npc.position.X < nPC4.position.X).ToDirectionInt();
                            Npc.ai[0] = 3f;
                            Npc.ai[1] = TalkTime;
                            Npc.ai[2] = NPCToTalk;
                            Npc.direction = num94;
                            Npc.netUpdate = true;
                            nPC4.ai[0] = 4f;
                            nPC4.ai[1] = TalkTime;
                            nPC4.ai[2] = Npc.whoAmI;
                            nPC4.direction = -num94;
                            nPC4.netUpdate = true;
                            break;
                        }
                    }
                }
                else if (Npc.CanTalk && ActNoDanger && Npc.ai[0] == 0f && Npc.velocity.Y == 0f && Main.rand.NextBool(1800))          //npc之间互相谈话
                {
                    int TalkTime = 420;
                    TalkTime = (!Main.rand.NextBool(2)) ? (TalkTime * Main.rand.Next(1, 3)) : (TalkTime * Main.rand.Next(1, 4));
                    int TalkRange = 100;
                    int TalkMinRange = 20;
                    for (int NPCToTalk = 0; NPCToTalk < 200; NPCToTalk++)
                    {
                        NPC nPC5 = Main.npc[NPCToTalk];
                        bool CannotTalk = (nPC5.ai[0] == 1f && nPC5.closeDoor) || (nPC5.ai[0] == 1f && nPC5.ai[1] > 200f) || nPC5.ai[0] > 1f || nPC5.wet;
                        if (nPC5 != Npc && nPC5.active && nPC5.CanBeTalkedTo && !NPCID.Sets.IsTownPet[nPC5.type] && !CannotTalk && nPC5.Distance(Npc.Center) < TalkRange && nPC5.Distance(Npc.Center) > TalkMinRange && Collision.CanHit(Npc.Center, 0, 0, nPC5.Center, 0, 0))
                        {
                            int num99 = (Npc.position.X < nPC5.position.X).ToDirectionInt();
                            Npc.ai[0] = 16f;
                            Npc.ai[1] = TalkTime;
                            Npc.ai[2] = NPCToTalk;
                            Npc.localAI[2] = Main.rand.Next(4);
                            Npc.localAI[3] = Main.rand.Next(3 - (int)Npc.localAI[2]);
                            Npc.direction = num99;
                            Npc.netUpdate = true;
                            nPC5.ai[0] = 17f;
                            nPC5.ai[1] = TalkTime;
                            nPC5.ai[2] = Npc.whoAmI;
                            nPC5.localAI[2] = 0f;
                            nPC5.localAI[3] = 0f;
                            nPC5.direction = -num99;
                            nPC5.netUpdate = true;
                            break;
                        }
                    }
                }
                else if (!NPCID.Sets.IsTownPet[Npc.type] && ActNoDanger && Npc.ai[0] == 0f && Npc.velocity.Y == 0f && Main.rand.NextBool(1200) && (Npc.type == NPCID.PartyGirl || (BirthdayParty.PartyIsUp && NPCID.Sets.AttackType[Npc.type] == NPCID.Sets.AttackType[NPCID.PartyGirl])))
                {
                    int num100 = 300;
                    int num101 = 150;
                    for (int num102 = 0; num102 < 255; num102++)
                    {
                        Player player = Main.player[num102];
                        if (player.active && !player.dead && player.Distance(Npc.Center) < num101 && Collision.CanHitLine(Npc.Top, 0, 0, player.Top, 0, 0))
                        {
                            int num103 = (Npc.position.X < player.position.X).ToDirectionInt();
                            Npc.ai[0] = 6f;
                            Npc.ai[1] = num100;
                            Npc.ai[2] = num102;
                            Npc.direction = num103;
                            Npc.netUpdate = true;
                            break;
                        }
                    }
                }
                else if (ActNoDanger && Npc.ai[0] == 0f && Npc.velocity.Y == 0f && Main.rand.NextBool(600) && Npc.type == NPCID.DD2Bartender)
                {
                    int num104 = 300;
                    int num105 = 150;
                    for (int num106 = 0; num106 < 255; num106++)
                    {
                        Player player2 = Main.player[num106];
                        if (player2.active && !player2.dead && player2.Distance(Npc.Center) < num105 && Collision.CanHitLine(Npc.Top, 0, 0, player2.Top, 0, 0))
                        {
                            int num107 = (Npc.position.X < player2.position.X).ToDirectionInt();
                            Npc.ai[0] = 18f;
                            Npc.ai[1] = num104;
                            Npc.ai[2] = num106;
                            Npc.direction = num107;
                            Npc.netUpdate = true;
                            break;
                        }
                    }
                }
                else if (!NPCID.Sets.IsTownPet[Npc.type] && ActNoDanger && Npc.ai[0] == 0f && Npc.velocity.Y == 0f && Main.rand.NextBool(1800))
                {
                    Npc.ai[0] = 2f;
                    Npc.ai[1] = 45 * Main.rand.Next(1, 2);
                    Npc.netUpdate = true;
                }
                else if (ActNoDanger && Npc.ai[0] == 0f && Npc.velocity.Y == 0f && Main.rand.NextBool(600) && Npc.type == NPCID.Pirate && !HasTarget2)
                {
                    Npc.ai[0] = 11f;
                    Npc.ai[1] = 30 * Main.rand.Next(1, 4);
                    Npc.netUpdate = true;
                }
                else if (ActNoDanger && Npc.ai[0] == 0f && Npc.velocity.Y == 0f && Main.rand.NextBool(1200))
                {
                    int TalkTime = 220;
                    int TalkRange = 150;
                    for (int num110 = 0; num110 < 255; num110++)
                    {
                        Player player3 = Main.player[num110];
                        if (player3.CanBeTalkedTo && player3.Distance(Npc.Center) < TalkRange && Collision.CanHitLine(Npc.Top, 0, 0, player3.Top, 0, 0))
                        {
                            int num111 = (Npc.position.X < player3.position.X).ToDirectionInt();
                            Npc.ai[0] = 7f;
                            Npc.ai[1] = TalkTime;
                            Npc.ai[2] = num110;
                            Npc.direction = num111;
                            Npc.netUpdate = true;
                            break;
                        }
                    }
                }
                else if (ActNoDanger && Npc.ai[0] == 1f && Npc.velocity.Y == 0f && SitDownChance > 0 && Main.rand.NextBool(SitDownChance))
                {
                    Point point = (Npc.Bottom + Vector2.UnitY * -2f).ToTileCoordinates();
                    bool CanSitDown = WorldGen.InWorld(point.X, point.Y, 1);
                    if (CanSitDown)     //避开玩家和其他NPC座位
                    {
                        for (int num112 = 0; num112 < 200; num112++)
                        {
                            if (Main.npc[num112].active && Main.npc[num112].aiStyle == 7 && Main.npc[num112].townNPC && Main.npc[num112].ai[0] == 5f && (Main.npc[num112].Bottom + Vector2.UnitY * -2f).ToTileCoordinates() == point)
                            {
                                CanSitDown = false;
                                break;
                            }
                        }
                        for (int num113 = 0; num113 < 255; num113++)
                        {
                            if (Main.player[num113].active && Main.player[num113].sitting.isSitting && Main.player[num113].Center.ToTileCoordinates() == point)
                            {
                                CanSitDown = false;
                                break;
                            }
                        }
                    }
                    if (CanSitDown)
                    {
                        Tile tile2 = Main.tile[point.X, point.Y];
                        CanSitDown = TileID.Sets.CanBeSatOnForNPCs[tile2.TileType];
                        if (CanSitDown && tile2.TileType == 15 && tile2.TileFrameY >= 1080 && tile2.TileFrameY <= 1098)
                        {
                            CanSitDown = false;
                        }
                        if (CanSitDown)
                        {
                            Npc.ai[0] = 5f;
                            Npc.ai[1] = 900 + Main.rand.Next(10800);
                            Npc.SitDown(point, out int targetDirection, out Vector2 bottom);
                            Npc.direction = targetDirection;
                            Npc.Bottom = bottom;
                            Npc.velocity = Vector2.Zero;
                            Npc.localAI[3] = 0f;
                            Npc.netUpdate = true;
                        }
                    }
                }
                else if (ActNoDanger && Npc.ai[0] == 1f && Npc.velocity.Y == 0f && Main.rand.NextBool(600) && Utils.PlotTileLine(Npc.Top, Npc.Bottom, Npc.width, new Utils.TileActionAttempt(DelegateMethods.SearchAvoidedByNPCs)))
                {
                    Point point2 = (Npc.Center + new Vector2(Npc.direction * 10, 0f)).ToTileCoordinates();
                    bool ThisTileCanGo = WorldGen.InWorld(point2.X, point2.Y, 1);
                    if (ThisTileCanGo)
                    {
                        Tile tileSafely7 = Framing.GetTileSafely(point2.X, point2.Y);
                        if (!tileSafely7.HasUnactuatedTile || !TileID.Sets.InteractibleByNPCs[tileSafely7.TileType])
                        {
                            ThisTileCanGo = false;
                        }
                    }
                    if (ThisTileCanGo)
                    {
                        Npc.ai[0] = 9f;
                        Npc.ai[1] = 40 + Main.rand.Next(90);
                        Npc.velocity = Vector2.Zero;
                        Npc.localAI[3] = 0f;
                        Npc.netUpdate = true;
                    }
                }

                if (Main.netMode != NetmodeID.MultiplayerClient && Npc.ai[0] < 2f && Npc.velocity.Y == 0f && Npc.type == NPCID.Nurse && Npc.breath > 0)
                {
                    //护士治疗其他NPC
                    int HealTarget = -1;
                    for (int num115 = 0; num115 < 200; num115++)
                    {
                        NPC nPC6 = Main.npc[num115];
                        if (nPC6.active && nPC6.townNPC && nPC6.life != nPC6.lifeMax && (HealTarget == -1 || nPC6.lifeMax - nPC6.life > Main.npc[HealTarget].lifeMax - Main.npc[HealTarget].life) && Collision.CanHitLine(Npc.position, Npc.width, Npc.height, nPC6.position, nPC6.width, nPC6.height) && Npc.Distance(nPC6.Center) < 500f)
                        {
                            HealTarget = num115;          //治疗损失生命最多的人
                        }
                    }

                    if (HealTarget == -1 && !ArmedGNPC.GetAltWeapon(Npc).IsAir)   //没有损血就治疗没buff的
                    {
                        for (int num106 = 0; num106 < 200; num106++)
                        {
                            NPC npc6 = Main.npc[num106];
                            if (npc6.active && npc6.townNPC && Collision.CanHitLine(Npc.position, Npc.width, Npc.height, npc6.position, npc6.width, npc6.height) && Npc.Distance(npc6.Center) < 500f)
                            {
                                if (!npc6.HasBuff(ArmedGNPC.GetAltWeapon(Npc).buffType) && !npc6.buffImmune[ArmedGNPC.GetAltWeapon(Npc).buffType])
                                {
                                    if (HealTarget == -1 || npc6.Distance(Npc.Center) < Main.npc[HealTarget].Distance(Npc.Center))
                                    {
                                        HealTarget = num106;
                                    }
                                }
                            }
                        }
                    }

                    if (Npc.GetGlobalNPC<ArmedGNPC>().actMode == ArmedGNPC.ActMode.Move)      //行军下不治疗
                    {
                        if (Math.Abs(Npc.Center.X - Main.npc[Npc.GetGlobalNPC<ArmedGNPC>().ActTargetNPC].Center.X) > 50)
                        {
                            HealTarget = -1;
                        }
                    }

                    if (HealTarget != -1)
                    {
                        Npc.ai[0] = 13f;
                        Npc.ai[1] = NPCStats.GetModifiedAttackTime(Npc);         //34
                        Npc.ai[2] = HealTarget;
                        Npc.localAI[3] = 0f;
                        Npc.direction = (Npc.position.X < Main.npc[HealTarget].position.X) ? 1 : -1;
                        Npc.netUpdate = true;
                    }
                }

                if (ReadyToFight && Npc.velocity.Y == 0f && NPCID.Sets.AttackType[Npc.type] == 0 && NPCChanceToAttack(Npc))
                {               //投掷攻击
                    int attackTime = NPCStats.GetModifiedAttackTime(Npc);
                    int AttackTarget = (ShootDir == 1) ? TargetRight : TargetLeft;
                    int AlterAttackTarget = (ShootDir == 1) ? TargetLeft : TargetRight;
                    if (AttackTarget != -1 && !Collision.CanHit(Npc.Center, 0, 0, Main.npc[AttackTarget].Center, 0, 0))
                    {
                        AttackTarget = (AlterAttackTarget == -1 || !Collision.CanHit(Npc.Center, 0, 0, Main.npc[AlterAttackTarget].Center, 0, 0)) ? -1 : AlterAttackTarget;
                    }
                    bool haveTarget = AttackTarget != -1;

                    if (haveTarget && Npc.type == NPCID.BestiaryGirl)
                    {
                        if (ArmedGNPC.GetWeapon(Npc).IsAir)        //携带武器时忽视距离限制
                        {
                            haveTarget = Vector2.Distance(Npc.Center, Main.npc[AttackTarget].Center) <= 50f;
                        }
                    }

                    if (haveTarget)
                    {
                        Npc.localAI[2] = Npc.ai[0];
                        Npc.ai[0] = 10f;
                        Npc.ai[1] = attackTime;
                        Npc.ai[2] = 0f;
                        Npc.localAI[3] = 0f;
                        Npc.direction = (Npc.position.X < Main.npc[AttackTarget].position.X) ? 1 : -1;
                        Npc.netUpdate = true;
                    }
                }
                else if (ReadyToFight && Npc.velocity.Y == 0f && NPCID.Sets.AttackType[Npc.type] == 1 && NPCChanceToAttack(Npc))
                {               //远程攻击
                    int attackTime = NPCStats.GetModifiedAttackTime(Npc);
                    int AttackTarget = (ShootDir == 1) ? TargetRight : TargetLeft;
                    int AlterAttackTarget = (ShootDir == 1) ? TargetLeft : TargetRight;
                    if (AttackTarget != -1 && !Collision.CanHitLine(Npc.Center, 0, 0, Main.npc[AttackTarget].Center, 0, 0))
                    {
                        AttackTarget = (AlterAttackTarget == -1 || !Collision.CanHitLine(Npc.Center, 0, 0, Main.npc[AlterAttackTarget].Center, 0, 0)) ? -1 : AlterAttackTarget;
                    }
                    if (AttackTarget != -1)
                    {
                        Vector2 vector8 = Npc.DirectionTo(Main.npc[AttackTarget].Center);
                        //if (vector8.Y <= 0.5f && vector8.Y >= -0.5f)         解除远程NPC角度限制
                        {
                            Npc.localAI[2] = Npc.ai[0];
                            Npc.ai[0] = 12f;
                            Npc.ai[1] = attackTime;
                            Npc.ai[2] = vector8.Y;
                            Npc.localAI[3] = 0f;
                            Npc.direction = (Npc.position.X < Main.npc[AttackTarget].position.X) ? 1 : -1;
                            Npc.netUpdate = true;
                        }
                    }
                }
                if (ReadyToFight && Npc.velocity.Y == 0f && NPCID.Sets.AttackType[Npc.type] == 2 && NPCChanceToAttack(Npc))
                {               //魔法攻击
                    int attackTime = NPCStats.GetModifiedAttackTime(Npc);
                    int AttackTarget = (ShootDir == 1) ? TargetRight : TargetLeft;
                    int AlterAttackTarget = (ShootDir == 1) ? TargetLeft : TargetRight;
                    if (AttackTarget != -1 && !Collision.CanHitLine(Npc.Center, 0, 0, Main.npc[AttackTarget].Center, 0, 0))
                    {
                        AttackTarget = (AlterAttackTarget == -1 || !Collision.CanHitLine(Npc.Center, 0, 0, Main.npc[AlterAttackTarget].Center, 0, 0)) ? -1 : AlterAttackTarget;
                    }
                    if (AttackTarget != -1)
                    {
                        Npc.localAI[2] = Npc.ai[0];
                        Npc.ai[0] = 14f;
                        Npc.ai[1] = attackTime;
                        Npc.ai[2] = 0f;
                        Npc.localAI[3] = 0f;
                        Npc.direction = (Npc.position.X < Main.npc[AttackTarget].position.X) ? 1 : -1;
                        Npc.netUpdate = true;
                    }
                    else if (Npc.type == NPCID.Dryad)
                    {
                        Npc.localAI[2] = Npc.ai[0];
                        Npc.ai[0] = 14f;
                        Npc.ai[1] = attackTime;
                        Npc.ai[2] = 0f;
                        Npc.localAI[3] = 0f;
                        Npc.netUpdate = true;
                    }
                }
                if (ReadyToFight && Npc.velocity.Y == 0f && NPCID.Sets.AttackType[Npc.type] == 3 && NPCChanceToAttack(Npc))
                {               //近战攻击
                    int attackTime = NPCStats.GetModifiedAttackTime(Npc);
                    int AttackTarget = (ShootDir == 1) ? TargetRight : TargetLeft;
                    int AlterAttackTarget = (ShootDir == 1) ? TargetLeft : TargetRight;
                    if (AttackTarget != -1 && !Collision.CanHit(Npc.Center, 0, 0, Main.npc[AttackTarget].Center, 0, 0))
                    {
                        AttackTarget = (AlterAttackTarget == -1 || !Collision.CanHit(Npc.Center, 0, 0, Main.npc[AlterAttackTarget].Center, 0, 0)) ? -1 : AlterAttackTarget;
                    }
                    if (AttackTarget != -1)
                    {
                        if (Npc.GetGlobalNPC<ArmedGNPC>().actMode == ArmedGNPC.ActMode.Attack)
                        {
                            bool HasEnemyOnRoad = false;
                            Rectangle MeleeHitbox;
                            if (Npc.direction <= 0)
                            {
                                MeleeHitbox = new Rectangle((int)Npc.position.X - 30, (int)Npc.position.Y, Npc.width + 30, Npc.height);
                            }
                            else
                            {
                                MeleeHitbox = new Rectangle((int)Npc.position.X, (int)Npc.position.Y, Npc.width + 30, Npc.height);
                            }
                            foreach (NPC target in Main.npc)
                            {
                                if (target.active && !target.dontTakeDamage && (target.damage > 0 || target.lifeMax > 5) && !target.friendly && MeleeHitbox.Intersects(target.Hitbox) && (target.noTileCollide || Collision.CanHit(Npc.position, Npc.width, Npc.height, target.position, target.width, target.height)))
                                {
                                    HasEnemyOnRoad = true;
                                    break;
                                }
                            }
                            if (HasEnemyOnRoad || Math.Abs(Main.npc[AttackTarget].Center.X - Npc.Center.X) < 30)
                            {
                                Npc.localAI[2] = Npc.ai[0];
                                Npc.ai[0] = 15f;
                                Npc.ai[1] = attackTime;
                                Npc.ai[2] = 0f;
                                Npc.localAI[3] = 0f;
                                Npc.direction = (Npc.position.X < Main.npc[AttackTarget].position.X) ? 1 : -1;
                                Npc.netUpdate = true;
                            }
                        }
                        else
                        {

                            Npc.localAI[2] = Npc.ai[0];
                            Npc.ai[0] = 15f;
                            Npc.ai[1] = attackTime;
                            Npc.ai[2] = 0f;
                            Npc.localAI[3] = 0f;
                            Npc.direction = (Npc.position.X < Main.npc[AttackTarget].position.X) ? 1 : -1;
                            Npc.netUpdate = true;
                        }
                    }
                }

                //神奇发光
                if (Npc.type == 681)
                {
                    TorchID.TorchColor(23, out float R, out float G, out float B);
                    float num128 = 0.35f;
                    R *= num128;
                    G *= num128;
                    B *= num128;
                    Lighting.AddLight(Npc.Center, R, G, B);
                }
                if (Npc.type == 683 || Npc.type == 687)
                {
                    float num129 = Utils.WrappedLerp(0.75f, 1f, (float)Main.timeForVisualEffects % 120f / 120f);
                    Lighting.AddLight(Npc.Center, 0.25f * num129, 0.25f * num129, 0.1f * num129);
                }
                return;
            }
        }

        internal static IEntitySource GetSpawnSource_ForProjectile(this NPC npc)
        {
            return npc.GetSource_FromAI(null);
        }

        internal static bool TopSlope(this Tile tile)
        {
            byte b = (byte)tile.Slope;
            return b == 1 || b == 2;
        }

        internal static void AI_007_TownEntities_Shimmer_TeleportToLandingSpot(this NPC Npc)
        {
            Vector2? vector = Npc.AI_007_TownEntities_Shimmer_ScanForBestSpotToLandOn();
            if (vector != null)
            {
                Vector2 vector2 = Npc.position;
                Npc.position = vector.Value;
                Vector2 movementVector = Npc.position - vector2;
                int num = 560;
                if (movementVector.Length() >= num)
                {
                    Npc.ai[2] = 30f;
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ShimmerTownNPCSend, new ParticleOrchestraSettings
                    {
                        PositionInWorld = vector2 + Npc.Size / 2f,
                        MovementVector = movementVector
                    });
                }
                Npc.netUpdate = true;
            }
        }

        internal static bool AI_007_TownEntities_IsInAGoodRestingSpot(this NPC npc, int num1, int num2, int num3, int num4)
        {
            Type type = npc.GetType();
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var m = type.GetMethod("AI_007_TownEntities_IsInAGoodRestingSpot", flags);
            return (bool)m.Invoke(npc, new object[] { num1, num2, num3, num4 });
        }

        internal static bool AI_007_TownEntities_CheckIfWillDrown(this NPC npc, bool currentlyDrowning)
        {
            Type type = npc.GetType();
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var m = type.GetMethod("AI_007_TownEntities_CheckIfWillDrown", flags);
            return (bool)m.Invoke(npc, new object[] { currentlyDrowning });
        }

        internal static void AI_007_AttemptToPlayIdleAnimationsForPets(this NPC npc, int num1)
        {
            Type type = npc.GetType();
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var m = type.GetMethod("AI_007_AttemptToPlayIdleAnimationsForPets", flags);
            m.Invoke(npc, new object[] { num1 });
        }

        internal static Vector2? AI_007_TownEntities_Shimmer_ScanForBestSpotToLandOn(this NPC Npc)
        {
            Point point = Npc.Top.ToTileCoordinates();
            int num = 30;
            Vector2? result = null;
            bool flag = Npc.homeless && (Npc.homeTileX == -1 || Npc.homeTileY == -1);
            for (int i = 1; i < num; i += 2)
            {
                Vector2? vector = ShimmerHelper.FindSpotWithoutShimmer(Npc, point.X, point.Y, i, flag);
                if (vector != null)
                {
                    result = new Vector2?(vector.Value);
                    break;
                }
            }
            if (result == null && Npc.homeTileX != -1 && Npc.homeTileY != -1)
            {
                for (int j = 1; j < num; j += 2)
                {
                    Vector2? vector2 = ShimmerHelper.FindSpotWithoutShimmer(Npc, Npc.homeTileX, Npc.homeTileY, j, flag);
                    if (vector2 != null)
                    {
                        result = new Vector2?(vector2.Value);
                        break;
                    }
                }
            }
            if (result == null)
            {
                int num2 = flag ? 30 : 0;
                num = 60;
                flag = true;
                for (int k = num2; k < num; k += 2)
                {
                    Vector2? vector3 = ShimmerHelper.FindSpotWithoutShimmer(Npc, point.X, point.Y, k, flag);
                    if (vector3 != null)
                    {
                        result = new Vector2?(vector3.Value);
                        break;
                    }
                }
            }
            if (result == null && Npc.homeTileX != -1 && Npc.homeTileY != -1)
            {
                num = 60;
                flag = true;
                for (int l = 30; l < num; l += 2)
                {
                    Vector2? vector4 = ShimmerHelper.FindSpotWithoutShimmer(Npc, Npc.homeTileX, Npc.homeTileY, l, flag);
                    if (vector4 != null)
                    {
                        result = new Vector2?(vector4.Value);
                        break;
                    }
                }
            }
            return result;
        }

        internal static void AI_007_FindGoodRestingSpot(this NPC Npc, int myTileX, int myTileY, out int floorX, out int floorY)
        {
            floorX = Npc.homeTileX;
            floorY = Npc.homeTileY;
            if (floorX == -1 || floorY == -1)
            {
                return;
            }
            while (!WorldGen.SolidOrSlopedTile(floorX, floorY) && floorY < Main.maxTilesY - 20)
            {
                floorY++;
            }
            if (Main.dayTime || (Npc.ai[0] == 5f && Math.Abs(myTileX - floorX) < 7 && Math.Abs(myTileY - floorY) < 7))
            {
                return;
            }
            Point point = new(floorX, floorY);
            Point point2 = new(-1, -1);
            int num = -1;
            if (Npc.type == NPCID.TownDog || Npc.type == NPCID.TownBunny || NPCID.Sets.IsTownSlime[Npc.type] || Npc.ai[0] == 5f)
            {
                return;
            }
            int num2 = 7;
            int num3 = 6;
            int num4 = 1;
            int num5 = 1;
            int num6 = 1;
            for (int i = point.X - num2; i <= point.X + num2; i += num5)
            {
                for (int num7 = point.Y + num4; num7 >= point.Y - num3; num7 -= num6)
                {
                    Tile tile = Main.tile[i, num7];
                    if (tile != null && tile.HasTile && TileID.Sets.CanBeSatOnForNPCs[tile.TileType])
                    {
                        int num8 = Math.Abs(i - point.X) + Math.Abs(num7 - point.Y);
                        if (num == -1 || num8 < num)
                        {
                            num = num8;
                            point2.X = i;
                            point2.Y = num7;
                        }
                    }
                }
            }
            if (num == -1)
            {
                return;
            }
            Tile tile2 = Main.tile[point2.X, point2.Y];
            if (tile2.TileType == 497 || tile2.TileType == 15)
            {
                if (tile2.TileFrameY % 40 != 0)
                {
                    point2.Y--;
                }
                point2.Y += 2;
            }
            else if (tile2.TileType >= TileID.Count)
            {
                TileRestingInfo info = new(Npc, point2, Vector2.Zero, Npc.direction, 0, default);
                TileLoader.ModifySittingTargetInfo(point2.X, point2.Y, tile2.TileType, ref info);
                point2 = info.AnchorTilePosition;
                point2.Y++;
            }
            for (int j = 0; j < 200; j++)
            {
                if (Main.npc[j].active && Main.npc[j].aiStyle == 7 && Main.npc[j].townNPC && Main.npc[j].ai[0] == 5f && (Main.npc[j].Bottom + Vector2.UnitY * -2f).ToTileCoordinates() == point2)
                {
                    return;
                }
            }
            floorX = point2.X;
            floorY = point2.Y;
        }


        internal static void AI_007_TownEntities_GetWalkPrediction(this NPC Npc, int myTileX, int homeFloorX, bool canBreathUnderWater, bool currentlyDrowning, int tileX, int tileY, out bool keepwalking, out bool avoidFalling)
        {
            keepwalking = false;
            avoidFalling = true;
            bool flag = myTileX >= homeFloorX - 35 && myTileX <= homeFloorX + 35;
            if (Npc.townNPC && Npc.ai[1] < 30f)
            {
                keepwalking = !Utils.PlotTileLine(Npc.Top, Npc.Bottom, Npc.width, new Utils.TileActionAttempt(DelegateMethods.SearchAvoidedByNPCs));
                if (!keepwalking)
                {
                    Rectangle hitbox = Npc.Hitbox;
                    hitbox.X -= 20;
                    hitbox.Width += 40;
                    for (int i = 0; i < 200; i++)
                    {
                        if (Main.npc[i].active && Main.npc[i].friendly && i != Npc.whoAmI && Main.npc[i].velocity.X == 0f && hitbox.Intersects(Main.npc[i].Hitbox))
                        {
                            keepwalking = true;
                            break;
                        }
                    }
                }
            }
            if (!keepwalking && currentlyDrowning)
            {
                keepwalking = true;
            }
            if (avoidFalling && (NPCID.Sets.TownCritter[Npc.type] || (!flag && Npc.direction == Math.Sign(homeFloorX - myTileX))))
            {
                avoidFalling = false;
            }
            if (!avoidFalling)
            {
                return;
            }
            bool flag2 = false;
            Point p = default;
            int num = 0;
            for (int j = -1; j <= 4; j++)
            {
                Tile tileSafely = Framing.GetTileSafely(tileX, tileY + j);
                if (tileSafely.LiquidAmount > 0)
                {
                    num++;
                    if (tileSafely.LiquidType == 1)       //Lava
                    {
                        flag2 = true;
                        break;
                    }
                }
                if (tileSafely.HasUnactuatedTile && Main.tileSolid[tileSafely.TileType])
                {
                    if (num > 0)
                    {
                        p.X = tileX;
                        p.Y = tileY + j;
                    }
                    avoidFalling = false;
                    break;
                }
            }
            avoidFalling = avoidFalling || flag2;
            double num2 = Math.Ceiling((double)(Npc.height / 16f));
            if (num >= num2)
            {
                avoidFalling = true;
            }
            if (!avoidFalling && p.X != 0 && p.Y != 0)
            {
                Vector2 vector = p.ToWorldCoordinates(8f, 0f) + new Vector2((float)(-(float)Npc.width / 2), (float)-(float)Npc.height);
                avoidFalling = Collision.DrownCollision(vector, Npc.width, Npc.height, 1f, false);
            }
        }

        public static void AI_007_TryForcingSitting(this NPC npc, int num1, int num2)
        {
            Type type = npc.GetType();
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var m = type.GetMethod("AI_007_TryForcingSitting", flags);
            m.Invoke(npc, new object[] { num1, num2 });
        }


        public static void AI_007_TownEntities_TeleportToHome(this NPC npc, int num1, int num2)
        {
            Type type = npc.GetType();
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var m = type.GetMethod("AI_007_TownEntities_TeleportToHome", flags);
            m.Invoke(npc, new object[] { num1, num2 });
        }

        private static bool NPCChanceToAttack(NPC npc)
        {
            if (NPCUtils.BuffNPC())
            {
                if (npc.GetGlobalNPC<ArmedGNPC>().actMode == ArmedGNPC.ActMode.Attack)
                {
                    return NPCID.Sets.AttackAverageChance[npc.type] > 0;
                }
                else if (npc.GetGlobalNPC<ArmedGNPC>().AlertMode)
                {
                    return NPCID.Sets.AttackAverageChance[npc.type] > 0;
                }
                else
                {
                    return NPCID.Sets.AttackAverageChance[npc.type] > 0 && Main.rand.NextBool(NPCID.Sets.AttackAverageChance[npc.type] * 2);
                }

            }
            else
            {
                return NPCID.Sets.AttackAverageChance[npc.type] > 0 && Main.rand.NextBool(NPCID.Sets.AttackAverageChance[npc.type] * 2);
            }
        }

    }
}
