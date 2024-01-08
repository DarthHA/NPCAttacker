

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace NPCAttacker.Projectiles.Magic
{
    public class SharpTearProj : BaseAtkProj
    {
        public override void AttackEffect()
        {
            Vector2 mouseWorld2 = Main.MouseWorld;
            Main.LocalPlayer.LimitPointToPlayerReachableArea(ref mouseWorld2);
            Vector2 vector33 = mouseWorld2 + Main.rand.NextVector2Circular(8f, 8f);
            Vector2 vector34 = FindSharpTearsSpot(vector33).ToWorldCoordinates(Main.rand.Next(17), Main.rand.Next(17));
            Vector2 vector35 = (vector33 - vector34).SafeNormalize(-Vector2.UnitY) * 16f;
            int protmp = Projectile.NewProjectile(null, vector34.X, vector34.Y, vector35.X, vector35.Y, ProjectileID.SharpTears, Projectile.damage, Projectile.knockBack, Main.myPlayer, 0f, Main.rand.NextFloat() * 0.5f + 0.5f);
            Main.projectile[protmp].npcProj = true;
            Main.projectile[protmp].noDropItem = true;


        }

        private Point FindSharpTearsSpot(Vector2 targetSpot)
        {
            Vector2 center = Projectile.Center;
            Vector2 endPoint = targetSpot;
            int samplesToTake = 3;
            float samplingWidth = 4f;
            Collision.AimingLaserScan(center, endPoint, samplingWidth, samplesToTake, out Vector2 v, out float[] array);
            float num = float.PositiveInfinity;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] < num)
                {
                    num = array[i];
                }
            }
            targetSpot = center + v.SafeNormalize(Vector2.Zero) * num;
            Point point = targetSpot.ToTileCoordinates();
            Rectangle rectangle = new(point.X, point.Y, 1, 1);
            rectangle.Inflate(6, 16);
            Rectangle rectangle2 = new(0, 0, Main.maxTilesX, Main.maxTilesY);
            rectangle2.Inflate(-40, -40);
            rectangle = Rectangle.Intersect(rectangle, rectangle2);
            List<Point> list = new List<Point>();
            List<Point> list2 = new List<Point>();
            for (int j = rectangle.Left; j <= rectangle.Right; j++)
            {
                for (int k = rectangle.Top; k <= rectangle.Bottom; k++)
                {
                    if (WorldGen.SolidTile(j, k, false))
                    {
                        Vector2 vector = new(j * 16 + 8, k * 16 + 8);
                        if (Vector2.Distance(targetSpot, vector) <= 200f)
                        {
                            if (this.FindSharpTearsOpening(j, k, j > point.X, j < point.X, k > point.Y, k < point.Y))
                            {
                                list.Add(new Point(j, k));
                            }
                            else
                            {
                                list2.Add(new Point(j, k));
                            }
                        }
                    }
                }
            }
            if (list.Count == 0 && list2.Count == 0)
            {
                list.Add((Projectile.Center.ToTileCoordinates().ToVector2() + Main.rand.NextVector2Square(-2f, 2f)).ToPoint());
            }
            List<Point> list3 = list;
            if (list3.Count == 0)
            {
                list3 = list2;
            }
            int index = Main.rand.Next(list3.Count);
            return list3[index];
        }

        private bool FindSharpTearsOpening(int x, int y, bool acceptLeft, bool acceptRight, bool acceptUp, bool acceptDown)
        {
            return (acceptLeft && !WorldGen.SolidTile(x - 1, y, false)) || (acceptRight && !WorldGen.SolidTile(x + 1, y, false)) || (acceptUp && !WorldGen.SolidTile(x, y - 1, false)) || (acceptDown && !WorldGen.SolidTile(x, y + 1, false));
        }

    }
}
