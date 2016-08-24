using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Infrastructure.ManagersInterfaces;
using Infrastructure.Models;
using SpaceInvaders.Interfaces;
using Infrastructure.Models.Animations;
using System.Collections.Generic;

namespace SpaceInvaders.Models
{
    public class PlayerShip : CollideableSprite
    {
        private const int k_Speed = 140;
        private const int k_Score = -1500;
        private const int k_MaxAmountOfBullets = 2;

        private readonly IKeyboardManager r_KeyboardManager;
        private readonly IMouseManager r_MouseManager;

        private int m_AmountOfAliveBullets;
        private bool m_CanShoot = true;
        private BlinkAnimation m_HitAnimation;
        private AnimationRepository m_DeathAnimations;
        private HashSet<Bullet> m_Bullets;

        public bool UseMouse { get; set; }

        public int Souls { get; private set; }

        public int PlayerScore { get; private set; }

        public Keys MoveLeftKey { get; set; }

        public Keys MoveRightKey { get; set; }

        public Keys ShootKey { get; set; }

        public PlayerShip(Game i_Game, string i_TexturePath)
            : base(i_Game, i_TexturePath)
        {
            PlayerScore = 0;
            Souls = 3;
            m_AmountOfAliveBullets = 0;

            r_KeyboardManager = (IKeyboardManager)Game.Services.GetService(typeof(IKeyboardManager));
            r_MouseManager = (IMouseManager)Game.Services.GetService(typeof(IMouseManager));

            AlphaBlending = true;

            m_Bullets = new HashSet<Bullet>();

            for (int i = 0; i < k_MaxAmountOfBullets; i++)
            {
                Bullet bullet = new Bullet(Game, new Vector2(0, -1), eBulletType.Player);
                bullet.Visible = false;
                bullet.CollidedAction += bulletCollided;
                bullet.VisibleChanged += bullet_VisibleChanged;
                bullet.TintColor = Color.Red;

                m_Bullets.Add(bullet);
            }
        }

        private void initializeAnimations()
        {
            m_HitAnimation = new BlinkAnimation(Game, this, TimeSpan.FromSeconds(2.6), 9);
            m_HitAnimation.Finished += HitAnimation_Finished;

            m_DeathAnimations = new AnimationRepository(Game, this, TimeSpan.FromSeconds(2.6));
            m_DeathAnimations.AddAnimation(new RotationAnimation(Game, this, TimeSpan.FromSeconds(2.6), 3));
            m_DeathAnimations.AddAnimation(new FadeOutAnimation(Game, this, TimeSpan.FromSeconds(2.6)));
            m_DeathAnimations.Finished += DeathAnimations_Finished;
        }

        public override void Initialize()
        {
            base.Initialize();

            Position = new Vector2(0f, Game.GraphicsDevice.Viewport.Height - (Height * 2 * 0.6f));
            RotationOrigin = TextureCenter;
            initializeAnimations();
        }

        public override void Update(GameTime i_GameTime)
        {
            if (r_KeyboardManager.IsKeyHeld(MoveRightKey))
            {
                Velocity = k_Speed * new Vector2(1, 0);
            }
            else if (r_KeyboardManager.IsKeyHeld(MoveLeftKey))
            {
                Velocity = k_Speed * new Vector2(-1, 0);
            }
            else if (UseMouse)
            {
                Velocity = new Vector2(r_MouseManager.DeltaX / (float)i_GameTime.ElapsedGameTime.TotalSeconds, 0);
            }
            else
            {
                Velocity = Vector2.Zero;
            }

            if (r_KeyboardManager.IsKeyPressed(ShootKey) || 
                (UseMouse && r_MouseManager.IsKeyPressed(eMouseButton.LeftButton)))
            {
                if (m_CanShoot && m_AmountOfAliveBullets < k_MaxAmountOfBullets)
                {
                    //m_Bullets.//[m_AmountOfAliveBullets].Position = new Vector2(Position.X + (Width / 2) - (m_BulletsArray[m_AmountOfAliveBullets].Width / 2), Position.Y - m_BulletsArray[m_AmountOfAliveBullets].Height);
                    //m_Bullets[m_AmountOfAliveBullets].Visible = true;
                    //m_Bullets[m_AmountOfAliveBullets].Enabled = true;

                    m_AmountOfAliveBullets++;
                }
            }

            base.Update(i_GameTime);

            Position = new Vector2(MathHelper.Clamp(Position.X, 0, this.Game.GraphicsDevice.Viewport.Width - Width), Position.Y);
        }

        private void bullet_VisibleChanged(object i_Sender, EventArgs i_EventArgs)
        {
            Bullet bullet = i_Sender as Bullet;
            if(bullet != null)
            {
                if(!bullet.Visible)
                {
                    bulletDead(i_Sender as Bullet);
                }
            }
        }

        private void bulletCollided(ICollideable i_Source, ICollideable i_Collided)
        {
            IScoreable scoreable = i_Collided as IScoreable;

            if (scoreable != null && scoreable.IsScoreAvailable)
            {
                PlayerScore += scoreable.Score;
                Game.Window.Title = PlayerScore.ToString();
            }
        }

        private void bulletDead(Bullet i_Bullet)
        {
            m_AmountOfAliveBullets--;
        }

        public override void Collided(ICollideable i_Collideable)
        {
            Bullet bullet = i_Collideable as Bullet;

            if (bullet != null && bullet.BulletType == eBulletType.Enemy)
            {
                Souls--;

                PlayerScore += k_Score;
                PlayerScore = MathHelper.Max(0, PlayerScore);

                if (Souls != 0)
                {
                    m_HitAnimation.Enabled = true;
                }
            }

            if (i_Collideable is Alien)
            {
                Souls = 0;
            }

            if (Souls == 0)
            {
                m_DeathAnimations.Start();
                m_CanShoot = false;
            }

            OnCollide(i_Collideable);
        }

        public void DrawPlayerInfo()
        {
            Text score = new Text(Game, "Calibri");
            score.Content = string.Empty;
        }

        private void HitAnimation_Finished(Animation i_Animation)
        {
            Position = new Vector2(0f, Position.Y);
            m_HitAnimation.Reset();
        }

        private void DeathAnimations_Finished(Animation i_Animation)
        {
            Game.EndGame();
        }
    }
}