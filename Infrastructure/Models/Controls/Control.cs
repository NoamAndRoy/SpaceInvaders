﻿using System;
using Infrastructure.Managers;
using Infrastructure.ManagersInterfaces;
using Microsoft.Xna.Framework;
using Infrastructure.Models.Screens;

namespace Infrastructure.Models.Controls
{
    public abstract class Control : Component2D
    {
        protected readonly Sprite r_Texture;

        public Sprite Texture
        {
            get { return r_Texture; }
        }

        private bool m_DrawTexture = true;

        public bool DrawTexture
        {
            get { return m_DrawTexture; }
            set { m_DrawTexture = value; }
        }

        protected readonly Text r_Text;

        public Text Text
        {
            get { return r_Text; }
        }

        private bool isTextureBlank = true;
        private readonly string r_Name;

        public string Name
        {
            get { return r_Name; }
        }

        private IKeyboardManager m_KeyboardManager;

        public IKeyboardManager KeyboardManager
        {
            get { return m_KeyboardManager; }
        }

        private IMouseManager m_DummyMouseManager = new DummyMouseManager();
        private IMouseManager m_MouseManager;

        public IMouseManager MouseManager
        {
            get { return IsMouseOn(m_MouseManager.X, m_MouseManager.Y) && Game.IsMouseVisible ? m_MouseManager : m_DummyMouseManager; }
        }

        private readonly bool r_IsActiveable;

        public bool IsActiveable
        {
            get
            {
                return r_IsActiveable;
            }
        }

        public Color InActiveColor = Color.Gray;
        public Color ActiveColor = Color.White;

        public Control(GameScreen i_GameScreen, string i_Name, Text i_Text, bool i_IsActiveable = true)
            : base(i_GameScreen, i_Text.AssetName)
        {
            r_Texture = new Sprite(i_GameScreen, @"..\Screens\blank");
            r_Text = i_Text;
            r_Text.SizeChanged += text_SizeChanged;
            Text.TintColor = Color.Black;

            r_Name = i_Name;
            r_IsActiveable = i_IsActiveable;
        }

        public Control(GameScreen i_GameScreen, string i_Name, Text i_Text, Sprite i_Texture, bool i_IsActiveable = true)
            : this(i_GameScreen, i_Name, i_Text)
        {
            r_Texture = i_Texture;
            isTextureBlank = false;
            r_IsActiveable = i_IsActiveable;
        }

        private void text_SizeChanged(object sender, EventArgs e)
        {
            Width = Math.Max(Width, Math.Max(Text.Width, Texture.Width));
            Height = Math.Max(Height, Math.Max(Text.Height, Texture.Height));
        }

        public override void Initialize()
        {
            Texture.Initialize();
            Text.Initialize();

            m_KeyboardManager = (IKeyboardManager)Game.Services.GetService(typeof(IKeyboardManager));
            m_MouseManager = (IMouseManager)Game.Services.GetService(typeof(IMouseManager));

            base.Initialize();
        }

        protected override void initBounds()
        {
            if (isTextureBlank)
            {
                WidthBeforeScale = Text.WidthBeforeScale;
                HeightBeforeScale = Text.HeightBeforeScale;
            }
            else
            {
                WidthBeforeScale = Math.Max(Texture.WidthBeforeScale, Text.WidthBeforeScale);
                HeightBeforeScale = Math.Max(Texture.HeightBeforeScale, Text.HeightBeforeScale);
            }
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            Texture.SpriteBatch = this.SpriteBatch;
            Text.SpriteBatch = this.SpriteBatch;
        }

        public override void Update(GameTime i_GameTime)
        {
            Texture.Update(i_GameTime);
            Text.Update(i_GameTime);

            base.Update(i_GameTime);
        }

        public override void Draw(GameTime i_GameTime)
        {
            Rectangle? sourceRectangle;

            if (isTextureBlank)
            {
                sourceRectangle = null;
            }
            else
            {
                sourceRectangle = Texture.SourceRectangle;
            }

            if (DrawTexture)
            {
                SpriteBatch.Draw(Texture.Texture, new Rectangle((int)positionForDraw.X, (int)positionForDraw.Y, (int)Width, (int)Height), sourceRectangle, this.TintColor, this.Rotation, this.RotationOrigin, this.SpriteEffects, this.LayerDepth);
            }

            SpriteBatch.DrawString(Text.Font, Text.Content, this.positionForDraw + new Vector2(Width / 2, Height / 2) - Text.TextureCenter, new Color(Text.TintColor, this.Opacity), this.Rotation, this.RotationOrigin, this.Scales, this.SpriteEffects, this.LayerDepth);

            base.Draw(i_GameTime);
        }

        protected override void OnEnabledChanged(object sender, EventArgs args)
        {
            if (IsActiveable)
            {
                TintColor = Enabled ? ActiveColor : InActiveColor;
            }

            base.OnEnabledChanged(sender, args);
        }

        public bool IsMouseOn(int i_X, int i_Y)
        {
            return i_X >= this.TopLeftPosition.X && i_X <= this.TopLeftPosition.X + Width
                && i_Y >= this.TopLeftPosition.Y && i_Y <= this.TopLeftPosition.Y + Height;
        }
    }
}
