using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class StarfuryProj : BaseAtkProj
    {
        public override void AttackEffect()
        {
            Vector2 Pos = projectile.Center + new Vector2(Math.Sign(projectile.velocity.X) * 40, 0);
            if (SomeUtils.AttackMode()) Pos = Main.MouseWorld;
            Vector2 ShootPos = new Vector2(projectile.Center.X + (float)Main.rand.Next(201) * -Math.Sign(projectile.velocity.X) + (Pos.X - projectile.Center.X), projectile.Center.Y - 600f);
            int protmp = Projectile.NewProjectile(ShootPos, Vector2.Normalize(Pos - ShootPos) * projectile.velocity.Length(), ProjectileID.Starfury, projectile.damage * 2, 0, projectile.owner);
            Main.projectile[protmp].npcProj = true;
            Main.projectile[protmp].noDropItem = true;
            Main.projectile[protmp].usesLocalNPCImmunity = true;
            Main.projectile[protmp].localNPCHitCooldown = 10;
        }
    }

}