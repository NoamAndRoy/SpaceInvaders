////*** Guy Ronen © 2008-2011 ***//
using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.Models.Animators
{
    public class SequencialAnimator : CompositeAnimator
    {
        public SequencialAnimator(
            string i_Name,
            TimeSpan i_AnimationLength,
            Component2D i_BoundComponent2D,
            params Animator2D[] i_Animations)
            : base(i_Name, i_AnimationLength, i_BoundComponent2D, i_Animations)
        {
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            bool allFinished = true;
            foreach (Animator2D animation in m_AnimationsList)
            {
                if (!animation.IsFinished)
                {
                    animation.Update(i_GameTime);
                    allFinished = false;
                    break;
                }
            }

            if (allFinished)
            {
                this.IsFinished = true;
            }
        }
    }
}
