using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.Models.Animations
{
    public delegate void FinishedEventHandler(SpriteAnimation i_Animation);

    public abstract class SpriteAnimation : RegisteredComponent
    {
        protected readonly Sprite r_OriginalSpriteState;
        private readonly TimeSpan r_AnimationLength;

        protected Sprite m_Sprite;
        private TimeSpan m_TimeTillAnimationEnd;

        private bool m_IsFinished = false;
        private bool m_ResetAfterFinished = true;

        public event FinishedEventHandler Finished;

        public bool IsFinished
        {
            get { return m_IsFinished; }
            set
            {
                m_IsFinished = value;

                if (value == true)
                {
                    OnFinished();
                }
            }
        }

        public SpriteAnimation(Game i_Game, Sprite i_Sprite, TimeSpan i_AnimationLength)
            : base(i_Game)
        {
            m_Sprite = i_Sprite;
            r_OriginalSpriteState = m_Sprite.ShallowClone();
            r_AnimationLength = i_AnimationLength;
            m_TimeTillAnimationEnd = r_AnimationLength;
        }

        public void Reset()
        {
            m_TimeTillAnimationEnd = r_AnimationLength;
            RevertToOriginalStart();
        }

        public virtual void RevertToOriginalStart()
        {
            
        }

        public override void Update(GameTime i_GameTime)
        {
            if (!IsFinished)
            {
                if (m_TimeTillAnimationEnd.TotalSeconds < 0 && r_AnimationLength != TimeSpan.Zero)
                {
                    IsFinished = true;
                }
                else
                {
                    doAnimation(i_GameTime);
                }

                m_TimeTillAnimationEnd -= i_GameTime.ElapsedGameTime;
            }
        }

        protected virtual void OnFinished()
        {
            if(Finished != null)
            {
                Finished(this);
            }
        }

        protected abstract void doAnimation(GameTime i_GameTime);
    }
}