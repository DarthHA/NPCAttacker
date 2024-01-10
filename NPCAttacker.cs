using NPCAttacker.Projectiles;
using NPCAttacker.Systems;
using Terraria.Localization;
using Terraria.ModLoader;

namespace NPCAttacker
{
    public class NPCAttacker : Mod
    {
        public static string FocusText1 = "";
        public static string FocusText3 = "";

        internal static ModKeybind ForceAttackKey;
        internal static ModKeybind DisperseKey;
        internal static ModKeybind AlertKey;
        internal static ModKeybind StopKey;
        internal static ModKeybind F2AKey;
        internal static ModKeybind F2A2Key;
        internal static ModKeybind Team1Key;
        internal static ModKeybind Team2Key;
        internal static ModKeybind Team3Key;
        internal static ModKeybind Team4Key;
        internal static ModKeybind FKey;

        public static int CurserType = 0;
        public static bool CurserInMap = false;

        public static bool FuckingInvincible = false;
        public static int SpawnForNPCIndex = -1;
        public static int SpawnForChannelTime = 0;

        public static PlayerDataSaver playerDataSaver = new PlayerDataSaver();

        public static CustomConfig config;

        public static NPCAttacker Instance;

        public NPCAttacker()
        {
            Instance = this;
        }
        public override void Load()
        {

            ForceAttackKey = KeybindLoader.RegisterKeybind(this, "ForceAttack", "LeftControl");
            DisperseKey = KeybindLoader.RegisterKeybind(this, "DisperseUnits", "X");
            AlertKey = KeybindLoader.RegisterKeybind(this, "AlertMode", "G");
            StopKey = KeybindLoader.RegisterKeybind(this, "StopCommand", "S");
            F2AKey = KeybindLoader.RegisterKeybind(this, "SelectAll", "P");
            F2A2Key = KeybindLoader.RegisterKeybind(this, "SelectTheSameKind", "T");
            Team1Key = KeybindLoader.RegisterKeybind(this, "Team1", "D1");
            Team2Key = KeybindLoader.RegisterKeybind(this, "Team2", "D2");
            Team3Key = KeybindLoader.RegisterKeybind(this, "Team3", "D3");
            Team4Key = KeybindLoader.RegisterKeybind(this, "Team4", "D4");
            FKey = KeybindLoader.RegisterKeybind(this, "FollowUnit", "F");

        }
        public static string UITranslation(string str)
        {
            return Language.GetTextValue("Mods.NPCAttacker.UI." + str);
        }

        public override void PostSetupContent()
        {
            VanillaItemProjFix.Load();
            TranslationToPotion.Load();
        }


        public override void Unload()
        {
            VanillaItemProjFix.UnLoad();
            TranslationToPotion.UnLoad();

            ForceAttackKey = null;
            DisperseKey = null;
            AlertKey = null;
            StopKey = null;
            F2AKey = null;
            F2A2Key = null;
            Team1Key = null;
            Team2Key = null;
            Team3Key = null;
            Team4Key = null;
            FKey = null;

            config = null;

            Instance = null;

        }




    }
}