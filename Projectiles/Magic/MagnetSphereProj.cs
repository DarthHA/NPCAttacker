using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class MagnetSphereProj : BaseAtkProj
	{
		public override void AttackEffect()
		{
			int protmp = Projectile.NewProjectile(projectile.Center, projectile.velocity * 1.5f, ProjectileID.MagnetSphereBall, projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
			Main.projectile[protmp].npcProj = true;
			Main.projectile[protmp].usesLocalNPCImmunity = true;
			Main.projectile[protmp].localNPCHitCooldown = 10;
            Main.projectile[protmp].timeLeft = 120;
		}

	}

}