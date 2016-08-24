using System.Collections.Generic;
using System;
using Infrastructure.Models;
using Microsoft.Xna.Framework;
using Infrastructure.ManagersInterfaces;

namespace SpaceInvaders.Models
{
    public class Player : IGameComponent
    { 
        private readonly string r_PlayerName;

        private readonly List<Sprite> r_SoulsSprites;

        private readonly Text r_ScoreText;

        private readonly Game r_Game;

        private PlayerShip m_PlayerShip;

        public int PlayerScore { get; private set; }

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
                }

                m_PlayerShip = value;
                m_PlayerShip.LostSoul += playerShip_LostSoul;
                m_PlayerShip.CollidedAction += playerShip_CollidedAction;
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

        public Player(Game i_Game, string i_PlayerName)
        {
            r_PlayerName = i_PlayerName;
            r_SoulsSprites = new List<Sprite>(3);

            r_ScoreText = new Text(i_Game, "Calibri");
            r_ScoreText.Content = string.Format("{0} Score: 0", r_PlayerName);

            r_Game = i_Game;

            Souls = 3;
            PlayerScore = 0;

            i_Game.Components.Add(this);
        }

        private void initializeSouls()
        {
            for(int i = 0; i < Souls; i++)
            {
                r_SoulsSprites.Add(new Sprite(PlayerShip.Game, PlayerShip.AssetName));
                r_SoulsSprites[i].Initialize();
                r_SoulsSprites[i].AlphaBlending = true;
                r_SoulsSprites[i].Opacity = 0.5f;
                r_SoulsSprites[i].Scales = new Vector2(0.5f, 0.5f);
                r_SoulsSprites[i].Position = new Vector2(r_Game.GraphicsDevice.Viewport.Width - (r_SoulsSprites[i].Width * Souls * 1.5f) + (i * r_SoulsSprites[i].Width * 1.5f), YPosition);
            }
        }

        private void initializeScore()
        {
            r_ScoreText.Position = new Vector2(10, YPosition);
            r_ScoreText.TintColor = TextColor;
        }

        public void Initialize()
        {
            initializeSouls();
            initializeScore();
        }

        private void playerShip_CollidedAction(ICollideable i_Source, ICollideable i_Collided)
        {
            Bullet bullet = i_Collided as Bullet;

            if (PlayerShip.HitAnimation != null && PlayerShip.DeathAnimations != null && !PlayerShip.HitAnimation.Enabled && !PlayerShip.DeathAnimations.Enabled)
            {
                if (bullet != null && bullet.BulletType == eBulletType.Enemy)
                {
                    PlayerScore += PlayerShip.k_Score;
                    PlayerScore = MathHelper.Max(0, PlayerScore);

                    if (Souls > 1)
                    {
                        PlayerShip.HitAnimation.Enabled = true;
                    }
                    else
                    {
                        PlayerShip.DeathAnimations.Start();
                    }
                }
            }

                if (i_Collided is Alien)
                {
                    Souls = 0;
                }

                if (Souls == 0)
                {
                    PlayerShip.DeathAnimations.Start();
                    PlayerShip.CanShoot = false;
                }
        }

        private void playerShip_LostSoul(object i_Sender, EventArgs i_EventArgs)
        {
            if (Souls > 0)
            {
                Souls--;
                r_SoulsSprites[Souls].DeleteComponent2D();
                r_SoulsSprites.RemoveAt(Souls);
            }
        }
    }
}