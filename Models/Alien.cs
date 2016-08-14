using System;
using Microsoft.Xna.Framework;
using Infrastructure.Models;
using SpaceInvaders.Interfaces;
using Infrastructure.ManagersInterfaces;

namespace SpaceInvaders.Models
{
    public class Alien : CollideableSprite, IScoreable
    { 
        private const int k_MinTimeToShoot = 30;
        private const int k_MaxTimeToShoot = 100;

        private static Random s_RandomShootTime = new Random();

        private readonly int r_Score;

        public int Score
        {
            get
            {
                return r_Score;
            }
        }

        private int m_SecondsToNextShoot;

        public Alien(Game i_Game, string i_TexturePath, int i_Score)
            : base(i_Game, i_TexturePath)
        {
            r_Score = i_Score;

            m_SecondsToNextShoot = s_RandomShootTime.Next(1, k_MaxTimeToShoot);
        }

        public override void Update(GameTime i_GameTime)
        {
            if(i_GameTime.TotalGameTime.Seconds > 0 && i_GameTime.TotalGameTime.Seconds % m_SecondsToNextShoot == 0)
            {
                Bullet bullet = new Bullet(Game, new Vector2(0, 1), eBulletType.Enemy);

                bullet.Position = new Vector2(Position.X + (Width / 2) - (bullet.Width / 2), Position.Y + Height);
                m_SecondsToNextShoot = s_RandomShootTime.Next(k_MinTimeToShoot, k_MaxTimeToShoot);
            }
        }

        public override void Collided(ICollideable i_Collideable)
        {
            Bullet bullet = i_Collideable as Bullet;

            if (bullet != null && bullet.BulletType == eBulletType.Player)
            {
                base.Collided(i_Collideable);
            }
        }
    }
}
