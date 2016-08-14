using System;
using Microsoft.Xna.Framework;
using Infrastructure.Models;

namespace SpaceInvaders.Models
{
    public class AlienMatrix : GameComponent
    {
        private const int k_Rows = 5;
        private const int k_Cols = 9;

        private readonly Alien[,] r_AlienMatrix;

        private double m_PasssedTime;
        private double m_TimeToJump = 500;
        private bool m_JumpRight;
        private float m_JumpDistance;
        private int m_AlienCounter;

        public AlienMatrix(Game i_Game) : base(i_Game)
        {
            m_PasssedTime = 0;
            m_JumpRight = true;
            m_AlienCounter = k_Rows * k_Cols;

            r_AlienMatrix = new Alien[k_Rows, k_Cols];
            initializeBoard();
            i_Game.Components.Add(this);
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
                for (int j = 0; j < k_Cols; j++)
                {
                    if (i == 0)
                    {
                        r_AlienMatrix[i, j] = new Alien(Game, "Enemy0101_32x32", 220);
                    }
                    else if (i >= 1 && i <= 2)
                    {
                        r_AlienMatrix[i, j] = new Alien(Game, "Enemy0201_32x32", 160);
                    }
                    else
                    {
                        r_AlienMatrix[i, j] = new Alien(Game, "Enemy0301_32x32", 90);
                    }

                    r_AlienMatrix[i, j].VisibleChanged += alienStatusChanged;
                }
            }
        }

        private void alienStatusChanged(object i_Sender, EventArgs i_EventArgs)
        {
            Alien alien = i_Sender as Alien;
            if(!alien.Visible)
            {
                m_TimeToJump *= 0.94;
                m_AlienCounter--;
            }

            if(m_AlienCounter == 0)
            {
                //r_BaseGame.EndGame();
            }
        }

        private void initializeBoardPositions()
        {
            m_JumpDistance = r_AlienMatrix[0, 0].Width / 2;

            for (int i = 0; i < k_Rows; i++)
            {
                for (int j = 0; j < k_Cols; j++)
                {
                    r_AlienMatrix[i, j].Position = new Vector2(0, r_AlienMatrix[i, j].Height * 3) + new Vector2(j * (r_AlienMatrix[i, j].Width * 1.6f), i * (r_AlienMatrix[i, j].Height * 1.6f));
                }
            }
        }

        public override void Update(GameTime i_GameTime)
        {
            m_PasssedTime += i_GameTime.ElapsedGameTime.TotalMilliseconds;

            if (m_PasssedTime >= m_TimeToJump)
            {
                m_PasssedTime -= m_TimeToJump;

                float distanceToJump = m_JumpRight ? getDistanceToJumpRight() : getDistanceToJumpLeft();

                if (isAtBottom())
                {
                    //r_BaseGame.EndGame();
                }

                if (distanceToJump == 0)
                {
                    m_TimeToJump *= 0.94;

                    m_JumpRight = !m_JumpRight;

                    for (int i = 0; i < k_Rows; i++)
                    {
                        for (int j = 0; j < k_Cols; j++)
                        {
                            r_AlienMatrix[i, j].Position += new Vector2(0, m_JumpDistance);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < k_Rows; i++)
                    {
                        for (int j = 0; j < k_Cols; j++)
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

            for (int j = 0; j < k_Cols; j++)
            {
                for (int i = 0; i < k_Rows; i++)
                {
                    if (r_AlienMatrix[i, j].Visible)
                    {
                        remainSpace = (r_AlienMatrix[i, j].Position.X + m_JumpDistance >= Game.GraphicsDevice.Viewport.Width - r_AlienMatrix[i, j].Width) ?
                            Game.GraphicsDevice.Viewport.Width - r_AlienMatrix[i, j].Position.X - r_AlienMatrix[i, j].Width : m_JumpDistance;
                    }
                }
            }

            return remainSpace;
        }

        private float getDistanceToJumpLeft()
        {
            float remainSpace = 0;

            for (int j = k_Cols - 1; j >= 0; j--)
            {
                for (int i = k_Rows - 1; i >= 0; i--)
                {
                    if (r_AlienMatrix[i, j].Visible)
                    {
                        remainSpace = (r_AlienMatrix[i, j].Position.X - m_JumpDistance <= 0) ? 
                            -1 * r_AlienMatrix[i, j].Position.X : -1 * m_JumpDistance;
                    }
                }
            }

            return remainSpace;
        }

        private bool isAtBottom()
        {
            bool isAtBottom = false;

            for (int j = k_Cols - 1; j >= 0; j--)
            {
                for (int i = k_Rows - 1; i >= 0; i--)
                {
                    if(r_AlienMatrix[i, j].Visible && r_AlienMatrix[i, j].Position.Y + r_AlienMatrix[i, j].Height >= Game.GraphicsDevice.Viewport.Height)
                    {
                        isAtBottom = true;
                    }
                }
            }

            return isAtBottom;
        }
    }
}