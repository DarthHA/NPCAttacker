using Microsoft.Xna.Framework;
using NPCAttacker.NPCs;
using NPCAttacker.UI;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace NPCAttacker
{
    public class NPCAttacker : Mod
	{
		

		public static string FocusText1 = "";
		public static string FocusText3 = "";

		public UINPCExtraButton _UINPCExtraButton;
		public UserInterface _UINPCExtraButtonUserInterface;


		public ArmUI _ArmUI;
		public UserInterface _ArmUserInterface;

		public static NPCAttacker Instance;

		public NPCAttacker()
        {
			Instance = this;
        }

		public override void Load()
        {
			On.Terraria.NPC.AI_007_TownEntities += AIHook;
			On.Terraria.NPC.StrikeNPC += StrikeNPCHook;
			On.Terraria.Main.DrawNPCChatButtons += DrawNPCChatButtonsHook;

			_UINPCExtraButton = new UINPCExtraButton();
			_UINPCExtraButton.Activate();
			_UINPCExtraButtonUserInterface = new UserInterface();
			_UINPCExtraButtonUserInterface.SetState(_UINPCExtraButton);

			_ArmUI = new ArmUI();
			_ArmUI.Activate();
			_ArmUserInterface = new UserInterface();
			_ArmUserInterface.SetState(_ArmUI);

			AddTrans();
		}

        public override void Unload()
        {
			On.Terraria.NPC.AI_007_TownEntities -= AIHook;
			On.Terraria.NPC.StrikeNPC -= StrikeNPCHook;
			On.Terraria.Main.DrawNPCChatButtons -= DrawNPCChatButtonsHook;
			Instance = null;
		}

		public override void UpdateUI(GameTime gameTime)
		{
			if (ArmUI.Visible)
			{
				_ArmUserInterface?.Update(gameTime);
			}
			if (UINPCExtraButton.Visible)
			{
				_UINPCExtraButtonUserInterface?.Update(gameTime);
			}
			base.UpdateUI(gameTime);
		}


		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int MouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Death Text"));
			if (MouseTextIndex != -1)
			{
				layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
				    "NPCAttacker : NPCExtraButton",
					delegate
					{
					    if (UINPCExtraButton.Visible)
						{
							_UINPCExtraButton.Draw(Main.spriteBatch);
						}
						return true;
					},
					InterfaceScaleType.UI)
			   );
			}

			int inventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
			if (inventoryIndex != -1)
			{
				layers.Insert(inventoryIndex, new LegacyGameInterfaceLayer(
					"NPCAttacker : ArmUI",
					delegate 
					{
						if (ArmUI.Visible)
						{
							_ArmUserInterface.Draw(Main.spriteBatch, new GameTime());
						}
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
			base.ModifyInterfaceLayers(layers);
		}

		public static double StrikeNPCHook(On.Terraria.NPC.orig_StrikeNPC orig,NPC self,int Damage, float knockBack, int hitDirection, bool crit = false, bool noEffect = false, bool fromNet = false)
		{
			if ((!self.townNPC && self.type != NPCID.SkeletonMerchant) || SomeUtils.NoMode())
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
				if (!SomeUtils.BuffNPC())
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

				if (self.townNPC && SomeUtils.AttackMode())
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
			if (!SomeUtils.NoMode() && (self.townNPC || self.type == NPCID.SkeletonMerchant))
			{
				Rectangle Screen = new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight);
				if (self.Hitbox.Intersects(Screen))
				{
					NPCOverrideAI.AI_007_TownEntities(self);
					return;
				}
			}
			orig.Invoke(self);
		}


		public static void DrawNPCChatButtonsHook(On.Terraria.Main.orig_DrawNPCChatButtons orig, int superColor, Color chatColor, int numLines, string focusText, string focusText3)
		{
			FocusText1 = focusText;
			FocusText3 = focusText3;
			orig.Invoke(superColor, chatColor, numLines, focusText, focusText3);
		}





		public void AddTrans()
        {
			TranslationUtils.AddTranslation("Arm", "武装");
			TranslationUtils.AddTranslation("ArmUIHoverText", "Arm this NPC", "武装该NPC");
			TranslationUtils.AddTranslation("ArmUIarmered", "Remove this weapon here to disarm this NPC", "将该物品移除以解除NPC武装");
			TranslationUtils.AddTranslation("ArmUIunarmered1", "Place a weapon here to arm this NPC", "放置武器以武装该NPC");
			TranslationUtils.AddTranslation("ArmUIClassMelee", "This NPC can be equipped with broadsword-like melee weapon[i:426]", "该NPC可以装备阔剑型的近战武器[i:426]");
			TranslationUtils.AddTranslation("ArmUIClassRanged", "This NPC can be equipped with ranged weapon[i:533],but you need to prepare enough ammos for it[i:1302]", "该NPC可以装备远程武器[i:533],但你得准备足够多的弹药[i:1302]");
			TranslationUtils.AddTranslation("ArmUIClassMagic", "This NPC can be equipped with magic weapon[i:1931]", "该NPC可以装备魔法武器[i:1931]");
			TranslationUtils.AddTranslation("ArmUIClassThrown", "This NPC can be equipped with enough thrown weapon[i:42]", "该NPC可以装备足够多的投掷武器[i:42]");
			TranslationUtils.AddTranslation("ArmUIClassNurse", "This NPC can be equipped with enough healing or buff potion[i:188]", "该NPC可以装备足够多的治疗或者增益药水[i:188]");
			TranslationUtils.AddTranslation("ArmUINote", "Note: It may be difficult for NPCs to use some special weapon[i:3541]", "注意：NPC难以使用比较特别的武器[i:3541]");
		}


	}


	
}