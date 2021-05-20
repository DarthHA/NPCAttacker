
using Terraria.Localization;
using Terraria.ModLoader;

namespace NPCAttacker
{
    public static class TranslationUtils
    {
        public static void AddTranslation(string En, string Zh)
        {
            string temp = En.Replace(" ", "_");
            ModTranslation CustomText = NPCAttacker.Instance.CreateTranslation(temp);
            CustomText.SetDefault(En);
            CustomText.AddTranslation(GameCulture.Chinese, Zh);
            NPCAttacker.Instance.AddTranslation(CustomText);
        }
        public static void AddTranslation(string key ,string En, string Zh)
        {
            ModTranslation CustomText = NPCAttacker.Instance.CreateTranslation(key);
            CustomText.SetDefault(En);
            CustomText.AddTranslation(GameCulture.Chinese, Zh);
            NPCAttacker.Instance.AddTranslation(CustomText);
        }
        public static string GetTranslation(string key)
        {
            return Language.GetTextValue("Mods.NPCAttacker." + key);
        }

        public static string GetTranslationConfig(string key)
        {
            return Language.GetTextValue("$Mods.NPCAttacker." + key);
        }
    }
}