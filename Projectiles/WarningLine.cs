using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NPCAttacker.Projectiles
{
    public class WarningLine : ModProjectile
    {
        public int EndTarget = 0;
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
            Projectile.timeLeft = 30;
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            if (!Main.npc[(int)Projectile.ai[1]].active || !Main.npc[EndTarget].active)
            {
                Projectile.Kill();
            }

        }

        public override bool PreDraw(ref Color lightColor)
        {
            Color color = Projectile.ai[0] == 0 ? Color.LawnGreen : Color.Red;
            Utils.DrawLine(Main.spriteBatch, Main.npc[(int)Projectile.ai[1]].Center, Main.npc[EndTarget].Center, color, color, 2);
            return false;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}