////*** Guy Ronen © 2008-2011 ***//
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Infrastructure.Models.Animators
{
    public class CompositeAnimator : Animator2D
    {
        private readonly Dictionary<string, Animator2D> m_AnimationsDictionary =
            new Dictionary<string, Animator2D>();

        protected readonly List<Animator2D> m_AnimationsList = new List<Animator2D>();

        // CTORs

        // CTOR: Me as an AnimationsMamager
        public CompositeAnimator(Component2D i_BoundComponent2D)
            : this("AnimationsManager", TimeSpan.Zero, i_BoundComponent2D, new Animator2D[] { })
        {
            this.Enabled = false;
        }

        // CTOR: me as a ParallelAnimations animation:
        public CompositeAnimator(
            string i_Name,
            TimeSpan i_AnimationLength,
            Component2D i_BoundComponent2D,
            params Animator2D[] i_Animations)
            : base(i_Name, i_AnimationLength)
        {
            this.BoundComponent2D = i_BoundComponent2D;

            foreach (Animator2D animation in i_Animations)
            {
                this.Add(animation);
            }
        }

        public void Add(Animator2D i_Animation)
        {
            i_Animation.BoundComponent2D = this.BoundComponent2D;
            i_Animation.Enabled = true;
            m_AnimationsDictionary.Add(i_Animation.Name, i_Animation);
            m_AnimationsList.Add(i_Animation);
        }

        public void Remove(string i_AnimationName)
        {
            Animator2D animationToRemove;
            m_AnimationsDictionary.TryGetValue(i_AnimationName, out animationToRemove);
            if (animationToRemove != null)
            {
                m_AnimationsDictionary.Remove(i_AnimationName);
                m_AnimationsList.Remove(animationToRemove);
            }
        }

        public Animator2D this[string i_Name]
        {
            get
            {
                Animator2D retVal = null;
                m_AnimationsDictionary.TryGetValue(i_Name, out retVal);
                return retVal;
            }
        }

        public override void Restart()
        {
            base.Restart();

            foreach (Animator2D animation in m_AnimationsList)
            {
                animation.Restart();
            }
        }

        public override void Restart(TimeSpan i_AnimationLength)
        {
            base.Restart(i_AnimationLength);

            foreach (Animator2D animation in m_AnimationsList)
            {
                animation.Restart();
            }
        }

        protected override void RevertToOriginal()
        {
            foreach (Animator2D animation in m_AnimationsList)
            {
                animation.Reset();
            }
        }

        protected override void CloneSpriteInfo()
        {
            base.CloneSpriteInfo();

            foreach (Animator2D animation in m_AnimationsList)
            {
                animation.m_OriginalComponent2DInfo = m_OriginalComponent2DInfo;
            }
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            foreach (Animator2D animation in m_AnimationsList)
            {
                animation.Update(i_GameTime);
            }
        }
    }
}
