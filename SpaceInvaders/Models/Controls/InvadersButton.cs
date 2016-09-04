using Microsoft.Xna.Framework;

namespace Infrastructure.Models.Controls
{
    public class InvadersButton : Button
    {
        public InvadersButton(Game i_Game, string i_Name, Text i_Text)
            : base(i_Game, i_Name, i_Text)
        {
        }

        public InvadersButton(Game i_Game, string i_Name, Text i_Text, Sprite i_Texture)
            : base(i_Game, i_Name, i_Text, i_Texture)
        {
        }
    }
}
