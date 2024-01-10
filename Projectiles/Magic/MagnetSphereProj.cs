using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class MagnetSphereProj : BaseAtkProj
    {
        public override int ItemType => ItemID.MagnetSphere;
        public override void AttackEffect()
        {
            int protmp = Projectile.NewProjectile(null, Projectile.Center, Projectile.velocity * 1.5f, ProjectileID.MagnetSphereBall, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
            Main.projectile[protmp].npcProj = true;
            Main.projectile[protmp].noDropItem = true;
        }

    }

}