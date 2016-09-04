﻿using Infrastructure.ManagersInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SpaceInvaders.Models.Screens
{
    public class PlayScreen : InvadersGameScreen
    {
        private const string k_GameOverSound = "GameOver";

        private readonly Player r_PlayerOne;
        private readonly Player r_PlayerTwo;
        private readonly MotherShip r_MotherShip;
        private readonly AlienMatrix r_Aliens;
        private readonly WallsLine r_Walls;

        private readonly PlayerShip r_PlayerShipOne;
        private readonly PlayerShip r_PlayerShipTwo;

        private readonly int r_LevelType;

        public PlayScreen(Game i_Game, int i_Level, int i_playerOneScore = 0, int i_playerTwoScore = 0) 
            : base(i_Game)
        {
            int levelType = (i_Level - 1) % 7, 
                cols = 9 + levelType,
                firstTypeAlienScore = 220 + 50 * levelType, 
                secondTypeAlienScore = 160 + 50 * levelType, 
                thirdTypeAlienScore = 90 + 50 * levelType, 
                maxAmountOfBullets = 1 + levelType, 
                wallsSpeed = levelType == 0 ? 0 : (int)(40 * Math.Pow(1.06, levelType - 1));

            r_LevelType = levelType + 1;

        r_MotherShip = new MotherShip(Game);
            this.Add(r_MotherShip);

            r_Aliens = new AlienMatrix(Game, firstTypeAlienScore, secondTypeAlienScore, thirdTypeAlienScore, maxAmountOfBullets, cols);
            r_Aliens.AllDead += R_Aliens_AllDead;
            r_Aliens.ReachBottom += R_Aliens_ReachBottom;
            this.Add(r_Aliens);

            r_Walls = new WallsLine(Game, wallsSpeed);
            this.Add(r_Walls);

            r_PlayerShipOne = new PlayerShip(Game, "Ship01_32x32");
            this.Add(r_PlayerShipOne);
            this.Add(r_PlayerShipOne.Shooter);

            r_PlayerOne = new Player(Game, "P1", i_playerOneScore);
            r_PlayerOne.TextColor = Color.Blue;
            r_PlayerOne.PlayerShip = r_PlayerShipOne;
            r_PlayerOne.YPosition = 10;
            this.Add(r_PlayerOne);

            if (AppSettings.Instance.AmountOfPlayers == 2)
            {
                r_PlayerShipTwo = new PlayerShip(Game, "Ship02_32x32");
                this.Add(r_PlayerShipTwo);
                this.Add(r_PlayerShipTwo.Shooter);

                r_PlayerTwo = new Player(Game, "P2", i_playerTwoScore);
                r_PlayerTwo.TextColor = Color.Green;
                r_PlayerTwo.PlayerShip = r_PlayerShipTwo;
                r_PlayerTwo.YPosition = r_PlayerOne.YPosition + 20;
                this.Add(r_PlayerTwo);
            }
        }

        private void R_Aliens_AllDead(object sender, EventArgs e)
        {
            ((ISoundManager)Game.Services.GetService(typeof(ISoundManager))).PlaySound(k_GameOverSound);

            ScreensManager.SetCurrentScreen(new LevelTransitionScreen(Game, r_LevelType + 1, r_PlayerOne.PlayerScore, r_PlayerTwo != null ? r_PlayerTwo.PlayerScore : 0));
            ExitScreen();
        }

        private void R_Aliens_ReachBottom(object sender, EventArgs e)
        {
            ((ISoundManager)Game.Services.GetService(typeof(ISoundManager))).PlaySound(k_GameOverSound);

            ScreensManager.SetCurrentScreen(new GameOverScreen(Game, r_LevelType));
            ExitScreen();
        }

        public override void Initialize()
        {
            base.Initialize();

            r_MotherShip.TintColor = Color.Red;

            r_PlayerShipOne.Position = new Vector2(0f, GraphicsDevice.Viewport.Height - (r_PlayerShipOne.Height * 2 * 0.6f));
            r_PlayerShipOne.UseMouse = true;
            r_PlayerShipOne.MoveLeftKey = Keys.Left;
            r_PlayerShipOne.MoveRightKey = Keys.Right;
            r_PlayerShipOne.ShootKey = Keys.Space;

            if (AppSettings.Instance.AmountOfPlayers == 2)
            {
                r_PlayerShipTwo.Position = new Vector2(r_PlayerShipOne.Width, GraphicsDevice.Viewport.Height - (r_PlayerShipOne.Height * 2 * 0.6f));
                r_PlayerShipTwo.UseMouse = false;
                r_PlayerShipTwo.MoveLeftKey = Keys.S;
                r_PlayerShipTwo.MoveRightKey = Keys.F;
                r_PlayerShipTwo.ShootKey = Keys.E;
            }

            r_Walls.YPosition = r_PlayerShipOne.TopLeftPosition.Y - (ContentManager.Load<Texture2D>(Wall.k_SpritesPath + Wall.k_TexturePath).Height * 2);

            r_Aliens.MaxBottomPosition = (int)r_PlayerShipOne.TopLeftPosition.Y;

            foreach(Alien alien in r_Aliens)
            {
                this.Add(alien.Shooter);
            }
        }

        public override void Update(GameTime i_GameTime)
        {
            if(KeyboardManager.IsKeyPressed(Keys.P))
            {
                ScreensManager.SetCurrentScreen(new PauseScreen(Game));

                //Microsoft.Xna.Framework.Media.MediaPlayer.Play(Game.Content.Load<Microsoft.Xna.Framework.Media.Song>("Sounds/BGMusic") as Microsoft.Xna.Framework.Media.Song);
            }

            if (r_PlayerOne.Souls == 0 && ((AppSettings.Instance.AmountOfPlayers == 2 && r_PlayerTwo.Souls == 0) || AppSettings.Instance.AmountOfPlayers == 1))
            {
                ((ISoundManager)Game.Services.GetService(typeof(ISoundManager))).PlaySound(k_GameOverSound);

                ExitScreen();
            }
            else
            {
                base.Update(i_GameTime);
            }
        }
    }
}
