

using NPCAttacker.NPCs;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace NPCAttacker
{
    public class WeaponsWorld : ModWorld
	{
		public Dictionary<string, Item> WeaponToNPC = new Dictionary<string, Item>();
		public override void Load(TagCompound tag)
		{
			var NPCType = tag.Get<List<string>>("NPCType");
			var NPCWeapon = tag.Get<List<Item>>("NPCWeapon");
			WeaponToNPC = NPCType.Zip(NPCWeapon, (k, v) => new { Key = k, Value = v }).ToDictionary(x => x.Key, x => x.Value);

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
							if (npc.modNPC == null && npc.type == VanillaType)
							{
								if (npc.GetGlobalNPC<ArmedGNPC>().Weapon.IsAir)
								{
									npc.GetGlobalNPC<ArmedGNPC>().Weapon = NPCAttacker.CloneItem(WeaponToNPC[type]);
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
							if (npc.modNPC != null)
							{
								if (npc.modNPC.mod.Name == ModName && npc.modNPC.Name == ModNPCName)
								{
									if (npc.GetGlobalNPC<ArmedGNPC>().Weapon.IsAir)
									{
										npc.GetGlobalNPC<ArmedGNPC>().Weapon = NPCAttacker.CloneItem(WeaponToNPC[type]);
										break;
									}
								}
								
							}
						}
					}
				}
			}
			WeaponToNPC.Clear();
		}

		public override TagCompound Save()
		{
			WeaponToNPC.Clear();
			foreach (NPC npc in Main.npc)
			{
				if (npc.active && (npc.townNPC || npc.type == NPCID.SkeletonMerchant))
				{
					if (!npc.GetGlobalNPC<ArmedGNPC>().Weapon.IsAir)
					{
						string TypeOrName;
						if (npc.modNPC == null)
						{
							TypeOrName = "Vanilla:" + npc.type.ToString();
						}
						else
						{
							TypeOrName = npc.modNPC.mod.Name + ":" + npc.modNPC.Name;
						}
						WeaponToNPC.Add(TypeOrName, npc.GetGlobalNPC<ArmedGNPC>().Weapon);
						//npc.GetGlobalNPC<ArmedGNPC>().Weapon.TurnToAir();
					}
				}
			}

			return new TagCompound
			{
				{"NPCType", WeaponToNPC.Keys.ToList()},
				{"NPCWeapon", WeaponToNPC.Values.ToList()}
			};
		}
	}
}