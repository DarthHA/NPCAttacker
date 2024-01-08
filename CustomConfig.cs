using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace NPCAttacker
{
    public class CustomConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;


        [Increment(0.1f)]
        [Range(1f, 20f)]
        [DefaultValue(1f)]
        public float DamageModifier;

        [Increment(0.05f)]
        [Range(0f, 0.95f)]
        [DefaultValue(0f)]
        public float DRModifier;

        [Increment(0.05f)]
        [Range(1f, 3f)]
        [DefaultValue(1f)]
        public float SpeedModifier;


        public override ModConfig Clone()
        {
            var clone = (CustomConfig)base.Clone();
            return clone;
        }

        public override void OnLoaded()
        {
            NPCAttacker.config = this;
        }


    }
}