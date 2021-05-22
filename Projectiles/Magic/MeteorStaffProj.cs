using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class MeteorStaffProj : BaseAtkProj
	{
		public override void AttackEffect()
		{
			if (HasValidTarget())
			{
				Vector2 vector2 = new Vector2(projectile.Center.X + Main.rand.Next(201) * -(float)Math.Sign(projectile.velocity.X) + (GetTargetPos().X - projectile.position.X), projectile.Center.Y - 600f);
				vector2.X = (vector2.X + projectile.Center.X) / 2f + Main.rand.Next(-200, 201);
				vector2.Y -= 100;
				float num82 = GetTargetPos().X - vector2.X + Main.rand.Next(-40, 41) * 0.03f;
				float num83 = GetTargetPos().Y - vector2.Y;
				if (num83 < 0f)
				{
					num83 *= -1f;
				}
				if (num83 < 20f)
				{
					num83 = 20f;
				}
				float num84 = (float)Math.Sqrt(num82 * num82 + num83 * num83);
				num84 = projectile.velocity.Length() / num84;
				num82 *= num84;
				num83 *= num84;
				float num115 = num82;
				float num116 = num83 + Main.rand.Next(-40, 41) * 0.02f;
				int protmp = Projectile.NewProjectile(vector2.X, vector2.Y, num115 * 0.75f, num116 * 0.75f, ProjectileID.Meteor1 + Main.rand.Next(3), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0.5f + (float)Main.rand.NextDouble() * 0.3f);
				Main.projectile[protmp].npcProj = true;
				Main.projectile[protmp].usesLocalNPCImmunity = true;
				Main.projectile[protmp].localNPCHitCooldown = 10;

			}
		}

	}

}