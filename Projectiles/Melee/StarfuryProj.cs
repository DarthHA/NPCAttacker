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
            Vector2 Pos = GetTargetPos();
            Vector2 ShootPos = new(Projectile.Center.X + (float)Main.rand.Next(201) * -Math.Sign(Projectile.velocity.X) + (Pos.X - Projectile.Center.X), Projectile.Center.Y - 600f);
            int protmp = Projectile.NewProjectile(null, ShootPos, Vector2.Normalize(Pos - ShootPos) * Projectile.velocity.Length(), ProjectileID.Starfury, Projectile.damage * 2, 0, Projectile.owner);
            Main.projectile[protmp].npcProj = true;
            Main.projectile[protmp].noDropItem = true;


        }
    }

}