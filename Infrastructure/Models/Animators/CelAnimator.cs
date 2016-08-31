////*** Guy Ronen © 2008-2011 ***//
using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.Models.Animators
{
    public class CelAnimator : Animator2D
    {
        private readonly Rectangle[] r_SourceRectangles;
        private TimeSpan m_CellTime;
        private TimeSpan m_TimeLeftForCell;
        private bool m_Loop = true;
        private int m_CurrentIndex = 0;

        public TimeSpan CellTime
        {
            get { return m_CellTime; }
            set { m_CellTime = value; }
        }
        
        public CelAnimator(TimeSpan i_CellTime, TimeSpan i_AnimationLength, params Rectangle[] i_SourceRectangles)
            : base("CelAnimation", i_AnimationLength)
        {
            this.m_CellTime = i_CellTime;
            this.m_TimeLeftForCell = i_CellTime;

            r_SourceRectangles = i_SourceRectangles;

            m_Loop = i_AnimationLength == TimeSpan.Zero;
        }

        private void goToNextFrame()
        {
            m_CurrentIndex++;
            if (m_CurrentIndex >= r_SourceRectangles.Length)
            {
                m_CurrentIndex = 0;
                if (m_Loop)
                {
                    m_CurrentIndex = 0;
                }
                else
                {
                    m_CurrentIndex = m_CurrentIndex - 1;
                    this.IsFinished = true;
                }
            }
        }

        protected override void RevertToOriginal()
        {
            (BoundComponent2D as Sprite).SourceRectangle = (m_OriginalComponent2DInfo as Sprite).SourceRectangle;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            if (m_CellTime != TimeSpan.Zero)
            {
                m_TimeLeftForCell -= i_GameTime.ElapsedGameTime;
                if (m_TimeLeftForCell.TotalSeconds <= 0)
                {
                    goToNextFrame();
                    m_TimeLeftForCell = m_CellTime;
                }
            }

            (BoundComponent2D as Sprite).SourceRectangle = r_SourceRectangles[m_CurrentIndex];
        }
    }
}
