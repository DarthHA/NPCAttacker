using NPCAttacker.UI;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace NPCAttacker.Systems
{
    public static class TranslationToPotion
    {
        public static Dictionary<int, string> ItemToText = new();
        public static void Load()
        {
            ItemToText.Add(BuffID.AmmoReservation, "AmmoReservation");
            ItemToText.Add(BuffID.Archery, "Archery");
            ItemToText.Add(BuffID.Battle, "Battle");
            ItemToText.Add(BuffID.Calm, "Calm");
            ItemToText.Add(BuffID.Endurance, "Endurance");
            ItemToText.Add(BuffID.Featherfall, "FeatherFall");
            ItemToText.Add(BuffID.Flipper, "Flipper");
            ItemToText.Add(BuffID.Hunter, "Hunter");
            ItemToText.Add(BuffID.Gills, "Gills");
            ItemToText.Add(BuffID.Invisibility, "Invisibility");
            ItemToText.Add(BuffID.Ironskin, "IronSkin");
            ItemToText.Add(BuffID.Lifeforce, "LifeForce");
            ItemToText.Add(BuffID.Lucky, "Lucky");
            ItemToText.Add(BuffID.MagicPower, "MagicPower");
            ItemToText.Add(BuffID.ManaRegeneration, "ManaRegeneration");
            ItemToText.Add(BuffID.NightOwl, "NightOwl");
            ItemToText.Add(BuffID.ObsidianSkin, "ObsidianSkin");
            ItemToText.Add(BuffID.Rage, "Rage");
            ItemToText.Add(BuffID.Regeneration, "Regeneration");
            ItemToText.Add(BuffID.Shine, "Shine");
            ItemToText.Add(BuffID.Swiftness, "Swiftness");
            ItemToText.Add(BuffID.Thorns, "Thorns");
            ItemToText.Add(BuffID.Titan, "Titan");
            ItemToText.Add(BuffID.Inferno, "Inferno");
            ItemToText.Add(BuffID.Wrath, "Wrath");
            ItemToText.Add(BuffID.Tipsy, "Tipsy");
            ItemToText.Add(BuffID.WellFed, "WellFed1");
            ItemToText.Add(BuffID.WellFed2, "WellFed2");
            ItemToText.Add(BuffID.WellFed3, "WellFed3");
        }

        public static void UnLoad()
        {
            ItemToText.Clear();
        }
        public static string GetLocalizedExtraText(Item item)
        {
            if (item.buffType <= 0) return "";
            int buffid = item.buffType;
            if (ItemToText.ContainsKey(buffid))
            {
                return Language.GetTextValue("Mods.NPCAttacker.PotionExtra.NPCEffects") + Language.GetTextValue("Mods.NPCAttacker.PotionExtra." + ItemToText[buffid]);
            }
            else
            {
                return "";
            }
        }
    }

}