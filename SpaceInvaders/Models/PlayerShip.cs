using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Infrastructure.ManagersInterfaces;
using Infrastructure.Managers;
using Infrastructure.Models;
using SpaceInvaders.Interfaces;

namespace SpaceInvaders.Models
{
    public class PlayerShip : CollideableSprite
    {
        private const int k_Speed = 130;
        private const int k_Score = -1500;
        private const int k_MaxAmountOfBullets = 2;
        private const string k_TexturePath = "Ship01_32x32";

        private readonly IKeyboardManager r_KeyboardManager;
        private readonly IMouseManager r_MouseManager;

        private int m_AmountOfAliveBullets;

        public int Souls { get; private set; }

        public int PlayerScore { get; private set; }

        public PlayerShip(Game i_Game)
            : base(i_Game, k_TexturePath)
        {
            PlayerScore = 0;
            Souls = 3;
            m_AmountOfAliveBullets = 0;
            r_KeyboardManager = (IKeyboardManager)Game.Services.GetService(typeof(IKeyboardManager));
            r_MouseManager = (IMouseManager)Game.Services.GetService(typeof(IMouseManager));
        }

        public override void Initialize()
        {
            base.Initialize();

            Position = new Vector2(0f, Game.GraphicsDevice.Viewport.Height - (Height * 2 * 0.6f));
        }

        public override void Update(GameTime i_GameTime)
        {
            if (r_KeyboardManager.IsKeyHeld(Keys.Right))
            {
                Velocity = k_Speed * new Vector2(1, 0);
            }
            else if (r_KeyboardManager.IsKeyHeld(Keys.Left))
            {
                Velocity = k_Speed * new Vector2(-1, 0);
            }
            else
            {
                Velocity = new Vector2(r_MouseManager.DeltaX / (float)i_GameTime.ElapsedGameTime.TotalSeconds, 0);
            }

            if (r_KeyboardManager.IsKeyPressed(Keys.Enter) || r_MouseManager.IsKeyPressed(eMouseButton.LeftButton))
            {
                if (m_AmountOfAliveBullets < k_MaxAmountOfBullets)
                {
                    Bullet bullet = new Bullet(Game, new Vector2(0, -1), eBulletType.Player);

                    bullet.Position = new Vector2(Position.X + (Width / 2) - (bullet.Width / 2), Position.Y - bullet.Height);
                    bullet.CollidedAction += bulletCollided;
                    bullet.VisibleChanged += bullet_VisibleChanged;
                    m_AmountOfAliveBullets++;
                }
            }

            base.Update(i_GameTime);

            Position = new Vector2(MathHelper.Clamp(Position.X, 0, this.Game.GraphicsDevice.Viewport.Width - Width), Position.Y);
        }

        private void bullet_VisibleChanged(object i_Sender, EventArgs i_EventArgs)
        {
            bulletDead(i_Sender as Bullet);
        }

        private void bulletCollided(ICollideable i_Source, ICollideable i_Collided)
        {
            bulletDead(i_Source as Bullet);
            if (i_Collided is IScoreable)
            {
                PlayerScore += (i_Collided as IScoreable).Score;
            }
        }

        private void bulletDead(Bullet i_Bullet)
        {
            i_Bullet.CollidedAction -= bulletCollided;
            m_AmountOfAliveBullets--;
        }

        public override void Collided(ICollideable i_Collideable)
        {
            Bullet bullet = i_Collideable as Bullet;

            if (bullet != null && bullet.BulletType == eBulletType.Enemy)
            {
                Souls--;
                Position = new Vector2(0f, Position.Y);

                PlayerScore -= k_Score;
                PlayerScore = MathHelper.Max(0, PlayerScore);

                new Infrastructure.Models.Animations.BlinkAnimation(Game, this, TimeSpan.FromSeconds(3), 3);
            }

            if (i_Collideable is Alien)
            {
                Souls = 0;
            }

            if (Souls == 0)
            {
                //Game.EndGame();
            }

            OnCollide(i_Collideable);
        }
    }
}