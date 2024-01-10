using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace NPCAttacker.Systems
{
    public class WeaponsSave : ModSystem
    {
        public Dictionary<string, Item> WeaponToNPC = new();
        public Dictionary<string, Item> WeaponAltToNPC = new();
        public Dictionary<string, Item> ArmorToNPC = new();


        public override void LoadWorldData(TagCompound tag)
        {
            var NPCType1 = tag.Get<List<string>>("NPCType1");
            var NPCType2 = tag.Get<List<string>>("NPCType2");
            var NPCType3 = tag.Get<List<string>>("NPCType3");
            var NPCWeapon = tag.Get<List<Item>>("NPCWeapon");
            var NPCWeaponAlt = tag.Get<List<Item>>("NPCWeaponAlt");
            var NPCArmor = tag.Get<List<Item>>("NPCArmor");
            WeaponToNPC = NPCType1.Zip(NPCWeapon, (k, v) => new { Key = k, Value = v }).ToDictionary(x => x.Key, x => x.Value);
            WeaponAltToNPC = NPCType2.Zip(NPCWeaponAlt, (k, v) => new { Key = k, Value = v }).ToDictionary(x => x.Key, x => x.Value);
            ArmorToNPC = NPCType3.Zip(NPCArmor, (k, v) => new { Key = k, Value = v }).ToDictionary(x => x.Key, x => x.Value);

            foreach (string type in WeaponToNPC.Keys)
            {
                bool IsModdedNPC;
                string ModName = "";
                string ModNPCName = "";
                int VanillaType = -1;

                string[] args = type.Split(':');
                if (args.Length < 2) continue;
                if (args[0] == "Vanilla")
                {
                    IsModdedNPC = false;
                    int result = -1;
                    int.TryParse(args[1], out result);
                    if (result == -1) continue;
                    VanillaType = result;
                }
                else
                {
                    IsModdedNPC = true;
                    ModName = args[0];
                    ModNPCName = args[1];
                }

                if (!IsModdedNPC)
                {
                    foreach (NPC npc in Main.npc)
                    {
                        if (npc.active)
                        {
                            if (npc.ModNPC == null && npc.type == VanillaType)
                            {
                                if (ArmedGNPC.GetWeapon(npc).IsAir)
                                {
                                    npc.GetGlobalNPC<ArmedGNPC>().Weapon = NPCUtils.CloneItem(WeaponToNPC[type]);
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (NPC npc in Main.npc)
                    {
                        if (npc.active)
                        {
                            if (npc.ModNPC != null)
                            {
                                if (npc.ModNPC.Mod.Name == ModName && npc.ModNPC.Name == ModNPCName)
                                {
                                    if (ArmedGNPC.GetWeapon(npc).IsAir)
                                    {
                                        npc.GetGlobalNPC<ArmedGNPC>().Weapon = NPCUtils.CloneItem(WeaponToNPC[type]);
                                        break;
                                    }
                                }

                            }
                        }
                    }
                }
            }

            foreach (string type in WeaponAltToNPC.Keys)
            {
                bool IsModdedNPC;
                string ModName = "";
                string ModNPCName = "";
                int VanillaType = -1;

                string[] args = type.Split(':');
                if (args.Length < 2) continue;
                if (args[0] == "Vanilla")
                {
                    IsModdedNPC = false;
                    int result = -1;
                    int.TryParse(args[1], out result);
                    if (result == -1) continue;
                    VanillaType = result;
                }
                else
                {
                    IsModdedNPC = true;
                    ModName = args[0];
                    ModNPCName = args[1];
                }

                if (!IsModdedNPC)
                {
                    foreach (NPC npc in Main.npc)
                    {
                        if (npc.active)
                        {
                            if (npc.ModNPC == null && npc.type == VanillaType)
                            {
                                if (ArmedGNPC.GetAltWeapon(npc).IsAir)
                                {
                                    npc.GetGlobalNPC<ArmedGNPC>().WeaponAlt = NPCUtils.CloneItem(WeaponAltToNPC[type]);
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (NPC npc in Main.npc)
                    {
                        if (npc.active)
                        {
                            if (npc.ModNPC != null)
                            {
                                if (npc.ModNPC.Mod.Name == ModName && npc.ModNPC.Name == ModNPCName)
                                {
                                    if (ArmedGNPC.GetAltWeapon(npc).IsAir)
                                    {
                                        npc.GetGlobalNPC<ArmedGNPC>().WeaponAlt = NPCUtils.CloneItem(WeaponAltToNPC[type]);
                                        break;
                                    }
                                }

                            }
                        }
                    }
                }
            }

            foreach (string type in ArmorToNPC.Keys)
            {
                bool IsModdedNPC;
                string ModName = "";
                string ModNPCName = "";
                int VanillaType = -1;

                string[] args = type.Split(':');
                if (args.Length < 2) continue;
                if (args[0] == "Vanilla")
                {
                    IsModdedNPC = false;
                    int result = -1;
                    int.TryParse(args[1], out result);
                    if (result == -1) continue;
                    VanillaType = result;
                }
                else
                {
                    IsModdedNPC = true;
                    ModName = args[0];
                    ModNPCName = args[1];
                }

                if (!IsModdedNPC)
                {
                    foreach (NPC npc in Main.npc)
                    {
                        if (npc.active)
                        {
                            if (npc.ModNPC == null && npc.type == VanillaType)
                            {
                                if (ArmedGNPC.GetArmor(npc).IsAir)
                                {
                                    npc.GetGlobalNPC<ArmedGNPC>().Armor = NPCUtils.CloneItem(ArmorToNPC[type]);
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (NPC npc in Main.npc)
                    {
                        if (npc.active)
                        {
                            if (npc.ModNPC != null)
                            {
                                if (npc.ModNPC.Mod.Name == ModName && npc.ModNPC.Name == ModNPCName)
                                {
                                    if (ArmedGNPC.GetArmor(npc).IsAir)
                                    {
                                        npc.GetGlobalNPC<ArmedGNPC>().Armor = NPCUtils.CloneItem(ArmorToNPC[type]);
                                        break;
                                    }
                                }

                            }
                        }
                    }
                }
            }

            WeaponToNPC.Clear();
            WeaponAltToNPC.Clear();
            ArmorToNPC.Clear();
        }
        public override void SaveWorldData(TagCompound tag)
        {
            WeaponToNPC.Clear();
            WeaponAltToNPC.Clear();
            ArmorToNPC.Clear();

            foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.IsTownNPC())
                {
                    if (!ArmedGNPC.GetWeapon(npc).IsAir)
                    {
                        string TypeOrName;
                        if (npc.ModNPC == null)
                        {
                            TypeOrName = "Vanilla:" + npc.type.ToString();
                        }
                        else
                        {
                            TypeOrName = npc.ModNPC.Mod.Name + ":" + npc.ModNPC.Name;
                        }
                        WeaponToNPC.Add(TypeOrName, ArmedGNPC.GetWeapon(npc));
                    }

                }
            }

            foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.IsTownNPC())
                {
                    if (!ArmedGNPC.GetAltWeapon(npc).IsAir)
                    {
                        string TypeOrName;
                        if (npc.ModNPC == null)
                        {
                            TypeOrName = "Vanilla:" + npc.type.ToString();
                        }
                        else
                        {
                            TypeOrName = npc.ModNPC.Mod.Name + ":" + npc.ModNPC.Name;
                        }
                        WeaponAltToNPC.Add(TypeOrName, ArmedGNPC.GetAltWeapon(npc));
                    }
                }
            }

            foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.IsTownNPC())
                {
                    if (!ArmedGNPC.GetArmor(npc).IsAir)
                    {
                        string TypeOrName;
                        if (npc.ModNPC == null)
                        {
                            TypeOrName = "Vanilla:" + npc.type.ToString();
                        }
                        else
                        {
                            TypeOrName = npc.ModNPC.Mod.Name + ":" + npc.ModNPC.Name;
                        }
                        ArmorToNPC.Add(TypeOrName, ArmedGNPC.GetArmor(npc));
                    }
                }
            }

            tag.Add("NPCType1", WeaponToNPC.Keys.ToList());
            tag.Add("NPCWeapon", WeaponToNPC.Values.ToList());
            tag.Add("NPCType2", WeaponAltToNPC.Keys.ToList());
            tag.Add("NPCWeaponAlt", WeaponAltToNPC.Values.ToList());
            tag.Add("NPCType3", ArmorToNPC.Keys.ToList());
            tag.Add("NPCArmor", ArmorToNPC.Values.ToList());
        }

    }
}