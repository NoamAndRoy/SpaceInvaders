using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.Models.Animations
{
    public class CelAnimation : SpriteAnimation
    {
        private readonly Rectangle[] r_SourceRectangles;
        private readonly TimeSpan r_AmountOfSecondsTillNextSourceRectangle;
        private TimeSpan m_PassedTime = TimeSpan.Zero;
        private int m_CurrentSourceRectangleIndex = 0;

        public CelAnimation(Game i_Game, Sprite i_Sprite, float i_AmountOfCellsInASecond, params Rectangle[] i_SourceRectangles)
            : base(i_Game, i_Sprite, TimeSpan.Zero)
        {
            r_SourceRectangles = i_SourceRectangles;
            r_AmountOfSecondsTillNextSourceRectangle = TimeSpan.FromSeconds(1 / i_AmountOfCellsInASecond);
        }

        protected override void doAnimation(GameTime i_GameTime)
        {
            if (m_PassedTime >= r_AmountOfSecondsTillNextSourceRectangle)
            {
                m_PassedTime -= r_AmountOfSecondsTillNextSourceRectangle;
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