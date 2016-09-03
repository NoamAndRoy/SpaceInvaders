using System;
using Microsoft.Xna.Framework;
using Infrastructure.ManagersInterfaces;
using Microsoft.Xna.Framework.Input;
using Infrastructure.Models;

namespace Infrastructure.Models.Controls
{
    public class Button : Control
    {
        public event EventHandler Click;

        public Button(Game i_Game, string i_Name, Text i_Text)
            : base(i_Game, i_Name, i_Text)
        {
        }

        public Button(Game i_Game, string i_Name, Text i_Text, Sprite i_Texture)
            : base(i_Game, i_Name, i_Text, i_Texture)
        {
        }

        public override void Update(GameTime i_GameTime)
        {
            if (KeyboardManager.IsKeyPressed(Keys.Enter) || MouseManager.IsKeyPressed(eMouseButton.LeftButton))
            {
                OnClick();
            }

            base.Update(i_GameTime);
        }

        protected virtual void OnClick()
        {
            if(Click != null)
            {
                Click.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
