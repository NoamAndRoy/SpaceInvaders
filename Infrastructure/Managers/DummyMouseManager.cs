using Infrastructure.ManagersInterfaces;
using Infrastructure.Models;
using Microsoft.Xna.Framework.Input;

namespace Infrastructure.Managers
{
    public class DummyMouseManager : IMouseManager
    {
        private MouseState m_DummyMouseState = new MouseState();

        public MouseState CurrentState
        {
            get
            {
                return m_DummyMouseState;
            }
        }

        public MouseState PrevState
        {
            get
            {
                return m_DummyMouseState;
            }
        }

        public int DeltaX
        {
            get
            {
                return 0;
            }
        }

        public int DeltaY
        {
            get
            {
                return 0;
            }
        }

        public int X
        {
            get
            {
                return -1;
            }
        }

        public int PrevX
        {
            get { return -1; }
        }

        public int Y
        {
            get
            {
                return -1;
            }
        }

        public int PrevY
        {
            get { return -1; }
        }

        public int MouseWheel
        {
            get
            {
                return 0;
            }
        }

        public int MouseWheelDelta
        {
            get
            {
                return 0;
            }
        }

        public bool IsKeyHeld(eMouseButton i_MouseButton)
        {
            return false;
        }

        public bool IsKeyPressed(eMouseButton i_MouseButton)
        {
            return false;
        }

        public bool IsKeyReleased(eMouseButton i_MouseButton)
        {
            return true;
        }
    }
}