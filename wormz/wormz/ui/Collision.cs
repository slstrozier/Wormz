using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace wormz.ui
{
    static class Collision
    {
        private static Rocket rocket;
        private static Color[,] foregroundColorArray;
        private static Color[,] rocketColorArray;
        private static Vector2 CollisionLocation;


        
        public static void setUpCollision(Rocket rocket, Texture2D foregroundTexture)
        {
            Collision.rocket = rocket;
            Collision.foregroundColorArray = TextureTo2DArray(foregroundTexture);
            Collision.rocketColorArray = TextureTo2DArray(rocket.rocketTexture);
        }

        private static Vector2 TexturesCollide(Color[,] tex1, Matrix mat1, Color[,] tex2, Matrix mat2)
        {
            Matrix mat1to2 = mat1 * Matrix.Invert(mat2);
            int width1 = tex1.GetLength(0);
            int height1 = tex1.GetLength(1);
            int width2 = tex2.GetLength(0);
            int height2 = tex2.GetLength(1);

            for (int x1 = 0; x1 < width1; x1++)
            {
                for (int y1 = 0; y1 < height1; y1++)
                {
                    Vector2 pos1 = new Vector2(x1, y1);
                    Vector2 pos2 = Vector2.Transform(pos1, mat1to2);

                    int x2 = (int)pos2.X;
                    int y2 = (int)pos2.Y;
                    if ((x2 >= 0) && (x2 < width2))
                    {
                        if ((y2 >= 0) && (y2 < height2))
                        {
                            if (tex1[x1, y1].A > 0)
                            {
                                if (tex2[x2, y2].A > 0)
                                {
                                    Vector2 screenPos = Vector2.Transform(pos1, mat1);
                                    return screenPos;
                                }
                            }
                        }
                    }
                }
            }

            return new Vector2(-1, -1);

        }

        private static Vector2 CheckTerrainCollision()
        {
            Matrix rocketMat = Matrix.CreateTranslation(-42, -240, 0) * Matrix.CreateRotationZ(rocket.Angle) * Matrix.CreateScale(rocket.Scaling) * Matrix.CreateTranslation(rocket.Position.X, rocket.Position.Y, 0);
            Matrix terrainMat = Matrix.Identity;
            Vector2 terrainCollisionPoint = TexturesCollide(rocketColorArray, rocketMat, foregroundColorArray, terrainMat);
            return terrainCollisionPoint;
        }

        private static Color[,] TextureTo2DArray(Texture2D texture)
        {
            Color[] colors1D = new Color[texture.Width * texture.Height];
            texture.GetData(colors1D);

            Color[,] colors2D = new Color[texture.Width, texture.Height];
            for (int x = 0; x < texture.Width; x++)
                for (int y = 0; y < texture.Height; y++)
                    colors2D[x, y] = colors1D[x + y * texture.Width];

            return colors2D;
        }

        public static bool CheckCollisions(GameTime gameTime)
        {

            CollisionLocation = CheckTerrainCollision();
            /*Vector2 playerCollisionPoint = CheckPlayersCollision();      
            if (playerCollisionPoint.X > -1)
            {
                rocketFlying = false;
                smokeList = new List<Vector2>();
            }
             */
            if (CollisionLocation.X > -1)
            {
                return true;
            }
            else
            {
                return false;
            }

            
        }
        /// <summary>
        /// Returns the point at which a collision has occured (the member CollisionLocation)
        /// </summary>
        /// <returns>The point at which a collision has occured</returns>
         public static Vector2 ReturnCollisionPoint()
        {
            return CollisionLocation;
        }
    }
}
