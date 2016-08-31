using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.Models.Animators
{
    public class RotationAnimator : Animator2D
    {
        private readonly float r_AmountOfRotationInASecond;

        public RotationAnimator(string i_Name, TimeSpan i_AnimationLength, float i_AmountOfRotationInASecond)
            : base(i_Name, i_AnimationLength)
        {
            r_AmountOfRotationInASecond = i_AmountOfRotationInASecond;
        }

        public RotationAnimator(TimeSpan i_AnimationLength, float i_AmountOfRotationInASecond)
            : this("RotationAnimator", i_AnimationLength, i_AmountOfRotationInASecond)
        {
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            BoundComponent2D.Rotation += r_AmountOfRotationInASecond * MathHelper.TwoPi * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
        }

        protected override void RevertToOriginal()
        {
            BoundComponent2D.Rotation = m_OriginalComponent2DInfo.Rotation;
        }
    }
}