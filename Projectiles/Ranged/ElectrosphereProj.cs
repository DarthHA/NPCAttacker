using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class ElectrosphereProj : BaseAtkProj
    {
        public override void AttackEffect()
        {
            int protmp = Projectile.NewProjectile(null, Projectile.Center, Projectile.velocity, ProjectileID.ElectrosphereMissile, Projectile.damage, Projectile.knockBack, Main.myPlayer);
            Main.projectile[protmp].npcProj = true;
            Main.projectile[protmp].noDropItem = true;
            Main.projectile[protmp].ai[0] = GetTargetPos().X / 16;
            Main.projectile[protmp].ai[1] = GetTargetPos().Y / 16;
            Main.projectile[protmp].GetGlobalProjectile<AttackerGProj>().ProjTarget = GetTarget();
        }

    }

}