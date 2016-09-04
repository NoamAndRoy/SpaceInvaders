using System;
using Infrastructure.ManagersInterfaces;
using Infrastructure.Models;
using Infrastructure.Models.Animators;
using Microsoft.Xna.Framework;
using SpaceInvaders.Interfaces;

namespace SpaceInvaders.Models
{
    public class Alien : CollideableSprite, IScoreable
    { 
        private const int k_MinTimeToShoot = 30;
        private const int k_MaxTimeToShoot = 100;
        private const string k_TexturePath = "Enemies";
        private const double k_AnimationLength = 1.8f;
        private const float k_AmountOfRotationInASecond = 7;
        private const string k_EnemyGunShotSound = "EnemyGunShot";

        private static Random s_RandomShootTime = new Random();

        private readonly int r_Score;

        private Shooter m_Shooter;

        public Rectangle[] SourceRectangles;

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

        public Alien(Game i_Game, int i_Score, int i_MaxAmountOfBullets)
            : base(i_Game, k_TexturePath)
        {
            r_Score = i_Score;
            m_SecondsToNextShoot = s_RandomShootTime.Next(1, k_MaxTimeToShoot);
            IsScoreAvailable = true;

            m_Shooter = new Shooter(i_Game, new Vector2(0, 1), eBulletType.Enemy, i_MaxAmountOfBullets, Color.Blue);
        }

        public override void Initialize()
        {
            base.Initialize();

            Animator2D shrink = new SizeAnimator(TimeSpan.FromSeconds(k_AnimationLength), this.Scales, new Vector2(0, 0));
            Animator2D rotation = new RotationAnimator(TimeSpan.FromSeconds(k_AnimationLength), k_AmountOfRotationInASecond);
            Animations.Add(new CompositeAnimator("DeathAnimations", TimeSpan.FromSeconds(k_AnimationLength), this, shrink, rotation));
            Animations["DeathAnimations"].Enabled = false;

            Animations["DeathAnimations"].Finished += deathAnimations_Finished;

            Animations.Add(new CelAnimator(TimeSpan.FromSeconds(0.5), TimeSpan.Zero, SourceRectangles));

            Animations.Resume();
            m_SecondsToNextShoot = s_RandomShootTime.Next(1, k_MaxTimeToShoot);
        }

        protected override void initBounds()
        {
            Width = 32;
            Height = 32;

            SourceRectangle = new Rectangle(0, 0, (int)WidthBeforeScale, (int)HeightBeforeScale);
            
            RotationOrigin = new Vector2(WidthBeforeScale / 2, HeightBeforeScale / 2);
        }

        public override void Update(GameTime i_GameTime)
        {
            if (i_GameTime.TotalGameTime.Seconds > 0 && i_GameTime.TotalGameTime.Seconds % m_SecondsToNextShoot == 0)
            {
                m_Shooter.Position = new Vector2(Position.X + (Width / 2), Position.Y + Height);
                m_Shooter.Shoot();

                m_SecondsToNextShoot = s_RandomShootTime.Next(k_MinTimeToShoot, k_MaxTimeToShoot);

                ((ISoundManager)Game.Services.GetService(typeof(ISoundManager))).PlaySound(k_EnemyGunShotSound);
            }

            Animations.Update(i_GameTime);
        }

        public override void Collided(ICollideable i_Collideable)
        {
            Bullet bullet = i_Collideable as Bullet;

            if (bullet != null && bullet.BulletType == eBulletType.Player)
            {
                Animations["DeathAnimations"].Resume();
            }

            base.Collided(i_Collideable);
        }

        private void deathAnimations_Finished(object sender, EventArgs e)
        {
            DeleteComponent2D();
        }
    }
}