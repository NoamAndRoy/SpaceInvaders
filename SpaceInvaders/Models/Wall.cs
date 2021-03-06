﻿using System;
using Infrastructure.ManagersInterfaces;
using Infrastructure.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.Models.Screens;

namespace SpaceInvaders.Models
{
    public class Wall : CollideableSprite
    {
        public const string k_TexturePath = "Barrier_44x32";
        private const string k_BarrierHitSound = "BarrierHit";

        private static bool s_TextureUsed = false;

        private readonly int r_Speed;

        public Vector2 SpeedFactor { get; set; }

        public Wall(GameScreen i_GameScreen, int i_Speed) 
            : base(i_GameScreen, k_TexturePath)
        {
            SpeedFactor = Vector2.Zero;
            r_Speed = i_Speed;
            Velocity = r_Speed * SpeedFactor;
            PixelBasedCollision = true;
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
        }

        public override void Update(GameTime i_GameTime)
        {
            Velocity = r_Speed * SpeedFactor;

            base.Update(i_GameTime);
        }

        public override void Collided(ICollideable i_Collideable)
        {
            int minX, maxX, minY = 0, maxY = 0;
            Bullet bullet = i_Collideable as Bullet;

            minX = Math.Max(i_Collideable.Bounds.X, Bounds.X);
            maxX = Math.Min(i_Collideable.Bounds.X + i_Collideable.Bounds.Width - 1, Bounds.X + Bounds.Width - 1);

            if (bullet != null)
            {
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

                ((ISoundManager)Game.Services.GetService(typeof(ISoundManager))).PlaySound(k_BarrierHitSound);
            }
            else
            {
                Alien alien = i_Collideable as Alien;

                if (alien != null)
                {
                    minY = Math.Max(Bounds.Y, alien.Bounds.Y);
                    maxY = Math.Min(Bounds.Y + Bounds.Height - 1, alien.Bounds.Y + alien.Bounds.Height - 1);

                    ClearArea(minX, maxX, minY, maxY);
                }
            }

            OnCollide(i_Collideable);
        }

        private void ClearArea(int minX, int maxX, int minY, int maxY)
        {
            int matrixMaxInnerY = maxY - Bounds.Y,
                matrixMinInnerY = minY - Bounds.Y,
                matrixMaxInnerX = maxX - Bounds.X,
                matrixMinInnerX = minX - Bounds.X;

            for (int i = matrixMinInnerX; i <= matrixMaxInnerX && matrixMaxInnerX < Bounds.Width; i++)
            {
                for (int j = matrixMinInnerY; j <= matrixMaxInnerY && matrixMaxInnerY < Bounds.Height; j++)
                {
                    PixelsMatrix[(j * Bounds.Width) + i].A = 0;
                }
            }

            Texture.SetData<Color>(PixelsMatrix);
        }
    }
}
