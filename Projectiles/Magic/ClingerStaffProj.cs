using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class ClingerStaffProj : BaseAtkProj
    {
        public override int ItemType => ItemID.ClingerStaff;
        public override void AttackEffect()
        {
            if (HasValidTarget())
            {
                Vector2 vector2 = GetTargetPos();
                bool flag15 = false;
                int num193 = (int)vector2.Y / 16;
                int num194 = (int)vector2.X / 16;
                int num195 = num193;
                while (num193 < Main.maxTilesY - 10 && num193 - num195 < 30 && !WorldGen.SolidTile(num194, num193) && !TileID.Sets.Platforms[Main.tile[num194, num193].TileType])
                {
                    num193++;
                }
                if (!WorldGen.SolidTile(num194, num193) && !TileID.Sets.Platforms[Main.tile[num194, num193].TileType])
                {
                    flag15 = true;
                }
                float num196 = num193 * 16;
                num193 = num195;
                while (num193 > 10 && num195 - num193 < 30 && !WorldGen.SolidTile(num194, num193))
                {
                    int num2 = num193;
                    num193 = num2 - 1;
                }
                float num197 = num193 * 16 + 16;
                float num198 = num196 - num197;
                int num199 = 10;
                if (num198 > 16 * num199)
                {
                    num198 = 16 * num199;
                }
                num197 = num196 - num198;
                vector2.X = (int)(vector2.X / 16f) * 16;
                if (!flag15)
                {
                    int protmp = Projectile.NewProjectile(null, vector2.X, vector2.Y, 0f, 0f, ProjectileID.ClingerStaff, Projectile.damage, Projectile.knockBack, Projectile.owner, num197, num198);
                    Main.projectile[protmp].npcProj = true;
                    Main.projectile[protmp].noDropItem = true;


                    Main.projectile[protmp].timeLeft = 120;
                }
            }
        }

    }

}