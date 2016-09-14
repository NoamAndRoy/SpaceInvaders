using Infrastructure.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SpaceInvaders.Models.Texts;

namespace SpaceInvaders.Models.Screens
{
    public class WelcomeScreen : InvadersGameScreen
    {
        private readonly Headline r_WelcomeText;
        private readonly Text r_Instructions;

        public WelcomeScreen(Game i_Game)
            : base(i_Game)
        {
            r_WelcomeText = new Headline(this);
            r_WelcomeText.Content = "Welcome to Space Invaders!";
            this.Add(r_WelcomeText);

            r_Instructions = new Text(this, "Calibri");
            r_Instructions.Content = 
@"Press Enter to start the Game
Press Esc to Exit
Press H to navigate to the Main Menu";

            this.Add(r_Instructions);
        }

        public override void Initialize()
        {
            base.Initialize();

            r_WelcomeText.Position = this.CenterOfViewPort - (new Vector2(r_WelcomeText.Width, r_WelcomeText.Height) / 2) - new Vector2(0, this.CenterOfViewPort.Y / 2);
            r_Instructions.Position = new Vector2(r_WelcomeText.Position.X, this.Game.GraphicsDevice.Viewport.Height - (r_Instructions.Height * 2));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (KeyboardManager.IsKeyPressed(Keys.Escape))
            {
                Game.Exit();
            }
            else if(KeyboardManager.IsKeyPressed(Keys.Enter))
            {
                ScreensManager.SetCurrentScreen(new LevelTransitionScreen(Game, 1));
                ExitScreen();
            }
            else if(KeyboardManager.IsKeyPressed(Keys.H))
            {
                ScreensManager.SetCurrentScreen(new MainMenuScreen(Game));
                ExitScreen();
            }
        }       
    }
}