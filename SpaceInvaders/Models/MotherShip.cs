﻿using System;
using Microsoft.Xna.Framework;
using Infrastructure.Models;
using SpaceInvaders.Interfaces;
using Infrastructure.ManagersInterfaces;
using Infrastructure.Models.Animations;

namespace SpaceInvaders.Models
{
    public class MotherShip : CollideableSprite, IScoreable
    {
        private const int k_Speed = 95;
        private const int k_Score = 600;
        private const string k_TexturePath = "MotherShip_32x120";

        private const double k_AnimationLength = 2.6;
        private const float k_AmountOfBlinksInASecond = 5;

        private const int k_MinAppearTime = 5;
        private const int k_MaxAppearTime = 20;

        private Vector2 m_StartingPos;

        private Random m_RandomAppearTime;
        private int m_SecondsToNextAppear;
        private bool m_IsDead;

        private AnimationRepository m_HitAnimations;

        public int Score
        {
            get
            {
                IsScoreAvailable = false;
                return k_Score;
            }
        }

        public bool IsScoreAvailable { get; private set; }

        public MotherShip(Game i_Game)
            : base(i_Game, k_TexturePath)
        {
            m_RandomAppearTime = new Random();
            m_IsDead = true;
            AlphaBlending = true;
            IsScoreAvailable = true;
        }

        public override void Initialize()
        {
            base.Initialize();

            m_StartingPos = new Vector2(-1 * Width, Height);
            Position = m_StartingPos;

            m_SecondsToNextAppear = m_RandomAppearTime.Next(k_MinAppearTime, k_MaxAppearTime);

            m_HitAnimations = new AnimationRepository(Game, this, TimeSpan.FromSeconds(k_AnimationLength));
            m_HitAnimations.AddAnimation(new ShrinkAnimation(Game, this, TimeSpan.FromSeconds(k_AnimationLength)));
            m_HitAnimations.AddAnimation(new BlinkAnimation(Game, this, TimeSpan.FromSeconds(k_AnimationLength), k_AmountOfBlinksInASecond));
            m_HitAnimations.AddAnimation(new FadeOutAnimation(Game, this, TimeSpan.FromSeconds(k_AnimationLength)));

            m_HitAnimations.Finished += hitAnimations_Finished;
        }

        public override void Collided(ICollideable i_Collideable)
        {
            m_HitAnimations.Start();
            OnCollide(i_Collideable);
        }

        private void hitAnimations_Finished(Animation i_Animation)
        {
            m_HitAnimations.Reset();
            resetMotherShip();
        }

        public override void Update(GameTime i_GameTime)
        {
            if (m_IsDead && i_GameTime.TotalGameTime.Seconds > 0 && i_GameTime.TotalGameTime.Seconds % m_SecondsToNextAppear == 0)
            {
                m_IsDead = false;
                Velocity = new Vector2(k_Speed, 0);
            }
            
            if (!m_IsDead && Position.X >= Game.GraphicsDevice.Viewport.Width)
            {
                resetMotherShip();
            }

            base.Update(i_GameTime);
        }

        private void resetMotherShip()
        {
            Position = m_StartingPos;
            m_IsDead = true;
            Velocity = Vector2.Zero;
            m_SecondsToNextAppear = m_RandomAppearTime.Next(k_MinAppearTime, k_MaxAppearTime);
            IsScoreAvailable = true;
        }
    }
}