using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Infrastructure.ManagersInterfaces;
using Infrastructure.Models;
using SpaceInvaders.Interfaces;
using Infrastructure.Models.Animations;

namespace SpaceInvaders.Models
{
    public class PlayerShip : CollideableSprite
    {
        private const int k_Speed = 140;
        public const int k_Score = -1500;
        private const int k_MaxAmountOfBullets = 2;

        private readonly IKeyboardManager r_KeyboardManager;
        private readonly IMouseManager r_MouseManager;

        private bool m_CanShoot = true;
        private BlinkAnimation m_HitAnimation;
        private AnimationRepository m_DeathAnimations;
        private Shooter m_Shooter;

        public bool UseMouse { get; set; }

        public Keys MoveLeftKey { get; set; }

        public Keys MoveRightKey { get; set; }

        public Keys ShootKey { get; set; }

        public BlinkAnimation HitAnimation
        {
            get { return m_HitAnimation; }
        }

        public AnimationRepository DeathAnimations
        {
            get { return m_DeathAnimations; }
        }

        public bool CanShoot
        {
            get { return m_CanShoot; }
            set { m_CanShoot = value; }
        }

        public Shooter Shooter
        {
            get
            {
                return m_Shooter;
            }
        }

        public event EventHandler LostSoul;

        public PlayerShip(Game i_Game, string i_TexturePath)
            : base(i_Game, i_TexturePath)
        {
            r_KeyboardManager = (IKeyboardManager)Game.Services.GetService(typeof(IKeyboardManager));
            r_MouseManager = (IMouseManager)Game.Services.GetService(typeof(IMouseManager));

            m_Shooter = new Shooter(i_Game, new Vector2(0, -1), eBulletType.Player, k_MaxAmountOfBullets, Color.Red);

            AlphaBlending = true;
        }

        private void initializeAnimations()
        {
            m_HitAnimation = new BlinkAnimation(Game, this, TimeSpan.FromSeconds(2.6), 9);
            m_HitAnimation.Finished += HitAnimation_Finished;
            m_HitAnimation.Finished += DeathAnimationOrHitAnimation_Finished;

            m_DeathAnimations = new AnimationRepository(Game, this, TimeSpan.FromSeconds(2.6));
            m_DeathAnimations.AddAnimation(new RotationAnimation(Game, this, TimeSpan.FromSeconds(2.6), 3));
            m_DeathAnimations.AddAnimation(new FadeOutAnimation(Game, this, TimeSpan.FromSeconds(2.6)));

            m_DeathAnimations.Finished += deathAnimations_Finished;
            m_DeathAnimations.Finished += DeathAnimationOrHitAnimation_Finished;
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
                if (m_CanShoot)
                {
                    m_Shooter.Position = new Vector2(Position.X + (Width / 2), Position.Y);
                    m_Shooter.Shoot();
                }
            }

            base.Update(i_GameTime);

            Position = new Vector2(MathHelper.Clamp(Position.X, 0, this.Game.GraphicsDevice.Viewport.Width - Width), Position.Y);
        }

        private void HitAnimation_Finished(Animation i_Animation)
        {
            Position = new Vector2(0f, Position.Y);
            m_HitAnimation.Reset();
        }

        private void deathAnimations_Finished(Animation i_Animation)
        {
            this.DeleteComponent2D();
        }

        private void DeathAnimationOrHitAnimation_Finished(Animation i_Animation)
        {
            OnSoulLost();
        }

        protected virtual void OnSoulLost()
        {
            if (LostSoul != null)
            {
                LostSoul(this, EventArgs.Empty);
            }
        }
    }
}