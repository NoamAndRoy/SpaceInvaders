using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.Models.Animations
{
    public class CelAnimation : Animation
    {
        private readonly Rectangle[] r_SourceRectangles;
        private TimeSpan m_AmountOfSecondsTillNextSourceRectangle;
        private TimeSpan m_PassedTime = TimeSpan.Zero;
        private int m_CurrentSourceRectangleIndex = 0;

        public float AmountOfCellsInASecond
        {
            get { return (float)(1 / m_AmountOfSecondsTillNextSourceRectangle.TotalSeconds); }
            set { m_AmountOfSecondsTillNextSourceRectangle = TimeSpan.FromSeconds(1 / value); }
        }

        public CelAnimation(Game i_Game, Sprite i_Sprite, float i_AmountOfCellsInASecond, params Rectangle[] i_SourceRectangles)
            : base(i_Game, i_Sprite, TimeSpan.Zero)
        {
            r_SourceRectangles = i_SourceRectangles;
            m_AmountOfSecondsTillNextSourceRectangle = TimeSpan.FromSeconds(1 / i_AmountOfCellsInASecond);
        }

        public override void Reset()
        {
            m_PassedTime = TimeSpan.Zero;
            m_CurrentSourceRectangleIndex = 0;
            base.Reset();
        }

        protected override void updateFrame(GameTime i_GameTime)
        {
            if (m_PassedTime >= m_AmountOfSecondsTillNextSourceRectangle)
            {
                m_PassedTime -= m_AmountOfSecondsTillNextSourceRectangle;
                if(++m_CurrentSourceRectangleIndex == r_SourceRectangles.Length)
                {
                    m_CurrentSourceRectangleIndex = 0;
                }

                m_Sprite.SourceRectangle = r_SourceRectangles[m_CurrentSourceRectangleIndex];
            }

            m_PassedTime += i_GameTime.ElapsedGameTime;
        }
    }
}