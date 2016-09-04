using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Infrastructure.Models.Controls
{
    public class Label : Control
    {
        public Label(Game i_Game, string i_Name, Text i_Text)
            : base(i_Game, i_Name, i_Text, false)
        {
        }

        public Label(Game i_Game, string i_Name, Text i_Text, Sprite i_Texture)
            : base(i_Game, i_Name, i_Text, i_Texture, false)
        {
        }
    }
}
