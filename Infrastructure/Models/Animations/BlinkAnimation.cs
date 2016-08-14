﻿using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.Models.Animations
{
    public class BlinkAnimation : Animation
    {
        private readonly TimeSpan r_AmountOfSecondsTillBlink;
        private TimeSpan m_PassedTime = TimeSpan.Zero;
        public BlinkAnimation(Game i_Game, Sprite i_Sprite, TimeSpan i_AnimationLength, float i_AmountOfBlinksInASecond)
            : base(i_Game, i_Sprite, i_AnimationLength)
        {
            r_AmountOfSecondsTillBlink = TimeSpan.FromSeconds(1 / i_AmountOfBlinksInASecond / 2);
        }

        protected override void doAnimation(GameTime i_GameTime)
        {
            m_PassedTime += i_GameTime.ElapsedGameTime;

            if(m_PassedTime >= r_AmountOfSecondsTillBlink)
            {
                m_PassedTime -= r_AmountOfSecondsTillBlink;
                m_Sprite.Visible = !m_Sprite.Visible;
            }
        }

        protected override void revertSpriteState()
        {
            m_Sprite.Visible = r_OriginalSpriteState.Visible;
        }
    }
}