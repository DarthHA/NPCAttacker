using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace NPCAttacker.Systems
{
    /*
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
    */

    public class NPCInfoSaver : TagSerializable
    {
        public static readonly Func<TagCompound, NPCInfoSaver> DESERIALIZER = Load;

        public int VanillaNPCType = -1;
        public string ModName = "";
        public string ModNPCName = "";
        public Item Weapon = new();
        public Item AltWeapon = new();
        public Item Armor = new();
        public int Team = 0;
        public int AlterUseType = 0;
        public int ChannelUseType = 0;
        public bool AlertMode = false;


        public NPCInfoSaver(NPC npc)
        {
            if (npc.TryGetGlobalNPC(out ArmedGNPC modnpc))
            {
                if (npc.ModNPC == null)
                {
                    VanillaNPCType = npc.type;
                }
                else
                {
                    VanillaNPCType = -1;
                    ModName = npc.ModNPC.Mod.Name;
                    ModNPCName = npc.ModNPC.Name;
                }
                Weapon = modnpc.Weapon.IsAir ? new Item() : modnpc.Weapon;
                AltWeapon = modnpc.WeaponAlt.IsAir ? new Item() : modnpc.WeaponAlt;
                Armor = modnpc.Armor.IsAir ? new Item() : modnpc.Armor;
                AlterUseType = modnpc.AlterUseType;
                ChannelUseType = modnpc.ChannelUseType;
                Team = modnpc.Team;
                AlertMode = modnpc.AlertMode;
            }
        }
        public NPCInfoSaver()
        {

        }

        public TagCompound SerializeData()
        {
            return new TagCompound
            {
                ["VanillaNPCType"] = VanillaNPCType,
                ["ModName"] = ModName,
                ["ModNPCName"] = ModNPCName,
                ["Weapon"] = Weapon,
                ["AltWeapon"] = AltWeapon,
                ["Armor"] = Armor,
                ["AlterUseType"] = AlterUseType,
                ["ChannelUseType"] = ChannelUseType,
                ["Team"] = Team,
                ["AlertMode"] = AlertMode,
            };
        }

        public static NPCInfoSaver Load(TagCompound tag)
        {
            var myData = new NPCInfoSaver
            {
                VanillaNPCType = tag.GetInt("VanillaNPCType"),
                ModName = tag.GetString("ModName"),
                ModNPCName = tag.GetString("ModNPCName"),
                Weapon = tag.Get<Item>("Weapon"),
                AltWeapon = tag.Get<Item>("AltWeapon"),
                Armor = tag.Get<Item>("Armor"),
                AlterUseType = tag.GetInt("AlterUseType"),
                ChannelUseType = tag.GetInt("ChannelUseType"),
                Team = tag.GetInt("Team"),
                AlertMode = tag.GetBool("AlertMode"),
            };

            return myData;
        }
    }

    public class WeaponsSave : ModSystem
    {
        public static List<NPCInfoSaver> saver = new();
        public static List<Item> LegacyItemSave = new();
        public override void LoadWorldData(TagCompound tag)
        {
            saver.Clear();
            saver = tag.Get<List<NPCInfoSaver>>("NPCInfoSave");
            foreach (NPCInfoSaver saveinfo in saver)
            {
                foreach (NPC npc in Main.npc)
                {
                    if (!npc.active || !npc.IsTownNPC()) continue;
                    bool IsThisNPC = false;
                    if (saveinfo.VanillaNPCType != -1)      //原版NPC
                    {
                        if (npc.ModNPC == null && saveinfo.VanillaNPCType == npc.type)
                            IsThisNPC = true;
                    }
                    else      //Mod NPC
                    {
                        if (npc.ModNPC != null && npc.ModNPC.Mod.Name == saveinfo.ModName && npc.ModNPC.Name == saveinfo.ModNPCName)
                        {
                            IsThisNPC = true;
                        }
                    }
                    if (IsThisNPC)
                    {
                        ArmedGNPC modnpc = npc.GetGlobalNPC<ArmedGNPC>();
                        modnpc.Weapon = NPCUtils.CloneItem(saveinfo.Weapon);
                        modnpc.WeaponAlt = NPCUtils.CloneItem(saveinfo.AltWeapon);
                        modnpc.Armor = NPCUtils.CloneItem(saveinfo.Armor);
                        modnpc.AlterUseType = saveinfo.AlterUseType;
                        modnpc.ChannelUseType = saveinfo.ChannelUseType;
                        modnpc.Team = saveinfo.Team;
                        modnpc.AlertMode = saveinfo.AlertMode;
                        break;
                    }

                }
            }

            #region 旧版本武器数据更新会失效，因此需要这玩意弹出所有武器
            LegacyItemSave.Clear();
            if (tag.TryGet("NPCWeapon", out List<Item> NPCWeapon))
            {
                foreach (Item item in NPCWeapon)
                {
                    if (!item.IsAir) LegacyItemSave.Add(NPCUtils.CloneItem(item));
                }
            }
            if (tag.TryGet("NPCWeaponAlt", out List<Item> NPCWeaponAlt))
            {
                foreach (Item item in NPCWeaponAlt)
                {
                    if (!item.IsAir) LegacyItemSave.Add(NPCUtils.CloneItem(item));
                }
            }
            if (tag.TryGet("NPCArmor", out List<Item> NPCArmor))
            {
                foreach (Item item in NPCArmor)
                {
                    if (!item.IsAir) LegacyItemSave.Add(NPCUtils.CloneItem(item));
                }
            }
            #endregion
        }

        public override void SaveWorldData(TagCompound tag)
        {
            saver.Clear();
            foreach (NPC npc in Main.npc)
            {
                if (!npc.active || !npc.IsTownNPC()) continue;
                NPCInfoSaver unit = new(npc);
                saver.Add(unit);
            }
            tag.Add("NPCInfoSave", saver);
        }

        public override void Unload()
        {
            saver.Clear();
            saver = null;

            LegacyItemSave.Clear();
            LegacyItemSave = null;
        }

    }

    #region 旧版本武器数据更新会失效，因此需要这玩意弹出所有武器
    public class LegacyItemGiver : ModPlayer
    {
        public override void OnEnterWorld()
        {
            if (WeaponsSave.LegacyItemSave.Count > 0)
            {
                foreach (Item item in WeaponsSave.LegacyItemSave)
                {
                    Item.NewItem(Player.GetSource_Loot(), Player.Hitbox, NPCUtils.CloneItem(item));
                }
                Main.NewText(Language.ActiveCulture.LegacyId == (int)GameCulture.CultureName.Chinese ?
                    "[指挥NPC Mod] 由于技术更新，旧版本存储的武器将会失效，因此将其归还，请谅解！" :
                    "[Command NPCs Mod] Due to technological update, the old version of stored weapon will become invalid, so it will be returned. Sorry for That!"
                    , Color.Cyan);
                WeaponsSave.LegacyItemSave.Clear();
            }
        }
    }
    #endregion
}