using Infrastructure.Models;
using Infrastructure.Models.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SpaceInvaders.Models.Texts;

namespace SpaceInvaders.Models.Screens
{
    public class PauseScreen : GameScreen
    {
        private readonly Headline r_Headline;
        private readonly Text r_ResumeText;

        public PauseScreen(Game i_Game) : base(i_Game)
        {
            this.IsOverlayed = true;
            this.BlackTintAlpha = 0.4f;

            r_Headline = new Headline(Game);
            r_Headline.Content = "Pause";
            this.Add(r_Headline);

            r_ResumeText = new Text(Game, "Calibri");
            r_ResumeText.Content = "To resume press R";
            this.Add(r_ResumeText);
        }

        public override void Initialize()
        {
            base.Initialize();

            r_Headline.Position = new Vector2(CenterOfViewPort.X - (r_Headline.Width / 2), 100);
            r_ResumeText.Position = new Vector2(50, CenterOfViewPort.Y - (r_ResumeText.Height / 2));
        }

        public override void Update(GameTime gameTime)
        {
            if(KeyboardManager.IsKeyPressed(Keys.R))
            {
                ExitScreen();
            }

            base.Update(gameTime);
        }
    }
}
