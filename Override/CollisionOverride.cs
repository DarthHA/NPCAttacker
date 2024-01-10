using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace NPCAttacker.Override
{
    public static class CollisionOverride
    {
        public static void Collision_MoveSlopesAndStairFall(On_NPC.orig_Collision_MoveSlopesAndStairFall orig, NPC Npc, bool fall)
        {
            if (!Npc.IsTownNPC())
            {
                orig.Invoke(Npc, fall);
                return;
            }
            float gravity = 0.3f;
            if (fall)
            {
                Npc.stairFall = true;
            }
            if (Npc.aiStyle == 7)
            {
                int x = (int)Npc.Center.X / 16;
                int y = (int)Npc.position.Y / 16;
                if (WorldGen.InWorld(x, y, 0))
                {
                    int num = 16;
                    bool flag = false;
                    if (Main.tile[x, y] != null && Main.tile[x, y].HasTile && Main.tileSolid[Main.tile[x, y].TileType])
                    {
                        flag = true;
                    }
                    if (!Npc.townNPC)
                    {
                        flag = false;
                    }
                    if (!Main.dayTime || Main.eclipse)
                    {
                        flag = true;
                    }
                    else
                    {
                        int num2 = (int)(Npc.position.Y + Npc.height) / 16;
                        if (Npc.homeTileY - num2 > num)
                        {
                            flag = true;
                        }
                    }

                    if (flag)
                    {
                        if ((Npc.position.Y + Npc.height - 8f) / 16f < Npc.homeTileY)
                        {
                            Npc.stairFall = true;
                        }
                        else
                        {
                            Npc.stairFall = false;
                        }
                    }
                }
            }
            Npc.GetTileCollisionParameters(out Vector2 vector, out int width, out int height);
            Vector2 vector2 = Npc.position - vector;
            if (NPCUtils.BuffNPC())
            {
                Npc.stairFall = false;
                if (Npc.GetGlobalNPC<ArmedGNPC>().actMode == ArmedGNPC.ActMode.Move)
                {
                    if (Main.npc[Npc.GetGlobalNPC<ArmedGNPC>().ActTargetNPC].Center.Y > Npc.position.Y + Npc.height)
                    {
                        Npc.stairFall = true;
                    }
                }
            }
            Vector4 vector3 = Collision.SlopeCollision(vector, Npc.velocity, width, height, gravity, Npc.stairFall);
            if (Collision.stairFall)
            {
                Npc.stairFall = true;
            }
            else if (!fall)
            {
                Npc.stairFall = false;
            }
            if (NPCUtils.BuffNPC())
            {
                Npc.stairFall = false;
                if (Npc.GetGlobalNPC<ArmedGNPC>().actMode == ArmedGNPC.ActMode.Move)
                {
                    if (Main.npc[Npc.GetGlobalNPC<ArmedGNPC>().ActTargetNPC].Center.Y > Npc.position.Y + Npc.height)
                    {
                        Npc.stairFall = true;
                    }
                }
            }
            if (Collision.stair && Math.Abs(vector3.Y - Npc.position.Y) > 8f)
            {
                Npc.gfxOffY -= vector3.Y - Npc.position.Y;
                Npc.stepSpeed = 2f;
            }
            Npc.position.X = vector3.X;
            Npc.position.Y = vector3.Y;
            Npc.velocity.X = vector3.Z;
            Npc.velocity.Y = vector3.W;
            Npc.position += vector2;
        }
    }
}
