using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.Models.Animations
{
    public delegate void FinishedEventHandler(Animation i_Animation);

    public abstract class Animation : RegisteredComponent
    {
        protected readonly Sprite r_OriginalSprite;

        protected Sprite m_Sprite;
        private TimeSpan m_TimeTillAnimationEnd;
        private TimeSpan m_AnimationLength;
        private bool m_IsFinished = false;
        private bool m_ResetAfterFinished = true;

        public event FinishedEventHandler Finished;

        internal TimeSpan AnimationLength
        {
            get { return m_AnimationLength; }
            set { m_AnimationLength = value; }
        }

        public Sprite Sprite
        {
            get { return m_Sprite; }
            set { m_Sprite = value; }
        }

        public bool IsFinished
        {
            get { return m_IsFinished; }
            protected set
            {
                m_IsFinished = value;

                if (value == true)
                {
                    OnFinished();
                }
            }
        }

        public Animation(Game i_Game, Sprite i_Sprite, TimeSpan i_AnimationLength)
            : base(i_Game)
        {
            m_Sprite = i_Sprite;
            r_OriginalSprite = m_Sprite.ShallowClone();
            m_AnimationLength = i_AnimationLength;
            m_TimeTillAnimationEnd = m_AnimationLength;
            Enabled = false;
        }

        public virtual void Start()
        {
            Enabled = true;
            IsFinished = false;
        }

        public virtual void Pause()
        {
            Enabled = false;
        }

        public virtual void Reset()
        {
            m_TimeTillAnimationEnd = m_AnimationLength;
            Enabled = false;
            RevertToOriginalStart();
        }

        public abstract void RevertToOriginalStart();

        public sealed override void Update(GameTime i_GameTime)
        {
            if (!IsFinished)
            {
                m_TimeTillAnimationEnd -= i_GameTime.ElapsedGameTime;
                if (m_TimeTillAnimationEnd.TotalSeconds <= 0 && m_AnimationLength != TimeSpan.Zero)
                {
                    IsFinished = true;
                }
                else
                {
                    updateFrame(i_GameTime);
                }
            }
        }

        protected virtual void OnFinished()
        {
            if(Finished != null)
            {
                Finished(this);
            }
        }

        protected abstract void updateFrame(GameTime i_GameTime);
    }
}