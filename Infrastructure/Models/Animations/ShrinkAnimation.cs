using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        protected override void doAnimation(GameTime i_GameTime)
        {
            m_Sprite.Scales -= r_ShrinkVelocity * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
        }

        protected override void revertSpriteState()
        {
            m_Sprite.Scales = r_OriginalSpriteState.Scales;
        }
    }
}
