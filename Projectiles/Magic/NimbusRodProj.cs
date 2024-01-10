using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class NimbusRodProj : BaseAtkProj
    {
        public override int ItemType => ItemID.NimbusRod;
        public override void AttackEffect()
        {
            if (HasValidTarget())
            {
                Vector2 ShootVel = Vector2.Normalize(new Vector2(GetTargetPos().X, GetTargetPos().Y - 60) - Projectile.Center) * Projectile.velocity.Length();
                int protmp = Projectile.NewProjectile(null, Projectile.Center, ShootVel, ProjectileID.RainCloudMoving, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                Main.projectile[protmp].ai[0] = GetTargetPos().X;
                Main.projectile[protmp].ai[1] = GetTargetPos().Y - 60;
                Main.projectile[protmp].npcProj = true;
                Main.projectile[protmp].noDropItem = true;
            }
        }

    }

}