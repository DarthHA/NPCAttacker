using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class StarWarthProj : BaseAtkProj
    {
		public override void AttackEffect()
		{
			Vector2 vector13 = projectile.Center + new Vector2(Math.Sign(projectile.velocity.X) * 40, 0);
			if (SomeUtils.AttackMode()) vector13 = Main.MouseWorld;
			float num119 = vector13.Y;
			if (num119 > projectile.Center.Y - 200f)
			{
				num119 = projectile.Center.Y - 200f;
			}
			for (int i = 0; i < 3; i++)
			{
				Vector2 vector2 = projectile.Center + new Vector2(-(float)Main.rand.Next(0, 401) * Math.Sign(projectile.velocity.X), -600f);
				vector2.Y -= 100 * i;
				Vector2 vector14 = vector13 - vector2;
				if (vector14.Y < 0f)
				{
					vector14.Y *= -1f;
				}
				if (vector14.Y < 20f)
				{
					vector14.Y = 20f;
				}
				vector14.Normalize();
				vector14 *= projectile.velocity.Length();
				Vector2 Speed;
				Speed.X = vector14.X;
				Speed.Y = vector14.Y + Main.rand.Next(-40, 41) * 0.02f;
				int protmp = Projectile.NewProjectile(vector2, Speed, ProjectileID.StarWrath, projectile.damage * 2, projectile.knockBack, projectile.owner, 0f, num119);
				Main.projectile[protmp].npcProj = true;
				Main.projectile[protmp].noDropItem = true;
				Main.projectile[protmp].usesLocalNPCImmunity = true;
				Main.projectile[protmp].localNPCHitCooldown = 10;
			}
		}

    }

}