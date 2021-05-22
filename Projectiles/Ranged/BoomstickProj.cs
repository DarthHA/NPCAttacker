using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class BoomstickProj : BaseAtkProj
	{
		public override void AttackEffect()
		{
			Item item = new Item();
			item.SetDefaults(ItemID.Boomstick);
			int ammo = SomeUtils.PickAmmo(item);
			if (ammo == 0) return;
			int num146 = Main.rand.Next(3, 5);
			for (int num147 = 0; num147 < num146; num147++)
			{
				float num148 = projectile.velocity.X;
				float num149 = projectile.velocity.Y;
				num148 += Main.rand.Next(-35, 36) * 0.04f;
				num149 += Main.rand.Next(-35, 36) * 0.04f;
				int protmp = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, num148, num149, ammo, projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
				Main.projectile[protmp].npcProj = true;
				Main.projectile[protmp].noDropItem = true;
				Main.projectile[protmp].usesLocalNPCImmunity = true;
				Main.projectile[protmp].localNPCHitCooldown = 10;
			}
		}

	}

}