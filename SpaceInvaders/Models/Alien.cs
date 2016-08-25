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
        private const int k_MaxAmountOfBullets = 1;
        private const string k_TexturePath = "Enemies";
        private const double k_AnimationLength = 1.8f;
        private const float k_AmountOfRotationInASecond = 7;

        private static Random s_RandomShootTime = new Random();

        private readonly int r_Score;

        public CelAnimation CelAnimation { get; private set; }

        private AnimationRepository m_DeathAnimations;
        private Shooter m_Shooter;

        public int Score
        {
            get
            {
                IsScoreAvailable = false;
                return r_Score;
            }
        }

        public Shooter Shooter
        {
            get
            {
                return m_Shooter;
            }
        }

        public bool IsScoreAvailable { get; set; }

        private int m_SecondsToNextShoot;

        public Alien(Game i_Game, int i_Score, params Rectangle[] i_SourceRectangles)
            : base(i_Game, k_TexturePath)
        {
            r_Score = i_Score;
            m_SecondsToNextShoot = s_RandomShootTime.Next(1, k_MaxTimeToShoot);
            CelAnimation = new CelAnimation(Game, this, 2, i_SourceRectangles);
            CelAnimation.Enabled = true;
            IsScoreAvailable = true;

            SourceRectangle = i_SourceRectangles[0];

            m_Shooter = new Shooter(i_Game, new Vector2(0, 1), eBulletType.Enemy, k_MaxAmountOfBullets, Color.Blue);
        }

        public override void Initialize()
        {
            base.Initialize();

            m_DeathAnimations = new AnimationRepository(Game, this, TimeSpan.FromSeconds(k_AnimationLength));
            m_DeathAnimations.AddAnimation(new ShrinkAnimation(Game, this, TimeSpan.FromSeconds(k_AnimationLength)));
            m_DeathAnimations.AddAnimation(new RotationAnimation(Game, this, TimeSpan.FromSeconds(k_AnimationLength), k_AmountOfRotationInASecond));

            m_DeathAnimations.Finished += deathAnimations_Finished;

            m_SecondsToNextShoot = s_RandomShootTime.Next(1, k_MaxTimeToShoot);
        }

        protected override void initBounds()
        {
            Width = 32;
            Height = 32;
            
            RotationOrigin = new Vector2(WidthBeforeScale / 2, HeightBeforeScale / 2);
        }

        public override void Update(GameTime i_GameTime)
        {
            if (i_GameTime.TotalGameTime.Seconds > 0 && i_GameTime.TotalGameTime.Seconds % m_SecondsToNextShoot == 0)
            {
                m_Shooter.Position = new Vector2(Position.X + (Width / 2), Position.Y + Height);
                m_Shooter.Shoot();

                m_SecondsToNextShoot = s_RandomShootTime.Next(k_MinTimeToShoot, k_MaxTimeToShoot);
            }
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

        private void deathAnimations_Finished(Animation i_Animation)
        {
            DeleteComponent2D();
        }
    }
}