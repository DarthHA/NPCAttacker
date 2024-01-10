using Microsoft.Xna.Framework;
using NPCAttacker.Items;
using NPCAttacker.NPCs;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace NPCAttacker.UI
{
    public class AttackerUIManager : ModSystem
    {

        public UINPCExtraButton _UINPCExtraButton;
        public UserInterface _UINPCExtraButtonUserInterface;


        public ArmUI _ArmUI;
        public UserInterface _ArmUserInterface;

        public ArmAltUI _ArmAltUI;
        public UserInterface _ArmAltUserInterface;

        public ArmorUI _ArmorUI;
        public UserInterface _ArmorUserInterface;

        public override void Load()
        {
            _UINPCExtraButton = new UINPCExtraButton();
            _UINPCExtraButton.Activate();
            _UINPCExtraButtonUserInterface = new UserInterface();
            _UINPCExtraButtonUserInterface.SetState(_UINPCExtraButton);

            _ArmUI = new ArmUI();
            _ArmUI.Activate();
            _ArmUserInterface = new UserInterface();
            _ArmUserInterface.SetState(_ArmUI);

            _ArmAltUI = new ArmAltUI();
            _ArmAltUI.Activate();
            _ArmAltUserInterface = new UserInterface();
            _ArmAltUserInterface.SetState(_ArmAltUI);

            _ArmorUI = new ArmorUI();
            _ArmorUI.Activate();
            _ArmorUserInterface = new UserInterface();
            _ArmorUserInterface.SetState(_ArmorUI);
        }


        public override void UpdateUI(GameTime gameTime)
        {
            UpdatePlayerUI(Main.LocalPlayer);

            if (ArmUI.Visible)
            {
                _ArmUserInterface?.Update(gameTime);
            }
            if (ArmAltUI.Visible)
            {
                _ArmAltUserInterface?.Update(gameTime);
            }
            if (ArmorUI.Visible)
            {
                _ArmorUserInterface?.Update(gameTime);
            }
            if (UINPCExtraButton.Visible)
            {
                _UINPCExtraButtonUserInterface?.Update(gameTime);
            }

            base.UpdateUI(gameTime);
        }


        public void UpdatePlayerUI(Player player)
        {
            if (player.talkNPC != -1)
            {
                if (Main.npc[player.talkNPC].IsTownNPC())
                {
                    if (!ArmUI.Visible && !ArmAltUI.Visible && !ArmorUI.Visible)
                    {
                        if (NPCUtils.BuffNPC())
                        {
                            UINPCExtraButton.Visible = true;
                        }
                        else
                        {
                            UINPCExtraButton.Visible = false;
                        }
                    }
                    else
                    {
                        UINPCExtraButton.Visible = false;
                    }
                    if (!Main.playerInventory)
                    {
                        ArmUI.CloseUI();
                        ArmAltUI.CloseUI();
                        ArmorUI.CloseUI();
                    }
                    if (Main.npcChatText == "")
                    {
                        UINPCExtraButton.Visible = false;
                    }
                }
                else
                {
                    ArmUI.CloseUI();
                    ArmAltUI.CloseUI();
                    ArmorUI.CloseUI();
                    UINPCExtraButton.Visible = false;
                }
            }
            else
            {
                ArmUI.CloseUI();
                ArmAltUI.CloseUI();
                ArmorUI.CloseUI();
                UINPCExtraButton.Visible = false;
            }

            if (!UIUtils.CanUseMapDrawing())
            {
                MapSystem.TargetSelected = false;
                MapSystem.StatueSelected = false;
                NPCAttacker.CurserInMap = false;
            }

            NPCAttacker.CurserType = 0;

            if (player.HeldItem.type == ModContent.ItemType<AttackerStick>())        //修改世界光标改变
            {
                bool attack = NPCAttacker.ForceAttackKey.Current;
                bool statue = false;
                bool selected = NPCUtils.AnyNPCSelected();
                if (!NPCAttacker.CurserInMap)
                {
                    foreach (NPC npc in Main.npc)
                    {
                        if (npc.active)
                        {
                            if (npc.CanBeChasedBy() && npc.type != ModContent.NPCType<AssembleNPC>() && npc.type != ModContent.NPCType<AttackTargetNPC>())
                            {
                                if (npc.Hitbox.Contains(Main.MouseWorld.ToPoint()))
                                {
                                    attack = true;
                                    break;
                                }
                            }
                        }
                    }
                    bool b = false;
                    statue = UIUtils.ClickTPStatue(Main.MouseWorld, ref b);
                }
                else
                {
                    if (MapSystem.TargetSelected)
                    {
                        attack = true;
                    }
                    if (MapSystem.StatueSelected)
                    {
                        statue = true;
                    }
                }

                if (selected)
                {
                    if (!player.mouseInterface)
                    {
                        if (attack)
                        {
                            NPCAttacker.CurserType = 2;
                        }
                        else if (statue)
                        {
                            NPCAttacker.CurserType = 3;
                        }
                        else
                        {
                            NPCAttacker.CurserType = 1;
                        }
                    }
                    else
                    {
                        if (NPCAttacker.CurserInMap)
                        {
                            if (attack)
                            {
                                NPCAttacker.CurserType = 2;
                            }
                            else if (statue)
                            {
                                NPCAttacker.CurserType = 3;
                            }
                            else
                            {
                                NPCAttacker.CurserType = 1;
                            }

                        }
                    }
                }
            }
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
                        if (ArmAltUI.Visible)
                        {
                            _ArmAltUserInterface.Draw(Main.spriteBatch, new GameTime());
                        }
                        if (ArmorUI.Visible)
                        {
                            _ArmorUserInterface.Draw(Main.spriteBatch, new GameTime());
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
            base.ModifyInterfaceLayers(layers);
        }

    }

}