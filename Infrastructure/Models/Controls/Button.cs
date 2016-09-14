using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Infrastructure.Models.Screens;

namespace Infrastructure.Models.Controls
{
    public class Button : Control
    {
        public event EventHandler Click;

        public Button(GameScreen i_GameScreen, string i_Name, Text i_Text)
            : base(i_GameScreen, i_Name, i_Text)
        {
        }

        public Button(GameScreen i_GameScreen, string i_Name, Text i_Text, Sprite i_Texture)
            : base(i_GameScreen, i_Name, i_Text, i_Texture)
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

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                Click = null;
            }

            base.Dispose(disposing);
        }
    }
}
