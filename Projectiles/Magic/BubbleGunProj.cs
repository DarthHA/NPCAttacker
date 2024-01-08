using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class BubbleGunProj : BaseAtkProj
    {
        public override void AttackEffect()
        {
            for (int num179 = 0; num179 < 3; num179++)
            {
                float num180 = Projectile.velocity.X;
                float num181 = Projectile.velocity.Y;
                num180 += Main.rand.Next(-40, 41) * 0.1f;
                num181 += Main.rand.Next(-40, 41) * 0.1f;
                int protmp = Projectile.NewProjectile(null, Projectile.Center, new Vector2(num180, num181), ProjectileID.Bubble, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                Main.projectile[protmp].npcProj = true;
                Main.projectile[protmp].noDropItem = true;


            }
        }

    }

}