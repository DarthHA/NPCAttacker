using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
namespace NPCAttacker.Projectiles
{
    public abstract class BaseAtkProj : ModProjectile
    {
        public override string Texture => "Terraria/Projectile_1";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("");
        }
        public override void SetDefaults()
        {
            projectile.width = 1;
            projectile.height = 1;
            projectile.timeLeft = 2;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.alpha = 0;
        }
        public override void AI()
        {
            projectile.Kill();
            return;
        }

        public virtual void AttackEffect()
        {
            
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public int GetTarget()
        {
            return projectile.GetGlobalProjectile<AttackerGProj>().ProjTarget;
        }

        public bool HasValidTarget()
        {
            int target = GetTarget();
            if (target != -1)
            {
                if (Main.npc[target].active)
                {
                    return true;
                }
            }
            return false;
        }

        public Vector2 GetTargetPos()
        {
            int target = GetTarget();
            if (target != -1)
            {
                if (Main.npc[target].active)
                {
                    return Main.npc[target].Center;
                }
            }
            return projectile.Center;
        }

        public override void Kill(int timeLeft)
        {
            AttackEffect();
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }


    }

}