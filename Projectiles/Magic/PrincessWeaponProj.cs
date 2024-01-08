using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace NPCAttacker.Projectiles.Magic
{
    public class PrincessWeaponProj : BaseAtkProj
    {
        public override void AttackEffect()
        {
            //Vector2 farthestSpawnPositionOnLine = Main.LocalPlayer.GetFarthestSpawnPositionOnLine(GetTargetPos(), Projectile.velocity.X, Projectile.velocity.Y);
            int protmp = Projectile.NewProjectile(null, GetTargetPos(), Vector2.Zero, ProjectileID.PrincessWeapon, Projectile.damage, Projectile.knockBack, Main.myPlayer, 0f, 0f);
            Main.projectile[protmp].npcProj = true;
            Main.projectile[protmp].noDropItem = true;
        }

    }

}
