using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.Models.Animators
{
    public class SizeAnimator : Animator2D
    {
        private readonly Vector2 r_SizeChangeVelocity;

        public SizeAnimator(string i_Name, TimeSpan i_AnimationLength, Vector2 i_InitialSize, Vector2 i_FinalSize)
            : base(i_Name, i_AnimationLength)
        {
            r_SizeChangeVelocity = new Vector2((float)((i_InitialSize.X - i_FinalSize.X) / i_AnimationLength.TotalSeconds), (float)((i_InitialSize.Y - i_FinalSize.Y) / i_AnimationLength.TotalSeconds));
        }

        public SizeAnimator(TimeSpan i_AnimationLength, Vector2 i_InitialSize, Vector2 i_FinalSize)
            : this("SizeAnimator", i_AnimationLength, i_InitialSize, i_FinalSize)
        {
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            BoundComponent2D.Scales -= r_SizeChangeVelocity * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
        }

        protected override void RevertToOriginal()
        {
            BoundComponent2D.Scales = m_OriginalComponent2DInfo.Scales;
        }
    }
}
