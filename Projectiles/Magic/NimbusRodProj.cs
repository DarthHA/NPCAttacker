using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class NimbusRodProj : BaseAtkProj
    {
        public override void AttackEffect()
        {
            if (HasValidTarget())
            {
                int protmp = Projectile.NewProjectile(null, Projectile.Center, Projectile.velocity, ProjectileID.RainCloudMoving, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                Main.projectile[protmp].ai[0] = GetTargetPos().X;
                Main.projectile[protmp].ai[1] = GetTargetPos().Y - 60;
                Main.projectile[protmp].npcProj = true;
                Main.projectile[protmp].noDropItem = true;
            }
        }

    }

}