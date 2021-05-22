using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class LunarFlareBookProj : BaseAtkProj
	{
		public override void AttackEffect()
		{
			if (HasValidTarget())
			{
				int num117 = 3;
				for (int num118 = 0; num118 < num117; num118++)
				{
					Vector2 vector2 = new Vector2(projectile.Center.X + Main.rand.Next(201) * -(float)Math.Sign(projectile.velocity.X) + (GetTargetPos().X - projectile.position.X), projectile.Center.Y - 600f);
					vector2.X = (vector2.X + projectile.Center.X) / 2f + Main.rand.Next(-200, 201);
					vector2.Y -= 100 * num118;
					float num82 = GetTargetPos().X - vector2.X;
					float num83 = GetTargetPos().Y - vector2.Y;
					float ai2 = num83 + vector2.Y;
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
					Vector2 vector12 = new Vector2(num82, num83) / 2f;
					Projectile.NewProjectile(vector2, vector12, ProjectileID.LunarFlare, projectile.damage, projectile.knockBack, projectile.owner, 0f, ai2);
				}

			}
		}

	}

}