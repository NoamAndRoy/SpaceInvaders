using System;
using Infrastructure.Models.Animators;
using Microsoft.Xna.Framework;

namespace Infrastructure.Models.Controls
{
    public abstract class InvadersControl : Control
    {
        private const double k_AnimationLength = 1.8f;
        private const double k_PulseLength = 7;

        public InvadersControl(Game i_Game, string i_Name, Text i_Text, bool i_IsActiveable = true)
            : base(i_Game, i_Name, i_Text, i_IsActiveable)
        {
        }

        public InvadersControl(Game i_Game, string i_Name, Text i_Text, Sprite i_Texture, bool i_IsActiveable = true)
            : base(i_Game, i_Name, i_Text, i_Texture, i_IsActiveable)
        {
        }

        public override void Initialize()
        {
            Animator2D pulseAnimator = new PulseAnimator(TimeSpan.FromSeconds(k_AnimationLength), TimeSpan.FromSeconds(k_PulseLength), this.Scales, this.Scales * 0.6f);
            Animations.Add(pulseAnimator);            

            base.Initialize();
        }

        protected override void OnEnabledChanged(object sender, EventArgs args)
        {
            base.OnEnabledChanged(sender, args);

            if(Enabled)
            {
                Animations["PulseAnimator"].Resume();
            }
            else
            {
                Animations["PulseAnimator"].Reset();
            }
        }

        public override void Update(GameTime i_GameTime)
        {
            Animations.Update(i_GameTime);

            base.Update(i_GameTime);
        }
    }
}
