using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Infrastructure.Models.Screens;

namespace Infrastructure.Models.Controls
{
    public class Label : Control
    {
        public Label(GameScreen i_GameScreen, string i_Name, Text i_Text)
            : base(i_GameScreen, i_Name, i_Text, false)
        {
            DrawTexture = false;
        }

        public Label(GameScreen i_GameScreen, string i_Name, Text i_Text, Sprite i_Texture)
            : base(i_GameScreen, i_Name, i_Text, i_Texture, false)
        {
            DrawTexture = false;
        }
    }
}
