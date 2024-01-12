using System;
using Terraria;

namespace NPCAttacker.Projectiles.Magic
{
    public class EndlessBookProj : BaseAtkProj
    {
        public override int ItemType => 3852;
        public override void AttackEffect()
        {
            if (Projectile.GetGlobalProjectile<AttackerGProj>().ExtraInfo == 2)
            {
                if (Main.rand.NextBool(5))
                {
                    Projectile.NewProjectile(null, Projectile.Center.X, Projectile.Bottom.Y - 100f, Math.Sign(Projectile.velocity.X + 0.01f) * Projectile.velocity.Length(), 0f, 704, (int)(Projectile.damage * 1.75f), Projectile.knockBack, Main.myPlayer, 0f, 0f, 0f);
                }
            }
            else
            {
                Projectile.NewProjectile(null, Projectile.Center, Projectile.velocity, 712, Projectile.damage, Projectile.knockBack, Main.myPlayer, 0f, 0f, 0f);
            }
        }
    }
}
