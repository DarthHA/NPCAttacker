using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class HellwingProj : BaseAtkProj
	{
		public override void AttackEffect()
		{
			Vector2 vector9 = projectile.velocity;
			float num82 = projectile.velocity.X;
			float num83 = projectile.velocity.Y;
			float num97 = vector9.Length();
			vector9.X += Main.rand.Next(-100, 101) * 0.01f * num97 * 0.15f;
			vector9.Y += Main.rand.Next(-100, 101) * 0.01f * num97 * 0.15f;
			float num98 = num82 + Main.rand.Next(-40, 41) * 0.03f;
			float num99 = num83 + Main.rand.Next(-40, 41) * 0.03f;
			vector9.Normalize();
			vector9 *= num97;
			num98 *= Main.rand.Next(50, 150) * 0.01f;
			num99 *= Main.rand.Next(50, 150) * 0.01f;
			Vector2 vector10 = new Vector2(num98, num99);
			vector10.X += Main.rand.Next(-100, 101) * 0.025f;
			vector10.Y += Main.rand.Next(-100, 101) * 0.025f;
			vector10.Normalize();
			vector10 *= num97;
			int protmp = Projectile.NewProjectile(projectile.Center, vector10, ProjectileID.Hellwing, projectile.damage, projectile.knockBack, projectile.owner, vector9.X, vector9.Y);
			Main.projectile[protmp].npcProj = true;
			Main.projectile[protmp].noDropItem = true;
			Main.projectile[protmp].usesLocalNPCImmunity = true;
			Main.projectile[protmp].localNPCHitCooldown = 10;
		}

	}

}