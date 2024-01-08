using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NPCAttacker.Projectiles
{
    public class TPLine : ModProjectile
    {
        public Vector2 BeginPos = Vector2.Zero;
        public Vector2 EndPos = Vector2.Zero;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 114514;
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 10;
            Projectile.aiStyle = -1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Color color = Color.Cyan;
            Utils.DrawLine(Main.spriteBatch, BeginPos, EndPos, color, color, 2);
            return false;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}