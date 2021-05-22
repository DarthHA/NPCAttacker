using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class XenopopperProj : BaseAtkProj
	{
		public override void AttackEffect()
		{
			Item item = new Item();
			item.SetDefaults(ItemID.Xenopopper);
			int ammo = SomeUtils.PickAmmo(item);
			if (ammo == 0) return;
			Vector2 vector2 = projectile.Center;
			Vector2 value3 = Vector2.Normalize(projectile.velocity) * 40f * item.scale;
			if (Collision.CanHit(vector2, 0, 0, vector2 + value3, 0, 0))
			{
				vector2 += value3;
			}
			float ai = projectile.velocity.ToRotation();
			float num100 = 2.09439516f;
			int num101 = Main.rand.Next(4, 5);
			if (Main.rand.Next(4) == 0)
			{
				num101++;
			}
			for (int num102 = 0; num102 < num101; num102++)
			{
				float scaleFactor2 = (float)Main.rand.NextDouble() * 0.2f + 0.05f;
				Vector2 vector11 = projectile.velocity.RotatedBy(num100 * (float)Main.rand.NextDouble() - num100 / 2f, default) * scaleFactor2;
				int protmp = Projectile.NewProjectile(vector2, vector11, ProjectileID.Xenopopper, projectile.damage, projectile.knockBack, projectile.owner, ai, 0f);
				Main.projectile[protmp].localAI[0] = ammo;
				Main.projectile[protmp].localAI[1] = projectile.velocity.Length();
				Main.projectile[protmp].npcProj = true;
				Main.projectile[protmp].noDropItem = true;
				Main.projectile[protmp].usesLocalNPCImmunity = true;
				Main.projectile[protmp].localNPCHitCooldown = 10;
				Main.projectile[protmp].GetGlobalProjectile<AttackerGProj>().ProjTarget = GetTarget();
			}

		}

	}

}