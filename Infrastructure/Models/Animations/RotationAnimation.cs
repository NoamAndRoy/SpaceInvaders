using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        protected override void DoAnimation(GameTime i_GameTime)
        {
            m_Sprite.Rotation += r_AmountOfRotationInASecond * MathHelper.TwoPi * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
        }

        protected override void OnFinished()
        {
            m_Sprite.Rotation = r_OriginalSpriteState.Rotation;
            base.OnFinished();
        }
    }
}