using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.Models.Animations
{
    public class RotationAnimation : Animation
    {
        private readonly float r_AmountOfRotationInASecond;

        public RotationAnimation(Game i_Game, Sprite i_Sprite, TimeSpan i_AnimationLength, float i_AmountOfRotationInASecond)
            : base(i_Game, i_Sprite, i_AnimationLength)
        {
            r_AmountOfRotationInASecond = i_AmountOfRotationInASecond;
        }

        protected override void updateFrame(GameTime i_GameTime)
        {
            m_Sprite.Rotation += r_AmountOfRotationInASecond * MathHelper.TwoPi * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void RevertToOriginalStart()
        {
            m_Sprite.Rotation = r_OriginalSpriteState.Rotation;
        }
    }
}