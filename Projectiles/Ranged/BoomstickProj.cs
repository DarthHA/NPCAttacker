using Terraria;
namespace NPCAttacker.Projectiles
{
    public class BoomstickProj : BaseAtkProj
    {
        public override void AttackEffect()
        {
            int ammo = VanillaItemProjFix.AmmoType;
            if (ammo == 0) return;
            int num146 = Main.rand.Next(3, 5);
            for (int num147 = 0; num147 < num146; num147++)
            {
                float num148 = Projectile.velocity.X;
                float num149 = Projectile.velocity.Y;
                num148 += Main.rand.Next(-35, 36) * 0.04f;
                num149 += Main.rand.Next(-35, 36) * 0.04f;
                int protmp = Projectile.NewProjectile(null, Projectile.Center.X, Projectile.Center.Y, num148, num149, ammo, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                Main.projectile[protmp].npcProj = true;
                Main.projectile[protmp].noDropItem = true;


            }
        }

    }

}