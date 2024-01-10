using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class ClockworkAssaultRifleProj : BaseAtkProj
    {
        public override int ItemType => ItemID.ClockworkAssaultRifle;
        public override void AttackEffect()
        {
            int ammo = VanillaItemProjFix.AmmoType;
            if (ammo == 0) return;


            int protmp = Projectile.NewProjectile(null, Projectile.Center, Projectile.velocity, ammo, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
            Main.projectile[protmp].npcProj = true;
            Main.projectile[protmp].noDropItem = true;
        }

    }

}