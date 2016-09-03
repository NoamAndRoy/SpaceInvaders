using System;
using Infrastructure.Models.Animators;
using Microsoft.Xna.Framework;
using SpaceInvaders.Models.Texts;

namespace SpaceInvaders.Models.Screens
{
    public class LevelTransitionScreen : InvadersGameScreen
    {
        private readonly Headline r_Level;
        private readonly Headline r_CountDown;

        private int m_Count = 3;

        public LevelTransitionScreen(Game i_Game, int i_LevelNumber) : base(i_Game)
        {
            r_Level = new Headline(Game);
            r_Level.Content = "Level " + i_LevelNumber;
            this.Add(r_Level);

            r_CountDown = new Headline(Game);
            r_CountDown.Content = m_Count.ToString();
            this.Add(r_CountDown);
        }

        public override void Initialize()
        {
            base.Initialize();

            r_Level.Position = new Vector2(CenterOfViewPort.X - (r_Level.Width / 2), r_Level.Height);

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
                ExitScreen();
                ScreensManager.SetCurrentScreen(new PlayScreen(Game));
            }
        }
    }
}
