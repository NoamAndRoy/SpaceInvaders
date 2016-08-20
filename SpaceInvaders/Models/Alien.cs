using System;
using Microsoft.Xna.Framework;
using Infrastructure.Models;
using SpaceInvaders.Interfaces;
using Infrastructure.ManagersInterfaces;
using Infrastructure.Models.Animations;

namespace SpaceInvaders.Models
{
    public class Alien : CollideableSprite, IScoreable
    { 
        private const int k_MinTimeToShoot = 30;
        private const int k_MaxTimeToShoot = 100;
        private const string k_TexturePath = "Enemies";
        private const int k_MaxAmountOfBullets = 1;

        private static Random s_RandomShootTime = new Random();

        public CelAnimation CelAnimation { get; private set; }

        private AnimationRespository m_DeathAnimations;

        private readonly int r_Score;

        private int m_AmountOfAliveBullets;

        public int Score
        {
            get
            {
                return r_Score;
            }
        }

        private int m_SecondsToNextShoot;

        public Alien(Game i_Game, int i_Score, params Rectangle[] i_SourceRectangles)
            : base(i_Game, k_TexturePath)
        {
            r_Score = i_Score;
            m_SecondsToNextShoot = s_RandomShootTime.Next(1, k_MaxTimeToShoot);
            CelAnimation = new CelAnimation(Game, this, 2, i_SourceRectangles);
            CelAnimation.Enabled = true;

            SourceRectangle = i_SourceRectangles[0];
        }

        public override void Initialize()
        {
            base.Initialize();

            m_DeathAnimations = new AnimationRespository(Game, this, TimeSpan.FromSeconds(1.8));
            m_DeathAnimations.AddAnimation(new ShrinkAnimation(Game, this, TimeSpan.FromSeconds(1.8)));
            m_DeathAnimations.AddAnimation(new RotationAnimation(Game, this, TimeSpan.FromSeconds(1.8), 7));

            m_DeathAnimations.Finished += DeathAnimations_Finished;
        }

        protected override void initBounds()
        {
            WidthBeforeScale = 32;
            HeightBeforeScale = 32;
            
            RotationOrigin = new Vector2(WidthBeforeScale / 2, HeightBeforeScale / 2);
            m_SecondsToNextShoot = s_RandomShootTime.Next(1, k_MaxTimeToShoot);
            m_AmountOfAliveBullets = 0;
        }

        public override void Update(GameTime i_GameTime)
        {
            if(i_GameTime.TotalGameTime.Seconds > 0 && i_GameTime.TotalGameTime.Seconds % m_SecondsToNextShoot == 0 && m_AmountOfAliveBullets < k_MaxAmountOfBullets)
            {
                Bullet bullet = new Bullet(Game, new Vector2(0, 1), eBulletType.Enemy);

                bullet.Position = new Vector2(Position.X + (Width / 2) - (bullet.Width / 2), Position.Y + Height);
                m_SecondsToNextShoot = s_RandomShootTime.Next(k_MinTimeToShoot, k_MaxTimeToShoot);

                bullet.CollidedAction += bulletCollided;
                bullet.VisibleChanged += bullet_VisibleChanged;
                m_AmountOfAliveBullets++;
            }
        }

        private void bullet_VisibleChanged(object i_Sender, EventArgs e)
        {
            bulletDead(i_Sender as Bullet);
        }

        private void bulletCollided(ICollideable i_Source, ICollideable I_Collided)
        {
            bulletDead(i_Source as Bullet);
        }
        private void bulletDead(Bullet i_Bullet)
        {
            i_Bullet.CollidedAction -= bulletCollided;
            m_AmountOfAliveBullets--;
        }

        public override void Collided(ICollideable i_Collideable)
        {
            Bullet bullet = i_Collideable as Bullet;

            if (bullet != null && bullet.BulletType == eBulletType.Player)
            {
                m_DeathAnimations.Start();
            }

            base.Collided(i_Collideable);
        }

        private void DeathAnimations_Finished(Animation i_Animation)
        {
            DeleteSprite();
        }
    }
}
