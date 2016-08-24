using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.Models.Animations
{
    public class FadeOutAnimation : Animation
    {
        private readonly float r_AmountOfFadeOutInASecond;

        public FadeOutAnimation(Game i_Game, Sprite i_Sprite, TimeSpan i_AnimationLength)
            : base(i_Game, i_Sprite, i_AnimationLength)
        {
            r_AmountOfFadeOutInASecond = 1 / (float)i_AnimationLength.TotalSeconds;
        }

        protected override void updateFrame(GameTime i_GameTime)
        {
            m_Sprite.Opacity = Math.Max(0, m_Sprite.Opacity - (r_AmountOfFadeOutInASecond * (float)i_GameTime.ElapsedGameTime.TotalSeconds));
        }

        public override void RevertToOriginalStart()
        {
            m_Sprite.Opacity = r_OriginalSprite.Opacity;
        }
    }
}
