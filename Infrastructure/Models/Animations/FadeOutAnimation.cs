﻿using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.Models.Animations
{
    public class FadeOutAnimation : SpriteAnimation
    {
        private readonly float r_AmountOfFadeOutInASecond;
        private float m_Opacity;

        public FadeOutAnimation(Game i_Game, Sprite i_Sprite, TimeSpan i_AnimationLength)
            : base(i_Game, i_Sprite, i_AnimationLength)
        {
            r_AmountOfFadeOutInASecond = 1 / (float)i_AnimationLength.TotalSeconds;
            m_Opacity = 1;
        }

        protected override void doAnimation(GameTime i_GameTime)
        {
            m_Opacity -= r_AmountOfFadeOutInASecond * (float)i_GameTime.ElapsedGameTime.TotalSeconds;

            if (m_Opacity > 0)
            {
                m_Sprite.Opacity = m_Opacity;
            }
            else
            {
                m_Sprite.Opacity = 0;
            }
        }

        public override void RevertToOriginalStart()
        {
            m_Sprite.Opacity = r_OriginalSpriteState.Opacity;
        }
    }
}