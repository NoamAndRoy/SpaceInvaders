using System;
using Infrastructure.ManagersInterfaces;
using Microsoft.Xna.Framework.Input;

namespace Infrastructure.Managers
{
    public class DummyKeyboardManager : IKeyboardManager
    {
        private KeyboardState m_DummyKeyboardState = new KeyboardState();

        public KeyboardState CurrentState
        {
            get
            {
                return m_DummyKeyboardState;
            }
        }

        public KeyboardState PrevState
        {
            get
            {
                return m_DummyKeyboardState;
            }
        }

        public bool IsKeyHeld(Keys i_Key)
        {
            return false;
        }

        public bool IsKeyPressed(Keys i_Key)
        {
            return false;
        }

        public bool IsKeyReleased(Keys i_Key)
        {
            return true;
        }
    }
}
