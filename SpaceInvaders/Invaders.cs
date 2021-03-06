﻿using System;
using Infrastructure.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SpaceInvaders.Models.Screens;

namespace SpaceInvaders
{
    public class Invaders : Game
    {
        private const string k_BGMusicSound = "BGMusic";

        private GraphicsDeviceManager m_Graphics;
        private CollisionManager collisionManager;
        private KeyboardManager keyBoardManager;
        private MouseManager mouseManager;
        private SoundManager soundManager;
        private ScreensMananger screensManager;

        public Invaders() : base()
        {
            m_Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.m_Graphics.PreferredBackBufferWidth = 800;
            this.m_Graphics.PreferredBackBufferHeight = 600;

            this.collisionManager = new CollisionManager(this);
            this.keyBoardManager = new KeyboardManager(this);
            this.mouseManager = new MouseManager(this);
            this.soundManager = new SoundManager(this);

            this.screensManager = new ScreensMananger(this);
            this.screensManager.SetCurrentScreen(new WelcomeScreen(this));   
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.Window.Title = "Invaders";
            this.Window.AllowUserResizing = AppSettings.Instance.AllowResizing;
            this.m_Graphics.IsFullScreen = AppSettings.Instance.FullScreen;
            m_Graphics.ApplyChanges();

            soundManager.SoundEffectsVolume = AppSettings.Instance.SoundEffectsVolume;
            soundManager.BackgroundMusicVolume = AppSettings.Instance.BackgroundMusicVolume;
            soundManager.ToggleSound = AppSettings.Instance.ToggleSound;

            soundManager.PlayContinuousSound(k_BGMusicSound, true);
        }

        protected override void Update(GameTime gameTime)
        {
            if (keyBoardManager.IsKeyPressed(Keys.M))
            {
                soundManager.ToggleSound = !soundManager.ToggleSound;
            }

            base.Update(gameTime);
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