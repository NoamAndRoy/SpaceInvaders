using Infrastructure.ManagersInterfaces;
using Infrastructure.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Infrastructure.Managers
{
    public class MouseManager : GameService, IMouseManager
    {
        private MouseState m_PrevMouseState;
        private MouseState m_MouseState;

        public MouseManager(BaseGame i_BaseGame)
            : base(i_BaseGame)
        {
        }

        protected override void registerService()
        {
            this.Game.Services.AddService(typeof(IMouseManager), this);
        }

        public override void Update(GameTime i_GameTime)
        {
            m_PrevMouseState = m_MouseState;
            m_MouseState = Mouse.GetState();

            base.Update(i_GameTime);
        }

        public int X
        {
            get { return m_MouseState.X; }
        }

        public int Y
        {
            get { return m_MouseState.Y; }
        }

        public int DeltaX
        {
            get { return m_MouseState.X - m_PrevMouseState.X; }
        }

        public int DeltaY
        {
            get { return m_MouseState.Y - m_PrevMouseState.Y; }
        }

        public bool IsKeyPressed(eMouseButton i_MouseButton)
        {
            return getMouseButtonState(m_MouseState, i_MouseButton) == ButtonState.Pressed
                && getMouseButtonState(m_PrevMouseState, i_MouseButton) == ButtonState.Released;
        }

        public bool IsKeyReleased(eMouseButton i_MouseButton)
        {
            return getMouseButtonState(m_MouseState, i_MouseButton) == ButtonState.Released
                && getMouseButtonState(m_PrevMouseState, i_MouseButton) == ButtonState.Pressed;
        }

        public bool IsKeyHeld(eMouseButton i_MouseButton)
        {
            return getMouseButtonState(m_MouseState, i_MouseButton) == ButtonState.Pressed
                && getMouseButtonState(m_PrevMouseState, i_MouseButton) == ButtonState.Pressed;
        }

        private ButtonState getMouseButtonState(MouseState i_MouseState, eMouseButton i_MouseButton)
        {
            ButtonState mouseButtonState = ButtonState.Released;
            switch (i_MouseButton)
            {
                case eMouseButton.LeftButton:
                    mouseButtonState = i_MouseState.LeftButton;
                    break;
                case eMouseButton.RightButton:
                    mouseButtonState = i_MouseState.RightButton;
                    break;
                case eMouseButton.MiddleButton:
                    mouseButtonState = i_MouseState.MiddleButton;
                    break;
            }

            return mouseButtonState;
        }
    }
}
