using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class ClockworkAssaultRifleProj : BaseAtkProj
	{
		public override void AttackEffect()
		{
			Item item = new Item();
			item.SetDefaults(ItemID.ClockworkAssaultRifle);
			int ammo = SomeUtils.PickAmmo(item);
			if (ammo == 0) return;
			for (int i = 0; i < 3; i++)
			{
				float k = 1.2f - 0.2f * i;
				int protmp = Projectile.NewProjectile(projectile.Center, projectile.velocity * k, ammo, projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
				Main.projectile[protmp].npcProj = true;
				Main.projectile[protmp].noDropItem = true;
				Main.projectile[protmp].usesLocalNPCImmunity = true;
				Main.projectile[protmp].localNPCHitCooldown = 10;
			}
		}

	}

}