using Infrastructure.Models;
using Microsoft.Xna.Framework;

namespace SpaceInvaders.Models
{
    public class WallsLine : CompositeDrawableComponent<Wall>
    {
        private const int k_Cells = 4;

        private readonly Wall[] r_WallsLine;

        private bool m_JumpRight;
        private float m_YPosition;

        public float YPosition
        {
            get { return m_YPosition; }
            set
            {
                m_YPosition = value;
                initializeLinePositions();
            }
        }

        public WallsLine(Game i_Game) 
            : base(i_Game)
        {
            r_WallsLine = new Wall[k_Cells];
            initializeLine();
        }

        public override void Initialize()
        {
            base.Initialize();

            m_JumpRight = true;
        }

        private void initializeLine()
        {
            for (int i = 0; i < k_Cells; i++)
            {
                r_WallsLine[i] = new Wall(Game);
                this.Add(r_WallsLine[i]);
            }
        }

        private void initializeLinePositions()
        {
            int x = (Game.GraphicsDevice.Viewport.Width - (7 * (int)r_WallsLine[0].Width)) / 2;
            for (int i = 0; i < k_Cells; i++)
            {
                r_WallsLine[i].Position = new Vector2(x + (i * 2 * r_WallsLine[i].Width), YPosition);
                r_WallsLine[i].SpeedFactor = new Vector2(1, 0);
            }
        }

        public override void Update(GameTime i_GameTime)
        {
            if(getDistanceToRight() <= 0 || getDistanceToLeft() <= 0)
            {
                m_JumpRight = !m_JumpRight;
            }

            for (int i = 0; i < k_Cells; i++)
            {
                r_WallsLine[i].SpeedFactor *= new Vector2(m_JumpRight ? 1 : -1, 0);
            }
            
            base.Update(i_GameTime);
        }

        private float getDistanceToRight()
        {
            float remainSpace = 0;
            bool found = false;
            int maxSpace = (Game.GraphicsDevice.Viewport.Width - (8 * (int)r_WallsLine[0].Width)) / 2;
            
            for (int i = k_Cells - 1; !found && i >= 0; i--)
            {
                if (r_WallsLine[i].Visible)
                {
                    remainSpace = Game.GraphicsDevice.Viewport.Width - r_WallsLine[i].Position.X - r_WallsLine[i].Width - maxSpace;
                    found = true;
                }
            }

            return remainSpace;
        }

        private float getDistanceToLeft()
        {
            float remainSpace = 0;
            bool found = false;
            int maxSpace = (Game.GraphicsDevice.Viewport.Width - (8 * (int)r_WallsLine[0].Width)) / 2;

            for (int i = 0; !found && i < k_Cells; i++)
            {
                if (r_WallsLine[i].Visible)
                {
                    remainSpace = r_WallsLine[i].Position.X - maxSpace; 
                    found = true;
                }
            }

            return remainSpace;
        }
    }
}
