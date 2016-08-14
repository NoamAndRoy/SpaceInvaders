using System;
using Microsoft.Xna.Framework;
using Infrastructure.Models;
using Infrastructure.ManagersInterfaces;

namespace SpaceInvaders.Models
{
    public enum eBulletType
    {
        Player,
        Enemy
    }

    public class Bullet : CollideableSprite
    {
        private const string k_TexturePath = "Bullet";
        private const int k_Speed = 110;

        public eBulletType BulletType { get; }

        public Bullet(BaseGame i_Game, Vector2 i_Direction, Color i_Tint, eBulletType i_BulletType)
            : base(i_Game, k_TexturePath, i_Tint)
        {
            Velocity = i_Direction * k_Speed;
            BulletType = i_BulletType;
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            if (Pos.Y + m_Texture.Height < 0 || Pos.Y > Game.GraphicsDevice.Viewport.Height)
            {
                DeleteSprite();
            }
        }

        public override void Collided(ICollideable i_Collideable)
        {
            if (!(i_Collideable is Bullet))
            {
                if ((BulletType == eBulletType.Player && !(i_Collideable is PlayerShip)) ||
                    (BulletType == eBulletType.Enemy && !(i_Collideable is Alien)))
                {
                    base.Collided(i_Collideable);
                }
            }
        }
    }
}