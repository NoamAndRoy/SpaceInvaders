using System;
using Microsoft.Xna.Framework;
using Infrastructure.Models;
using Infrastructure.ManagersInterfaces;

namespace SpaceInvaders.Models
{
    public class Bullet : CollideableSprite
    {
        private const string k_TexturePath = "Bullet";
        private const int k_Speed = 110;

        private static Random s_RandomDestroy = new Random();

        public eBulletType BulletType { get; }

        public Bullet(Game i_Game, Vector2 i_Direction, eBulletType i_BulletType)
            : base(i_Game, k_TexturePath)
        {
            Velocity = i_Direction * k_Speed;
            BulletType = i_BulletType;
            PixelBasedCollision = true;
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            if (Position.Y + Height < 0 || Position.Y > Game.GraphicsDevice.Viewport.Height)
            {
                if (BulletType == eBulletType.Enemy)
                {
                    DeleteComponent2D();
                }
                else
                {
                    this.Visible = false;
                    this.Enabled = false;
                }
            }
        }

        public override void Collided(ICollideable i_Collideable)
        {
            if (!(i_Collideable is Bullet))
            {
                if ((BulletType == eBulletType.Player && (i_Collideable is Alien || i_Collideable is MotherShip || i_Collideable is Wall)) ||
                    (BulletType == eBulletType.Enemy && (i_Collideable is PlayerShip || i_Collideable is Wall)))
                {
                    if (BulletType == eBulletType.Enemy)
                    {
                        DeleteComponent2D();
                    }
                    else
                    {
                        this.Visible = false;
                        this.Enabled = false;
                    }
                }
            }
            else
            {
                Bullet bullet = i_Collideable as Bullet;

                if(bullet != null)
                {
                    if (BulletType == eBulletType.Player && bullet.BulletType == eBulletType.Enemy)
                    {
                        this.Visible = false;
                        this.Enabled = false;
                    }
                    else if (BulletType == eBulletType.Enemy && bullet.BulletType == eBulletType.Player)
                    {
                        if(s_RandomDestroy.Next(0, 2) == 0)
                        {
                            DeleteComponent2D();
                        }
                    }
                }
            }

            base.Collided(i_Collideable);
        }
    }
}