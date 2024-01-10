using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics;
using Terraria.ID;

namespace NPCAttacker.Projectiles.Melee
{
    public class ZenithProj : BaseAtkProj
    {
        public override int ItemType => ItemID.Zenith;
        public override void AttackEffect()
        {
            Vector2 vector41;
            int num162 = FinalFractalHelper.GetRandomProfileIndex();
            if (Main.rand.NextBool(3))
                num162 = 4956;

            Vector2 pointPoisition4;
            if (HasValidTarget())
            {
                pointPoisition4 = GetTargetPos();
            }
            else
            {
                pointPoisition4 = Main.MouseWorld;
            }

            Vector2 vector42 = pointPoisition4 - Projectile.Center;

            vector42 += Main.rand.NextVector2Circular(150f, 150f);

            vector41 = vector42 / 2f;
            float ai5 = Main.rand.Next(-100, 101);
            int protmp = Projectile.NewProjectile(null, Projectile.Center, vector41, 933, Projectile.damage, Projectile.knockBack, Main.myPlayer, ai5, num162);
            Main.projectile[protmp].npcProj = true;
            Main.projectile[protmp].noDropItem = true;
        }
    }
}
