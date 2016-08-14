using Infrastructure.Models;
using Microsoft.Xna.Framework;

namespace SpaceInvaders.Models
{
    public class Wall : CollideableSprite
    {
        public Wall(Game i_Game, string i_TexturePath) 
            : base(i_Game, i_TexturePath)
        {
        }
    }
}
