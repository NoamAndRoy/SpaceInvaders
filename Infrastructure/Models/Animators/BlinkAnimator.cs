using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.Models.Animators
{
    public class BlinkAnimator : Animator2D
    {
        private TimeSpan m_BlinkLength;
        private TimeSpan m_TimeLeftForNextBlink;

        public TimeSpan BlinkLength
        {
            get { return m_BlinkLength; }
            set { m_BlinkLength = value; }
        }

        public BlinkAnimator(string i_Name, TimeSpan i_BlinkLength, TimeSpan i_AnimationLength)
            : base(i_Name, i_AnimationLength)
        {
            this.m_BlinkLength = i_BlinkLength;
            this.m_TimeLeftForNextBlink = i_BlinkLength;
        }

        public BlinkAnimator(TimeSpan i_BlinkLength, TimeSpan i_AnimationLength)
            : this("Blink", i_BlinkLength, i_AnimationLength)
        {
            this.m_BlinkLength = i_BlinkLength;
            this.m_TimeLeftForNextBlink = i_BlinkLength;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            m_TimeLeftForNextBlink -= i_GameTime.ElapsedGameTime;
            if (m_TimeLeftForNextBlink.TotalSeconds < 0)
            {
                this.BoundComponent2D.Visible = !this.BoundComponent2D.Visible;
                m_TimeLeftForNextBlink = m_BlinkLength;
            }
        }

        protected override void RevertToOriginal()
        {
            this.BoundComponent2D.Visible = m_OriginalComponent2DInfo.Visible;
        }
    }
}
