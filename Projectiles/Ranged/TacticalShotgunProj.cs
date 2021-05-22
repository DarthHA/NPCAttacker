using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class TacticalShotgunProj : BaseAtkProj
	{
		public override void AttackEffect()
		{
			Item item = new Item();
			item.SetDefaults(ItemID.TacticalShotgun);
			int ammo = SomeUtils.PickAmmo(item);
			if (ammo == 0) return;
			for (int num176 = 0; num176 < 6; num176++)
			{
				Vector2 vector2 = projectile.Center;
				float num177 = projectile.velocity.X;
				float num178 = projectile.velocity.Y;
				num177 += Main.rand.Next(-40, 41) * 0.05f;
				num178 += Main.rand.Next(-40, 41) * 0.05f;
				Projectile.NewProjectile(vector2.X, vector2.Y, num177, num178, ammo, projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
			}
		}

	}

}