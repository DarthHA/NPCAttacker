using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class OnyxBlasterProj : BaseAtkProj
	{
		public override void AttackEffect()
		{
			Item item = new Item();
			item.SetDefaults(ItemID.OnyxBlaster);
			int ammo = SomeUtils.PickAmmo(item);
			if (ammo == 0) return;
			Vector2 vector29 = projectile.velocity;
			float num210 = 0.7853982f;
			int protmp;
			for (int num211 = 0; num211 < 2; num211++)
			{
				protmp = Projectile.NewProjectile(projectile.Center, vector29 + vector29.SafeNormalize(Vector2.Zero).RotatedBy(num210 * (Main.rand.NextFloat() * 0.5f + 0.5f), default) * Main.rand.NextFloatDirection() * 2f, ammo, projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
				Main.projectile[protmp].npcProj = true;
				Main.projectile[protmp].noDropItem = true;
				Main.projectile[protmp].usesLocalNPCImmunity = true;
				Main.projectile[protmp].localNPCHitCooldown = 10;
				protmp = Projectile.NewProjectile(projectile.Center, vector29 + vector29.SafeNormalize(Vector2.Zero).RotatedBy(-num210 * (double)(Main.rand.NextFloat() * 0.5f + 0.5f), default) * Main.rand.NextFloatDirection() * 2f, ammo, projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
				Main.projectile[protmp].npcProj = true;
				Main.projectile[protmp].noDropItem = true;
				Main.projectile[protmp].usesLocalNPCImmunity = true;
				Main.projectile[protmp].localNPCHitCooldown = 10;
			}
			protmp = Projectile.NewProjectile(projectile.Center, vector29.SafeNormalize(Vector2.UnitX * Math.Sign(projectile.velocity.X)) * (projectile.velocity.Length() * 1.3f), ProjectileID.BlackBolt, projectile.damage * 2, projectile.knockBack, projectile.owner, 0f, 0f);
			Main.projectile[protmp].npcProj = true;
			Main.projectile[protmp].noDropItem = true;
			Main.projectile[protmp].usesLocalNPCImmunity = true;
			Main.projectile[protmp].localNPCHitCooldown = 10;
		}

	}

}