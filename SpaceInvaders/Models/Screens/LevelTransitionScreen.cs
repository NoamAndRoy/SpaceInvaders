using System;
using Infrastructure.Models.Animators;
using Microsoft.Xna.Framework;
using SpaceInvaders.Models.Texts;
using Infrastructure.Models.Screens;

namespace SpaceInvaders.Models.Screens
{
    public class LevelTransitionScreen : InvadersGameScreen
    {
        private readonly Headline r_LevelTitle;
        private readonly Headline r_CountDown;
        private readonly int r_LevelNumber;
        private readonly int r_playerOneScore;
        private readonly int r_playerTwoScore;

        private int m_Count = 3;

        public LevelTransitionScreen(Game i_Game, int i_LevelNumber, int i_playerOneScore = 0, int i_playerTwoScore = 0) 
            : base(i_Game)
        {
            r_LevelNumber = i_LevelNumber;
            r_playerOneScore = i_playerOneScore;
            r_playerTwoScore = i_playerTwoScore;

            r_LevelTitle = new Headline(this);
            r_LevelTitle.Content = "Level " + i_LevelNumber;
            this.Add(r_LevelTitle);

            r_CountDown = new Headline(this);
            r_CountDown.Content = m_Count.ToString();
            this.Add(r_CountDown);
        }

        public override void Initialize()
        {
            base.Initialize();

            r_LevelTitle.Position = new Vector2(CenterOfViewPort.X - (r_LevelTitle.Width / 2), r_LevelTitle.Height);

            r_CountDown.Position = CenterOfViewPort - (new Vector2(r_CountDown.Width, r_CountDown.Height) / 2);
            r_CountDown.RotationOrigin = r_CountDown.TextureCenter;
            r_CountDown.Scales = Vector2.Zero;

            r_CountDown.Animations.Add(new SizeAnimator(TimeSpan.FromSeconds(1), r_CountDown.Scales, new Vector2(1)));
            r_CountDown.Animations["SizeAnimator"].Finished += animations_Finished;
            r_CountDown.Animations.Resume();
        }

        private void animations_Finished(object sender, EventArgs e)
        {
            r_CountDown.Animations.Reset();
            r_CountDown.Animations.Resume();
            r_CountDown.Content = (--m_Count).ToString();

            if(m_Count == 0)
            {
                ScreensManager.SetCurrentScreen(new PlayScreen(Game, r_LevelNumber, r_playerOneScore, r_playerTwoScore));
                ExitScreen();
            }
        }
    }
}
