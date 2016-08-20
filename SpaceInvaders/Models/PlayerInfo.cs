using Infrastructure.Models;
using Microsoft.Xna.Framework;

namespace SpaceInvaders.Models
{
    public class PlayerInfo : Sprite
    {
        public PlayerInfo(Game i_Game, string i_TexturePath)
            : base(i_Game, i_TexturePath)
        {
        }
    }
}