using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Infrastructure.Models.Animations
{
    public class AnimationRepository : Animation
    {
        private readonly Dictionary<Type, Animation> r_Animations;

        public AnimationRepository(Game i_Game, Sprite i_Sprite, TimeSpan i_AnimationLength)
            : base(i_Game, i_Sprite, i_AnimationLength)
        {
            r_Animations = new Dictionary<Type, Animation>();
        }

        public Animation this[Type i_AnimationType]
        {
            get { return r_Animations[i_AnimationType]; }
        }

        public void AddAnimation(Animation i_Animation)
        {
            if (!r_Animations.ContainsKey(i_Animation.GetType()))
            {
                i_Animation.Sprite = m_Sprite;
                i_Animation.AnimationLength = AnimationLength;
                r_Animations.Add(i_Animation.GetType(), i_Animation);
            }
        }

        public void RemoveAnimation(Animation i_Animation)
        {
            if (r_Animations.ContainsKey(i_Animation.GetType()))
            {
                r_Animations.Remove(i_Animation.GetType());
            }
        }

        public void Start()
        {
            Enabled = true;
            foreach(Animation animation in r_Animations.Values)
            {
                animation.Enabled = true;
            }
        }

        public void Stop()
        {
            Enabled = false;
            foreach (Animation animation in r_Animations.Values)
            {
                animation.Enabled = false;
            }
        }

        public override void Reset()
        {
            foreach(Animation animation in r_Animations.Values)
            {
                animation.Reset();
            }

            base.Reset();
        }

        public override void RevertToOriginalStart()
        {
            foreach (Animation animation in r_Animations.Values)
            {
                animation.RevertToOriginalStart();
            }
        }

        protected override void updateFrame(GameTime i_GameTime)
        {
        }
    }
}