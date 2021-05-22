using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class CrimsonRodProj : BaseAtkProj
	{
		public override void AttackEffect()
		{
			if (HasValidTarget())
			{
				int protmp = Projectile.NewProjectile(projectile.Center, projectile.velocity, ProjectileID.BloodCloudMoving, projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
				Main.projectile[protmp].ai[0] = GetTargetPos().X;
				Main.projectile[protmp].ai[1] = GetTargetPos().Y - 60;
				Main.projectile[protmp].npcProj = true;
				Main.projectile[protmp].usesLocalNPCImmunity = true;
				Main.projectile[protmp].localNPCHitCooldown = 10;
			}
		}

	}

}