using System;
using Microsoft.Xna.Framework;
using Infrastructure.Models;

namespace SpaceInvaders.Models
{
    public class AlienMatrix : RegisteredComponent
    {
        public event EventHandler ReachBottom;

        public event EventHandler AllDead;

        private const int k_Rows = 5;
        private const int k_Cols = 9;

        private readonly Alien[,] r_AlienMatrix;

        private double m_PasssedTime;
        private float m_TimeToJump = 500;
        private bool m_JumpRight;
        private float m_JumpDistance;
        private int m_AlienCounter;

        public int YBottomPosition { get; set; }

        private float timeToJump
        {
            get { return m_TimeToJump; }
            set
            {
                m_TimeToJump = value;
                for (int i = 0; i < k_Rows; i++)
                {
                    for (int j = 0; j < k_Cols; j++)
                    {
                        r_AlienMatrix[i, j].CelAnimation.AmountOfCellsInASecond = m_TimeToJump / 1000;
                    }
                }
            }
        }

        public AlienMatrix(Game i_Game) : base(i_Game)
        {
            m_PasssedTime = 0;
            m_JumpRight = true;
            m_AlienCounter = k_Rows * k_Cols;

            r_AlienMatrix = new Alien[k_Rows, k_Cols];
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
                for (int j = 0; j < k_Cols; j++)
                {
                    if (i == 0)
                    {
                        r_AlienMatrix[i, j] = new Alien(Game, 220, new Rectangle[] { new Rectangle(0, 0, 32, 32), new Rectangle(32, 0, 32, 32) });
                        r_AlienMatrix[i, j].TintColor = Color.Pink;
                    }
                    else if (i >= 1 && i <= 2)
                    {
                        r_AlienMatrix[i, j] = new Alien(Game, 160, new Rectangle[] { new Rectangle(32 * (i % 2), 32, 32, 32), new Rectangle(32 * ((i + 1) % 2), 32, 32, 32) });
                        r_AlienMatrix[i, j].TintColor = Color.PowderBlue;
                    }
                    else
                    {
                        r_AlienMatrix[i, j] = new Alien(Game, 90, new Rectangle[] { new Rectangle(32 * (i % 2), 64, 32, 32), new Rectangle(32 * ((i + 1) % 2), 64, 32, 32) });
                        r_AlienMatrix[i, j].TintColor = Color.LightGoldenrodYellow;
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
                timeToJump = timeToJump * 0.94f;
                m_AlienCounter--;
            }

            if(m_AlienCounter == 0)
            {
                OnAllDead();
            }
        }

        private void initializeBoardPositions()
        {
            m_JumpDistance = r_AlienMatrix[0, 0].SourceRectangle.Width / 2;

            for (int i = 0; i < k_Rows; i++)
            {
                for (int j = 0; j < k_Cols; j++)
                {
                    r_AlienMatrix[i, j].Position = new Vector2(0, r_AlienMatrix[i, j].SourceRectangle.Height * 3) + new Vector2((int)Math.Round(j * r_AlienMatrix[i, j].SourceRectangle.Width * 1.6), (int)Math.Round(i * r_AlienMatrix[i, j].SourceRectangle.Height * 1.6f));
                }
            }
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
            bool foundMostRight = false;

            for (int j = k_Cols - 1; j >= 0 && !foundMostRight; j--)
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

            for (int j = 0; j < k_Cols && !foundMostLeft; j++)
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

            for (int j = k_Cols - 1; j >= 0 && !isAtBottom; j--)
            {
                for (int i = k_Rows - 1; i >= 0 && !isAtBottom; i--)
                {
                    if(r_AlienMatrix[i, j].Visible && r_AlienMatrix[i, j].Position.Y + r_AlienMatrix[i, j].Height >= YBottomPosition)
                    {
                        isAtBottom = true;
                    }
                }
            }

            return isAtBottom;
        }

        protected virtual void OnAllDead()
        {
            if(AllDead != null)
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
    }
}