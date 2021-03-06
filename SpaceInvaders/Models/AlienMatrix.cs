﻿using System;
using Microsoft.Xna.Framework;
using Infrastructure.Models;
using Infrastructure.Models.Animators;
using Infrastructure.ManagersInterfaces;
using Infrastructure.Models.Screens;

namespace SpaceInvaders.Models
{
    public class AlienMatrix : CompositeDrawableComponent<Alien>
    {
        public event EventHandler ReachBottom;

        public event EventHandler AllDead;

        private const int k_Rows = 5;
        private const string k_LevelWinSound = "LevelWin";
        private const string k_EnemyKillSound = "EnemyKill";

        private readonly Alien[,] r_AlienMatrix;
        private readonly int r_Cols;
        private readonly int r_FirstTypeAlienScore;
        private readonly int r_SecondTypeAlienScore;
        private readonly int r_ThirdTypeAlienScore;
        private readonly int r_MaxAmountOfBullets;

        private double m_PasssedTime;
        private float m_TimeToJump = 500;
        private bool m_JumpRight;
        private float m_JumpDistance;
        private int m_AlienCounter;

        public int MaxBottomPosition { get; set; }

        private float timeToJump
        {
            get { return m_TimeToJump; }
            set
            {
                m_TimeToJump = value;

                for (int i = 0; i < k_Rows; i++)
                {
                    for (int j = 0; j < r_Cols; j++)
                    {
                        (r_AlienMatrix[i, j].Animations["CelAnimation"] as CelAnimator).CellTime = TimeSpan.FromSeconds(m_TimeToJump / 1000);
                    }
                }
            }
        }

        public AlienMatrix(GameScreen i_GameScreen, int i_FirstTypeAlienScore, int i_SecondTypeAlienScore, int i_ThirdTypeAlienScore, int i_MaxAmountOfBullets, int i_Cols) 
            : base(i_GameScreen)
        {
            this.DrawOrder = int.MaxValue;
            m_PasssedTime = 0;
            m_JumpRight = true;
            r_Cols = i_Cols;
            m_AlienCounter = k_Rows * r_Cols;

            r_FirstTypeAlienScore = i_FirstTypeAlienScore;
            r_SecondTypeAlienScore = i_SecondTypeAlienScore;
            r_ThirdTypeAlienScore = i_ThirdTypeAlienScore;
            r_MaxAmountOfBullets = i_MaxAmountOfBullets;

            r_AlienMatrix = new Alien[k_Rows, r_Cols];
            initializeBoard();
        }

        public override void Initialize()
        {
            base.Initialize();
            initializeBoardPositions();
        }

        private void initializeBoard()
        { 
            for (int i = 0; i < k_Rows; i++)
            {
                for (int j = 0; j < r_Cols; j++)
                {
                    if (i == 0)
                    {
                        r_AlienMatrix[i, j] = new Alien(GameScreen, r_FirstTypeAlienScore, r_MaxAmountOfBullets); //220
                        r_AlienMatrix[i, j].SourceRectangles = new Rectangle[] { new Rectangle(0, 0, 32, 32), new Rectangle(32, 0, 32, 32) };
                        r_AlienMatrix[i, j].TintColor = Color.Pink;
                    }
                    else if (i >= 1 && i <= 2)
                    {
                        r_AlienMatrix[i, j] = new Alien(GameScreen, r_SecondTypeAlienScore, r_MaxAmountOfBullets); //160
                        r_AlienMatrix[i, j].SourceRectangles = new Rectangle[] { new Rectangle(32 * (i % 2), 32, 32, 32), new Rectangle(32 * ((i + 1) % 2), 32, 32, 32) };
                        r_AlienMatrix[i, j].TintColor = Color.PowderBlue;
                    }
                    else
                    {
                        r_AlienMatrix[i, j] = new Alien(GameScreen, r_ThirdTypeAlienScore, r_MaxAmountOfBullets); //90
                        r_AlienMatrix[i, j].SourceRectangles = new Rectangle[] { new Rectangle(32 * (i % 2), 64, 32, 32), new Rectangle(32 * ((i + 1) % 2), 64, 32, 32) };
                        r_AlienMatrix[i, j].TintColor = Color.LightGoldenrodYellow;
                    }

                    r_AlienMatrix[i, j].VisibleChanged += alienStatusChanged;
                    this.Add(r_AlienMatrix[i, j]);
                }
            }
        }

        private void alienStatusChanged(object i_Sender, EventArgs i_EventArgs)
        {
            Alien alien = i_Sender as Alien;

            if (!alien.Visible)
            {
                timeToJump = timeToJump * 0.94f;
                m_AlienCounter--;

                ((ISoundManager)Game.Services.GetService(typeof(ISoundManager))).PlaySound(k_EnemyKillSound);

                if (m_AlienCounter == 0)
                {
                    OnAllDead();
                }
            }
        }

        private void initializeBoardPositions()
        {
            for (int i = 0; i < k_Rows; i++)
            {
                for (int j = 0; j < r_Cols; j++)
                {
                    r_AlienMatrix[i, j].Initialize();
                    r_AlienMatrix[i, j].Position = new Vector2(0, r_AlienMatrix[i, j].SourceRectangle.Height * 3) + new Vector2((int)Math.Round(j * r_AlienMatrix[i, j].SourceRectangle.Width * 1.6), (int)Math.Round(i * r_AlienMatrix[i, j].SourceRectangle.Height * 1.6f));
                }
            }

            m_JumpDistance = r_AlienMatrix[0, 0].SourceRectangle.Width / 2;
        }

        public override void Update(GameTime i_GameTime)
        {
            m_PasssedTime += i_GameTime.ElapsedGameTime.TotalMilliseconds;

            if (m_PasssedTime >= m_TimeToJump)
            {
                m_PasssedTime -= m_TimeToJump;

                float right = getDistanceToJumpRight(), left = getDistanceToJumpLeft();
                float distanceToJump = m_JumpRight ? right : left;

                if (isAtBottom())
                {
                    OnReachBottom();
                }

                if (distanceToJump == 0)
                {
                    timeToJump = timeToJump * 0.94f;
                    m_JumpRight = !m_JumpRight;

                    for (int i = 0; i < k_Rows; i++)
                    {
                        for (int j = 0; j < r_Cols; j++)
                        {
                            r_AlienMatrix[i, j].Position += new Vector2(0, m_JumpDistance);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < k_Rows; i++)
                    {
                        for (int j = 0; j < r_Cols; j++)
                        {
                            r_AlienMatrix[i, j].Position += new Vector2(distanceToJump, 0);
                        }
                    }
                }
            }

            base.Update(i_GameTime);
        }

        private float getDistanceToJumpRight()
        {
            float remainSpace = 0;
            bool foundMostRight = false;

            for (int j = r_Cols - 1; j >= 0 && !foundMostRight; j--)
            {
                for (int i = 0; i < k_Rows && !foundMostRight; i++)
                {
                    if (r_AlienMatrix[i, j].Visible)
                    {
                        remainSpace = (r_AlienMatrix[i, j].Position.X + m_JumpDistance >= Game.GraphicsDevice.Viewport.Width - r_AlienMatrix[i, j].WidthBeforeScale) ?
                            Game.GraphicsDevice.Viewport.Width - r_AlienMatrix[i, j].Position.X - r_AlienMatrix[i, j].WidthBeforeScale : m_JumpDistance;

                        foundMostRight = true;
                    }
                }
            }

            return remainSpace;
        }

        private float getDistanceToJumpLeft()
        {
            float remainSpace = 0;
            bool foundMostLeft = false;

            for (int j = 0; j < r_Cols && !foundMostLeft; j++)
            {
                for (int i = 0; i < k_Rows && !foundMostLeft; i++)
                {
                    if (r_AlienMatrix[i, j].Visible)
                    {
                        remainSpace = (r_AlienMatrix[i, j].Position.X - m_JumpDistance <= 0) ? 
                            -1 * r_AlienMatrix[i, j].Position.X : -1 * m_JumpDistance;

                        foundMostLeft = true;
                    }
                }
            }

            return remainSpace;
        }

        private bool isAtBottom()
        {
            bool isAtBottom = false;

            for (int j = r_Cols - 1; j >= 0 && !isAtBottom; j--)
            {
                for (int i = k_Rows - 1; i >= 0 && !isAtBottom; i--)
                {
                    if(r_AlienMatrix[i, j].Visible && r_AlienMatrix[i, j].Position.Y + r_AlienMatrix[i, j].Height >= MaxBottomPosition)
                    {
                        isAtBottom = true;
                    }
                }
            }

            return isAtBottom;
        }

        protected virtual void OnAllDead()
        {
            ((ISoundManager)Game.Services.GetService(typeof(ISoundManager))).PlaySound(k_LevelWinSound);

            if (AllDead != null)
            {
                AllDead.Invoke(this, EventArgs.Empty);
            }
        }

        protected virtual void OnReachBottom()
        {
            if (ReachBottom != null)
            {
                ReachBottom.Invoke(this, EventArgs.Empty);
            }
        }

        protected override void Dispose(bool disposing)
        {
            AllDead = null;
            ReachBottom = null;

            base.Dispose(disposing);
        }
    }
}