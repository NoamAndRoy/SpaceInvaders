using Infrastructure.ManagersInterfaces;
using Infrastructure.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace SpaceInvaders.Models
{
    public class Wall : CollideableSprite
    {
        public const string k_TexturePath = "Barrier_44x32";
        private const int k_Speed = 40;

        private static bool s_TextureUsed = false;

        public Vector2 SpeedFactor { get; set; }

        public int MostLeftX { get; private set; }

        public int MostRightX { get; private set; }

        public Wall(Game i_Game) 
            : base(i_Game, k_TexturePath)
        {
            SpeedFactor = Vector2.Zero;
            Velocity = k_Speed * SpeedFactor;
            PixelBasedCollision = true;
            AlphaBlending = true;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            if(s_TextureUsed)
            {
                Color[] pixelArray = new Color[Texture.Width * Texture.Height];
                Texture.GetData<Color>(pixelArray);

                Texture = new Texture2D(Game.GraphicsDevice, Texture.Width, Texture.Height);
                Texture.SetData<Color>(pixelArray);
            }
            else
            {
                s_TextureUsed = true;
            }

            MostLeftX = 0;
            MostRightX = Texture.Width - 1;
        }

        public override void Update(GameTime i_GameTime)
        {
            Velocity = k_Speed * SpeedFactor;

            base.Update(i_GameTime);
        }

        public override void Collided(ICollideable i_Collideable)
        {
            int minX, maxX, minY, maxY;
            Bullet bullet = i_Collideable as Bullet;

            if (bullet != null)
            {
                minX = Math.Max(bullet.Bounds.X, Bounds.X);
                maxX = Math.Min(bullet.Bounds.X + bullet.Bounds.Width - 1, Bounds.X + Bounds.Width - 1);

                if (bullet.BulletType == eBulletType.Player)
                {
                    maxY = Math.Min(bullet.Bounds.Y + 1, Bounds.Y + Bounds.Height - 1);
                    minY = Math.Max(maxY - (int)(bullet.Bounds.Height * 0.4), Bounds.Y);
                }
                else
                {
                    minY = Math.Max(bullet.Bounds.Y + bullet.Bounds.Height - 2, Bounds.Y);
                    maxY = Math.Min(minY + (int)(bullet.Bounds.Height * 0.4), Bounds.Y + Bounds.Height);
                }

                ClearArea(minX, maxX, minY, maxY);

                OnCollide(i_Collideable);
            }
            else
            {
                Alien alien = i_Collideable as Alien;

                if (alien != null)
                {
                    minX = Math.Max(Bounds.X, alien.Bounds.X);
                    minY = Math.Max(Bounds.Y, alien.Bounds.Y);
                    maxX = Math.Min(Bounds.X + Bounds.Width - 1, alien.Bounds.X + alien.Bounds.Width - 1);
                    maxY = Math.Min(Bounds.Y + Bounds.Height - 1, alien.Bounds.Y + alien.Bounds.Height - 1);

                    ClearArea(minX, maxX, minY, maxY);
                }
            }
        }

        private void ClearArea(int minX, int maxX, int minY, int maxY)
        {
            int matrixMaxInnerY = maxY - Bounds.Y,
                matrixMinInnerY = minY - Bounds.Y,
                matrixMaxInnerX = maxX - Bounds.X,
                matrixMinInnerX = minX - Bounds.X,
                tmpMostLeftX,
                tmpMosRightX;

            bool transparentRightCol = false, transparentLeftCol = false;

            for (int i = matrixMinInnerX; i <= matrixMaxInnerX && matrixMaxInnerX < Bounds.Width; i++)
            {
                for (int j = matrixMinInnerY; j <= matrixMaxInnerY && matrixMaxInnerY < Bounds.Height; j++)
                {
                    PixelsMatrix[j * Bounds.Width + i].A = 0;
                }
            }

            Texture.SetData<Color>(PixelsMatrix);

            tmpMostLeftX = MostLeftX;
            tmpMosRightX = MostRightX;

            for (int i = MostLeftX, k = MostRightX; i <= MostRightX  || k >= MostLeftX; i++, k--)
            {
                if (k >= MostLeftX)
                {
                    transparentLeftCol = true;

                    for (int j = 0; j < Bounds.Height; j++)
                    {
                        if (PixelsMatrix[j * Bounds.Width + k].A != 0)
                        {
                            transparentLeftCol = false;
                            break;
                        }
                    }

                    if (!transparentLeftCol)
                    {
                        tmpMostLeftX = k;
                    }
                }

                if (i <= MostRightX)
                {
                    transparentRightCol = true;

                    for (int j = 0; j < Bounds.Height; j++)
                    {
                        if (PixelsMatrix[j * Bounds.Width + i].A != 0)
                        {
                            transparentRightCol = false;
                            break;
                        }
                    }

                    if (!transparentRightCol)
                    {
                        tmpMosRightX = i;
                    }
                }
            }

            MostLeftX = tmpMostLeftX;
            MostRightX = tmpMosRightX;

            if (MostRightX - MostLeftX <= maxX - minX)
            {
                Visible = false;
            }
        }
    }
}
