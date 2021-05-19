using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.GameContent.Events;
using System.Reflection;
using NPCAttacker.NPCs;
using NPCAttacker.Items;

namespace NPCAttacker
{
	public class NPCAttacker : Mod
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
		public const float MageAtk = 14f;
		public const float MeleeAtk = 15f;
		public const float Talking3 = 16f;
		public const float Talking4 = 17f;
		public const float TalkingBartenderToPlayer = 6f;

        public override void Load()
        {
			On.Terraria.NPC.AI_007_TownEntities += AIHook;
			On.Terraria.NPC.StrikeNPC += StrikeNPCHook;
		}

        public override void Unload()
        {
			On.Terraria.NPC.AI_007_TownEntities -= AIHook;
			On.Terraria.NPC.StrikeNPC -= StrikeNPCHook;
        }

        public static double StrikeNPCHook(On.Terraria.NPC.orig_StrikeNPC orig,NPC self,int Damage, float knockBack, int hitDirection, bool crit = false, bool noEffect = false, bool fromNet = false)
		{
			if ((!self.townNPC && self.type != NPCID.SkeletonMerchant) || NoMode())
            {
				return orig.Invoke(self, Damage, knockBack, hitDirection, crit, noEffect, fromNet);
            }

			bool flag = Main.netMode == NetmodeID.SinglePlayer;
			var ReflectTarget = typeof(NPC).GetField("ignorePlayerInteractions", BindingFlags.NonPublic | BindingFlags.Static);

			if (flag && (int)ReflectTarget.GetValue(new NPC()) > 0)
			{
				ReflectTarget.SetValue(new NPC(), (int)ReflectTarget.GetValue(new NPC()) - 1);
				flag = false;
			}
			if (!self.active || self.life <= 0)
			{
				return 0.0;
			}
			double num = Damage;
			int num2 = self.defense;
			if (self.ichor)
			{
				num2 -= 20;
			}
			if (self.betsysCurse)
			{
				num2 -= 40;
			}
			if (num2 < 0)
			{
				num2 = 0;
			}
			if (NPCLoader.StrikeNPC(self, ref num, num2, ref knockBack, hitDirection, ref crit))
			{
				num = Main.CalculateDamage((int)num, num2);
				if (crit)
				{
					num *= 2.0;
				}
				if (self.takenDamageMultiplier > 1f)
				{
					num *= self.takenDamageMultiplier;
				}
			}
			if ((self.takenDamageMultiplier > 1f || Damage != 9999) && self.lifeMax > 1)
			{
				if (self.friendly)
				{
					Color color = crit ? CombatText.DamagedFriendlyCrit : CombatText.DamagedFriendly;
					CombatText.NewText(new Rectangle((int)self.position.X, (int)self.position.Y, self.width, self.height), color, (int)num, crit, false);
				}
				else
				{
					Color color2 = crit ? CombatText.DamagedHostileCrit : CombatText.DamagedHostile;
					if (fromNet)
					{
						color2 = (crit ? CombatText.OthersDamagedHostileCrit : CombatText.OthersDamagedHostile);
					}
					CombatText.NewText(new Rectangle((int)self.position.X, (int)self.position.Y, self.width, self.height), color2, (int)num, crit, false);
				}
			}
			if (num >= 1.0)
			{
				if (flag)
				{
					self.PlayerInteraction(Main.myPlayer);
				}
				self.justHit = true;
				if (!BuffNPC())
				{
					if (self.townNPC)
					{
						bool flag2 = self.aiStyle == 7 && (self.ai[0] == 3f || self.ai[0] == 4f || self.ai[0] == 16f || self.ai[0] == 17f);
						if (flag2)
						{
							NPC npc = Main.npc[(int)self.ai[2]];
							if (npc.active)
							{
								npc.ai[0] = 1f;
								npc.ai[1] = 300 + Main.rand.Next(300);
								npc.ai[2] = 0f;
								npc.localAI[3] = 0f;
								npc.direction = hitDirection;
								npc.netUpdate = true;
							}
						}
						self.ai[0] = 1f;
						self.ai[1] = 300 + Main.rand.Next(300);
						self.ai[2] = 0f;
						self.localAI[3] = 0f;
						self.direction = hitDirection;
						self.netUpdate = true;
					}
				}

				if (self.townNPC && AttackMode())
				{
					if (NPCID.Sets.AttackType[self.type] == 3)
					{
						knockBack = 0;
					}
				}
				if (self.aiStyle == 8 && Main.netMode != NetmodeID.MultiplayerClient)
				{
					if (self.type == NPCID.RuneWizard)
					{
						self.ai[0] = 450f;
					}
					else if (self.type == NPCID.Necromancer || self.type == NPCID.NecromancerArmored)
					{
						if (Main.rand.Next(2) == 0)
						{
							self.ai[0] = 390f;
							self.netUpdate = true;
						}
					}
					else if (self.type == NPCID.DesertDjinn)
					{
						if (Main.rand.Next(3) != 0)
						{
							self.ai[0] = 181f;
							self.netUpdate = true;
						}
					}
					else
					{
						self.ai[0] = 400f;
					}
					self.TargetClosest(true);
				}
				if (self.aiStyle == 97 && Main.netMode != NetmodeID.MultiplayerClient)
				{
					self.localAI[1] = 1f;
					self.TargetClosest(true);
				}
				if (self.type == NPCID.DetonatingBubble)
				{
					num = 0.0;
					self.ai[0] = 1f;
					self.ai[1] = 4f;
					self.dontTakeDamage = true;
				}
				if (self.type == NPCID.SantaNK1 && self.life >= self.lifeMax * 0.5 && self.life - num < self.lifeMax * 0.5)
				{
					Gore.NewGore(self.position, self.velocity, 517, 1f);
				}
				if (self.type == NPCID.SpikedIceSlime)
				{
					self.localAI[0] = 60f;
				}
				if (self.type == NPCID.SlimeSpiked)
				{
					self.localAI[0] = 60f;
				}
				if (self.type == NPCID.SnowFlinx)
				{
					self.localAI[0] = 1f;
				}
				if (!self.immortal)
				{
					if (self.realLife >= 0)
					{
						Main.npc[self.realLife].life -= (int)num;
						self.life = Main.npc[self.realLife].life;
						self.lifeMax = Main.npc[self.realLife].lifeMax;
					}
					else
					{
						self.life -= (int)num;
					}
				}
				if (knockBack > 0f && self.knockBackResist > 0f)
				{
					float num3 = knockBack * self.knockBackResist;
					if (num3 > 8f)
					{
						float num4 = num3 - 8f;
						num4 *= 0.9f;
						num3 = 8f + num4;
					}
					if (num3 > 10f)
					{
						float num5 = num3 - 10f;
						num5 *= 0.8f;
						num3 = 10f + num5;
					}
					if (num3 > 12f)
					{
						float num6 = num3 - 12f;
						num6 *= 0.7f;
						num3 = 12f + num6;
					}
					if (num3 > 14f)
					{
						float num7 = num3 - 14f;
						num7 *= 0.6f;
						num3 = 14f + num7;
					}
					if (num3 > 16f)
					{
						num3 = 16f;
					}
					if (crit)
					{
						num3 *= 1.4f;
					}
					int num8 = (int)num * 10;
					if (Main.expertMode)
					{
						num8 = (int)num * 15;
					}
					if (num8 > self.lifeMax)
					{
						if (hitDirection < 0 && self.velocity.X > -num3)
						{
							if (self.velocity.X > 0f)
							{
								self.velocity.X -= num3;
							}
							self.velocity.X -= num3;
							if (self.velocity.X < -num3)
							{
								self.velocity.X = -num3;
							}
						}
						else if (hitDirection > 0 && self.velocity.X < num3)
						{
							if (self.velocity.X < 0f)
							{
								self.velocity.X += num3;
							}
							self.velocity.X += num3;
							if (self.velocity.X > num3)
							{
								self.velocity.X = num3;
							}
						}
						if (self.type == NPCID.SnowFlinx)
						{
							num3 *= 1.5f;
						}
						if (!self.noGravity)
						{
							num3 *= -0.75f;
						}
						else
						{
							num3 *= -0.5f;
						}
						if (self.velocity.Y > num3)
						{
							self.velocity.Y += num3;
							if (self.velocity.Y < num3)
							{
								self.velocity.Y = num3;
							}
						}
					}
					else
					{
						if (!self.noGravity)
						{
							self.velocity.Y = -num3 * 0.75f * self.knockBackResist;
						}
						else
						{
							self.velocity.Y = -num3 * 0.5f * self.knockBackResist;
						}
						self.velocity.X = num3 * hitDirection * self.knockBackResist;
					}
				}

				if ((self.type == NPCID.WallofFlesh || self.type == NPCID.WallofFleshEye) && self.life <= 0)
				{
					for (int i = 0; i < 200; i++)
					{
						if (Main.npc[i].active && (Main.npc[i].type == NPCID.WallofFlesh || Main.npc[i].type == NPCID.WallofFleshEye))
						{
							Main.npc[i].HitEffect(hitDirection, num);
						}
					}
				}
				else
				{
					self.HitEffect(hitDirection, num);
				}
				if (self.HitSound != null)
				{
					Main.PlaySound(self.HitSound, self.position);
				}
				if (self.realLife >= 0)
				{
					Main.npc[self.realLife].checkDead();
				}
				else
				{
					self.checkDead();
				}
				return num;
			}
			return 0.0;
		}


		public static void AIHook(On.Terraria.NPC.orig_AI_007_TownEntities orig,NPC self)
        {
			if (!NoMode() && (self.townNPC || self.type == NPCID.SkeletonMerchant))
			{
				Rectangle Screen = new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight);
				if (self.Hitbox.Intersects(Screen))
				{
					AI_007_TownEntities(self);
					return;
				}
			}
			orig.Invoke(self);
		}


		public static void AI_007_TownEntities(NPC npc)
		{
			bool ShouldGoHome = Main.raining;             //是否TP回家
			if (!Main.dayTime)
			{
				ShouldGoHome = true;
			}
			if (Main.eclipse)
			{
				ShouldGoHome = true;
			}
			if (Main.slimeRain)
			{
				ShouldGoHome = true;
			}
			float dmgMult = 1f;

			if (Main.expertMode)                //树妖护佑
			{
				npc.defense = npc.dryadWard ? (npc.defDefense + 10) : npc.defDefense;
			}
			else
			{
				npc.defense = npc.dryadWard ? (npc.defDefense + 6) : npc.defDefense;
			}

			if (npc.townNPC || npc.type == NPCID.SkeletonMerchant)       //按流程强化NPC防御和伤害
			{
				if (NPC.downedBoss1)
				{
					dmgMult += 0.1f;
					npc.defense += 3;
				}
				if (NPC.downedBoss2)
				{
					dmgMult += 0.1f;
					npc.defense += 3;
				}
				if (NPC.downedBoss3)
				{
					dmgMult += 0.1f;
					npc.defense += 3;
				}
				if (NPC.downedQueenBee)
				{
					dmgMult += 0.1f;
					npc.defense += 3;
				}
				if (Main.hardMode)
				{
					dmgMult += 0.4f;
					npc.defense += 12;
				}
				if (NPC.downedMechBoss1)
				{
					dmgMult += 0.15f;
					npc.defense += 6;
				}
				if (NPC.downedMechBoss2)
				{
					dmgMult += 0.15f;
					npc.defense += 6;
				}
				if (NPC.downedMechBoss3)
				{
					dmgMult += 0.15f;
					npc.defense += 6;
				}
				if (NPC.downedPlantBoss)
				{
					dmgMult += 0.15f;
					npc.defense += 8;
				}
				if (NPC.downedGolemBoss)
				{
					dmgMult += 0.15f;
					npc.defense += 8;
				}
				if (NPC.downedAncientCultist)
				{
					dmgMult += 0.15f;
					npc.defense += 8;
				}



                if (BuffNPC())             //只要战斗状态就有额外加成
                {
					int k = 1;
					if (NPC.downedMoonlord)
					{
						k = 12;
					}
					else if (NPC.downedPlantBoss)
					{
						k = 8;
					}
					else if (Main.hardMode)
					{
						k = 4;
					}
					switch (NPCID.Sets.AttackType[npc.type])
                    {
						case 0:
							dmgMult += (Main.LocalPlayer.thrownDamage - 1) * k;
							break;
						case 1:
							dmgMult += (Main.LocalPlayer.rangedDamage - 1) * k;
							break;
						case 2:
							dmgMult += (Main.LocalPlayer.magicDamage - 1) * k;
							break;
						case 3:
							dmgMult += (Main.LocalPlayer.meleeDamage - 1) * k * 2;
							break;
					}
					npc.defense += Main.LocalPlayer.statDefense;
                }
				NPCLoader.BuffTownNPC(ref dmgMult, ref npc.defense);
			}

			if (npc.type == NPCID.SantaClaus && Main.netMode != NetmodeID.MultiplayerClient && !Main.xMas)           //圣诞老人自杀
			{
				npc.StrikeNPCNoInteraction(9999, 0f, 0, false, false, false);
				if (Main.netMode == NetmodeID.Server)
				{
					NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, npc.whoAmI, 9999f, 0f, 0f, 0, 0, 0);
				}
			}

			if ((npc.type == NPCID.Penguin || npc.type == NPCID.PenguinBlack) && npc.localAI[0] == 0f)
			{
				npc.localAI[0] = Main.rand.Next(1, 5);
			}

			if (npc.type == NPCID.Mechanic)           //控制机械师回旋镖攻击
			{
				bool flag2 = false;
				for (int i = 0; i < 1000; i++)
				{
					if (Main.projectile[i].active && Main.projectile[i].type == ProjectileID.MechanicWrench && Main.projectile[i].ai[1] == npc.whoAmI)
					{
						flag2 = true;
						break;
					}
				}
				npc.localAI[0] = flag2.ToInt();
			}
			if ((npc.type == NPCID.Duck || npc.type == NPCID.DuckWhite) && Main.netMode != NetmodeID.MultiplayerClient && (npc.velocity.Y > 4f || npc.velocity.Y < -4f || npc.wet))
			{
				int direction = npc.direction;
				npc.Transform(npc.type + 1);
				npc.TargetClosest(true);
				npc.direction = direction;
				npc.netUpdate = true;             //鸭子形态切换
				return;
			}

			switch (npc.type)                 //是否救了人
			{
				case NPCID.Stylist:
					NPC.savedStylist = true;
					break;
				case NPCID.GoblinTinkerer:
					NPC.savedGoblin = true;
					break;
				case NPCID.Wizard:
					break;
				case NPCID.Mechanic:
					NPC.savedMech = true;
					break;
				case NPCID.Angler:
					NPC.savedAngler = true;
					break;
				case NPCID.DD2Bartender:
					NPC.savedBartender = true;
					break;
				case NPCID.TaxCollector:
					NPC.savedTaxCollector = true;
					break;
			}

			if (npc.type >= NPCID.None && NPCID.Sets.TownCritter[npc.type] && npc.target == 255)           //判定位置
			{
				npc.TargetClosest(true);
				if (npc.position.X < Main.player[npc.target].position.X)
				{
					npc.direction = 1;
					npc.spriteDirection = npc.direction;
				}
				if (npc.position.X > Main.player[npc.target].position.X)
				{
					npc.direction = -1;
					npc.spriteDirection = npc.direction;
				}
				if (npc.homeTileX == -1)
				{
					npc.homeTileX = (int)(npc.Center.X / 16f);
				}
			}
			else if (npc.homeTileX == -1 && npc.homeTileY == -1 && npc.velocity.Y == 0f)
			{
				npc.homeTileX = (int)npc.Center.X / 16;
				npc.homeTileY = (int)(npc.Bottom.Y + 4f) / 16;
			}

			bool TalkingToPlayer = false;
			int TrueHomeTile = npc.homeTileY;

			if (npc.type == NPCID.TaxCollector)
			{
				NPC.taxCollector = true;
			}

			npc.directionY = -1;
			if (npc.direction == 0)
			{
				npc.direction = 1;
			}

			foreach (Player talkPlayer in Main.player)             //与玩家对话时对准玩家
			{
				if (talkPlayer.active && talkPlayer.talkNPC == npc.whoAmI)
				{
					TalkingToPlayer = true;
					if (npc.ai[0] != Default)
					{
						npc.netUpdate = true;
					}
					npc.ai[0] = 0f;
					npc.ai[1] = 300f;
					npc.localAI[3] = 100f;
					npc.direction = Math.Sign(talkPlayer.Center.X - npc.Center.X);
				}
			}

			if (npc.ai[3] == 1f)           //判定老人死亡
			{
				npc.life = -1;
				npc.HitEffect(0, 10.0);
				npc.active = false;
				npc.netUpdate = true;
				if (npc.type == NPCID.OldMan)
				{
					Main.PlaySound(SoundID.Roar, (int)npc.position.X, (int)npc.position.Y, 0, 1f, 0f);
				}
				return;
			}
			if (npc.type == NPCID.OldMan && Main.netMode != NetmodeID.MultiplayerClient)
			{
				npc.homeless = false;
				npc.homeTileX = Main.dungeonX;
				npc.homeTileY = Main.dungeonY;
				if (NPC.downedBoss3)
				{
					npc.ai[3] = 1f;
					npc.netUpdate = true;
				}
			}

			if (Main.netMode != NetmodeID.MultiplayerClient && npc.homeTileY > 0)
			{
				while (!WorldGen.SolidTile(npc.homeTileX, TrueHomeTile) && TrueHomeTile < Main.maxTilesY - 20)
				{
					TrueHomeTile++;
				}
			}

			if (npc.type == NPCID.TravellingMerchant)
			{
				npc.homeless = true;
				if (!Main.dayTime)
				{
					npc.homeTileX = (int)(npc.Center.X / 16f);
					npc.homeTileY = (int)(npc.position.Y + npc.height + 2f) / 16;
					if (!TalkingToPlayer && npc.ai[0] == Default)
					{
						npc.ai[0] = Walking;
						npc.ai[1] = 200f;
					}
					ShouldGoHome = false;
				}
			}

			if (npc.type == NPCID.Angler && npc.homeless && npc.wet)              //泡水渔夫？
			{
				if (npc.Center.X / 16f < 380f || npc.Center.X / 16f > Main.maxTilesX - 380)
				{
					npc.homeTileX = Main.spawnTileX;
					npc.homeTileY = Main.spawnTileY;
					npc.ai[0] = Walking;
					npc.ai[1] = 200f;
				}
				if (npc.position.X / 16f < 200f)
				{
					npc.direction = 1;
				}
				else if (npc.position.X / 16f > Main.maxTilesX - 200)
				{
					npc.direction = -1;
				}
			}
			int TileCoordX = (int)(npc.Center.X) / 16;
			int TileCoordY = (int)(npc.Bottom.Y + 1f) / 16;
			if (!WorldGen.InWorld(TileCoordX, TileCoordY, 0) || Main.tile[TileCoordX, TileCoordY] == null)
			{
				return;
			}

			//瞬移回家，传统艺能
			if (!npc.homeless && Main.netMode != NetmodeID.MultiplayerClient && npc.townNPC && (ShouldGoHome || Main.tileDungeon[Main.tile[TileCoordX, TileCoordY].type]) && (TileCoordX != npc.homeTileX || TileCoordY != TrueHomeTile))
			{
				bool ShouldGoHome2 = true;
				for (int k = 0; k < 2; k++)
				{
					Rectangle detectRect = new Rectangle((int)(npc.Center.X - NPC.sWidth / 2 - NPC.safeRangeX), (int)(npc.Center.Y - NPC.sHeight / 2 - NPC.safeRangeY), NPC.sWidth + NPC.safeRangeX * 2, NPC.sHeight + NPC.safeRangeY * 2);
					if (k == 1)
					{
						detectRect = new Rectangle(npc.homeTileX * 16 + 8 - NPC.sWidth / 2 - NPC.safeRangeX, TrueHomeTile * 16 + 8 - NPC.sHeight / 2 - NPC.safeRangeY, NPC.sWidth + NPC.safeRangeX * 2, NPC.sHeight + NPC.safeRangeY * 2);
					}
					foreach (Player player in Main.player)
					{
						if (player.active)
						{
							if (player.Hitbox.Intersects(detectRect))
							{
								ShouldGoHome2 = false;
								break;
							}
						}
						if (!ShouldGoHome2)
						{
							break;
						}
					}
				}
				if (ShouldGoHome2)
				{
					if (npc.type == NPCID.OldMan || !Collision.SolidTiles(npc.homeTileX - 1, npc.homeTileX + 1, TrueHomeTile - 3, TrueHomeTile - 1))
					{
						npc.velocity.X = 0f;
						npc.velocity.Y = 0f;
						npc.position.X = npc.homeTileX * 16 + 8 - npc.width / 2;
						npc.position.Y = TrueHomeTile * 16 - npc.height - 0.1f;
						npc.netUpdate = true;
					}
					else
					{
						npc.homeless = true;
						WorldGen.QuickFindHome(npc.whoAmI);
					}
				}
			}


			bool IsMouse = npc.type == NPCID.Mouse || npc.type == NPCID.GoldMouse;
			float DangerDetectRange = 200f;
			if (NPCID.Sets.DangerDetectRange[npc.type] > 0)
			{
				DangerDetectRange = NPCID.Sets.DangerDetectRange[npc.type];
			}
			bool HasTarget = false;
			bool HasTarget2 = false;
			float ShootLeft = -1f;
			float ShootRight = -1f;
			int ShootDir = 0;
			int TargetLeft = -1;
			int TargetRight = -1;
			if (Main.netMode != NetmodeID.MultiplayerClient && !TalkingToPlayer)
			{
				if (AttackMode())
				{
					NPC target = Main.npc[NPC.FindFirstNPC(ModContent.NPCType<TargetNPC>())];

					HasTarget = true;
					float ShootX = target.Center.X - npc.Center.X;
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
				else if(AssembleMode())
				{
					//一无所获
					HasTarget = false;
				}
				else 
				{
					foreach (NPC target in Main.npc)
					{
						if (target.active)
						{
							bool? CanHit = NPCLoader.CanHitNPC(target, npc);
							if (CanHit == null || CanHit.Value)
							{
								bool MustHit = CanHit != null && CanHit.Value;
								if (target.active && !target.friendly && target.damage > 0 && target.Distance(npc.Center) < DangerDetectRange && (npc.type != NPCID.SkeletonMerchant || !NPCID.Sets.Skeletons.Contains(target.netID) || MustHit))
								{
									HasTarget = true;
									float ShootX = target.Center.X - npc.Center.X;
									if (ShootX < 0f && (ShootLeft == -1f || ShootX > ShootLeft))
									{
										ShootLeft = ShootX;
										TargetLeft = target.whoAmI;
									}
									if (ShootX > 0f && (ShootRight == -1f || ShootX < ShootRight))
									{
										ShootRight = ShootX;
										TargetRight = target.whoAmI;
									}
								}
							}
						}
					}
				}
				if (HasTarget)
				{
					if (ShootLeft == -1f)
					{
						ShootDir = 1;
					}
					else if (ShootRight == -1f)
					{
						ShootDir = -1;
					}
					else
					{
						ShootDir = (ShootRight < -ShootLeft).ToDirectionInt();
					}
					float NearestDist = 0f;
					if (ShootLeft != -1f)
					{
						NearestDist = -ShootLeft;
					}
					if (NearestDist == 0f || (ShootRight < NearestDist && ShootRight > 0f))
					{
						NearestDist = ShootRight;
					}
					if (npc.ai[0] == WalkingForBattle)
					{
						if (npc.direction == ShootDir)        //(npc.direction == -ShootDir)
						{
							npc.ai[0] = Walking;
							npc.ai[1] = 300 + Main.rand.Next(300);
							npc.ai[2] = 0f;
							npc.localAI[3] = 0f;
							npc.netUpdate = true;
						}
					}
					else if (npc.ai[0] != ThrowerAtk && npc.ai[0] != RangerAtk && npc.ai[0] != NurseHeal && npc.ai[0] != MageAtk && npc.ai[0] != MeleeAtk)    //没有攻击时
					{
						if (NPCID.Sets.PrettySafe[npc.type] != -1 && NPCID.Sets.PrettySafe[npc.type] < NearestDist)
						{
							if (AttackMode())
							{
								HasTarget = true;
								HasTarget2 = true;
							}
							else if (AssembleMode())
                            {
								HasTarget = false;
								HasTarget2 = false;
                            }
							else
							{
								HasTarget = false;
								HasTarget2 = true;
							}
						}
						else if (npc.ai[0] != Walking)
						{
							bool flag10 = npc.ai[0] == Talking1 || npc.ai[0] == Talking2 || npc.ai[0] == Talking3 || npc.ai[0] == Talking4;
							if (flag10)
							{
								NPC partner = Main.npc[(int)npc.ai[2]];
								if (partner.active)
								{
									partner.ai[0] = Walking;
									partner.ai[1] = 120 + Main.rand.Next(120);
									partner.ai[2] = 0f;
									partner.localAI[3] = 0f;
									partner.direction = -ShootDir;
									partner.netUpdate = true;
								}
							}
							npc.ai[0] = Walking;
							npc.ai[1] = 120 + Main.rand.Next(120);
							npc.ai[2] = 0f;
							npc.localAI[3] = 0f;
							npc.direction = -ShootDir;
							npc.netUpdate = true;
						}
						else if (npc.ai[0] == Walking)
						{
							if (AttackMode())
							{
								if (npc.direction != ShootDir)
								{
									npc.direction = ShootDir;
									npc.netUpdate = true;
								}
							}
							else if (AssembleMode())
                            {
								npc.netUpdate = true;
							}
							else
							{
								if (npc.direction != -ShootDir)
								{
									npc.direction = -ShootDir;
									npc.netUpdate = true;
								}
							}
						}
					}
				}
			}

			if (npc.ai[0] == Default)
			{
				if (npc.localAI[3] > 0f)
				{
					npc.localAI[3] -= 1f;
				}
				if (ShouldGoHome && !TalkingToPlayer && !NPCID.Sets.TownCritter[npc.type])
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						if (AssembleMode())
						{
							npc.ai[0] = Walking;
							npc.ai[1] = 120;
							npc.ai[2] = 0f;
						}

						if (TileCoordX == npc.homeTileX && TileCoordY == TrueHomeTile)
						{
							if (npc.velocity.X != 0f)
							{
								npc.netUpdate = true;
							}
							if (npc.velocity.X > 0.1f)
							{
								npc.velocity.X -= 0.1f;
							}
							else if (npc.velocity.X < -0.1f)
							{
								npc.velocity.X += 0.1f;
							}
							else
							{
								npc.velocity.X = 0f;
							}
						}
						else
						{
							if (TileCoordX > npc.homeTileX)
							{
								npc.direction = -1;
							}
							else
							{
								npc.direction = 1;
							}
							npc.ai[0] = Walking;
							npc.ai[1] = 200 + Main.rand.Next(200);
							npc.ai[2] = 0f;
							npc.localAI[3] = 0f;
							npc.netUpdate = true;
						}
					}
				}
				else
				{
					if (IsMouse)
					{
						npc.velocity.X *= 0.5f;
					}
					if (npc.velocity.X > 0.1f)
					{
						npc.velocity.X -= 0.1f;
					}
					else if (npc.velocity.X < -0.1f)
					{
						npc.velocity.X += 0.1f;
					}
					else
					{
						npc.velocity.X = 0f;
					}
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
                        if (AssembleMode())
                        {
							npc.ai[0] = Walking;
							npc.ai[1] = 120;
							npc.ai[2] = 0f;
						}

						if (npc.ai[1] > 0f)
						{
							npc.ai[1] -= 1f;
						}
						if (npc.ai[1] <= 0f)
						{
							npc.ai[0] = Walking;
							npc.ai[1] = 200 + Main.rand.Next(300);
							npc.ai[2] = 0f;
							if (NPCID.Sets.TownCritter[npc.type])
							{
								npc.ai[1] += Main.rand.Next(200, 400);
							}
							npc.localAI[3] = 0f;
							npc.netUpdate = true;
						}
					}
				}
				if (Main.netMode != NetmodeID.MultiplayerClient && (!ShouldGoHome || (TileCoordX == npc.homeTileX && TileCoordY == TrueHomeTile)))
				{
					if (TileCoordX < npc.homeTileX - 25 || TileCoordX > npc.homeTileX + 25)
					{
						if (npc.localAI[3] == 0f)
						{
							if (TileCoordX < npc.homeTileX - 50 && npc.direction == -1)
							{
								npc.direction = 1;
								npc.netUpdate = true;
							}
							else if (TileCoordX > npc.homeTileX + 50 && npc.direction == 1)
							{
								npc.direction = -1;
								npc.netUpdate = true;
							}
						}
					}
					else if (Main.rand.Next(80) == 0 && npc.localAI[3] == 0f)
					{
						npc.localAI[3] = 200f;
						npc.direction *= -1;
						npc.netUpdate = true;
					}
				}
			}
			else if (npc.ai[0] == Walking)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient && ShouldGoHome && TileCoordX == npc.homeTileX && TileCoordY == npc.homeTileY && !NPCID.Sets.TownCritter[npc.type])
				{
					if (NoMode())
					{
						npc.ai[0] = Default;
						npc.ai[1] = 200 + Main.rand.Next(200);
						npc.localAI[3] = 60f;
						npc.netUpdate = true;
					}
				}
				else
				{
					bool flag11 = Collision.DrownCollision(npc.position, npc.width, npc.height, 1f);
					if (!flag11)
					{
						if (Main.netMode != NetmodeID.MultiplayerClient && !npc.homeless && !Main.tileDungeon[Main.tile[TileCoordX, TileCoordY].type] && (TileCoordX < npc.homeTileX - 35 || TileCoordX > npc.homeTileX + 35))
						{
							if (npc.position.X < npc.homeTileX * 16 && npc.direction == -1)
							{
								npc.ai[1] -= 5f;
							}
							else if (npc.position.X > npc.homeTileX * 16 && npc.direction == 1)
							{
								npc.ai[1] -= 5f;
							}
						}
						npc.ai[1] -= 1f;
					}
					if (npc.ai[1] <= 0f)
					{
						if (!AssembleMode())
						{
							npc.ai[0] = Default;
							npc.ai[1] = 300 + Main.rand.Next(300);
							npc.ai[2] = 0f;
							if (NPCID.Sets.TownCritter[npc.type])
							{
								npc.ai[1] -= Main.rand.Next(100);
							}
							else
							{
								npc.ai[1] += Main.rand.Next(900);
							}
							npc.localAI[3] = 60f;
							npc.netUpdate = true;
						}
                        else
                        {
							npc.netUpdate = true;
                        }
					}
					if (npc.closeDoor && (npc.Center.X / 16f > npc.doorX + 2 || npc.Center.X / 16f < npc.doorX - 2))
					{
						Tile tileSafely = Framing.GetTileSafely(npc.doorX, npc.doorY);
						if (TileLoader.CloseDoorID(tileSafely) >= 0)
						{
							if (WorldGen.CloseDoor(npc.doorX, npc.doorY, false))
							{
								npc.closeDoor = false;
								NetMessage.SendData(MessageID.ChangeDoor, -1, -1, null, 1, npc.doorX, npc.doorY, npc.direction, 0, 0, 0);
							}
							if (npc.Center.X / 16f > npc.doorX + 4 || npc.Center.X / 16f < npc.doorX - 4 || npc.Center.Y / 16f > npc.doorY + 4 || npc.Center.Y / 16f < npc.doorY - 4)
							{
								npc.closeDoor = false;
							}
						}
						else if (tileSafely.type == 389)
						{
							bool flag13 = WorldGen.ShiftTallGate(npc.doorX, npc.doorY, true);
							if (flag13)
							{
								npc.closeDoor = false;
								NetMessage.SendData(MessageID.ChangeDoor, -1, -1, null, 5, npc.doorX, npc.doorY, 0f, 0, 0, 0);
							}
							if (npc.Center.X / 16f > npc.doorX + 4 || npc.Center.X / 16f < npc.doorX - 4 || npc.Center.Y / 16f > npc.doorY + 4 || npc.Center.Y / 16f < npc.doorY - 4)
							{
								npc.closeDoor = false;
							}
						}
						else
						{
							npc.closeDoor = false;
						}
					}
					float SpeedX = 1f;
					float AccX = 0.07f;
					if (npc.type == NPCID.Squirrel || npc.type == NPCID.SquirrelGold || npc.type == NPCID.SquirrelRed)
					{
						SpeedX = 1.5f;
					}
					if (IsMouse)
					{
						SpeedX = 2f;
						AccX = 1f;
					}
					if (npc.friendly && (HasTarget || flag11))
					{
						SpeedX = 1.5f;
						float num17 = 1f - npc.life / (float)npc.lifeMax;
						SpeedX += num17 * 0.9f;
						AccX = 0.1f;
					}
					if (AttackMode())
					{
						if (NPCID.Sets.AttackType[npc.type] == 3)
						{
                            if (NPC.downedMoonlord)
                            {
								SpeedX *= 2.5f;
								AccX *= 2.5f;
							}
							else if (Main.hardMode)
                            {
								SpeedX *= 2;
								AccX *= 2;
							}
                            else
                            {
								SpeedX *= 1.5f;
								AccX *= 1.5f;
							}
						}
						else              // if (NPCID.Sets.AttackType[npc.type] != 0 || npc.type == NPCID.Nurse)
						{
							SpeedX /= 2;
						}
					}

                    if (AssembleMode())
					{
						if (NPC.downedMoonlord)
						{
							SpeedX *= 2.5f;
							AccX *= 2.5f;
						}
						else
						{
							AccX *= 2;
							SpeedX *= 2;
						}
						npc.direction = Math.Sign(Main.npc[NPC.FindFirstNPC(ModContent.NPCType<AssembleNPC>())].Center.X - npc.Center.X);
					}

					if (npc.velocity.X < -SpeedX || npc.velocity.X > SpeedX)
					{
						if (npc.velocity.Y == 0f)
						{
							npc.velocity *= 0.8f;
						}
					}
					else if (npc.velocity.X < SpeedX && npc.direction == 1)
					{
						npc.velocity.X += AccX;
						if (npc.velocity.X > SpeedX)
						{
							npc.velocity.X = SpeedX;
						}
					}
					else if (npc.velocity.X > -SpeedX && npc.direction == -1)
					{
						npc.velocity.X -= AccX;
						if (npc.velocity.X > SpeedX)
						{
							npc.velocity.X = SpeedX;
						}
					}
					bool holdsMatching = true;
					if (npc.homeTileY * 16 - 32 > npc.position.Y)
					{
						holdsMatching = false;
					}
					if ((npc.direction == 1 && npc.position.Y + npc.width / 2 > npc.homeTileX * 16) || (npc.direction == -1 && npc.position.Y + npc.width / 2 < npc.homeTileX * 16))
					{
						holdsMatching = true;
					}
					if (npc.velocity.Y == 0f)
					{
						Collision.StepDown(ref npc.position, ref npc.velocity, npc.width, npc.height, ref npc.stepSpeed, ref npc.gfxOffY, 1, false);
					}
					if (npc.velocity.Y >= 0f)
					{
						Collision.StepUp(ref npc.position, ref npc.velocity, npc.width, npc.height, ref npc.stepSpeed, ref npc.gfxOffY, 1, holdsMatching, 1);
					}
					if (npc.velocity.Y == 0f)
					{
						int num18 = (int)((npc.Center.X + 15 * npc.direction) / 16f);
						int num19 = (int)((npc.Bottom.Y - 16f) / 16f);
						bool flag14 = false;
						bool flag15 = true;
						bool flag16 = TileCoordX >= npc.homeTileX - 35 && TileCoordX <= npc.homeTileX + 35;
						if (npc.townNPC && npc.ai[1] < 30f)
						{
							flag14 = !Utils.PlotTileLine(npc.Top, npc.Bottom, npc.width, new Utils.PerLinePoint(DelegateMethods.SearchAvoidedByNPCs));
							if (!flag14)
							{
								Rectangle hitbox = npc.Hitbox;
								hitbox.X -= 20;
								hitbox.Width += 40;
								for (int n = 0; n < 200; n++)
								{
									if (Main.npc[n].active && Main.npc[n].friendly && n != npc.whoAmI && Main.npc[n].velocity.X == 0f && hitbox.Intersects(Main.npc[n].Hitbox))
									{
										flag14 = true;
										break;
									}
								}
							}
						}
						if (!flag14 && flag11)
						{
							flag14 = true;
						}
						if (flag15 && (NPCID.Sets.TownCritter[npc.type] || (!flag16 && npc.direction == Math.Sign(npc.homeTileX - TileCoordX))))
						{
							flag15 = false;
						}
						if (flag15)
						{
							int num20 = 0;
							for (int num21 = -1; num21 <= 4; num21++)
							{
								Tile tileSafely2 = Framing.GetTileSafely(num18 - npc.direction * num20, num19 + num21);
								if (tileSafely2.lava() && tileSafely2.liquid > 0)
								{
									flag15 = true;
									break;
								}
								if (tileSafely2.nactive() && Main.tileSolid[tileSafely2.type])
								{
									flag15 = false;
									break;
								}
							}
						}
						if (!flag15 && npc.wet)
						{
							bool flag17 = flag11;
							bool flag18 = false;
							if (!flag17)
							{
								flag18 = Collision.DrownCollision(npc.position + new Vector2(npc.width * npc.direction, 0f), npc.width, npc.height, 1f);
							}
							flag18 = (flag18 || Collision.DrownCollision(npc.position + new Vector2(npc.width * npc.direction, npc.height * 2 - 16 - (flag17 ? 16 : 0)), npc.width, 16 + (flag17 ? 16 : 0), 1f));
							if (flag18 && npc.localAI[3] <= 0f)
							{
								flag15 = true;
								npc.localAI[3] = 600f;
							}
						}
						if (npc.position.X == npc.localAI[3])
						{
							npc.direction *= -1;
							npc.netUpdate = true;
							npc.localAI[3] = 600f;
						}
						if (flag11)
						{
							if (npc.localAI[3] > 0f)
							{
								npc.localAI[3] -= 1f;
							}
						}
						else
						{
							npc.localAI[3] = -1f;
						}
						Tile tileSafely3 = Framing.GetTileSafely(num18, num19);
						Tile tileSafely4 = Framing.GetTileSafely(num18, num19 - 1);
						Tile tileSafely5 = Framing.GetTileSafely(num18, num19 - 2);
						if (npc.townNPC && tileSafely5.nactive() && (TileLoader.OpenDoorID(tileSafely5) >= 0 || tileSafely5.type == 388) && (Main.rand.Next(10) == 0 || ShouldGoHome))
						{
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								if (WorldGen.OpenDoor(num18, num19 - 2, npc.direction))
								{
									npc.closeDoor = true;
									npc.doorX = num18;
									npc.doorY = num19 - 2;
									NetMessage.SendData(MessageID.ChangeDoor, -1, -1, null, 0, num18, num19 - 2, npc.direction, 0, 0, 0);
									npc.netUpdate = true;
									npc.ai[1] += 80f;
								}
								else if (WorldGen.OpenDoor(num18, num19 - 2, -npc.direction))
								{
									npc.closeDoor = true;
									npc.doorX = num18;
									npc.doorY = num19 - 2;
									NetMessage.SendData(MessageID.ChangeDoor, -1, -1, null, 0, num18, num19 - 2, -npc.direction, 0, 0, 0);
									npc.netUpdate = true;
									npc.ai[1] += 80f;
								}
								else if (WorldGen.ShiftTallGate(num18, num19 - 2, false))
								{
									npc.closeDoor = true;
									npc.doorX = num18;
									npc.doorY = num19 - 2;
									NetMessage.SendData(MessageID.ChangeDoor, -1, -1, null, 4, num18, num19 - 2, 0f, 0, 0, 0);
									npc.netUpdate = true;
									npc.ai[1] += 80f;
								}
								else
								{
									npc.direction *= -1;
									npc.netUpdate = true;
								}
							}
						}
						else
						{
							if ((npc.velocity.X < 0f && npc.spriteDirection == -1) || (npc.velocity.X > 0f && npc.spriteDirection == 1))
							{
								if (tileSafely5.nactive() && Main.tileSolid[tileSafely5.type] && !Main.tileSolidTop[tileSafely5.type])
								{
									if (!Collision.SolidTilesVersatile(num18 - npc.direction * 2, num18 - npc.direction, num19 - 5, num19 - 1) && !Collision.SolidTiles(num18, num18, num19 - 5, num19 - 3))
									{
										npc.velocity.Y = -6f;
										npc.netUpdate = true;
									}
									else if (IsMouse)
									{
										if (WorldGen.SolidTile((int)(npc.Center.X / 16f) + npc.direction, (int)(npc.Center.Y / 16f)))
										{
											npc.direction *= -1;
											npc.velocity.X *= 0f;
											npc.netUpdate = true;
										}
									}
									else if (HasTarget)
									{
										flag14 = false;
										npc.velocity.X = 0f;
										npc.direction *= -1;
										npc.netUpdate = true;
										npc.ai[0] = WalkingForBattle;
										npc.ai[1] = 240f;
									}
									else
									{
										npc.direction *= -1;
										npc.netUpdate = true;
									}
								}
								else if (tileSafely4.nactive() && Main.tileSolid[tileSafely4.type] && !Main.tileSolidTop[tileSafely4.type])
								{
									if (!Collision.SolidTilesVersatile(num18 - npc.direction * 2, num18 - npc.direction, num19 - 4, num19 - 1) && !Collision.SolidTiles(num18, num18, num19 - 4, num19 - 2))
									{
										npc.velocity.Y = -5f;
										npc.netUpdate = true;
									}
									else if (HasTarget)
									{
										flag14 = false;
										npc.velocity.X = 0f;
										npc.direction *= -1;
										npc.netUpdate = true;
										npc.ai[0] = WalkingForBattle;
										npc.ai[1] = 240f;
									}
									else
									{
										npc.direction *= -1;
										npc.netUpdate = true;
									}
								}
								else if (npc.position.Y + npc.height - num19 * 16 > 20f && tileSafely3.nactive() && Main.tileSolid[tileSafely3.type] && !tileSafely3.topSlope())
								{
									if (!Collision.SolidTilesVersatile(num18 - npc.direction * 2, num18, num19 - 3, num19 - 1))
									{
										npc.velocity.Y = -4.4f;
										npc.netUpdate = true;
									}
									else if (HasTarget)
									{
										flag14 = false;
										npc.velocity.X = 0f;
										npc.direction *= -1;
										npc.netUpdate = true;
										npc.ai[0] = WalkingForBattle;
										npc.ai[1] = 240f;
									}
									else
									{
										npc.direction *= -1;
										npc.netUpdate = true;
									}
								}
								else if (flag15)
								{
									npc.direction *= -1;
									npc.velocity.X *= -1f;
									npc.netUpdate = true;
									if (HasTarget)
									{
										flag14 = false;
										npc.velocity.X = 0f;
										npc.ai[0] = WalkingForBattle;
										npc.ai[1] = 240f;
									}
								}
								if (flag14)
								{
									npc.ai[1] = 90f;
									npc.netUpdate = true;
								}
								if (npc.velocity.Y < 0f)
								{
									npc.localAI[3] = npc.position.X;
								}
							}
							if (npc.velocity.Y < 0f && npc.wet)
							{
								npc.velocity.Y *= 1.2f;
							}
							if (npc.velocity.Y < 0f && NPCID.Sets.TownCritter[npc.type] && !IsMouse)
							{
								npc.velocity.Y *= 1.2f;
							}
						}
					}
				}
			}
			else if (npc.ai[0] == Stop || npc.ai[0] == PriateSpecial)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					npc.localAI[3] -= 1f;
					if (Main.rand.Next(60) == 0 && npc.localAI[3] == 0f)
					{
						npc.localAI[3] = 60f;
						npc.direction *= -1;
						npc.netUpdate = true;
					}
				}
				npc.ai[1] -= 1f;
				npc.velocity.X *= 0.8f;
				if (npc.ai[1] <= 0f)
				{
					npc.localAI[3] = 40f;
					npc.ai[0] = Default;
					npc.ai[1] = 60 + Main.rand.Next(60);
					npc.netUpdate = true;
				}
			}
			else if (npc.ai[0] == Talking1 || npc.ai[0] == Talking2 || npc.ai[0] == Sit || npc.ai[0] == WalkingForBattle || npc.ai[0] == Interacting || npc.ai[0] == Talking3 || npc.ai[0] == Talking4)
			{
                if (AssembleMode())
                {
					npc.ai[0] = Walking;
					npc.ai[1] = 180f;
                }
				npc.velocity.X *= 0.8f;
				npc.ai[1] -= 1f;
				if (npc.ai[0] == WalkingForBattle && npc.ai[1] < 60f && HasTarget)
				{
					npc.ai[1] = 180f;
					npc.netUpdate = true;
				}
				if (npc.ai[0] == Sit)
				{
					Point point = npc.Center.ToTileCoordinates();
					Tile tile = Main.tile[point.X, point.Y];
					if (tile.type != 15)
					{
						npc.ai[1] = 0f;
					}
				}
				if (npc.ai[1] <= 0f)
				{
					npc.ai[0] = Default;
					npc.ai[1] = 60 + Main.rand.Next(60);
					npc.ai[2] = 0f;
					npc.localAI[3] = 30 + Main.rand.Next(60);
					npc.netUpdate = true;
				}
			}
			else if (npc.ai[0] == TalkingPartyToPlayer || npc.ai[0] == NPCAttacker.TalkingToPlayer || npc.ai[0] == TalkingBartenderToPlayer)
			{
				if (npc.ai[0] == TalkingBartenderToPlayer && (npc.localAI[3] < 1f || npc.localAI[3] > 2f))
				{
					npc.localAI[3] = 2f;
				}
				npc.velocity.X *= 0.8f;
				npc.ai[1] -= 1f;
				int num22 = (int)npc.ai[2];
				if (num22 < 0 || num22 > 255 || !Main.player[num22].active || Main.player[num22].dead || Main.player[num22].Distance(npc.Center) > 200f || !Collision.CanHitLine(npc.Top, 0, 0, Main.player[num22].Top, 0, 0))
				{
					npc.ai[1] = 0f;
				}
				if (npc.ai[1] > 0f)
				{
					int num23 = (npc.Center.X < Main.player[num22].Center.X) ? 1 : -1;
					if (num23 != npc.direction)
					{
						npc.netUpdate = true;
					}
					npc.direction = num23;
				}
				else
				{
					npc.ai[0] = 0f;
					npc.ai[1] = 60 + Main.rand.Next(60);
					npc.ai[2] = 0f;
					npc.localAI[3] = 30 + Main.rand.Next(60);
					npc.netUpdate = true;
				}
			}
			else if (npc.ai[0] == ThrowerAtk)               //投手攻击
			{
				int ProjType = 0;
				int ProjDmg = 0;
				float knockBack = 0f;
				float SpeedMultiplier = 0f;
				int AtkDelay = 0;    //NPC在出招后多长时间才会发射弹幕
				int AtkCD = 0;
				int AtkExtraRandCD = 0;
				float gravityCorrection = 0f;             //重力修正
				float AtkRange = NPCID.Sets.DangerDetectRange[npc.type];
				float randomOffset = 0f;                 //精准度
				if (NPCID.Sets.AttackTime[npc.type] == npc.ai[1])
				{
					npc.frameCounter = 0.0;
					npc.localAI[3] = 0f;
				}
				if (npc.type == NPCID.Demolitionist)
				{
					ProjType = 30;
					SpeedMultiplier = 6f;
					ProjDmg = 20;
					AtkDelay = 10;
					AtkCD = 180;
					AtkExtraRandCD = 120;
					gravityCorrection = 16f;
					knockBack = 7f;
				}
				else if (npc.type == NPCID.DD2Bartender)
				{
					ProjType = 669;
					SpeedMultiplier = 6f;
					ProjDmg = 24;
					AtkDelay = 10;
					AtkCD = 120;
					AtkExtraRandCD = 60;
					gravityCorrection = 16f;
					knockBack = 9f;
				}
				else if (npc.type == NPCID.PartyGirl)
				{
					ProjType = 588;
					SpeedMultiplier = 6f;
					ProjDmg = 30;
					AtkDelay = 10;
					AtkCD = 60;
					AtkExtraRandCD = 120;
					gravityCorrection = 16f;
					knockBack = 6f;
				}
				else if (npc.type == NPCID.Merchant)
				{
					ProjType = 48;
					SpeedMultiplier = 9f;
					ProjDmg = 12;
					AtkDelay = 10;
					AtkCD = 60;
					AtkExtraRandCD = 60;
					gravityCorrection = 16f;
					knockBack = 1.5f;
				}
				else if (npc.type == NPCID.Angler)
				{
					ProjType = 520;
					SpeedMultiplier = 12f;
					ProjDmg = 10;
					AtkDelay = 10;
					AtkCD = 0;
					AtkExtraRandCD = 1;
					gravityCorrection = 16f;
					knockBack = 3f;
				}
				else if (npc.type == NPCID.SkeletonMerchant)
				{
					ProjType = 21;
					SpeedMultiplier = 14f;
					ProjDmg = 14;
					AtkDelay = 10;
					AtkCD = 0;
					AtkExtraRandCD = 1;
					gravityCorrection = 16f;
					knockBack = 3f;
				}
				else if (npc.type == NPCID.GoblinTinkerer)
				{
					ProjType = 24;
					SpeedMultiplier = 5f;
					ProjDmg = 15;
					AtkDelay = 10;
					AtkCD = 60;
					AtkExtraRandCD = 60;
					gravityCorrection = 16f;
					knockBack = 1f;
				}
				else if (npc.type == NPCID.Mechanic)
				{
					ProjType = 582;
					SpeedMultiplier = 10f;
					ProjDmg = 11;
					AtkDelay = 1;
					AtkCD = 30;
					AtkExtraRandCD = 30;
					knockBack = 3.5f;
				}
				else if (npc.type == NPCID.Nurse)
				{
					ProjType = 583;
					SpeedMultiplier = 8f;
					ProjDmg = 8;
					AtkDelay = 1;
					AtkCD = 15;
					AtkExtraRandCD = 10;
					knockBack = 2f;
					gravityCorrection = 10f;
				}
				else if (npc.type == NPCID.SantaClaus)
				{
					ProjType = 589;
					SpeedMultiplier = 7f;
					ProjDmg = 22;
					AtkDelay = 1;
					AtkCD = 10;
					AtkExtraRandCD = 1;
					knockBack = 2f;
					gravityCorrection = 10f;
				}
				NPCLoader.TownNPCAttackStrength(npc, ref ProjDmg, ref knockBack);
				NPCLoader.TownNPCAttackCooldown(npc, ref AtkCD, ref AtkExtraRandCD);
				NPCLoader.TownNPCAttackProj(npc, ref ProjType, ref AtkDelay);
				NPCLoader.TownNPCAttackProjSpeed(npc, ref SpeedMultiplier, ref gravityCorrection, ref randomOffset);
				if (Main.expertMode)
				{
					ProjDmg = (int)(ProjDmg * Main.expertNPCDamage);
				}
				ProjDmg = (int)(ProjDmg * dmgMult);
				npc.velocity.X *= 0.8f;
				npc.ai[1] -= 1f;
				npc.localAI[3] += 1f;
				if (npc.localAI[3] == AtkDelay && Main.netMode != NetmodeID.MultiplayerClient)
				{
					Vector2 vector = -Vector2.UnitY;
					if (ShootDir == 1 && npc.spriteDirection == 1 && TargetRight != -1)
					{
						float dist = Main.npc[TargetRight].Distance(npc.Center);
						if (dist > AtkRange) AtkRange = dist;
						vector = npc.DirectionTo(Main.npc[TargetRight].Center + new Vector2(0f, -gravityCorrection * MathHelper.Clamp(npc.Distance(Main.npc[TargetRight].Center) / AtkRange, 0f, 1f)));
					}
					if (ShootDir == -1 && npc.spriteDirection == -1 && TargetLeft != -1)
					{
						float dist = Main.npc[TargetLeft].Distance(npc.Center);
						if (dist > AtkRange) AtkRange = dist;
						vector = npc.DirectionTo(Main.npc[TargetLeft].Center + new Vector2(0f, -gravityCorrection * MathHelper.Clamp(npc.Distance(Main.npc[TargetLeft].Center) / AtkRange, 0f, 1f)));
					}
					if (vector.HasNaNs() || Math.Sign(vector.X) != npc.spriteDirection)
					{
						vector = new Vector2(npc.spriteDirection, -1f);
					}
					vector *= SpeedMultiplier;
					vector += Utils.RandomVector2(Main.rand, -randomOffset, randomOffset);
					int protmp;
					if (npc.type == NPCID.Mechanic)
					{
						protmp = Projectile.NewProjectile(npc.Center.X + npc.spriteDirection * 16, npc.Center.Y - 2f, vector.X, vector.Y, ProjType, ProjDmg, knockBack, Main.myPlayer, 0f, npc.whoAmI);
					}
					else if (npc.type == NPCID.SantaClaus)
					{
						protmp = Projectile.NewProjectile(npc.Center.X + npc.spriteDirection * 16, npc.Center.Y - 2f, vector.X, vector.Y, ProjType, ProjDmg, knockBack, Main.myPlayer, 0f, Main.rand.Next(5));
					}
					else
					{
						protmp = Projectile.NewProjectile(npc.Center.X + npc.spriteDirection * 16, npc.Center.Y - 2f, vector.X, vector.Y, ProjType, ProjDmg, knockBack, Main.myPlayer, 0f, 0f);
					}
					Main.projectile[protmp].npcProj = true;
					Main.projectile[protmp].noDropItem = true;
					Main.projectile[protmp].usesLocalNPCImmunity = true;
					Main.projectile[protmp].localNPCHitCooldown = 10;
				}
				if (npc.ai[1] <= 0f)
				{
					npc.ai[0] = (npc.localAI[2] == 8f && HasTarget) ? 8 : 0;
					npc.ai[1] = AtkCD + Main.rand.Next(AtkExtraRandCD);
					npc.ai[2] = 0f;
					npc.localAI[1] = (npc.localAI[3] = AtkCD / 2 + Main.rand.Next(AtkExtraRandCD));
					npc.netUpdate = true;
				}
			}
			else if (npc.ai[0] == RangerAtk)              //射手类
			{
				int ProjType = 0;
				int ProjDmg = 0;
				float SpeedMultiplier = 0f;
				int AtkDelay = 0;
				int AtkCD = 0;
				int AtkRandExtraCD = 0;
				float knockBack2 = 0f;
				float gravityCorrection = 0f;
				bool inBetweenShots = false;               //连射间隔
				float randomOffset = 0f;
				if (NPCID.Sets.AttackTime[npc.type] == npc.ai[1])
				{
					npc.frameCounter = 0.0;
					npc.localAI[3] = 0f;
				}
				int Target = -1;
				if (ShootDir == 1 && npc.spriteDirection == 1)
				{
					Target = TargetRight;
				}
				if (ShootDir == -1 && npc.spriteDirection == -1)
				{
					Target = TargetLeft;
				}
				if (npc.type == NPCID.ArmsDealer)
				{
					ProjType = 14;
					SpeedMultiplier = 13f;
					ProjDmg = 24;
					AtkCD = 14;
					AtkRandExtraCD = 4;
					knockBack2 = 3f;
					AtkDelay = 1;
					randomOffset = 0.5f;
					if (NPCID.Sets.AttackTime[npc.type] == npc.ai[1])
					{
						npc.frameCounter = 0.0;
						npc.localAI[3] = 0f;
					}
					if (Main.hardMode)
					{
						ProjDmg = 15;
						if (npc.localAI[3] > AtkDelay)
						{
							AtkDelay = 10;
							inBetweenShots = true;
						}
						if (npc.localAI[3] > AtkDelay)
						{
							AtkDelay = 20;
							inBetweenShots = true;
						}
						if (npc.localAI[3] > AtkDelay)
						{
							AtkDelay = 30;
							inBetweenShots = true;
						}
					}
				}
				else if (npc.type == NPCID.Painter)
				{
					ProjType = 587;
					SpeedMultiplier = 10f;
					ProjDmg = 8;
					AtkCD = 10;
					AtkRandExtraCD = 1;
					knockBack2 = 1.75f;
					AtkDelay = 1;
					randomOffset = 0.5f;
					if (npc.localAI[3] > AtkDelay)
					{
						AtkDelay = 12;
						inBetweenShots = true;
					}
					if (npc.localAI[3] > AtkDelay)
					{
						AtkDelay = 24;
						inBetweenShots = true;
					}
					if (Main.hardMode)
					{
						ProjDmg += 2;
					}
				}
				else if (npc.type == NPCID.TravellingMerchant)
				{
					ProjType = 14;
					SpeedMultiplier = 13f;
					ProjDmg = 24;
					AtkCD = 12;
					AtkRandExtraCD = 5;
					knockBack2 = 2f;
					AtkDelay = 1;
					randomOffset = 0.2f;
					if (Main.hardMode)
					{
						ProjDmg = 30;
						ProjType = 357;
					}
				}
				else if (npc.type == NPCID.Guide)
				{
					SpeedMultiplier = 10f;
					ProjDmg = 8;
					AtkDelay = 1;
					if (Main.hardMode)
					{
						ProjType = 2;
						AtkCD = 15;
						AtkRandExtraCD = 10;
						ProjDmg += 6;
					}
					else
					{
						ProjType = 1;
						AtkCD = 30;
						AtkRandExtraCD = 20;
					}
					knockBack2 = 2.75f;
					gravityCorrection = 4f;
					randomOffset = 0.7f;
				}
				else if (npc.type == NPCID.WitchDoctor)
				{
					ProjType = 267;
					SpeedMultiplier = 14f;
					ProjDmg = 20;
					AtkDelay = 1;
					AtkCD = 10;
					AtkRandExtraCD = 1;
					knockBack2 = 3f;
					gravityCorrection = 6f;
					randomOffset = 0.4f;
				}
				else if (npc.type == NPCID.Steampunker)
				{
					ProjType = 242;
					SpeedMultiplier = 13f;
					ProjDmg = 15;
					AtkCD = 10;
					AtkRandExtraCD = 1;
					knockBack2 = 2f;
					AtkDelay = 1;
					if (npc.localAI[3] > AtkDelay)
					{
						AtkDelay = 8;
						inBetweenShots = true;
					}
					if (npc.localAI[3] > AtkDelay)
					{
						AtkDelay = 16;
						inBetweenShots = true;
					}
					randomOffset = 0.3f;
				}
				else if (npc.type == NPCID.Pirate)
				{
					ProjType = ProjectileID.Bullet;
					SpeedMultiplier = 14f;
					ProjDmg = 24;
					AtkCD = 10;
					AtkRandExtraCD = 1;
					knockBack2 = 2f;
					AtkDelay = 1;
					randomOffset = 0.7f;
					if (npc.localAI[3] > AtkDelay)
					{
						AtkDelay = 16;
						inBetweenShots = true;
					}
					if (npc.localAI[3] > AtkDelay)
					{
						AtkDelay = 24;
						inBetweenShots = true;
					}
					if (npc.localAI[3] > AtkDelay)
					{
						AtkDelay = 32;
						inBetweenShots = true;
					}
					if (npc.localAI[3] > AtkDelay)
					{
						AtkDelay = 40;
						inBetweenShots = true;
					}
					if (npc.localAI[3] > AtkDelay)
					{
						AtkDelay = 48;
						inBetweenShots = true;
					}
					if (npc.localAI[3] == 0f && Target != -1 && npc.Distance(Main.npc[Target].Center) < NPCID.Sets.PrettySafe[npc.type])
					{
						randomOffset = 0.1f;
						ProjType = ProjectileID.CannonballFriendly;
						ProjDmg = 50;
						knockBack2 = 10f;
						SpeedMultiplier = 24f;
					}
				}
				else if (npc.type == NPCID.Cyborg)
				{
					ProjType = Utils.SelectRandom(Main.rand, new int[]
					{
						ProjectileID.RocketI,
						ProjectileID.GrenadeI,
						ProjectileID.ProximityMineI
					});
					AtkDelay = 1;
					if (ProjType == ProjectileID.ProximityMineI)
					{
						SpeedMultiplier = 12f;
						ProjDmg = 30;
						AtkCD = 30;
						AtkRandExtraCD = 10;
						knockBack2 = 7f;
						randomOffset = 0.2f;
					}
					else if (ProjType == ProjectileID.GrenadeI)
					{
						SpeedMultiplier = 10f;
						ProjDmg = 25;
						AtkCD = 10;
						AtkRandExtraCD = 1;
						knockBack2 = 6f;
						randomOffset = 0.2f;
					}
					else if (ProjType == ProjectileID.RocketI)
					{
						SpeedMultiplier = 13f;
						ProjDmg = 20;
						AtkCD = 20;
						AtkRandExtraCD = 10;
						knockBack2 = 4f;
						randomOffset = 0.1f;
					}
				}
				NPCLoader.TownNPCAttackStrength(npc, ref ProjDmg, ref knockBack2);
				NPCLoader.TownNPCAttackCooldown(npc, ref AtkCD, ref AtkRandExtraCD);
				NPCLoader.TownNPCAttackProj(npc, ref ProjType, ref AtkDelay);
				NPCLoader.TownNPCAttackProjSpeed(npc, ref SpeedMultiplier, ref gravityCorrection, ref randomOffset);
				NPCLoader.TownNPCAttackShoot(npc, ref inBetweenShots);
				if (Main.expertMode)
				{
					ProjDmg = (int)(ProjDmg * Main.expertNPCDamage);
				}
				ProjDmg = (int)(ProjDmg * dmgMult);
				npc.velocity.X *= 0.8f;
				npc.ai[1] -= 1f;
				npc.localAI[3] += 1f;
				if (npc.localAI[3] == AtkDelay && Main.netMode != NetmodeID.MultiplayerClient)
				{
					Vector2 vector2 = Vector2.Zero;
					if (Target != -1)
					{
						vector2 = npc.DirectionTo(Main.npc[Target].Center + new Vector2(0f, -gravityCorrection));
					}
					if (vector2.HasNaNs() || Math.Sign(vector2.X) != npc.spriteDirection)
					{
						vector2 = new Vector2(npc.spriteDirection, 0f);
					}
					vector2 *= SpeedMultiplier;
					vector2 += Utils.RandomVector2(Main.rand, -randomOffset, randomOffset);
					int protmp;
					if (npc.type == NPCID.Painter)
					{
						protmp = Projectile.NewProjectile(npc.Center.X + npc.spriteDirection * 16, npc.Center.Y - 2f, vector2.X, vector2.Y, ProjType, ProjDmg, knockBack2, Main.myPlayer, 0f, Main.rand.Next(12) / 6f);
					}
					else
					{
						protmp = Projectile.NewProjectile(npc.Center.X + npc.spriteDirection * 16, npc.Center.Y - 2f, vector2.X, vector2.Y, ProjType, ProjDmg, knockBack2, Main.myPlayer, 0f, 0f);
					}
					Main.projectile[protmp].npcProj = true;
					Main.projectile[protmp].noDropItem = true;
					Main.projectile[protmp].usesLocalNPCImmunity = true;
					Main.projectile[protmp].localNPCHitCooldown = 10;
				}
				if (npc.localAI[3] == AtkDelay && inBetweenShots && Target != -1)
				{
					Vector2 vector3 = npc.DirectionTo(Main.npc[Target].Center);
					if (vector3.Y <= 0.5f && vector3.Y >= -0.5f)
					{
						npc.ai[2] = vector3.Y;
					}
				}
				if (npc.ai[1] <= 0f)
				{
					npc.ai[0] = (npc.localAI[2] == 8f && HasTarget) ? 8 : 0;
					npc.ai[1] = AtkCD + Main.rand.Next(AtkRandExtraCD);
					npc.ai[2] = 0f;
					npc.localAI[1] = (npc.localAI[3] = AtkCD / 2 + Main.rand.Next(AtkRandExtraCD));
					npc.netUpdate = true;
				}
			}
			else if (npc.ai[0] == NurseHeal)             //护士治疗
			{
				npc.velocity.X *= 0.8f;
				if (NPCID.Sets.AttackTime[npc.type] == npc.ai[1])
				{
					npc.frameCounter = 0.0;
				}
				npc.ai[1] -= 1f;
				npc.localAI[3] += 1f;
				if (npc.localAI[3] == 1f && Main.netMode != NetmodeID.MultiplayerClient)
				{
					Vector2 ShootVel = npc.DirectionTo(Main.npc[(int)npc.ai[2]].Center + new Vector2(0f, -20f));
					if (ShootVel.HasNaNs() || Math.Sign(ShootVel.X) == -npc.spriteDirection)
					{
						ShootVel = new Vector2(npc.spriteDirection, -1f);
					}
					ShootVel *= 8f;
					int protmp = Projectile.NewProjectile(npc.Center.X + npc.spriteDirection * 16, npc.Center.Y - 2f, ShootVel.X, ShootVel.Y, ProjectileID.NurseSyringeHeal, 0, 0f, Main.myPlayer, npc.ai[2], 0f);
					Main.projectile[protmp].npcProj = true;
					Main.projectile[protmp].noDropItem = true;
				}
				if (npc.ai[1] <= 0f)
				{
					npc.ai[0] = 0f;
					npc.ai[1] = 10 + Main.rand.Next(10);
					npc.ai[2] = 0f;
					npc.localAI[3] = 5 + Main.rand.Next(10);
					npc.netUpdate = true;
				}
			}
			else if (npc.ai[0] == MageAtk)           //法师攻击
			{
				int ProjType = 0;
				int ProjDmg = 0;
				float SpeedMultiplier = 0f;
				int AtkDelay = 0;
				int AtkCD = 0;
				int AtkRandExtraCD = 0;
				float knockBack3 = 0f;
				float gravityCorrection = 0f;
				float DetectRange = NPCID.Sets.DangerDetectRange[npc.type];
				float auraLightMultiplier = 1f;
				float randomOffset = 0f;
				if (NPCID.Sets.AttackTime[npc.type] == npc.ai[1])
				{
					npc.frameCounter = 0.0;
					npc.localAI[3] = 0f;
				}
				int Target = -1;
				if (ShootDir == 1 && npc.spriteDirection == 1)
				{
					Target = TargetRight;
				}
				if (ShootDir == -1 && npc.spriteDirection == -1)
				{
					Target = TargetLeft;
				}
				if (npc.type == NPCID.Clothier)
				{
					ProjType = ProjectileID.ClothiersCurse;
					SpeedMultiplier = 10f;
					ProjDmg = 16;
					AtkDelay = 30;
					AtkCD = 20;
					AtkRandExtraCD = 15;
					knockBack3 = 2f;
					randomOffset = 1f;
				}
				else if (npc.type == NPCID.Wizard)
				{
					ProjType = ProjectileID.BallofFire;
					SpeedMultiplier = 6f;
					ProjDmg = 18;
					AtkDelay = 15;
					AtkCD = 15;
					AtkRandExtraCD = 5;
					knockBack3 = 3f;
					gravityCorrection = 20f;
				}
				else if (npc.type == NPCID.Truffle)
				{
					ProjType = ProjectileID.TruffleSpore;
					ProjDmg = 40;
					AtkDelay = 15;
					AtkCD = 10;
					AtkRandExtraCD = 1;
					knockBack3 = 3f;
					while (npc.localAI[3] > AtkDelay)
					{
						AtkDelay += 15;
					}
				}
				else if (npc.type == NPCID.Dryad)
				{
					ProjType = ProjectileID.DryadsWardCircle;
					AtkDelay = 24;
					AtkCD = 10;
					AtkRandExtraCD = 1;
					knockBack3 = 3f;
				}
				NPCLoader.TownNPCAttackStrength(npc, ref ProjDmg, ref knockBack3);
				NPCLoader.TownNPCAttackCooldown(npc, ref AtkCD, ref AtkRandExtraCD);
				NPCLoader.TownNPCAttackProj(npc, ref ProjType, ref AtkDelay);
				NPCLoader.TownNPCAttackProjSpeed(npc, ref SpeedMultiplier, ref gravityCorrection, ref randomOffset);
				NPCLoader.TownNPCAttackMagic(npc, ref auraLightMultiplier);
				if (Main.expertMode)
				{
					ProjDmg = (int)(ProjDmg * Main.expertNPCDamage);
				}
				ProjDmg = (int)(ProjDmg * dmgMult);
				npc.velocity.X *= 0.8f;
				npc.ai[1] -= 1f;
				npc.localAI[3] += 1f;
				if (npc.localAI[3] == AtkDelay && Main.netMode != NetmodeID.MultiplayerClient)
				{
					Vector2 vector5 = Vector2.Zero;
					if (Target != -1)
					{
						float dist = Main.npc[Target].Distance(npc.Center);
						if (dist > DetectRange) DetectRange = dist;
						vector5 = npc.DirectionTo(Main.npc[Target].Center + new Vector2(0f, -gravityCorrection * MathHelper.Clamp(npc.Distance(Main.npc[Target].Center) / DetectRange, 0f, 1f)));
					}
					if (vector5.HasNaNs() || Math.Sign(vector5.X) != npc.spriteDirection)
					{
						vector5 = new Vector2(npc.spriteDirection, 0f);
					}
					vector5 *= SpeedMultiplier;
					vector5 += Utils.RandomVector2(Main.rand, -randomOffset, randomOffset);
					if (npc.type == NPCID.Wizard)
					{
						int AtkAmount = Utils.SelectRandom(Main.rand, new int[]
						{
							1,
							1,
							1,
							1,
							2,
							2,
							3
						});
						for (int i = 0; i < AtkAmount; i++)
						{
							Vector2 vector6 = Utils.RandomVector2(Main.rand, -3.4f, 3.4f);
							int protmp = Projectile.NewProjectile(npc.Center.X + npc.spriteDirection * 16, npc.Center.Y - 2f, vector5.X + vector6.X, vector5.Y + vector6.Y, ProjType, ProjDmg, knockBack3, Main.myPlayer, 0f, 0f);
							Main.projectile[protmp].npcProj = true;
							Main.projectile[protmp].noDropItem = true;
							Main.projectile[protmp].usesLocalNPCImmunity = true;
							Main.projectile[protmp].localNPCHitCooldown = 10;
						}
					}
					else if (npc.type == NPCID.Truffle)
					{
						if (Target != -1)
						{
							Vector2 vector7 = Main.npc[Target].position - Main.npc[Target].Size * 2f + Main.npc[Target].Size * Utils.RandomVector2(Main.rand, 0f, 1f) * 5f;
							int TryCount = 10;
							while (TryCount > 0 && WorldGen.SolidTile(Framing.GetTileSafely((int)vector7.X / 16, (int)vector7.Y / 16)))
							{
								TryCount--;
								vector7 = Main.npc[Target].position - Main.npc[Target].Size * 2f + Main.npc[Target].Size * Utils.RandomVector2(Main.rand, 0f, 1f) * 5f;
							}
							int protmp = Projectile.NewProjectile(vector7.X, vector7.Y, 0f, 0f, ProjType, ProjDmg, knockBack3, Main.myPlayer, 0f, 0f);
							Main.projectile[protmp].npcProj = true;
							Main.projectile[protmp].noDropItem = true;
							Main.projectile[protmp].usesLocalNPCImmunity = true;
							Main.projectile[protmp].localNPCHitCooldown = 10;
						}
					}
					else if (npc.type == NPCID.Dryad)
					{
						int protmp = Projectile.NewProjectile(npc.Center.X + npc.spriteDirection * 16, npc.Center.Y - 2f, vector5.X, vector5.Y, ProjType, ProjDmg, knockBack3, Main.myPlayer, 0f, npc.whoAmI);
						Main.projectile[protmp].npcProj = true;
						Main.projectile[protmp].noDropItem = true;
						Main.projectile[protmp].usesLocalNPCImmunity = true;
						Main.projectile[protmp].localNPCHitCooldown = 10;
					}
					else
					{
						int protmp = Projectile.NewProjectile(npc.Center.X + npc.spriteDirection * 16, npc.Center.Y - 2f, vector5.X, vector5.Y, ProjType, ProjDmg, knockBack3, Main.myPlayer, 0f, 0f);
						Main.projectile[protmp].npcProj = true;
						Main.projectile[protmp].noDropItem = true;
						Main.projectile[protmp].usesLocalNPCImmunity = true;
						Main.projectile[protmp].localNPCHitCooldown = 10;
					}
				}
				if (auraLightMultiplier > 0f)
				{
					Vector3 vector8 = NPCID.Sets.MagicAuraColor[npc.type].ToVector3() * auraLightMultiplier;
					Lighting.AddLight(npc.Center, vector8.X, vector8.Y, vector8.Z);
				}
				if (npc.ai[1] <= 0f)
				{
					npc.ai[0] = (npc.localAI[2] == 8f && HasTarget) ? 8 : 0;
					npc.ai[1] = AtkCD + Main.rand.Next(AtkRandExtraCD);
					npc.ai[2] = 0f;
					npc.localAI[1] = npc.localAI[3] = AtkCD / 2 + Main.rand.Next(AtkRandExtraCD);
					npc.netUpdate = true;
				}
			}
			else if (npc.ai[0] == MeleeAtk)                   //近战攻击
			{
				int AtkCD = 0;
				int AtkRandExtraCD = 0;
				if (NPCID.Sets.AttackTime[npc.type] == npc.ai[1])
				{
					npc.frameCounter = 0.0;
					npc.localAI[3] = 0f;
				}
				int AtkDmg = 0;
				float AtkKB = 0f;
				int itemWidth = 0;
				int itemHeight = 0;
				if (npc.type == NPCID.DyeTrader)
				{
					AtkDmg = 11;
					itemWidth = itemHeight = 32;
					AtkCD = 12;
					AtkRandExtraCD = 6;
					AtkKB = 4.25f;
				}
				else if (npc.type == NPCID.TaxCollector)
				{
					AtkDmg = 9;
					itemWidth = itemHeight = 28;
					AtkCD = 9;
					AtkRandExtraCD = 3;
					AtkKB = 3.5f;
				}
				else if (npc.type == NPCID.Stylist)
				{
					AtkDmg = 10;
					itemWidth = itemHeight = 32;
					AtkCD = 15;
					AtkRandExtraCD = 8;
					AtkKB = 5f;
				}
				NPCLoader.TownNPCAttackStrength(npc, ref AtkDmg, ref AtkKB);
				NPCLoader.TownNPCAttackCooldown(npc, ref AtkCD, ref AtkRandExtraCD);
				NPCLoader.TownNPCAttackSwing(npc, ref itemWidth, ref itemHeight);
				if (Main.expertMode)
				{
					AtkDmg = (int)(AtkDmg * Main.expertNPCDamage);
				}
				AtkDmg = (int)(AtkDmg * dmgMult);
				npc.velocity.X *= 0.8f;
				npc.ai[1] -= 1f;
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					Tuple<Vector2, float> swingStats = npc.GetSwingStats(NPCID.Sets.AttackTime[npc.type] * 2, (int)npc.ai[1], npc.spriteDirection, itemWidth, itemHeight);
					Rectangle rectangle3 = new Rectangle((int)swingStats.Item1.X, (int)swingStats.Item1.Y, itemWidth, itemHeight);
					if (npc.spriteDirection == -1)
					{
						rectangle3.X -= itemWidth;
					}
					rectangle3.Y -= itemHeight;
					npc.TweakSwingStats(NPCID.Sets.AttackTime[npc.type] * 2, (int)npc.ai[1], npc.spriteDirection, ref rectangle3);

					if (BuffNPC())
					{
						foreach (NPC target in Main.npc)
						{
							if (target.active && target.immune[npc.whoAmI] == 0 && !target.dontTakeDamage && !target.friendly && rectangle3.Intersects(target.Hitbox) && (target.noTileCollide || Collision.CanHit(npc.position, npc.width, npc.height, target.position, target.width, target.height)))
							{
								bool crit = Main.rand.Next(100) <= Main.LocalPlayer.meleeCrit;
								target.StrikeNPCNoInteraction(AtkDmg, AtkKB, npc.spriteDirection, crit, false, false);
								if (Main.netMode != NetmodeID.SinglePlayer)
								{
									NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, itemHeight, AtkDmg, AtkKB, npc.spriteDirection, 0, 0, 0);
								}
								target.netUpdate = true;
								target.immune[npc.whoAmI] = (int)npc.ai[1] + 2;
							}
						}
					}
                    else
                    {
						int myPlayer = Main.myPlayer;
						foreach (NPC target in Main.npc)
						{
							if (target.active && target.immune[myPlayer] == 0 && !target.dontTakeDamage && !target.friendly && target.damage > 0 && rectangle3.Intersects(target.Hitbox) && (target.noTileCollide || Collision.CanHit(npc.position, npc.width, npc.height, target.position, target.width, target.height)))
							{
								target.StrikeNPCNoInteraction(AtkDmg, AtkKB, npc.spriteDirection, false, false, false);
								if (Main.netMode != NetmodeID.SinglePlayer)
								{
									NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, itemHeight, AtkDmg, AtkKB, npc.spriteDirection, 0, 0, 0);
								}
								target.netUpdate = true;
								target.immune[myPlayer] = (int)npc.ai[1] + 2;
							}
						}
					}
				}
				if (npc.ai[1] <= 0f)
				{
					bool StopAtk = false;
					if (HasTarget)
					{
						if (!Collision.CanHit(npc.Center, 0, 0, npc.Center + Vector2.UnitX * -ShootDir * 32f, 0, 0) || npc.localAI[2] == 8f)
						{
							StopAtk = true;
						}
						if (StopAtk)
						{
							int TargetRightMelee = (ShootDir == 1) ? TargetRight : TargetLeft;
							int TargetLeftMelee = (ShootDir == 1) ? TargetLeft : TargetRight;
							if (TargetRightMelee != -1 && !Collision.CanHit(npc.Center, 0, 0, Main.npc[TargetRightMelee].Center, 0, 0))
							{
								if (TargetLeftMelee != -1 && Collision.CanHit(npc.Center, 0, 0, Main.npc[TargetLeftMelee].Center, 0, 0))
								{
									TargetRightMelee = TargetLeftMelee;
								}
								else
								{
									TargetRightMelee = -1;
								}
							}
							if (TargetRightMelee != -1)
							{
								if (AttackMode())
								{
									bool HasEnemyOnRoad = false;
									Rectangle MeleeHitbox;
                                    if (npc.direction <= 0)
                                    {
										MeleeHitbox = new Rectangle((int)npc.position.X - 30, (int)npc.position.Y, npc.width + 30, npc.height);
									}
                                    else
                                    {
										MeleeHitbox = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width + 30, npc.height);
									}
									foreach(NPC target in Main.npc)
                                    {
										if (target.active && !target.dontTakeDamage && (target.damage > 0 || target.lifeMax > 5) && !target.friendly && MeleeHitbox.Intersects(target.Hitbox) && (target.noTileCollide || Collision.CanHit(npc.position, npc.width, npc.height, target.position, target.width, target.height)))
										{
											HasEnemyOnRoad = true;
											break;
                                        }
									}
									if (HasEnemyOnRoad || Math.Abs(Main.npc[TargetRightMelee].Center.X - npc.Center.X) < 30)
									{
										npc.ai[0] = MeleeAtk;
										npc.ai[1] = NPCID.Sets.AttackTime[npc.type];
										npc.ai[2] = 0f;
										npc.localAI[3] = 0f;
										npc.direction = (npc.position.X < Main.npc[TargetRightMelee].position.X) ? 1 : -1;
										npc.netUpdate = true;
									}
									else
									{
										StopAtk = false;
									}
								}
								else
								{
									npc.ai[0] = MeleeAtk;
									npc.ai[1] = NPCID.Sets.AttackTime[npc.type];
									npc.ai[2] = 0f;
									npc.localAI[3] = 0f;
									npc.direction = (npc.position.X < Main.npc[TargetRightMelee].position.X) ? 1 : -1;
									npc.netUpdate = true;
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
						if (BuffNPC())
						{
							AtkCD = (int)(AtkCD / Main.LocalPlayer.meleeSpeed);
							AtkRandExtraCD = (int)(AtkRandExtraCD / Main.LocalPlayer.meleeSpeed);
						}
						npc.ai[0] = (npc.localAI[2] == 8f && HasTarget) ? WalkingForBattle : 0;
						npc.ai[1] = AtkCD + Main.rand.Next(AtkRandExtraCD);
						npc.ai[2] = 0f;
						npc.localAI[1] = npc.localAI[3] = AtkCD / 2 + Main.rand.Next(AtkRandExtraCD);
						npc.netUpdate = true;
					}
				}
			}


			//NPC间交谈
			if (Main.netMode != NetmodeID.MultiplayerClient && (npc.townNPC || npc.type == NPCID.SkeletonMerchant) && !TalkingToPlayer)
			{
				bool NotAtk = npc.ai[0] < Stop && !HasTarget;
				bool CanAtk = (npc.ai[0] < Stop || npc.ai[0] == WalkingForBattle) && (HasTarget || HasTarget2);
				if (npc.localAI[1] > 0f)
				{
					npc.localAI[1] -= 1f;
				}
				if (npc.localAI[1] > 0f)
				{
					CanAtk = false;
				}
				if (CanAtk && npc.type == NPCID.Mechanic && npc.localAI[0] == 1f)
				{
					CanAtk = false;
				}
				if (CanAtk && npc.type == NPCID.Dryad)
				{
					CanAtk = false;
					foreach (NPC friend in Main.npc)
					{
						if (friend.active && friend.townNPC && npc.Distance(friend.Center) <= 1200f && friend.FindBuffIndex(165) == -1)
						{
							CanAtk = true;
							break;
						}
					}
				}
				if (NotAtk && npc.ai[0] == Default && npc.velocity.Y == 0f && Main.rand.Next(300) == 0)
				{
					int talkTime = 420;       //不太确定
					if (Main.rand.Next(2) == 0)
					{
						talkTime *= Main.rand.Next(1, 4);
					}
					else
					{
						talkTime *= Main.rand.Next(1, 3);
					}
					foreach (NPC talkNPC in Main.npc)
					{
						bool flag23 = (talkNPC.ai[0] == Walking && talkNPC.closeDoor) || (talkNPC.ai[0] == Walking && talkNPC.ai[1] > 200f) || talkNPC.ai[0] > 1f;
						if (talkNPC != npc && talkNPC.active && talkNPC.CanTalk && !flag23 && talkNPC.Distance(npc.Center) < 100 && talkNPC.Distance(npc.Center) > 20 && Collision.CanHit(npc.Center, 0, 0, talkNPC.Center, 0, 0))
						{
							int dir = (npc.position.X < talkNPC.position.X).ToDirectionInt();
							npc.ai[0] = Talking1;
							npc.ai[1] = talkTime;
							npc.ai[2] = talkNPC.whoAmI;
							npc.direction = dir;
							npc.netUpdate = true;
							talkNPC.ai[0] = Talking2;
							talkNPC.ai[1] = talkTime;
							talkNPC.ai[2] = npc.whoAmI;
							talkNPC.direction = -dir;
							talkNPC.netUpdate = true;
							break;
						}
					}
				}
				else if (NotAtk && npc.ai[0] == Default && npc.velocity.Y == 0f && Main.rand.Next(1800) == 0)
				{
					int TalkTime = 420;
					if (Main.rand.Next(2) == 0)
					{
						TalkTime *= Main.rand.Next(1, 4);
					}
					else
					{
						TalkTime *= Main.rand.Next(1, 3);
					}
					foreach (NPC talkNPC in Main.npc)
					{
						bool flag24 = (talkNPC.ai[0] == Walking && talkNPC.closeDoor) || (talkNPC.ai[0] == Walking && talkNPC.ai[1] > 200f) || talkNPC.ai[0] > 1f;
						if (talkNPC != npc && talkNPC.active && talkNPC.CanTalk && !flag24 && talkNPC.Distance(npc.Center) < 100 && talkNPC.Distance(npc.Center) > 20 && Collision.CanHit(npc.Center, 0, 0, talkNPC.Center, 0, 0))
						{
							int dir = (npc.position.X < talkNPC.position.X).ToDirectionInt();
							npc.ai[0] = Talking3;
							npc.ai[1] = TalkTime;
							npc.ai[2] = talkNPC.whoAmI;
							npc.localAI[2] = Main.rand.Next(4);
							npc.localAI[3] = Main.rand.Next(3 - (int)npc.localAI[2]);
							npc.direction = dir;
							npc.netUpdate = true;
							talkNPC.ai[0] = Talking4;
							talkNPC.ai[1] = TalkTime;
							talkNPC.ai[2] = npc.whoAmI;
							talkNPC.localAI[2] = 0f;
							talkNPC.localAI[3] = 0f;
							talkNPC.direction = -dir;
							talkNPC.netUpdate = true;
							break;
						}
					}
				}
				else if (NotAtk && npc.ai[0] == Default && npc.velocity.Y == 0f && Main.rand.Next(1200) == 0 && (npc.type == NPCID.PartyGirl || (BirthdayParty.PartyIsUp && NPCID.Sets.AttackType[npc.type] == NPCID.Sets.AttackType[208])))
				{
					int TalkTime = 300;
					foreach (Player talkPlayer in Main.player)
					{
						if (talkPlayer.active && !talkPlayer.dead && talkPlayer.Distance(npc.Center) < 150 && Collision.CanHitLine(npc.Top, 0, 0, talkPlayer.Top, 0, 0))
						{
							int direction2 = (npc.position.X < talkPlayer.position.X).ToDirectionInt();
							npc.ai[0] = TalkingPartyToPlayer;
							npc.ai[1] = TalkTime;
							npc.ai[2] = talkPlayer.whoAmI;
							npc.direction = direction2;
							npc.netUpdate = true;
							break;
						}
					}
				}
				else if (NotAtk && npc.ai[0] == Default && npc.velocity.Y == 0f && Main.rand.Next(600) == 0 && npc.type == NPCID.DD2Bartender)
				{
					int TalkTime = 300;
					foreach (Player talkPlayer in Main.player)
					{
						if (talkPlayer.active && !talkPlayer.dead && talkPlayer.Distance(npc.Center) < 150 && Collision.CanHitLine(npc.Top, 0, 0, talkPlayer.Top, 0, 0))
						{
							int direction3 = (npc.position.X < talkPlayer.position.X).ToDirectionInt();
							npc.ai[0] = TalkingBartenderToPlayer;
							npc.ai[1] = TalkTime;
							npc.ai[2] = talkPlayer.whoAmI;
							npc.direction = direction3;
							npc.netUpdate = true;
							break;
						}
					}
				}
				else if (NotAtk && npc.ai[0] == Default && npc.velocity.Y == 0f && Main.rand.Next(1800) == 0)
				{
					npc.ai[0] = Stop;
					npc.ai[1] = 45 * Main.rand.Next(1, 2);
					npc.netUpdate = true;
				}
				else if (NotAtk && npc.ai[0] == Default && npc.velocity.Y == 0f && Main.rand.Next(600) == 0 && npc.type == NPCID.Pirate && !HasTarget2)
				{
					npc.ai[0] = PriateSpecial;
					npc.ai[1] = 30 * Main.rand.Next(1, 4);
					npc.netUpdate = true;
				}
				else if (NotAtk && npc.ai[0] == Default && npc.velocity.Y == 0f && Main.rand.Next(1200) == 0)
				{
					int TalkTime = 220;
					foreach (Player player in Main.player)
					{
						if (player.active && !player.dead && player.Distance(npc.Center) < 150 && Collision.CanHitLine(npc.Top, 0, 0, player.Top, 0, 0))
						{
							int direction4 = (npc.position.X < player.position.X).ToDirectionInt();
							npc.ai[0] = NPCAttacker.TalkingToPlayer;
							npc.ai[1] = TalkTime;
							npc.ai[2] = player.whoAmI;
							npc.direction = direction4;
							npc.netUpdate = true;
							break;
						}
					}
				}
				else if (NotAtk && npc.ai[0] == Walking && npc.velocity.Y == 0f && Main.rand.Next(300) == 0)            //你坐啊NPC
				{
					Point NPCTileCoord = npc.Center.ToTileCoordinates();
					bool CanSit = WorldGen.InWorld(NPCTileCoord.X, NPCTileCoord.Y, 1);
					if (CanSit)
					{
						foreach (NPC friend in Main.npc)
						{
							if (friend.active && friend.aiStyle == 7 && friend.townNPC && friend.ai[0] == Sit)
							{
								if (friend.Center.ToTileCoordinates() == NPCTileCoord)
								{
									CanSit = false;
									break;
								}
							}
						}
					}
					if (CanSit)
					{
						Tile Chair = Main.tile[NPCTileCoord.X, NPCTileCoord.Y];
						CanSit = Chair.type == TileID.Chairs;
						if (CanSit && Chair.frameY == 1080)
						{
							CanSit = false;
						}
						if (CanSit)
						{
							npc.ai[0] = Sit;
							npc.ai[1] = 900 + Main.rand.Next(10800);
							npc.direction = (Chair.frameX == 0) ? -1 : 1;
							npc.Bottom = new Vector2(NPCTileCoord.X * 16 + 8 + 2 * npc.direction, NPCTileCoord.Y * 16 + 32);
							npc.velocity = Vector2.Zero;
							npc.localAI[3] = 0f;
							npc.netUpdate = true;
						}
					}
				}
				//神奇互动（？
				else if (NotAtk && npc.ai[0] == Walking && npc.velocity.Y == 0f && Main.rand.Next(600) == 0 && Utils.PlotTileLine(npc.Top, npc.Bottom, npc.width, new Utils.PerLinePoint(DelegateMethods.SearchAvoidedByNPCs)))
				{
					Point NPCFrontCoord = (npc.Center + new Vector2(npc.direction * 10, 0f)).ToTileCoordinates();
					bool CanInteract = WorldGen.InWorld(NPCFrontCoord.X, NPCFrontCoord.Y, 1);
					if (CanInteract)
					{
						Tile tileSafely6 = Framing.GetTileSafely(NPCFrontCoord.X, NPCFrontCoord.Y);
						if (!tileSafely6.nactive() || !TileID.Sets.InteractibleByNPCs[tileSafely6.type])
						{
							CanInteract = false;
						}
					}
					if (CanInteract)
					{
						npc.ai[0] = Interacting;
						npc.ai[1] = 40 + Main.rand.Next(90);
						npc.velocity = Vector2.Zero;
						npc.localAI[3] = 0f;
						npc.netUpdate = true;
					}
				}
				//护士治疗
				if (npc.ai[0] < 2f && npc.velocity.Y == 0f && npc.type == NPCID.Nurse)
				{
					int HealTarget = -1;
					foreach (NPC target in Main.npc)
					{
						if (target.active && target.townNPC && target.life != target.lifeMax && (HealTarget == -1 || target.lifeMax - target.life > Main.npc[HealTarget].lifeMax - Main.npc[HealTarget].life) && Collision.CanHitLine(npc.position, npc.width, npc.height, target.position, target.width, target.height) && npc.Distance(target.Center) < 500f)
						{
							HealTarget = target.whoAmI;
						}
					}
					if (AssembleMode())
                    {
						if (Math.Abs(npc.Center.X - Main.npc[NPC.FindFirstNPC(ModContent.NPCType<AssembleNPC>())].Center.X) > 50)
						{
							HealTarget = -1; 
						}
                    }
					if (HealTarget != -1)
					{
						npc.ai[0] = NurseHeal;
						npc.ai[1] = 34f;
						npc.ai[2] = HealTarget;
						npc.localAI[3] = 0f;
						npc.direction = (npc.position.X < Main.npc[HealTarget].position.X) ? 1 : -1;
						npc.netUpdate = true;
					}
				}
				//投掷
				if (CanAtk && NPCID.Sets.AttackType[npc.type] == 0 && (AttackMode() || (npc.velocity.Y == 0f && NPCID.Sets.AttackAverageChance[npc.type] > 0 && Main.rand.Next(NPCID.Sets.AttackAverageChance[npc.type] * 2) == 0)))
				{
					int AttackTime = NPCID.Sets.AttackTime[npc.type];
					int TargetRight2 = (ShootDir == 1) ? TargetRight : TargetLeft;
					int TargetLeft2 = (ShootDir == 1) ? TargetLeft : TargetRight;
					if (TargetRight2 != -1 && !Collision.CanHit(npc.Center, 0, 0, Main.npc[TargetRight2].Center, 0, 0))
					{
						if (TargetLeft2 != -1 && Collision.CanHit(npc.Center, 0, 0, Main.npc[TargetLeft2].Center, 0, 0))
						{
							TargetRight2 = TargetLeft2;
						}
						else
						{
							TargetRight2 = -1;
						}
					}
					if (TargetRight2 != -1)
					{
						npc.localAI[2] = npc.ai[0];
						npc.ai[0] = ThrowerAtk;
						npc.ai[1] = AttackTime;
						npc.ai[2] = 0f;
						npc.localAI[3] = 0f;
						npc.direction = (npc.position.X < Main.npc[TargetRight].position.X) ? 1 : -1;
						npc.netUpdate = true;
					}
				}
				//远程
				else if (CanAtk && NPCID.Sets.AttackType[npc.type] == 1 && (AttackMode() || (npc.velocity.Y == 0f && NPCID.Sets.AttackAverageChance[npc.type] > 0 && Main.rand.Next(NPCID.Sets.AttackAverageChance[npc.type] * 2) == 0)))
				{
					int AttackTime = NPCID.Sets.AttackTime[npc.type];
					int TargetRight2 = (ShootDir == 1) ? TargetRight : TargetLeft;
					int TargetLeft2 = (ShootDir == 1) ? TargetLeft : TargetRight;
					if (TargetRight2 != -1 && !Collision.CanHitLine(npc.Center, 0, 0, Main.npc[TargetRight2].Center, 0, 0))
					{
						if (TargetLeft2 != -1 && Collision.CanHitLine(npc.Center, 0, 0, Main.npc[TargetLeft2].Center, 0, 0))
						{
							TargetRight2 = TargetLeft2;
						}
						else
						{
							TargetRight2 = -1;
						}
					}
					if (TargetRight2 != -1)
					{
						Vector2 vector9 = npc.DirectionTo(Main.npc[TargetRight2].Center);
						//if (vector9.Y <= 0.5f && vector9.Y >= -0.5f)
						{
							npc.localAI[2] = npc.ai[0];
							npc.ai[0] = RangerAtk;
							npc.ai[1] = AttackTime;
							npc.ai[2] = vector9.Y;
							npc.localAI[3] = 0f;
							npc.direction = ((npc.position.X < Main.npc[TargetRight2].position.X) ? 1 : -1);
							npc.netUpdate = true;
						}
					}
				}
				//魔法
				if (CanAtk && NPCID.Sets.AttackType[npc.type] == 2 && (AttackMode() || (npc.velocity.Y == 0f && NPCID.Sets.AttackAverageChance[npc.type] > 0 && Main.rand.Next(NPCID.Sets.AttackAverageChance[npc.type] * 2) == 0)))
				{
					int AttackTime = NPCID.Sets.AttackTime[npc.type];
					int TargetRight2 = (ShootDir == 1) ? TargetRight : TargetLeft;
					int TargetLeft2 = (ShootDir == 1) ? TargetLeft : TargetRight;
					if (TargetRight2 != -1 && !Collision.CanHitLine(npc.Center, 0, 0, Main.npc[TargetRight2].Center, 0, 0))
					{
						if (TargetLeft2 != -1 && Collision.CanHitLine(npc.Center, 0, 0, Main.npc[TargetLeft2].Center, 0, 0))
						{
							TargetRight2 = TargetLeft2;
						}
						else
						{
							TargetRight2 = -1;
						}
					}
					if (TargetRight2 != -1)
					{
						npc.localAI[2] = npc.ai[0];
						npc.ai[0] = MageAtk;
						npc.ai[1] = AttackTime;
						npc.ai[2] = 0f;
						npc.localAI[3] = 0f;
						npc.direction = (npc.position.X < Main.npc[TargetRight2].position.X) ? 1 : -1;
						npc.netUpdate = true;
					}
					else if (npc.type == NPCID.Dryad)
					{
						npc.localAI[2] = npc.ai[0];
						npc.ai[0] = MageAtk;
						npc.ai[1] = AttackTime;
						npc.ai[2] = 0f;
						npc.localAI[3] = 0f;
						npc.netUpdate = true;
					}
				}
				//近战
				if (CanAtk && NPCID.Sets.AttackType[npc.type] == 3 && (AttackMode() || (npc.velocity.Y == 0f && NPCID.Sets.AttackAverageChance[npc.type] > 0 && Main.rand.Next(NPCID.Sets.AttackAverageChance[npc.type] * 2) == 0)))
				{
					int AttackTime = NPCID.Sets.AttackTime[npc.type];
					int TargetRight2 = (ShootDir == 1) ? TargetRight : TargetLeft;
					int TargetLeft2 = (ShootDir == 1) ? TargetLeft : TargetRight;
					if (TargetRight2 != -1 && !Collision.CanHit(npc.Center, 0, 0, Main.npc[TargetRight2].Center, 0, 0))
					{
						if (TargetLeft2 != -1 && Collision.CanHit(npc.Center, 0, 0, Main.npc[TargetLeft2].Center, 0, 0))
						{
							TargetRight2 = TargetLeft2;
						}
						else
						{
							TargetRight2 = -1;
						}
					}
					if (TargetRight2 != -1)
					{
						if (AttackMode())
						{
							bool HasEnemyOnRoad = false;
							Rectangle MeleeHitbox;
							if (npc.direction <= 0)
							{
								MeleeHitbox = new Rectangle((int)npc.position.X - 30, (int)npc.position.Y, npc.width + 30, npc.height);
							}
							else
							{
								MeleeHitbox = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width + 30, npc.height);
							}
							foreach (NPC target in Main.npc)
							{
								if (target.active && !target.dontTakeDamage && (target.damage > 0 || target.lifeMax > 5) && !target.friendly && MeleeHitbox.Intersects(target.Hitbox) && (target.noTileCollide || Collision.CanHit(npc.position, npc.width, npc.height, target.position, target.width, target.height)))
								{
									HasEnemyOnRoad = true;
									break;
								}
							}
							if (HasEnemyOnRoad || Math.Abs(Main.npc[TargetRight2].Center.X - npc.Center.X) < 30)
							{
								npc.localAI[2] = npc.ai[0];
								npc.ai[0] = MeleeAtk;
								npc.ai[1] = AttackTime;
								npc.ai[2] = 0f;
								npc.localAI[3] = 0f;
								npc.direction = (npc.position.X < Main.npc[TargetRight2].position.X) ? 1 : -1;
								npc.netUpdate = true;
							}
						}
						else
						{
							npc.localAI[2] = npc.ai[0];
							npc.ai[0] = MeleeAtk;
							npc.ai[1] = AttackTime;
							npc.ai[2] = 0f;
							npc.localAI[3] = 0f;
							npc.direction = (npc.position.X < Main.npc[TargetRight2].position.X) ? 1 : -1;
							npc.netUpdate = true;
						}
					}
				}
			}
		}


		public static bool AttackMode()
        {
			return NPC.AnyNPCs(ModContent.NPCType<TargetNPC>()) && !NPC.AnyNPCs(ModContent.NPCType<AssembleNPC>());
        }

		public static bool AssembleMode()
        {
			return !NPC.AnyNPCs(ModContent.NPCType<TargetNPC>()) && NPC.AnyNPCs(ModContent.NPCType<AssembleNPC>());
		}

		public static bool NoMode()
        {
			return !NPC.AnyNPCs(ModContent.NPCType<TargetNPC>()) && !NPC.AnyNPCs(ModContent.NPCType<AssembleNPC>());
		}

		public static bool BuffNPC()
        {
			return Main.LocalPlayer.HeldItem.type == ModContent.ItemType<AttackerStick>();
        }
	}
}