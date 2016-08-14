using Infrastructure.ManagersInterfaces;
using Infrastructure.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Infrastructure.Managers
{
    public class KeyboardManager : GameService, IKeyboardManager
    {
        private KeyboardState m_PrevKeyboardState;
        private KeyboardState m_KeyboardState;

        public KeyboardManager(BaseGame i_BaseGame)
            : base(i_BaseGame)
        {
        }

        protected override void registerService()
        {
            this.Game.Services.AddService(typeof(IKeyboardManager), this);
        }

        public override void Update(GameTime i_GameTime)
        {
            m_PrevKeyboardState = m_KeyboardState;
            m_KeyboardState = Keyboard.GetState();

            base.Update(i_GameTime);
        }

        public bool IsKeyPressed(Keys i_Key)
        {
            return m_KeyboardState.IsKeyDown(i_Key) && m_PrevKeyboardState.IsKeyUp(i_Key);
        }

        public bool IsKeyReleased(Keys i_Key)
        {
            return m_KeyboardState.IsKeyUp(i_Key) && m_PrevKeyboardState.IsKeyDown(i_Key);
        }

        public bool IsKeyHeld(Keys i_Key)
        {
            return m_KeyboardState.IsKeyDown(i_Key) && m_PrevKeyboardState.IsKeyDown(i_Key);
        }
    }
}
