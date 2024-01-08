using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class SpiritFlameProj : BaseAtkProj
    {
        public override void AttackEffect()
        {
            if (HasValidTarget())
            {
                float num207 = Main.rand.NextFloat() * 6.28318548f;
                int num208 = 0;
                Vector2 vector2 = Projectile.Center;
                while (num208 < 10 && !Collision.CanHit(vector2, 0, 0, vector2 + Projectile.velocity.RotatedBy(num207, default) * 100f, 0, 0))
                {
                    num207 = Main.rand.NextFloat() * 6.28318548f;
                    num208++;
                }
                Vector2 value7 = Projectile.velocity.RotatedBy(num207, default) * (0.95f + Main.rand.NextFloat() * 0.3f);
                int protmp = Projectile.NewProjectile(null, vector2 + value7 * 30f, Vector2.Zero, ProjectileID.SpiritFlame, Projectile.damage, Projectile.knockBack, Projectile.owner, -2f, 0f);
                Main.projectile[protmp].npcProj = true;
                Main.projectile[protmp].noDropItem = true;


            }
        }

    }

}