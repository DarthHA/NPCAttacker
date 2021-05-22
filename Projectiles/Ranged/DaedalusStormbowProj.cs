using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class DaedalusStormbowProj : BaseAtkProj
	{
		public override void AttackEffect()
		{
            if (HasValidTarget())
            {
				Item item = new Item();
				item.SetDefaults(ItemID.DaedalusStormbow);
				int num75 = SomeUtils.PickAmmo(item);
				if (num75 == 0) return;
				int num90 = 3;
				if (Main.rand.Next(3) == 0)
				{
					num90++;
				}
				for (int num91 = 0; num91 < num90; num91++)
				{
					Vector2 vector2 = new Vector2(projectile.Center.X + (float)Main.rand.Next(201) * -Math.Sign(projectile.velocity.X) + (GetTargetPos().X - projectile.Center.X), projectile.Center.Y - 600f);
					vector2.X = (vector2.X * 10f + projectile.Center.X) / 11f + Main.rand.Next(-100, 101);
					vector2.Y -= 150 * num91;
					float num82 = GetTargetPos().X - vector2.X;
					float num83 = GetTargetPos().Y - vector2.Y;
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
					float num92 = num82 + Main.rand.Next(-40, 41) * 0.03f;
					float speedY = num83 + Main.rand.Next(-40, 41) * 0.03f;
					num92 *= Main.rand.Next(75, 150) * 0.01f;
					vector2.X += Main.rand.Next(-50, 51);
					int protmp = Projectile.NewProjectile(vector2.X, vector2.Y, num92, speedY, num75, projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
					Main.projectile[protmp].npcProj = true;
					Main.projectile[protmp].noDropItem = true;
					Main.projectile[protmp].usesLocalNPCImmunity = true;
					Main.projectile[protmp].localNPCHitCooldown = 10;
				}
			}
		}

	}

}