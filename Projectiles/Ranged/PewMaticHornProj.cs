using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace NPCAttacker.Projectiles.Ranged
{

    public class PewMaticHornProj : BaseAtkProj
    {
        public override void AttackEffect()
        {
            float speedX4 = Main.rand.Next(-15, 16) * 0.075f;
            float speedY6 = Main.rand.Next(-15, 16) * 0.075f;
            int num33 = Main.rand.Next(Main.projFrames[ProjectileID.PewMaticHornShot]);
            int protmp = Projectile.NewProjectile(null, Projectile.Center, Projectile.velocity + new Vector2(speedX4, speedY6), ProjectileID.PewMaticHornShot, Projectile.damage, Projectile.knockBack, Main.myPlayer, 0f, num33);
            Main.projectile[protmp].npcProj = true;
            Main.projectile[protmp].noDropItem = true;


        }
    }
}
