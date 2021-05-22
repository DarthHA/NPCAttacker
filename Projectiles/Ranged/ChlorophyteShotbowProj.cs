using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class ChlorophyteShotbowProj : BaseAtkProj
	{
		public override void AttackEffect()
		{
			Item item = new Item();
			item.SetDefaults(ItemID.ChlorophyteShotbow);
			int ammo = SomeUtils.PickAmmo(item);
			if (ammo == 0) return;
			int num158 = Main.rand.Next(2, 4);
			if (Main.rand.Next(5) == 0)
			{
				num158++;
			}
			for (int num159 = 0; num159 < num158; num159++)
			{
				float num160 = projectile.velocity.X;
				float num161 = projectile.velocity.Y;
				if (num159 > 0)
				{
					num160 += Main.rand.Next(-35, 36) * 0.04f;
					num161 += Main.rand.Next(-35, 36) * 0.04f;
				}
				if (num159 > 1)
				{
					num160 += Main.rand.Next(-35, 36) * 0.04f;
					num161 += Main.rand.Next(-35, 36) * 0.04f;
				}
				if (num159 > 2)
				{
					num160 += Main.rand.Next(-35, 36) * 0.04f;
					num161 += Main.rand.Next(-35, 36) * 0.04f;
				}
				int protmp = Projectile.NewProjectile(projectile.Center,new Vector2(num160, num161), ammo, projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
				Main.projectile[protmp].npcProj = true;
				Main.projectile[protmp].noDropItem = true;
				Main.projectile[protmp].usesLocalNPCImmunity = true;
                Main.projectile[protmp].localNPCHitCooldown = 10;
			}
		}

	}

}