using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.Models.Animators
{
    public class FadeAnimator : Animator2D
    {
        private readonly float r_AmountOfFadeInASecond;

        public FadeAnimator(string i_Name, TimeSpan i_AnimationLength, float i_StartOpacity, float i_FinalOpacity)
            : base(i_Name, i_AnimationLength)
        {
            r_AmountOfFadeInASecond = (i_StartOpacity - i_FinalOpacity) / (float)i_AnimationLength.TotalSeconds;
        }

        public FadeAnimator(TimeSpan i_AnimationLength, float i_StartOpacity, float i_FinalOpacity)
            : this("FadeAnimator", i_AnimationLength, i_StartOpacity, i_FinalOpacity)
        {
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            float opacityChange = r_AmountOfFadeInASecond * (float)i_GameTime.ElapsedGameTime.TotalSeconds;

            if (opacityChange > 0)
            {
                BoundComponent2D.Opacity = Math.Max(0, BoundComponent2D.Opacity - opacityChange);
            }
            else
            {
                BoundComponent2D.Opacity = Math.Min(1, BoundComponent2D.Opacity - opacityChange);
            }
        }

        protected override void RevertToOriginal()
        {
            BoundComponent2D.Opacity = m_OriginalComponent2DInfo.Opacity;
        }
    }
}
