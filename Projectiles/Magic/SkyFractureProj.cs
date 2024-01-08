using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
namespace NPCAttacker.Projectiles
{
    public class SkyFractureProj : BaseAtkProj
    {
        public override void AttackEffect()
        {
            if (HasValidTarget())
            {
                for (int i = 0; i < 3; i++)
                {
                    float f = Main.rand.NextFloat() * 6.28318548f;
                    float value8 = 20f;
                    float value9 = 60f;
                    Vector2 vector26 = Projectile.Center + f.ToRotationVector2() * MathHelper.Lerp(value8, value9, Main.rand.NextFloat());
                    for (int num209 = 0; num209 < 50; num209++)
                    {
                        vector26 = Projectile.Center + f.ToRotationVector2() * MathHelper.Lerp(value8, value9, Main.rand.NextFloat());
                        if (Collision.CanHit(Projectile.Center, 0, 0, vector26 + (vector26 - Projectile.Center).SafeNormalize(Vector2.UnitX) * 8f, 0, 0))
                        {
                            break;
                        }
                        f = Main.rand.NextFloat() * 6.28318548f;
                    }
                    Vector2 vector27 = GetTargetPos() - vector26;
                    Vector2 vector28 = Projectile.velocity.SafeNormalize(Vector2.UnitY) * Projectile.velocity.Length();
                    vector27 = vector27.SafeNormalize(vector28) * Projectile.velocity.Length();
                    vector27 = Vector2.Lerp(vector27, vector28, 0.25f);
                    int protmp = Projectile.NewProjectile(null, vector26, vector27, ProjectileID.SkyFracture, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                    Main.projectile[protmp].npcProj = true;
                    Main.projectile[protmp].noDropItem = true;


                }
            }
        }

    }

}