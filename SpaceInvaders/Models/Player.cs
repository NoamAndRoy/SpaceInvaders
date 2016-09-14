using System.Collections.Generic;
using System;
using Infrastructure.Models;
using Microsoft.Xna.Framework;
using Infrastructure.ManagersInterfaces;
using SpaceInvaders.Interfaces;
using Infrastructure.Models.Screens;

namespace SpaceInvaders.Models
{
    public class Player : CompositeDrawableComponent<Component2D>
    {
        private const string k_LifeDieSound = "LifeDie";

        private readonly string r_PlayerName;
        private readonly List<Sprite> r_SoulsSprites;
        private readonly Text r_ScoreText;

        private int m_PlayerScore;
        private PlayerShip m_PlayerShip;

        public int PlayerScore
        {
            get { return m_PlayerScore; }
            private set
            {
                m_PlayerScore = value;
                r_ScoreText.Content = string.Format("{0} Score: {1}", r_PlayerName, PlayerScore);
            }
        }

        public int YPosition { get; set; }

        public int Souls { get; private set; }

        public PlayerShip PlayerShip
        {
            get { return m_PlayerShip; }
            set
            {
                if(m_PlayerShip != null)
                {
                    m_PlayerShip.LostSoul -= playerShip_LostSoul;
                    m_PlayerShip.CollidedAction -= playerShip_CollidedAction;
                    m_PlayerShip.Shooter.BulletCollided -= shooter_BulletCollided;
                }

                m_PlayerShip = value;
                m_PlayerShip.LostSoul += playerShip_LostSoul;
                m_PlayerShip.CollidedAction += playerShip_CollidedAction;
                m_PlayerShip.Shooter.BulletCollided += shooter_BulletCollided;
            }
        }

        public Color TextColor { get; set; }

        public string PlayerName
        {
            get
            {
                return r_PlayerName;
            }
        }

        public Player(GameScreen i_GameScreen, string i_PlayerName, int i_Score) : base(i_GameScreen.Game)
        {
            r_PlayerName = i_PlayerName;
            r_SoulsSprites = new List<Sprite>(3);

            r_ScoreText = new Text(i_GameScreen, "Calibri");
            this.Add(r_ScoreText);

            Souls = 3;
            PlayerScore = i_Score;
        }

        private void initializeSouls()
        {
            for(int i = 0; i < Souls; i++)
            {
                r_SoulsSprites.Add(new Sprite(PlayerShip.GameScreen, PlayerShip.AssetName));
                r_SoulsSprites[i].Initialize();
                r_SoulsSprites[i].Opacity = 0.5f;
                r_SoulsSprites[i].Scales = new Vector2(0.5f, 0.5f);
                r_SoulsSprites[i].Position = new Vector2(Game.GraphicsDevice.Viewport.Width - (r_SoulsSprites[i].Width * Souls * 1.5f) + (i * r_SoulsSprites[i].Width * 1.5f), YPosition);
                this.Add(r_SoulsSprites[i]);
            }
        }

        private void initializeScore()
        {
            r_ScoreText.Position = new Vector2(10, YPosition);
            r_ScoreText.TintColor = TextColor;
        }

        public override void Initialize()
        {
            base.Initialize();

            initializeSouls();
            initializeScore();
        }

        private void playerShip_CollidedAction(ICollideable i_Source, ICollideable i_Collided)
        {
            Bullet bullet = i_Collided as Bullet;

            if (PlayerShip.Animations["HitAnimation"] != null && PlayerShip.Animations["DeathAnimations"] != null && !PlayerShip.Animations["HitAnimation"].Enabled && !PlayerShip.Animations["DeathAnimations"].Enabled)
            {
                if (bullet != null && bullet.BulletType == eBulletType.Enemy)
                {
                    PlayerScore += PlayerShip.k_Score;
                    PlayerScore = MathHelper.Max(0, PlayerScore);

                    if (Souls > 1)
                    {
                        PlayerShip.Animations["HitAnimation"].Resume();
                    }
                }
            }

                if (i_Collided is Alien)
                {
                    Souls = 0;
                }

                if (Souls <= 1)
                {
                    PlayerShip.Animations["DeathAnimations"].Resume();
                    PlayerShip.IsFunctional = false;
                }
        }

        private void playerShip_LostSoul(object i_Sender, EventArgs i_EventArgs)
        {
            ((ISoundManager)Game.Services.GetService(typeof(ISoundManager))).PlaySound(k_LifeDieSound);

            if (Souls > 0)
            {
                Souls--;
                r_SoulsSprites[Souls].Dispose();
                r_SoulsSprites.RemoveAt(Souls);
            }
        }

        private void shooter_BulletCollided(ICollideable i_Source, ICollideable i_Collided)
        {
            IScoreable scoreable = i_Collided as IScoreable;

            if(scoreable != null)
            {
                if (scoreable.IsScoreAvailable)
                {
                    PlayerScore += scoreable.Score;
                }
            }
        }
    }
}