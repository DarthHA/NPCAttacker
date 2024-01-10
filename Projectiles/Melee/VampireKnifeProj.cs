using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class VampireKnifeProj : BaseAtkProj
    {
        public override int ItemType => ItemID.VampireKnives;
        public override void AttackEffect()
        {
            int num106 = 4;
            if (Main.rand.NextBool(2))
            {
                num106++;
            }
            if (Main.rand.NextBool(4))
            {
                num106++;
            }
            if (Main.rand.NextBool(8))
            {
                num106++;
            }
            if (Main.rand.NextBool(16))
            {
                num106++;
            }
            for (int num107 = 0; num107 < num106; num107++)
            {
                Vector2 ShootVel = Projectile.velocity;
                float num110 = 0.05f * num107;
                ShootVel.X += Main.rand.Next(-35, 36) * num110;
                ShootVel.Y += Main.rand.Next(-35, 36) * num110;
                ShootVel = Vector2.Normalize(ShootVel) * Projectile.velocity.Length();
                int protmp = Projectile.NewProjectile(null, Projectile.Center, ShootVel, ProjectileID.VampireKnife, Projectile.damage, Projectile.knockBack, Main.myPlayer, 0f, 0f);
                Main.projectile[protmp].npcProj = true;
                Main.projectile[protmp].noDropItem = true;
            }

        }
    }

}