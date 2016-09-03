////*** Guy Ronen © 2008-2011 ***//
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.ManagersInterfaces;
using Infrastructure.Managers;

namespace Infrastructure.Models.Screens
{
    public enum eScreenState
    {
        Activating,
        Active,
        Deactivating,
        Inactive,
        Closing,
        Closed
    }

    public class StateChangedEventArgs : EventArgs
    {
        protected eScreenState m_PrevState;

        public eScreenState PrevState
        {
            get { return m_PrevState; }
            set { m_PrevState = value; }
        }

        protected eScreenState m_CurrentState;

        public eScreenState CurrentState
        {
            get { return m_CurrentState; }
            set { m_CurrentState = value; }
        }

        public StateChangedEventArgs()
        {
        }

        public StateChangedEventArgs(eScreenState i_PrevState, eScreenState i_CurrState)
        {
            m_PrevState = i_PrevState;
            m_CurrentState = i_CurrState;
        }
    }

    public abstract class GameScreen : CompositeDrawableComponent<IGameComponent>
    {
        private Sprite m_Background;

        public Sprite Background
        {
            get { return m_Background; }
            set
            {
                if (m_Background != value)
                {
                    if (m_Background != null)
                    {
                        this.Remove(m_Background);
                    }

                    m_Background = value;
                    this.Add(m_Background);
                }
            }
        }

        public GameScreen(Game i_Game)
            : base(i_Game)
        {
            this.Enabled = false;
            this.Visible = false;
        }

        protected eScreenState m_State = eScreenState.Inactive;

        public eScreenState State
        {
            get { return m_State; }
            set
            {
                if (m_State != value)
                {
                    StateChangedEventArgs args = new StateChangedEventArgs(m_State, value);
                    m_State = value;
                    OnStateChanged(args);
                }
            }
        }

        public event EventHandler<StateChangedEventArgs> StateChanged;

        private void OnStateChanged(StateChangedEventArgs args)
        {
            switch (args.CurrentState)
            {
                case eScreenState.Activating:
                    OnActivating();
                    break;
                case eScreenState.Active:
                    OnActivated();
                    break;
                case eScreenState.Deactivating:
                    break;
                case eScreenState.Closing:
                    break;
                case eScreenState.Inactive:
                case eScreenState.Closed:
                    OnDeactivated();
                    break;
                default:
                    break;
            }

            if (StateChanged != null)
            {
                StateChanged(this, args);
            }
        }

        protected IScreensMananger m_ScreensManager;

        public IScreensMananger ScreensManager
        {
            get { return m_ScreensManager; }
            set { m_ScreensManager = value; }
        }

        protected bool m_IsModal = true;

        public bool IsModal // background screen should not be updated
        {
            get { return m_IsModal; }
            set { m_IsModal = value; }
        }

        protected bool m_IsOverlayed;

        public bool IsOverlayed // background screen should be drawn
        {
            get { return m_IsOverlayed; }
            set { m_IsOverlayed = value; }
        }

        protected GameScreen m_PreviousScreen;

        public GameScreen PreviousScreen
        {
            get { return m_PreviousScreen; }
            set { m_PreviousScreen = value; }
        }

        protected bool m_HasFocus;

        public bool HasFocus // should handle input
        {
            get { return m_HasFocus; }
            set { m_HasFocus = value; }
        }

        protected float m_BlackTintAlpha = 0;

        public float BlackTintAlpha
        {
            get { return m_BlackTintAlpha; }
            set
            {
                if (m_BlackTintAlpha < 0 || m_BlackTintAlpha > 1)
                {
                    throw new ArgumentException("value must be between 0 and 1", "BackgroundDarkness");
                }

                m_BlackTintAlpha = value;
            }
        }

        private IKeyboardManager m_KeyboardManager;
        private IKeyboardManager m_DummyKeyboardManager = new DummyKeyboardManager();

        public IKeyboardManager KeyboardManager
        {
            get { return this.HasFocus ? m_KeyboardManager : m_DummyKeyboardManager; }
        }

        private IMouseManager m_MouseManager;
        private IMouseManager m_DummyMouseManager = new DummyMouseManager();

        public IMouseManager MouseManager
        {
            get { return this.HasFocus ? m_MouseManager : m_DummyMouseManager; }
        }

        public override void Initialize()
        {
            m_KeyboardManager = Game.Services.GetService(typeof(IKeyboardManager)) as IKeyboardManager;
            if (m_KeyboardManager == null)
            {
                m_KeyboardManager = m_DummyKeyboardManager;
            }

            m_MouseManager = Game.Services.GetService(typeof(IMouseManager)) as IMouseManager;
            if (m_MouseManager == null)
            {
                m_MouseManager = m_DummyMouseManager;
            }

            base.Initialize();
        }

        internal virtual void Activate()
        {
            if (this.State == eScreenState.Inactive
                || this.State == eScreenState.Deactivating
                || this.State == eScreenState.Closed
                || this.State == eScreenState.Closing)
            {
                this.State = eScreenState.Activating;

                if (m_ActivationLength == TimeSpan.Zero)
                {
                    this.State = eScreenState.Active;
                }
            }
        }

        protected virtual void OnActivating()
        {
            this.Enabled = true;
            this.Visible = true;
            this.HasFocus = true;
        }

        protected virtual void OnActivated()
        {
            if (PreviousScreen != null)
            {
                PreviousScreen.HasFocus = !this.HasFocus;
            }

            m_TransitionPosition = 1;
        }

        protected internal virtual void Deactivate()
        {
            if (this.State == eScreenState.Active
                || this.State == eScreenState.Activating)
            {
                this.State = eScreenState.Deactivating;

                if (m_DeactivationLength == TimeSpan.Zero)
                {
                    this.State = eScreenState.Inactive;
                }
            }
        }

        protected void ExitScreen()
        {
            this.State = eScreenState.Closing;
            if (DeactivationLength == TimeSpan.Zero)
            {
                this.State = eScreenState.Closed;
            }
        }

        protected virtual void OnDeactivated()
        {
            this.Enabled = false;
            this.Visible = false;
            this.HasFocus = false;

            m_TransitionPosition = 0;
        }

        private Texture2D m_GradientTexture;
        private Texture2D m_BlankTexture;

        protected override void LoadContent()
        {
            base.LoadContent();

            m_GradientTexture = this.ContentManager.Load<Texture2D>(@"Screens\gradient");
            m_BlankTexture = this.ContentManager.Load<Texture2D>(@"Screens\blank");
        }

        public override void Draw(GameTime i_GameTime)
        {
            bool fading = UseFadeTransition
                && TransitionPosition > 0
                && TransitionPosition < 1;

            if (PreviousScreen != null
                && IsOverlayed)
            {
                PreviousScreen.Draw(i_GameTime);

                if (!fading && (BlackTintAlpha > 0 || UseGradientBackground))
                {
                    FadeBackBufferToBlack((byte)(m_BlackTintAlpha * byte.MaxValue));
                }
            }

            if(m_DrawableComponents.Count > 0)
            {
                if (Background != null && Background.Visible)
                {
                    m_SpriteBatch.Begin(this.SpritesSortMode, this.BlendState, this.SamplerState, this.DepthStencilState, this.RasterizerState, this.Shader, this.TransformMatrix);
                    Background.Draw(i_GameTime);
                    m_SpriteBatch.End();
                }

                base.Draw(i_GameTime);
            }
            else
            {
                draw2DComponents(i_GameTime);
            }

            if (fading)
            {
                FadeBackBufferToBlack(TransitionAlpha);
            }
        }

        protected override void draw2DComponents(GameTime i_GameTime)
        {
            m_SpriteBatch.Begin(this.SpritesSortMode, this.BlendState, this.SamplerState, this.DepthStencilState, this.RasterizerState, this.Shader, this.TransformMatrix);

            foreach (Component2D component2D in m_2DComponents)
            {
                if (component2D.Visible && (m_DrawableComponents.Count <= 0 || component2D != Background))
                {
                    component2D.Draw(i_GameTime);
                }
            }

            m_SpriteBatch.End();
        }

        protected bool m_UseGradientBackground = false;

        public bool UseGradientBackground
        {
            get { return m_UseGradientBackground; }
            set { m_UseGradientBackground = value; }
        }

        public void FadeBackBufferToBlack(byte i_Alpha)
        {
            Viewport viewport = this.GraphicsDevice.Viewport;

            Texture2D background = UseGradientBackground ? m_GradientTexture : m_BlankTexture;

            SpriteBatch.Begin();
            SpriteBatch.Draw(background, new Rectangle(0, 0, viewport.Width, viewport.Height), new Color(0, 0, 0, i_Alpha));
            SpriteBatch.End();
        }

        #region Transitions Support
        /// <summary>
        /// Indicates how long the screen takes to
        /// transition on when it is activated.
        /// </summary>
        public TimeSpan ActivationLength
        {
            get { return m_ActivationLength; }
            protected set { m_ActivationLength = value; }
        }

        private TimeSpan m_ActivationLength = TimeSpan.Zero;

        /// <summary>
        /// Indicates how long the screen takes to
        /// transition off when it is deactivated.
        /// </summary>
        public TimeSpan DeactivationLength
        {
            get { return m_DeactivationLength; }
            protected set { m_DeactivationLength = value; }
        }

        private TimeSpan m_DeactivationLength = TimeSpan.Zero;

        private float m_TransitionPosition = 0;

        /// <summary>
        /// Gets the current position of the screen transition, ranging
        /// from 0 (deactive, no transition) to 1 (active, no transition).
        /// </summary>
        public float TransitionPosition
        {
            get { return m_TransitionPosition; }
            protected set { m_TransitionPosition = value; }
        }

        /// <summary>
        /// There are two possible reasons why a screen might be transitioning
        /// off. It could be temporarily going away to make room for another
        /// screen that is on top of it, or it could be going away for good.
        /// This property indicates whether the screen is exiting for real:
        /// if set, the screen will automatically remove itself as soon as the
        /// transition finishes.
        /// </summary>
        public bool IsClosing
        {
            get { return m_IsClosing; }
            protected internal set { m_IsClosing = value; }
        }

        private bool m_IsClosing = false;

        public override void Update(GameTime gameTime)
        {
            bool doUpdate = true;
            switch (this.State)
            {
                case eScreenState.Activating:
                case eScreenState.Deactivating:
                case eScreenState.Closing:
                    updateTransition(gameTime);
                    break;
                case eScreenState.Active:
                    break;
                case eScreenState.Inactive:
                case eScreenState.Closed:
                    doUpdate = false;
                    break;
                default:
                    break;
            }

            if (doUpdate)
            {
                base.Update(gameTime);

                if (PreviousScreen != null && !this.IsModal)
                {
                    PreviousScreen.Update(gameTime);
                }
            }
        }

        /// <summary>
        /// Helper for updating the screen transition position.
        /// </summary>
        private void updateTransition(GameTime i_GameTime)
        {
            bool transionEnded = false;

            int direction = this.State == eScreenState.Activating ? 1 : -1;

            TimeSpan transitionLength = this.State == eScreenState.Activating ? m_ActivationLength : m_DeactivationLength;

            // How much should we move by?
            float transitionDelta;

            if (transitionLength == TimeSpan.Zero)
            {
                transitionDelta = 1;
            }
            else
            {
                transitionDelta = (float)(
                    i_GameTime.ElapsedGameTime.TotalMilliseconds
                    / transitionLength.TotalMilliseconds);
            }

            // Update the transition position.
            m_TransitionPosition += transitionDelta * direction;

            // Did we reach the end of the transition?
            if (((direction < 0) && (m_TransitionPosition <= 0)) ||
                ((direction > 0) && (m_TransitionPosition >= 1)))
            {
                m_TransitionPosition = MathHelper.Clamp(m_TransitionPosition, 0, 1);
                transionEnded = true;
            }

            if (transionEnded)
            {
                OnTransitionEnded();
            }
        }

        private void OnTransitionEnded()
        {
            switch (this.State)
            {
                case eScreenState.Inactive:
                case eScreenState.Activating:
                    this.State = eScreenState.Active;
                    break;
                case eScreenState.Active:
                case eScreenState.Deactivating:
                    this.State = eScreenState.Inactive;
                    break;
                case eScreenState.Closing:
                    this.State = eScreenState.Closed;
                    break;
            }
        }

        protected byte TransitionAlpha
        {
            get { return (byte)(byte.MaxValue * m_TransitionPosition * m_BlackTintAlpha); }
        }

        protected bool m_UseFadeTransition = true;

        public bool UseFadeTransition
        {
            get { return m_UseFadeTransition; }
            set { m_UseFadeTransition = value; }
        }

        #endregion Transitions Support
    }
}
