using Microsoft.Xna.Framework;
using NPCAttacker.Systems;
using System;
using Terraria;

namespace NPCAttacker.Projectiles.Melee
{
    public class DragonRageProj : BaseAtkProj
    {
        public override int ItemType => 3858;
        public override void AttackEffect()
        {
            Vector2 vector41 = Projectile.velocity;
            if (Projectile.GetGlobalProjectile<AttackerGProj>().ExtraInfo == 2)
            {
                vector41 *= 1.5f;
                float ai7 = (0.3f + 0.7f * Main.rand.NextFloat()) * Projectile.velocity.Length() * 1.75f * Math.Sign(Projectile.velocity.X + 0.01f);
                int protmp = Projectile.NewProjectile(null, Projectile.Center, vector41, 708, (int)(Projectile.damage * 0.5f), Projectile.knockBack + 4f, Main.myPlayer, ai7, 0f, 0f);
                Main.projectile[protmp].GetGlobalProjectile<SpecialUseProj>().ChannelTimer = Projectile.GetGlobalProjectile<SpecialUseProj>().ChannelTimer;
            }
            else
            {
                int protmp = Projectile.NewProjectile(null, Projectile.Center, vector41, 707, Projectile.damage, Projectile.knockBack, Main.myPlayer, 0f, 0f, 0f);
                Main.projectile[protmp].GetGlobalProjectile<SpecialUseProj>().ChannelTimer = Projectile.GetGlobalProjectile<SpecialUseProj>().ChannelTimer;
            }
        }
    }
}
