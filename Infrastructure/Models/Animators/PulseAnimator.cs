using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.Models.Animators
{
    public class PulseAnimator : Animator2D
    {
        private Vector2 m_SizeChangeVelocity;
        private Vector2 m_InitialSize;
        private Vector2 m_PulseSize;

        private TimeSpan m_HalfPulseLength;
        private TimeSpan m_TimeLeftForNextPulse;

        private bool m_ToPulseSize;

        public PulseAnimator(string i_Name, TimeSpan i_AnimationLength, TimeSpan i_PulseLength, Vector2 i_InitialSize, Vector2 i_PulseSize)
            : base(i_Name, i_AnimationLength)
        {
            m_ToPulseSize = true;

            this.m_HalfPulseLength = TimeSpan.FromMilliseconds(i_PulseLength.TotalMilliseconds / 2);
            this.m_TimeLeftForNextPulse = i_PulseLength;

            this.m_InitialSize = i_InitialSize;
            this.m_PulseSize = i_PulseSize;
        }

        public PulseAnimator(TimeSpan i_AnimationLength, TimeSpan i_PulseLength, Vector2 i_InitialSize, Vector2 i_PulseSize)
            : this("PulseAnimator", i_AnimationLength, i_PulseLength, i_InitialSize, i_PulseSize)
        {
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            BoundComponent2D.Scales = m_OriginalComponent2DInfo.Scales/2;

            m_TimeLeftForNextPulse -= i_GameTime.ElapsedGameTime;

            if (m_TimeLeftForNextPulse.TotalSeconds < 0)
            {
                m_ToPulseSize = !m_ToPulseSize;
                m_TimeLeftForNextPulse = m_HalfPulseLength;
            }

            m_SizeChangeVelocity = new Vector2((float)((m_InitialSize.X - m_PulseSize.X) / m_TimeLeftForNextPulse.TotalSeconds), (float)((m_InitialSize.Y - m_PulseSize.Y) / m_TimeLeftForNextPulse.TotalSeconds));

            if(m_ToPulseSize)
            {
                BoundComponent2D.Scales -= m_SizeChangeVelocity * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                BoundComponent2D.Scales += m_SizeChangeVelocity * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        protected override void RevertToOriginal()
        {
            BoundComponent2D.Scales = m_OriginalComponent2DInfo.Scales;
        }
    }
}
