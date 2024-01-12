using Microsoft.Xna.Framework;
using NPCAttacker.Systems;
using Terraria;
using Terraria.ModLoader;
namespace NPCAttacker.Projectiles
{
    public abstract class BaseAtkProj : ModProjectile
    {
        public override string Texture => "NPCAttacker/icon";

        public virtual int ItemType => 0;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("");
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.timeLeft = 2;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.alpha = 0;
        }
        public override void AI()
        {
            Projectile.Kill();
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
            return Projectile.GetGlobalProjectile<AttackerGProj>().ProjTarget;
        }

        public int GetOwner()
        {
            return Projectile.GetGlobalProjectile<SpecialUseProj>().NPCProjOwner;
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
            return Projectile.Center;
        }

        public override void OnKill(int timeLeft)
        {
            if (GetOwner() != -1 && Main.npc[GetOwner()].active)
            {
                AttackEffect();
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }



    }

}