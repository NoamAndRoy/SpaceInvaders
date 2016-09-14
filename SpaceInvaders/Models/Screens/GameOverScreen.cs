using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SpaceInvaders.Models.Texts;
using Infrastructure.Models.Screens;

namespace SpaceInvaders.Models.Screens
{
    public class GameOverScreen : InvadersGameScreen
    {
        private readonly Headline r_GameOverText;
        private readonly Text r_Score;
        private readonly Text r_Instructions;

        public GameOverScreen(Game i_Game, params Player[] i_Players) 
            : base(i_Game)
        {
            r_GameOverText = new Headline(this);
            r_GameOverText.Content = "Game Over";
            this.Add(r_GameOverText);

            r_Score = new Text(this, "Calibri");
            r_Score.Content = determineWinMessage(i_Players);
            this.Add(r_Score);

            r_Instructions = new Text(this, "Calibri");
            r_Instructions.Content =
@"Press Esc to Exit
Press R to Reset
Press H to navigate to the Main Menu";

            this.Add(r_Instructions);
        }

        public override void Initialize()
        {
            base.Initialize();
            r_GameOverText.Position = this.CenterOfViewPort - (new Vector2(r_GameOverText.Width, r_GameOverText.Height) / 2) - new Vector2(0, this.CenterOfViewPort.Y / 2);
            r_Instructions.Position = new Vector2(r_GameOverText.Position.X, this.Game.GraphicsDevice.Viewport.Height - (r_Instructions.Height * 2));
            r_Score.Position = CenterOfViewPort - (r_Score.TextureCenter / 2);
        }

        protected override void OnActivated()
        {
            r_Score.Position = r_GameOverText.Position + new Vector2(0, r_GameOverText.Height * 2);
            base.OnActivated();
        }

        private string determineWinMessage(Player[] i_Player)
        {
            StringBuilder winMessage = new StringBuilder();
            int maxScore = 0;
            List<string> playersThatWon = new List<string>();

            for(int i = 0; i < i_Player.Length && i_Player[i] != null; i++)
            {
                winMessage.AppendLine(string.Format("{0} score is: {1}", i_Player[i].PlayerName, i_Player[i].PlayerScore));

                if(i_Player[i].PlayerScore > maxScore)
                {
                    playersThatWon.Clear();
                    playersThatWon.Add(i_Player[i].PlayerName);
                    maxScore = i_Player[i].PlayerScore;
                }
                else if(i_Player[i].PlayerScore == maxScore)
                {
                    playersThatWon.Add(i_Player[i].PlayerName);
                }
            }

            if(playersThatWon.Count == 1)
            {
                winMessage.Insert(0, string.Format("The winner is {0} with score {1}{2}", playersThatWon[0], maxScore, Environment.NewLine));
            }
            else
            {
                StringBuilder winners = new StringBuilder("Tie!! ");
                for(int i = 0; i < playersThatWon.Count; i++)
                {
                    if (i != playersThatWon.Count - 1)
                    {
                        winners.AppendFormat("{0}, ", playersThatWon[i]);
                    }
                    else
                    {
                        winners.AppendFormat("{0} ", playersThatWon[i]);
                    }
                }

                winners.AppendFormat("with score {0}{1}", maxScore, Environment.NewLine);

                winMessage.Insert(0, winners.ToString());
            }

            return winMessage.ToString();
        }

        public override void Update(GameTime gameTime)
        {
             base.Update(gameTime);

            if (KeyboardManager.IsKeyPressed(Keys.Escape))
            {
                Game.Exit();
            }
            else if (KeyboardManager.IsKeyPressed(Keys.R))
            {
                ScreensManager.SetCurrentScreen(new LevelTransitionScreen(Game, 1));
                ExitScreen();
            }
            else if (KeyboardManager.IsKeyPressed(Keys.H))
            {
                ScreensManager.SetCurrentScreen(new MainMenuScreen(Game));
                ExitScreen();
            }
        }
    }
}
