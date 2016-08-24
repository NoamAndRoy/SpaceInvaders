using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Infrastructure.ManagersInterfaces;

namespace SpaceInvaders.Models
{
    public class Shooter
    {
        public event CollidedEventHandler BulletCollided;

        private readonly int r_MaxAmountOfBullets;

        private Queue<Bullet> m_Bullets;
        private eBulletType m_BulletType;

        public Vector2 Direction { get; set; }

        public Vector2 Position { get; set; }

        public Color Tint { get; set; }

        public eBulletType BulletType
        {
            get
            {
                return m_BulletType;
            }

            set
            {
                m_BulletType = value;
            }
        }

        public int MaxAmountOfBullets
        {
            get
            {
                return r_MaxAmountOfBullets;
            }
        }

        public Shooter(Game i_Game, Vector2 i_Direction, eBulletType i_BulletType, int i_MaxAmountOfBullets, Color i_Tint)
        {
            Direction = i_Direction;
            m_BulletType = i_BulletType;
            r_MaxAmountOfBullets = i_MaxAmountOfBullets;
            Position = Vector2.Zero;

            m_Bullets = new Queue<Bullet>(i_MaxAmountOfBullets);

            for (int i = 0; i < i_MaxAmountOfBullets; i++)
            {
                Bullet bullet = new Bullet(i_Game, Direction, m_BulletType);
                bullet.Visible = false;
                bullet.VisibleChanged += bullet_VisibleChanged;
                bullet.CollidedAction += bullet_CollidedAction;
                bullet.TintColor = i_Tint;

                m_Bullets.Enqueue(bullet);
            }
        }

        public void Shoot()
        {
            if (m_Bullets.Count > 0)
            {
                Bullet bullet = m_Bullets.Peek();
                m_Bullets.Dequeue();

                bullet.Position = Position;
                bullet.Visible = true;
                bullet.Enabled = true;
            }
        }

        private void bullet_VisibleChanged(object i_Sender, EventArgs i_EventArgs)
        {
            Bullet bullet = i_Sender as Bullet;
            if (bullet != null)
            {
                if (!bullet.Visible)
                {
                    bulletDead(i_Sender as Bullet);
                }
            }
        }

        private void bullet_CollidedAction(ICollideable i_Source, ICollideable i_Collided)
        {
            OnBulletCollide(i_Source, i_Collided);
        }

        private void OnBulletCollide(ICollideable i_Source, ICollideable i_Collided)
        {
            if (BulletCollided != null)
            {
                BulletCollided.Invoke(i_Source, i_Collided);
            }
        }

        private void bulletDead(Bullet i_Bullet)
        {
            m_Bullets.Enqueue(i_Bullet);
        }
    }
}