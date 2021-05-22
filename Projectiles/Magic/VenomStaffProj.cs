using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class VenomStaffProj : BaseAtkProj
	{
		public override void AttackEffect()
		{
			int num134 = 4;
			if (Main.rand.Next(3) == 0)
			{
				num134++;
			}
			if (Main.rand.Next(4) == 0)
			{
				num134++;
			}
			if (Main.rand.Next(5) == 0)
			{
				num134++;
			}
			for (int num135 = 0; num135 < num134; num135++)
			{
				float num136 = projectile.velocity.X;
				float num137 = projectile.velocity.Y;
				float num138 = 0.05f * num135;
				num136 += Main.rand.Next(-35, 36) * num138;
				num137 += Main.rand.Next(-35, 36) * num138;
				float num84 = (float)Math.Sqrt(num136 * num136 + num137 * num137);
				num84 = projectile.velocity.Length() / num84;
				num136 *= num84;
				num137 *= num84;
				int protmp = Projectile.NewProjectile(projectile.Center, new Vector2(num136, num137), ProjectileID.VenomFang, projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
				Main.projectile[protmp].npcProj = true;
				Main.projectile[protmp].noDropItem = true;
				Main.projectile[protmp].usesLocalNPCImmunity = true;
				Main.projectile[protmp].localNPCHitCooldown = 10;
			}
		}

	}

}