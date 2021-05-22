using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class ShotgunProj : BaseAtkProj
	{
		public override void AttackEffect()
		{
			Item item = new Item();
			item.SetDefaults(ItemID.Shotgun);
			int ammo = SomeUtils.PickAmmo(item);
			if (ammo == 0) return;
			int num130 = Main.rand.Next(4, 6);
			for (int num131 = 0; num131 < num130; num131++)
			{
				float num132 = projectile.velocity.X;
				float num133 = projectile.velocity.Y;
				num132 += Main.rand.Next(-40, 41) * 0.05f;
				num133 += Main.rand.Next(-40, 41) * 0.05f;
				int protmp = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, num132, num133, ammo, projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
				Main.projectile[protmp].npcProj = true;
				Main.projectile[protmp].noDropItem = true;
				Main.projectile[protmp].usesLocalNPCImmunity = true;
				Main.projectile[protmp].localNPCHitCooldown = 10;
			}
		}

	}

}