using Terraria;
namespace NPCAttacker.Projectiles
{
    public class ShotgunProj : BaseAtkProj
    {
        public override void AttackEffect()
        {
            int ammo = VanillaItemProjFix.AmmoType;
            if (ammo == 0) return;
            int num130 = Main.rand.Next(4, 6);
            for (int num131 = 0; num131 < num130; num131++)
            {
                float num132 = Projectile.velocity.X;
                float num133 = Projectile.velocity.Y;
                num132 += Main.rand.Next(-40, 41) * 0.05f;
                num133 += Main.rand.Next(-40, 41) * 0.05f;
                int protmp = Projectile.NewProjectile(null, Projectile.Center.X, Projectile.Center.Y, num132, num133, ammo, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                Main.projectile[protmp].npcProj = true;
                Main.projectile[protmp].noDropItem = true;


            }
        }

    }

}