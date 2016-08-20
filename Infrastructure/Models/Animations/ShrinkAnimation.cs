using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.Models.Animations
{
    public class ShrinkAnimation : Animation
    {
        private readonly Vector2 r_ShrinkVelocity;

        public ShrinkAnimation(Game i_Game, Sprite i_Sprite, TimeSpan i_AnimationLength)
            : base(i_Game, i_Sprite, i_AnimationLength)
        {
            r_ShrinkVelocity = new Vector2((float)(1 / i_AnimationLength.TotalSeconds));
        }

        protected override void updateFrame(GameTime i_GameTime)
        {
            m_Sprite.Scales -= r_ShrinkVelocity * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void RevertToOriginalStart()
        {
            m_Sprite.Scales = r_OriginalSpriteState.Scales;
        }
    }
}
