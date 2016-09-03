using System;
using Microsoft.Xna.Framework;
using Infrastructure.Managers;
using SpaceInvaders.Models.Screens;

namespace SpaceInvaders
{
    public class Invaders : Game
    {
        private GraphicsDeviceManager m_Graphics;

        public Invaders() : base()
        {
            m_Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.m_Graphics.PreferredBackBufferWidth = 800;
            this.m_Graphics.PreferredBackBufferHeight = 600;

            CollisionManager collisionManager = new CollisionManager(this);
            KeyboardManager keyBoardManager = new KeyboardManager(this);
            MouseManager mouseManager = new MouseManager(this);

            ScreensMananger screensManager = new ScreensMananger(this);
            screensManager.Push(new GameOverScreen(this));
            screensManager.SetCurrentScreen(new WelcomeScreen(this));   
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.Window.Title = "Invaders";
            this.Window.AllowUserResizing = AppSettings.Instance.AllowResizing;
            this.m_Graphics.IsFullScreen = AppSettings.Instance.FullScreen;
            m_Graphics.ApplyChanges();
        }

        protected override void Draw(GameTime i_GameTime)
        {
            m_Graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(i_GameTime);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            AppSettings.Instance.SaveSettings();
            base.OnExiting(sender, args);
        }
    }
}