using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.Models.Animations
{

    public delegate void FinishedEventHandler(Animation i_Animation);

    public abstract class Animation : RegisteredComponent
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

        public bool ResetAfterFinished
        {
            get { return m_ResetAfterFinished; }
            set { m_ResetAfterFinished = value; }
        }

        public Animation(Game i_Game, Sprite i_Sprite, TimeSpan i_AnimationLength)
            : base(i_Game)
        {
            m_Sprite = i_Sprite;
            r_OriginalSpriteState = m_Sprite.ShallowClone();
            r_AnimationLength = i_AnimationLength;
            m_TimeTillAnimationEnd = r_AnimationLength;
        }

        public void Start()
        {
            Enabled = true;
        }

        public void Pause()
        {
            Enabled = false;
        }

        public void Reset()
        {
            m_TimeTillAnimationEnd = r_AnimationLength;
        }

        public override void Update(GameTime i_GameTime)
        {
            if (!IsFinished)
            {
                m_TimeTillAnimationEnd -= i_GameTime.ElapsedGameTime;
                if (m_TimeTillAnimationEnd.TotalSeconds <= 0 && r_AnimationLength != TimeSpan.Zero)
                {
                    IsFinished = true;
                }
                else
                {
                    DoAnimation(i_GameTime);
                }
            }
        }

        protected virtual void OnFinished()
        {
            if(Finished != null)
            {
                Finished(this);
            }

            if (ResetAfterFinished)
            {
                Reset();
            }
        }

        protected abstract void DoAnimation(GameTime i_GameTime);
    }
}
