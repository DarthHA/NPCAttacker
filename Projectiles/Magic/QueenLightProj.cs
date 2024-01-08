using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace NPCAttacker.Projectiles.Magic
{
    public class QueenLightProj : BaseAtkProj
    {
        public override void AttackEffect()
        {
            if (Main.rand.Next(4) < 3) return;
            Vector2 vector25 = Main.rand.NextVector2Circular(1f, 1f) + Main.rand.NextVector2CircularEdge(3f, 3f);
            if (vector25.Y > 0f)
            {
                vector25.Y *= -1f;
            }
            float num65 = Main.rand.NextFloat() * 0.66f + (Main.LocalPlayer.miscCounter / 300f);
            Vector2 vector = Projectile.Center + new Vector2(Math.Sign(Projectile.velocity.X) * 15, 3f);
            Point point3 = vector.ToTileCoordinates();
            Tile tile = Main.tile[point3.X, point3.Y];
            if (tile != null && tile.HasUnactuatedTile && Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType] && !TileID.Sets.Platforms[tile.TileType])
            {
                vector = Projectile.Center;
            }
            int protmp = Projectile.NewProjectile(null, vector, vector25, ProjectileID.FairyQueenMagicItemShot, Projectile.damage, Projectile.knockBack, Main.myPlayer, -1f, num65);
            Main.projectile[protmp].npcProj = true;
            Main.projectile[protmp].noDropItem = true;


        }

    }
}
