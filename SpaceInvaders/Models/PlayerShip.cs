using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Infrastructure.ManagersInterfaces;
using Infrastructure.Models;
using Infrastructure.Models.Animators;

namespace SpaceInvaders.Models
{
    public class PlayerShip : CollideableSprite
    {
        private const int k_Speed = 140;
        public const int k_Score = -1500;
        private const int k_MaxAmountOfBullets = 2000;
        private const double k_AnimationLength = 2.6;
        private const float k_AmountOfBlinksInASecond = 9;
        private const float k_AmountOfRotationInASecond = 3;
        private const string k_SSGunShotSound = "SSGunShot";

        private readonly IKeyboardManager r_KeyboardManager;
        private readonly IMouseManager r_MouseManager;

        private bool m_CanShoot = true;
        private Shooter m_Shooter;

        public bool UseMouse { get; set; }

        public Keys MoveLeftKey { get; set; }

        public Keys MoveRightKey { get; set; }

        public Keys ShootKey { get; set; }

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
            Animations.Add(new BlinkAnimator("HitAnimation", TimeSpan.FromSeconds(1f / 9f), TimeSpan.FromSeconds(k_AnimationLength)));
            Animations["HitAnimation"].Finished += hitAnimation_Finished;
            Animations["HitAnimation"].Finished += deathAnimationOrHitAnimation_Finished;
            Animations["HitAnimation"].Enabled = false;

            Animator2D rotation = new RotationAnimator(TimeSpan.FromSeconds(k_AnimationLength), k_AmountOfRotationInASecond);
            Animator2D fadeOut = new FadeAnimator(TimeSpan.FromSeconds(k_AnimationLength), this.Opacity, 0);

            Animations.Add(new CompositeAnimator("DeathAnimations", TimeSpan.FromSeconds(k_AnimationLength), this, rotation, fadeOut));
            Animations.Enabled = true;

            Animations["DeathAnimations"].Finished += deathAnimations_Finished;
            Animations["DeathAnimations"].Finished += deathAnimationOrHitAnimation_Finished;
            Animations["DeathAnimations"].Enabled = false;
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

                    ((ISoundManager)Game.Services.GetService(typeof(ISoundManager))).PlaySound(k_SSGunShotSound);
                }
            }

            base.Update(i_GameTime);

            Position = new Vector2(MathHelper.Clamp(Position.X, 0, this.Game.GraphicsDevice.Viewport.Width - Width), Position.Y);
        }

        private void hitAnimation_Finished(object sender,  EventArgs e)
        {
            Position = new Vector2(0f, Position.Y);
            Animations["HitAnimation"].Pause();
            Animations["HitAnimation"].Reset();
        }

        private void deathAnimations_Finished(object sender, EventArgs e)
        {
            this.DeleteComponent2D();
        }

        private void deathAnimationOrHitAnimation_Finished(object sender, EventArgs e)
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