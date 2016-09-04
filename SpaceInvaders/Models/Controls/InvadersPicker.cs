using Microsoft.Xna.Framework;

namespace Infrastructure.Models.Controls
{
    public class InvadersPicker<T> : Picker<T>
    {
        public InvadersPicker(Game i_Game, string i_Name, string i_Title, Text i_Text)
            : base(i_Game, i_Name, i_Title, i_Text)
        {
        }

        public InvadersPicker(Game i_Game, string i_Name, string i_Title, Text i_Text, Sprite i_Texture)
            : base(i_Game, i_Name, i_Title, i_Text, i_Texture)
        {
        }
    }
}