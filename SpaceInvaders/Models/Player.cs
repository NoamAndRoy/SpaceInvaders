using Infrastructure.Models;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace SpaceInvaders.Models
{
    public class Player : IGameComponent
    { 
        private readonly string m_PlayerName;

        private readonly List<Sprite> r_SoulsSprites;

        private readonly Text r_ScoreText;

        private readonly Game m_Game;

        public int PlayerScore { get; private set; }

        public int YPosition { get; set; }

        public int Souls { get; private set; }

        public PlayerShip PlayerShip { get; set; }

        public Color TextColor { get; set; }

        public string PlayerName
        {
            get
            {
                return m_PlayerName;
            }
        }

        public Player(Game i_Game, string i_PlayerName)
        {
            m_PlayerName = i_PlayerName;
            r_SoulsSprites = new List<Sprite>(3);

            r_ScoreText = new Text(i_Game, "Calibri");
            r_ScoreText.Content = string.Format("{0} Score: 0", m_PlayerName);

            m_Game = i_Game;

            i_Game.Components.Add(this);
        }

        private void initializeSouls()
        {
            Souls = 3;

            for(int i = 0; i < Souls; i++)
            {
                r_SoulsSprites.Add(new Sprite(PlayerShip.Game, PlayerShip.AssetName));
                r_SoulsSprites[i].Initialize();
                r_SoulsSprites[i].AlphaBlending = true;
                r_SoulsSprites[i].Opacity = 0.5f;
                r_SoulsSprites[i].Scales = new Vector2(0.5f, 0.5f);
                r_SoulsSprites[i].Position = new Vector2(m_Game.GraphicsDevice.Viewport.Width - r_SoulsSprites[i].Width * (Souls * 1.5f) + i * r_SoulsSprites[i].Width * 1.5f, YPosition);
            }

            //register to event soul is lost

        }

        private void initializeScore()
        {
            PlayerScore = 0;
            r_ScoreText.Position = new Vector2(10 , YPosition);
            r_ScoreText.TintColor = TextColor;
            //register to event hit
        }

        public void Initialize()
        {
            initializeSouls();
            initializeScore();
        }
    }
}